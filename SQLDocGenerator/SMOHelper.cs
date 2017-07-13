using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using System.IO;

namespace SQLDocGenerator
{
    public class SMOHelper
    {
        public class TriggersHelper
        {
            private static DataTable triggers = new DataTable("Triggers");
            public static DataTable Triggers { get { return triggers; } }

            public static void GetTriggers(string database)
            {
                ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
                Server server = new Server(serverConnection);
                Database db = server.Databases[database];

                ScriptingOptions so = new ScriptingOptions();
                so.ChangeTracking = true;
                so.ClusteredIndexes = true;
                so.ExtendedProperties = true;

                CreateTriggersSchema();

                foreach (Table table in db.Tables)
                {
                    TriggerCollection triggerCollection = table.Triggers;
                    if (triggerCollection.Count > 0)
                    {
                        foreach (Trigger trigger in triggerCollection)
                        {
                            StringCollection script = trigger.Script(so);
                            string[] scriptArray = new string[script.Count];
                            script.CopyTo(scriptArray, 0);

                            DataRow dr = triggers.NewRow();
                            dr["Name"] = trigger.Name;
                            //dr["Schema"] = trigger.;
                            dr["Description"] = trigger.ExtendedProperties["MS_Description"] != null ? trigger.ExtendedProperties["MS_Description"].Value.ToString() : string.Empty;
                            dr["TableName"] = table.Name;
                            dr["InsteadOf"] = trigger.InsteadOf;
                            dr["IsEnabled"] = trigger.IsEnabled;
                            dr["CreateDate"] = trigger.CreateDate;
                            dr["DateLastModified"] = trigger.DateLastModified;
                            dr["Insert"] = trigger.Insert;
                            dr["Update"] = trigger.Update;
                            dr["Delete"] = trigger.Delete;

                            for (int i = 0; i < scriptArray.Length; i++)
                                //scriptArray[i] = Utility.SplitString(scriptArray[i], 150);
                                scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                            dr["Script"] = string.Join(Environment.NewLine, scriptArray);

                            triggers.Rows.Add(dr);
                        }
                    }
                }
            }

            public static void WriteTriggers()
            {
                WriteTriggerList();
                WriteTriggerDetails();
            }

            private static void WriteTriggerList()
            {
                string xmlfile = "TriggerList.xml";
                string xslFile = "TriggerList.xsl";
                string htmFile = "TriggerList.htm";

                DataSet ds = new DataSet("TriggerList");
                ds.Tables.Add(triggers);

                File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

                HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                ht.TableTransformer();
            }
            private static void WriteTriggerDetails()
            {
                string xmlfile = string.Empty;
                string xslFile = string.Empty;
                string htmFile = string.Empty;

                DataSet ds = new DataSet();

                #region "Define DataTable for TableProperties"

                DataTable tableProperties = new DataTable("TableProperties");
                tableProperties.Columns.Add("NAME", typeof(String));
                tableProperties.Columns.Add("VALUE", typeof(String));
                ds.Tables.Add(tableProperties);

                #endregion

                #region "Define DataTable for Type"

                DataTable tableType = new DataTable("TableType");
                tableType.Columns.Add("InsteadOf", typeof(String));
                tableType.Columns.Add("Insert", typeof(String));
                tableType.Columns.Add("Update", typeof(String));
                tableType.Columns.Add("Delete", typeof(String));
                ds.Tables.Add(tableType);

                #endregion

                #region "Define DataTable for SQL"

                DataTable tableSQL = new DataTable("SQL");
                tableSQL.Columns.Add("SQL", typeof(String));
                ds.Tables.Add(tableSQL);

                #endregion

                foreach (DataRow row in triggers.Rows)
                {
                    string tableName = row["Name"].ToString();
                    xmlfile = tableName + ".xml";
                    xslFile = "TriggerDetails.xsl";
                    htmFile = tableName + ".htm";

                    #region "Fill TableProperties"
                    DataRow dr = tableProperties.NewRow();
                    dr["NAME"] = "Description";
                    dr["VALUE"] = row["Description"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Name";
                    dr["VALUE"] = tableName;
                    tableProperties.Rows.Add(dr);

                    //dr = tableProperties.NewRow();
                    //dr["NAME"] = "Schema";
                    //dr["VALUE"] = row["Schema"].ToString();
                    //tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "CreateDate";
                    dr["VALUE"] = row["CreateDate"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "IsEnabled";
                    dr["VALUE"] = row["IsEnabled"].ToString();
                    tableProperties.Rows.Add(dr);

                    #endregion

                    #region "Fill Columns"

                    DataRow[] colRows = triggers.Select("Name = '" + row["Name"] + "'");
                    foreach (DataRow tempDr in colRows)
                    {
                        DataRow type = tableType.NewRow();
                        type["InsteadOf"] = tempDr["InsteadOf"];
                        type["Insert"] = tempDr["Insert"];
                        type["Update"] = tempDr["Update"];
                        type["Delete"] = tempDr["Delete"];
                        tableType.Rows.Add(type);
                    }

                    #endregion

                    #region "SQL"

                    DataRow[] sqlRows = (triggers.Select("Name = '" + row["Name"] + "'"));
                    foreach (DataRow tempDr in sqlRows)
                    {
                        DataRow sql = tableSQL.NewRow();
                        sql["SQL"] = tempDr["Script"];
                        tableSQL.Rows.Add(sql);
                    }

                    #endregion

                    File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
                    HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                    ht.TableTransformer();

                    tableProperties.Clear();
                    tableType.Clear();
                    tableSQL.Clear();
                }
            }
            private static void CreateTriggersSchema()
            {
                triggers.Columns.Add(new DataColumn("Name", typeof(string)));
                //triggers.Columns.Add(new DataColumn("Schema", typeof(string)));
                triggers.Columns.Add(new DataColumn("Description", typeof(string)));
                triggers.Columns.Add(new DataColumn("TableName", typeof(string)));
                triggers.Columns.Add(new DataColumn("InsteadOf", typeof(string)));
                triggers.Columns.Add(new DataColumn("IsEnabled", typeof(string)));
                triggers.Columns.Add(new DataColumn("Script", typeof(string)));
                triggers.Columns.Add(new DataColumn("CreateDate", typeof(string)));
                triggers.Columns.Add(new DataColumn("DateLastModified", typeof(string)));
                triggers.Columns.Add(new DataColumn("Insert", typeof(string)));
                triggers.Columns.Add(new DataColumn("Update", typeof(string)));
                triggers.Columns.Add(new DataColumn("Delete", typeof(string)));
            }
        }

        public class UserDefinedDataTypesHelper
        {
            private static DataTable userDefinedDataTypes = new DataTable("UserDefinedDataTypes");
            public static DataTable UserDefinedDataTypes { get { return userDefinedDataTypes; } }

            public static void GetUserDefinedDataTypes(string database)
            {
                ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
                Server server = new Server(serverConnection);
                Database db = server.Databases[database];

                ScriptingOptions so = new ScriptingOptions();
                so.ChangeTracking = true;
                so.ClusteredIndexes = true;
                so.ExtendedProperties = true;

                CreateUserDefinedDataTypesSchema();

                foreach (UserDefinedDataType uddt in db.UserDefinedDataTypes)
                {
                    StringCollection script = uddt.Script(so);
                    string[] scriptArray = new string[script.Count];
                    script.CopyTo(scriptArray, 0);

                    DataRow dr = userDefinedDataTypes.NewRow();
                    dr["Name"] = uddt.Name;
                    dr["Schema"] = uddt.Schema;
                    dr["Parent"] = uddt.Parent;
                    dr["Description"] = ""; // uddt.ExtendedProperties["MS_Description"].Value.ToString();
                    dr["SystemType"] = uddt.SystemType;
                    dr["Nullable"] = uddt.Nullable;
                    dr["Rule"] = uddt.Rule;
                    dr["Default"] = uddt.Default;
                    dr["AllowIdentity"] = uddt.AllowIdentity;
                    dr["MaxLength"] = uddt.MaxLength;
                    dr["VariableLength"] = uddt.VariableLength;

                    for (int i = 0; i < scriptArray.Length; i++)
                        //scriptArray[i] = Utility.SplitString(scriptArray[i], 150);
                        scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                    dr["Script"] = string.Join(Environment.NewLine, scriptArray);

                    userDefinedDataTypes.Rows.Add(dr);
                }
            }

            public static void WriteUserDefinedDataTypes()
            {
                WriteUserDefinedDataTypesList();
                WriteUserDefinedDataTypesDetails();
            }

            private static void WriteUserDefinedDataTypesList()
            {
                string xmlfile = "UserDefinedDataTypeList.xml";
                string xslFile = "UserDefinedDataTypeList.xsl";
                string htmFile = "UserDefinedDataTypeList.htm";

                DataSet ds = new DataSet("UserDefinedDataTypeList");
                ds.Tables.Add(userDefinedDataTypes);

                File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

                HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                ht.TableTransformer();
            }
            private static void WriteUserDefinedDataTypesDetails()
            {
                string xmlfile = string.Empty;
                string xslFile = string.Empty;
                string htmFile = string.Empty;

                DataSet ds = new DataSet();

                #region "Define DataTable for TableProperties"

                DataTable tableProperties = new DataTable("TableProperties");
                tableProperties.Columns.Add("NAME", typeof(String));
                tableProperties.Columns.Add("VALUE", typeof(String));
                ds.Tables.Add(tableProperties);

                #endregion

                #region "Define DataTable for Columns Defined on"

                DataTable columnsDefinedOn = new DataTable("ColumnsDefinedOn");
                columnsDefinedOn.Columns.Add("Name", typeof(String));
                columnsDefinedOn.Columns.Add("Parent", typeof(String));
                columnsDefinedOn.Columns.Add("Description", typeof(String));
                ds.Tables.Add(columnsDefinedOn);

                #endregion

                #region "Define DataTable for SQL"

                DataTable tableSQL = new DataTable("SQL");
                tableSQL.Columns.Add("SQL", typeof(String));
                ds.Tables.Add(tableSQL);

                #endregion

                foreach (DataRow row in userDefinedDataTypes.Rows)
                {
                    string tableName = row["Name"].ToString();
                    xmlfile = tableName + ".xml";
                    xslFile = "UserDefinedDataTypeDetails.xsl";
                    htmFile = tableName + ".htm";

                    #region "Fill TableProperties"
                    DataRow dr = tableProperties.NewRow();
                    dr["NAME"] = "Description";
                    dr["VALUE"] = row["Description"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Name";
                    dr["VALUE"] = tableName;
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Schema";
                    dr["VALUE"] = row["Schema"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "SystemType";
                    dr["VALUE"] = row["SystemType"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Nullable";
                    dr["VALUE"] = row["Nullable"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Rule";
                    dr["VALUE"] = row["Rule"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Default";
                    dr["VALUE"] = row["Default"].ToString();
                    tableProperties.Rows.Add(dr);

                    #endregion

                    #region "Fill Columns"

                    DataRow[] colRows = userDefinedDataTypes.Select("Name = '" + row["Name"] + "'");
                    foreach (DataRow tempDr in colRows)
                    {
                        DataRow type = columnsDefinedOn.NewRow();
                        type["Name"] = tempDr["Name"];
                        type["Parent"] = tempDr["Parent"];
                        type["Description"] = tempDr["Description"];
                        columnsDefinedOn.Rows.Add(type);
                    }

                    #endregion

                    #region "SQL"

                    DataRow[] sqlRows = (userDefinedDataTypes.Select("Name = '" + row["Name"] + "'"));
                    foreach (DataRow tempDr in sqlRows)
                    {
                        DataRow sql = tableSQL.NewRow();
                        sql["SQL"] = tempDr["Script"];
                        tableSQL.Rows.Add(sql);
                    }

                    #endregion

                    File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
                    HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                    ht.TableTransformer();

                    tableProperties.Clear();
                    columnsDefinedOn.Clear();
                    tableSQL.Clear();
                }
            }
            private static void CreateUserDefinedDataTypesSchema()
            {
                userDefinedDataTypes.Columns.Add(new DataColumn("Name", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Schema", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Parent", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Description", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("SystemType", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Nullable", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Rule", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Default", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("AllowIdentity", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("MaxLength", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("VariableLength", typeof(string)));
                userDefinedDataTypes.Columns.Add(new DataColumn("Script", typeof(string)));
            }
        }
    }
}
