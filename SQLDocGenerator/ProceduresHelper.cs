using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;

namespace SQLDocGenerator
{
    public class ProceduresHelper
    {
        private static DataTable procedures = new DataTable("Procedures");
        private static DataTable functions = new DataTable("Functions");

        public static DataTable Procedures { get { return procedures; } }
        public static DataTable Functions { get { return functions; } }

        public static void GetProcedures()
        {
            DataTable proceduresAndFunctions = Utility.DBConnection.GetSchema("Procedures");

            procedures = proceduresAndFunctions.Clone();
            procedures.TableName = "Procedures";
            DataRow[] spRows = (proceduresAndFunctions.Select("ROUTINE_TYPE='PROCEDURE'"));
            foreach (DataRow tempDr in spRows)
            {
                procedures.ImportRow(tempDr);
            }

            functions = proceduresAndFunctions.Clone();
            functions.TableName = "Functions";
            DataRow[] fnRows = (proceduresAndFunctions.Select("ROUTINE_TYPE='FUNCTION'"));
            foreach (DataRow tempDr in fnRows)
            {
                functions.ImportRow(tempDr);
            }
        }

        public static void WriteProcedures()
        {
            WriteProcedureList();
            GetStoredProcedureScript(Utility.DatabaseName);

            WriteFunctionList();
            GetFunctionScript(Utility.DatabaseName);

            WriteProcedureDetails();
            WriteFunctionDetails();
        }

        private static void WriteProcedureList()
        {
            //Name,  Schema,  Description, Input,  Output,  Encrypted, Creation Date 
            //SPECIFIC_CATALOG,	SPECIFIC_SCHEMA, SPECIFIC_NAME, ROUTINE_CATALOG, ROUTINE_SCHEMA
            //ROUTINE_NAME, ROUTINE_TYPE, CREATED, LAST_ALTERED	

            string xmlfile = "ProcedureList.xml";
            string xslFile = "ProcedureList.xsl";
            string htmFile = "ProcedureList.htm";

            DataSet ds = new DataSet("ProceduresList");
            ds.Tables.Add(procedures);

            File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

            HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
            ht.TableTransformer();
        }
        private static void WriteFunctionList()
        {
            //Name,  Schema,  Description, Input,  Output,  Encrypted, Creation Date 
            //SPECIFIC_CATALOG,	SPECIFIC_SCHEMA, SPECIFIC_NAME, ROUTINE_CATALOG, ROUTINE_SCHEMA
            //ROUTINE_NAME, ROUTINE_TYPE, CREATED, LAST_ALTERED	

            string xmlfile = "FunctionList.xml";
            string xslFile = "FunctionList.xsl";
            string htmFile = "FunctionList.htm";

            DataSet ds = new DataSet("FunctionList");
            ds.Tables.Add(functions);

            File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

            HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
            ht.TableTransformer();
        }
        private static void WriteProcedureDetails()
        {
            string xmlfile = string.Empty;
            string xslFile = string.Empty;
            string htmFile = string.Empty;

            DataSet ds = new DataSet();

            #region "Define DataTable for ProcedureProperties"

            DataTable procedureProperties = new DataTable("ProcedureProperties");
            procedureProperties.Columns.Add("NAME", typeof(String));
            procedureProperties.Columns.Add("VALUE", typeof(String));
            ds.Tables.Add(procedureProperties);

            #endregion

            #region "Define DataTable for IN Parameters"

            DataTable tableINParameters = ProcedureParametersHelper.ProcedureParameters.Clone();
            tableINParameters.TableName = "ProcedureINParameters";
            ds.Tables.Add(tableINParameters);

            #endregion

            #region "Define DataTable for OUT Parameters"

            DataTable tableOUTParameters = ProcedureParametersHelper.ProcedureParameters.Clone();
            tableOUTParameters.TableName = "ProcedureOUTParameters";
            ds.Tables.Add(tableOUTParameters);

            #endregion

            #region "Define DataTable for SQL"

            DataTable tableSQL = new DataTable("SQL");
            tableSQL.Columns.Add("SQL", typeof(String));
            ds.Tables.Add(tableSQL);

            #endregion

            foreach (DataRow row in procedures.Rows)
            {
                string tableName = row["SPECIFIC_NAME"].ToString();
                xmlfile = tableName + ".xml";
                xslFile = "ProcedureDetails.xsl";
                htmFile = tableName + ".htm";

                #region "Fill TableProperties"
                DataRow dr;
                //DataRow dr = procedureProperties.NewRow();
                //dr["NAME"] = "Description";
                //dr["VALUE"] = row["TABLE_DESCRIPTION"].ToString();
                //procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Name";
                dr["VALUE"] = tableName;
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Schema";
                dr["VALUE"] = row["ROUTINE_SCHEMA"].ToString();
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Creation Date";
                dr["VALUE"] = row["CREATED"].ToString();
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Last Altered";
                dr["VALUE"] = row["LAST_ALTERED"].ToString();
                procedureProperties.Rows.Add(dr);

                #endregion

                #region "Fill IN Parameters"

                DataRow[] inRows = (ProcedureParametersHelper.ProcedureParameters.Select("(SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='IN') OR (SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='INOUT')"));
                foreach (DataRow tempDr in inRows)
                {
                    tableINParameters.ImportRow(tempDr);
                }

                #endregion

                #region "Fill OUT Parameters"

                DataRow[] outRows = (ProcedureParametersHelper.ProcedureParameters.Select("(SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='OUT') OR (SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='INOUT')"));
                foreach (DataRow tempDr in outRows)
                {
                    tableOUTParameters.ImportRow(tempDr);
                }

                #endregion

                #region "T-SQL"

                DataRow[] sqlRows = (procedures.Select("SPECIFIC_NAME = '" + row["SPECIFIC_NAME"] + "'"));
                foreach (DataRow tempDr in sqlRows)
                {
                    DataRow sql = tableSQL.NewRow();
                    sql["SQL"] = tempDr["STOREDPROCEDURE_SCRIPT"];
                    tableSQL.Rows.Add(sql);
                }

                #endregion

                File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
                HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                ht.TableTransformer();

                procedureProperties.Clear();
                tableINParameters.Clear();
                tableOUTParameters.Clear();
                tableSQL.Clear();
            }

        }
        private static void WriteFunctionDetails()
        {
            string xmlfile = string.Empty;
            string xslFile = string.Empty;
            string htmFile = string.Empty;

            DataSet ds = new DataSet();

            #region "Define DataTable for ProcedureProperties"

            DataTable procedureProperties = new DataTable("FunctionProperties");
            procedureProperties.Columns.Add("NAME", typeof(String));
            procedureProperties.Columns.Add("VALUE", typeof(String));
            ds.Tables.Add(procedureProperties);

            #endregion

            #region "Define DataTable for IN Parameters"

            DataTable tableINParameters = ProcedureParametersHelper.ProcedureParameters.Clone();
            tableINParameters.TableName = "FunctionINParameters";
            ds.Tables.Add(tableINParameters);

            #endregion

            #region "Define DataTable for OUT Parameters"

            DataTable tableOUTParameters = ProcedureParametersHelper.ProcedureParameters.Clone();
            tableOUTParameters.TableName = "FunctionOUTParameters";
            ds.Tables.Add(tableOUTParameters);

            #endregion

            #region "Define DataTable for SQL"

            DataTable tableSQL = new DataTable("SQL");
            tableSQL.Columns.Add("SQL", typeof(String));
            ds.Tables.Add(tableSQL);

            #endregion

            foreach (DataRow row in functions.Rows)
            {
                string tableName = row["SPECIFIC_NAME"].ToString();
                xmlfile = tableName + ".xml";
                xslFile = "FunctionDetails.xsl";
                htmFile = tableName + ".htm";

                #region "Fill TableProperties"
                DataRow dr;
                //DataRow dr = procedureProperties.NewRow();
                //dr["NAME"] = "Description";
                //dr["VALUE"] = row["TABLE_DESCRIPTION"].ToString();
                //procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Name";
                dr["VALUE"] = tableName;
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Schema";
                dr["VALUE"] = row["ROUTINE_SCHEMA"].ToString();
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Creation Date";
                dr["VALUE"] = row["CREATED"].ToString();
                procedureProperties.Rows.Add(dr);

                dr = procedureProperties.NewRow();
                dr["NAME"] = "Last Altered";
                dr["VALUE"] = row["LAST_ALTERED"].ToString();
                procedureProperties.Rows.Add(dr);

                #endregion

                #region "Fill IN Parameters"

                DataRow[] inRows = (ProcedureParametersHelper.ProcedureParameters.Select("(SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='IN') OR (SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='INOUT')"));
                foreach (DataRow tempDr in inRows)
                {
                    tableINParameters.ImportRow(tempDr);
                }

                #endregion

                #region "Fill OUT Parameters"

                DataRow[] outRows = (ProcedureParametersHelper.ProcedureParameters.Select("(SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='OUT') OR (SPECIFIC_NAME='" + row["SPECIFIC_NAME"] + "' AND PARAMETER_MODE='INOUT')"));
                foreach (DataRow tempDr in outRows)
                {
                    tableOUTParameters.ImportRow(tempDr);
                }

                #endregion

                #region "T-SQL"

                DataRow[] sqlRows = (functions.Select("SPECIFIC_NAME = '" + row["SPECIFIC_NAME"] + "'"));
                foreach (DataRow tempDr in sqlRows)
                {
                    DataRow sql = tableSQL.NewRow();
                    sql["SQL"] = tempDr["FUNCTION_SCRIPT"];
                    tableSQL.Rows.Add(sql);
                }

                #endregion

                File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
                HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                ht.TableTransformer();

                procedureProperties.Clear();
                tableINParameters.Clear();
                tableOUTParameters.Clear();
                tableSQL.Clear();
            }
        }
        private static void GetStoredProcedureScript(string database)
        {
            procedures.Columns.Add("STOREDPROCEDURE_SCRIPT", typeof(String));

            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);
            Database db = server.Databases[database];

            ScriptingOptions so = new ScriptingOptions();
            so.ChangeTracking = true;
            so.ClusteredIndexes = true;
            so.ExtendedProperties = true;

            foreach (StoredProcedure proc in db.StoredProcedures)
            {
                if (!proc.IsSystemObject)
                {
                    StringCollection script = proc.Script(so);
                    string[] scriptArray = new string[script.Count];
                    script.CopyTo(scriptArray, 0);

                    DataRow tableRow = (procedures.Select("SPECIFIC_NAME = '" + proc.Name + "'"))[0];
                    for (int i = 0; i < scriptArray.Length; i++)
                        //scriptArray[i] = Utility.SplitString(scriptArray[i], 150);
                        scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                    tableRow["STOREDPROCEDURE_SCRIPT"] = string.Join(Environment.NewLine, scriptArray);
                }
            }
        }
        private static void GetFunctionScript(string database)
        {
            functions.Columns.Add("FUNCTION_SCRIPT", typeof(String));

            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);
            Database db = server.Databases[database];

            ScriptingOptions so = new ScriptingOptions();
            so.ChangeTracking = true;
            so.ClusteredIndexes = true;
            so.ExtendedProperties = true;

            foreach (UserDefinedFunction func in db.UserDefinedFunctions)
            {
                if (!func.IsSystemObject)
                {
                    StringCollection script = func.Script(so);
                    string[] scriptArray = new string[script.Count];
                    script.CopyTo(scriptArray, 0);

                    DataRow tableRow = (functions.Select("SPECIFIC_NAME = '" + func.Name + "'"))[0];
                    for (int i = 0; i < scriptArray.Length; i++)
                        //scriptArray[i] = Utility.SplitString(scriptArray[i], 150);
                        scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                    tableRow["FUNCTION_SCRIPT"] = string.Join(Environment.NewLine, scriptArray);
                }
            }
        }

    }
}
