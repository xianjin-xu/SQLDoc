using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using System.IO;
using System.Collections;

namespace SQLDocGenerator
{
    public class ViewsHelper
    {
        private static DataTable views = new DataTable("Views");

        public static DataTable Views { get { return views; } }

        public static void GetViews()
        {
            views = Utility.DBConnection.GetSchema("Views");
            Utility.PrintDatatable(views);
            //Utility.WriteXML(views, views.TableName + ".xml");

            JoinViewDescription();
            //JoinViewColumnsDescription(); // no need, as we are using the already existing columns fecthed for tables. 
            GetViewScript(Utility.DatabaseName);

        }

        public static void WriteViews()
        {
            WriteViewList();
            WriteViewDetails();
        }

        private static void JoinViewDescription()
        {
            views.Columns.Add("TABLE_DESCRIPTION", typeof(String));

            foreach (DataRow row in views.Rows)
            {
                DataRow[] rows = (TablesHelper.TableDescription.Select("OBJNAME = '" + row["TABLE_NAME"] + "'"));

                if (rows.Length > 0)
                    row["TABLE_DESCRIPTION"] = rows[0]["VALUE"];
            }
        }

        private static void GetViewScript(string database)
        {
            views.Columns.Add("TABLE_SCRIPT", typeof(String));

            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);
            Database db = server.Databases[database];

            ScriptingOptions so = new ScriptingOptions();
            so.ChangeTracking = true;
            so.ClusteredIndexes = true;
            so.ExtendedProperties = true;

            foreach (View view in db.Views)
            {
                if (view.Owner.ToLower().Equals("dbo"))
                {
                    StringCollection script = view.Script(so);
                    string[] scriptArray = new string[script.Count];
                    script.CopyTo(scriptArray, 0);

                    DataRow tableRow = (views.Select("TABLE_NAME = '" + view.Name + "'"))[0];
                    for (int i = 0; i < scriptArray.Length; i++)
                        //scriptArray[i] = Utility.SplitString(scriptArray[i], 150);
                        scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                    tableRow["TABLE_SCRIPT"] = string.Join(Environment.NewLine, scriptArray);
                }
            }
        }
        private static void WriteViewList()
        {
            string xmlfile = "ViewList.xml";
            string xslFile = "ViewList.xsl";
            string htmFile = "ViewList.htm";

            DataSet ds = new DataSet("ViewList");
            ds.Tables.Add(views);

            File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

            HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
            ht.TableTransformer();
        }
        private static void WriteViewDetails()
        {
            string xmlfile = string.Empty;
            string xslFile = string.Empty;
            string htmFile = string.Empty;

            DataSet ds = new DataSet();

            #region "Define DataTable for ViewProperties"

            DataTable tableProperties = new DataTable("TableProperties");
            tableProperties.Columns.Add("NAME", typeof(String));
            tableProperties.Columns.Add("VALUE", typeof(String));
            ds.Tables.Add(tableProperties);

            #endregion

            #region "Define DataTable for Resultset"

            DataTable tableColumns = ColumnsHelper.Columns.Clone();
            ds.Tables.Add(tableColumns);

            #endregion

            #region "Define DataTable for Indexes"

            DataTable tableIndexes = IndexesHelper.Indexes.Clone();
            ds.Tables.Add(tableIndexes);

            #endregion

            #region "Define DataTable for SQL"

            DataTable tableSQL = new DataTable("SQL");
            tableSQL.Columns.Add("SQL", typeof(String));
            ds.Tables.Add(tableSQL);

            #endregion

            foreach (DataRow row in views.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                xmlfile = tableName + ".xml";
                xslFile = "ViewDetails.xsl";
                htmFile = tableName + ".htm";

                #region "Fill TableProperties"
                DataRow dr = tableProperties.NewRow();
                dr["NAME"] = "Description";
                dr["VALUE"] = row["TABLE_DESCRIPTION"].ToString();
                tableProperties.Rows.Add(dr);

                dr = tableProperties.NewRow();
                dr["NAME"] = "Name";
                dr["VALUE"] = tableName;
                tableProperties.Rows.Add(dr);

                dr = tableProperties.NewRow();
                dr["NAME"] = "Schema";
                dr["VALUE"] = row["TABLE_SCHEMA"].ToString();
                tableProperties.Rows.Add(dr);

                #endregion

                #region "Fill Columns"

                string cloumnTable = string.Empty;
                DataRow[] colRows = (ColumnsHelper.Columns.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
                foreach (DataRow tempDr in colRows)
                {
                    // TODO: currently, adding description to the views columns is buggy inplementation
                    DataRow[] viewRows = (ViewColumnsHelper.ViewColumns.Select("VIEW_NAME = '" + row["TABLE_NAME"] + "' AND COLUMN_NAME = '" + tempDr["COLUMN_NAME"] + "'"));
                    if (viewRows.Length > 0)
                    cloumnTable = viewRows[0]["TABLE_NAME"].ToString();
                    DataRow[] rows = (ColumnsHelper.Columns.Select("TABLE_NAME = '" + cloumnTable + "' AND COLUMN_NAME = '" + tempDr["COLUMN_NAME"] + "'"));
                    if (rows.Length > 0)
                        tempDr["COLUMN_DESCRIPTION"] = rows[0]["COLUMN_DESCRIPTION"];
                    tableColumns.ImportRow(tempDr);
                }

                #endregion

                #region "Fill Indexes"

                DataRow[] indexRows = (IndexesHelper.Indexes.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
                foreach (DataRow tempDr in indexRows)
                {
                    tableIndexes.ImportRow(tempDr);
                }

                #endregion

                #region "SQL"

                DataRow[] sqlRows = (views.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
                foreach (DataRow tempDr in sqlRows)
                {
                    DataRow sql = tableSQL.NewRow();
                    sql["SQL"] = tempDr["TABLE_SCRIPT"];
                    tableSQL.Rows.Add(sql);
                }

                #endregion

                File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
                HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
                ht.TableTransformer();

                tableProperties.Clear();
                tableColumns.Clear();
                tableIndexes.Clear();
                tableSQL.Clear();
            }
        }
    }
}