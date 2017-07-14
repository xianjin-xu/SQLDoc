using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using System.IO;

namespace SQLDocGenerator
{
    public class TablesHelper
    {
        private static DataTable schemas = new DataTable("Schema");
        private static DataTable tables = new DataTable("Tables");
        private static DataTable tableDescription = new DataTable("TableDescription");
        private static DataTable columnsDescription = new DataTable("ColumnsDescription");
        private static DataTable tablePKs = new DataTable("TablePKs");

        public static DataTable Tables { get { return tables; } }
        public static DataTable TableDescription { get { return tableDescription; } }
        public static DataTable ColumnsDescription { get { return columnsDescription; } }

        public static void GetTables()
        {
            string[] restrictions = new string[4];
            restrictions[0] = null;             //database/catalog name
            restrictions[1] = null;             //owner/schema name
            restrictions[2] = null;             //table name
            // commented as we need both tables + views
            //restrictions[3] = "BASE TABLE";   //table type 

            tables = Utility.DBConnection.GetSchema("Tables", restrictions);
            //Utility.PrintDatatable(tables);

            GetSchemas();
            GetTableDescription();
            GetAllTablePrimaryKeys();
            GetColumnsDescription();
            GetTablesSpaceUsed();
            GetTableColumnCount();
            GetTablesScript(Utility.DatabaseName);

        }

        public static void WriteTables()
        {
            WriteTableList();
            WriteTableDetails();
        }

        private static void GetSchemas()
        {
            String query = "SELECT CATALOG_NAME, SCHEMA_NAME, SCHEMA_OWNER, DEFAULT_CHARACTER_SET_CATALOG, " +
                            "DEFAULT_CHARACTER_SET_SCHEMA, DEFAULT_CHARACTER_SET_NAME " +
                            "FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_OWNER='dbo'";

            SqlCommand cmd = new SqlCommand(query, Utility.DBConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(query, Utility.DBConnection);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "schemas");
            schemas = ds.Tables["schemas"];

        }
        private static void GetTableDescription()
        {
            string query = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet("tableDescription");

            foreach (DataRow row in schemas.Rows)
            {
                query = "SELECT OBJTYPE, OBJNAME, NAME, VALUE " +
                        "FROM fn_listextendedproperty ('MS_Description', 'schema', '" + row["SCHEMA_NAME"].ToString() + "', 'table', default, null, null) " +
                        "UNION " +
                        "SELECT OBJTYPE, OBJNAME, NAME, VALUE " +
                        "FROM fn_listextendedproperty ('MS_Description', 'schema', '" + row["SCHEMA_NAME"].ToString() + "', 'view', default, null, null); ";

                cmd = new SqlCommand(query, Utility.DBConnection);
                adapter = new SqlDataAdapter(query, Utility.DBConnection);
                adapter.Fill(ds, "tableDescription");
            }
            tableDescription = ds.Tables["tableDescription"];
            JoinTableDescription();
        }
        private static void JoinTableDescription()
        {
            tables.Columns.Add("TABLE_DESCRIPTION", typeof(String));

            foreach (DataRow row in tables.Rows)
            {
                DataRow[] rows = (tableDescription.Select("OBJNAME = '" + row["TABLE_NAME"] + "'"));

                if (rows.Length > 0)
                    row["TABLE_DESCRIPTION"] = rows[0]["VALUE"];
            }
        }

        private static void GetAllTablePrimaryKeys()
        {
            string query = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet("TablePKs");

            query =@"SELECT d.id as objId, d.name as Table_Name, a.colid, a.name as PKName
              FROM syscolumns a inner join sysobjects d on a.id = d.id
              where exists(SELECT 1 FROM sysobjects where xtype = 'PK' and parent_obj = a.id and name in (
                SELECT name FROM sysindexes WHERE indid in(
             SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid
              )))";

            cmd = new SqlCommand(query, Utility.DBConnection);
            adapter = new SqlDataAdapter(query, Utility.DBConnection);
            adapter.Fill(ds, "TablePKs");
            tablePKs = ds.Tables["TablePKs"];
        }

        private static void GetColumnsDescription()
        {
            string query = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet("columnsDescription");

            foreach (DataRow schemaRow in schemas.Rows)
            {
                foreach (DataRow tableRow in tables.Rows)
                {
                    query = "SELECT OBJTYPE, OBJNAME, NAME, VALUE, '" + tableRow["TABLE_NAME"].ToString() + "' AS TABLE_NAME " +
                            "FROM fn_listextendedproperty ('MS_Description', 'schema', '" + schemaRow["SCHEMA_NAME"].ToString() + "', 'table', '" + tableRow["TABLE_NAME"].ToString() + "', 'column', default);";

                    cmd = new SqlCommand(query, Utility.DBConnection);
                    adapter = new SqlDataAdapter(query, Utility.DBConnection);
                    adapter.Fill(ds, "columnsDescription");
                }
            }
            columnsDescription = ds.Tables["columnsDescription"];
            JoinColumnsDescription();
        }
        private static void JoinColumnsDescription()
        {
            ColumnsHelper.Columns.Columns.Add("COLUMN_DESCRIPTION", typeof(String));
            var pkcolumn = ColumnsHelper.Columns.Columns.Add("ISPK", typeof(bool));
            Utility.PrintDatatable(columnsDescription);

            foreach (DataRow row in ColumnsHelper.Columns.Rows)
            {
                DataRow[] rows = (columnsDescription.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "' AND OBJNAME = '" + row["COLUMN_NAME"] + "'"));
                if (rows.Length > 0)
                    row["COLUMN_DESCRIPTION"] = rows[0]["VALUE"];

                rows = tablePKs.Select("Table_Name = '" + row["TABLE_NAME"] + "' AND PKName = '"+ row["COLUMN_NAME"] + "'");
                if (rows.Length > 0)
                {
                    row["ISPK"] = true;
                }
            }
        }
        private static void GetTablesSpaceUsed()
        {
            string query = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter adapter;
            DataSet ds = new DataSet("SpaceUsed");
            DataTable spaceUsed;

            foreach (DataRow row in tables.Rows)
            {
                query = "sp_spaceused N'" + row["TABLE_SCHEMA"].ToString() + "." + row["TABLE_NAME"] + "'";

                cmd = new SqlCommand(query, Utility.DBConnection);
                adapter = new SqlDataAdapter(query, Utility.DBConnection);
                adapter.Fill(ds, "SpaceUsed");
            }

            spaceUsed = ds.Tables["SpaceUsed"];
            JoinTableSpaceUsed(spaceUsed);
        }
        private static void JoinTableSpaceUsed(DataTable dt)
        {
            tables.Columns.Add("TABLE_ROWS", typeof(String));
            tables.Columns.Add("TABLE_RESERVED", typeof(String));
            tables.Columns.Add("TABLE_DATA", typeof(String));
            tables.Columns.Add("TABLE_INDEX_SIZE", typeof(String));
            tables.Columns.Add("TABLE_UNUSED", typeof(String));

            foreach (DataRow row in tables.Rows)
            {
                DataRow dr = (dt.Select("name = '" + row["TABLE_NAME"] + "'"))[0];
                row["TABLE_ROWS"] = dr["rows"].ToString();
                row["TABLE_RESERVED"] = dr["reserved"].ToString();
                row["TABLE_DATA"] = dr["data"].ToString();
                row["TABLE_INDEX_SIZE"] = dr["index_size"].ToString();
                row["TABLE_UNUSED"] = dr["unused"].ToString();
            }
        }
        private static void GetTableColumnCount()
        {
            Hashtable ht = new Hashtable(20);
            Int32 columnCount = 0;
            foreach (DataRow row in tables.Rows)
            {
                columnCount = (ColumnsHelper.Columns.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'")).Length;
                ht.Add(row["TABLE_NAME"].ToString(), columnCount);
            }
            JoinTableColumnCount(ht);
        }
        private static void JoinTableColumnCount(Hashtable ht)
        {
            tables.Columns.Add("TABLE_COLUMNSCOUNT", typeof(String));

            foreach (DataRow row in tables.Rows)
            {
                row["TABLE_COLUMNSCOUNT"] = ht[row["TABLE_NAME"].ToString()];
            }
        }
        private static void GetTablesScript(string database)
        {
            tables.Columns.Add("TABLE_SCRIPT", typeof(String));

            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);
            Database db = server.Databases[database];

            ScriptingOptions so = new ScriptingOptions();
            so.ChangeTracking = true;
            so.ClusteredIndexes = true;
            so.ExtendedProperties = true;

            foreach (Table table in db.Tables)
            {
                StringCollection script = table.Script(so);
                string[] scriptArray = new string[script.Count];
                script.CopyTo(scriptArray, 0);

                DataRow tableRow = (tables.Select("TABLE_NAME = '" + table.Name + "'"))[0];
                for (int i = 0; i < scriptArray.Length; i++)
                    //scriptArray[i] = (scriptArray[i].Length > 150) ? Utility.SplitString(scriptArray[i], 150) :scriptArray[i];
                    scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                tableRow["TABLE_SCRIPT"] = string.Join(Environment.NewLine, scriptArray);
            }
        }
        private static void WriteTableList()
        {
            string xmlfile = "TableList.xml";
            string xslFile = "TableList.xsl";
            string htmFile = "TableList.htm";

            DataSet ds = new DataSet("TableList");
            DataView dv = new DataView(tables);
            dv.RowFilter = "TABLE_TYPE = 'BASE TABLE'";
            ds.Tables.Add(dv.ToTable());

            File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());

            HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmFile);
            ht.TableTransformer();
        }
        private static void WriteTableDetails()
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

            #region "Define DataTable for Columns"

            DataTable tableColumns = ColumnsHelper.Columns.Clone();
            ds.Tables.Add(tableColumns);

            #endregion

            #region "Define DataTable for Indexes"

            DataTable tableIndexes = IndexesHelper.Indexes.Clone();
            ds.Tables.Add(tableIndexes);

            #endregion

            #region "Define DataTable for Indexes"

            DataTable foreignKeys = ForeignKeysHelper.ForeignKeys.Clone();
            ds.Tables.Add(foreignKeys);

            #endregion

            #region "Define DataTable for SQL"

            DataTable tableSQL = new DataTable("SQL");
            tableSQL.Columns.Add("SQL", typeof(String));
            ds.Tables.Add(tableSQL);

            #endregion

            foreach (DataRow row in tables.Rows)
            {
                if (row["TABLE_TYPE"].ToString().Equals("BASE TABLE"))
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    xmlfile = tableName + ".xml";
                    xslFile = "TableDetails.xsl";
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

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Data Size";
                    dr["VALUE"] = row["TABLE_DATA"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Index Size";
                    dr["VALUE"] = row["TABLE_INDEX_SIZE"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Rows";
                    dr["VALUE"] = row["TABLE_ROWS"].ToString();
                    tableProperties.Rows.Add(dr);

                    dr = tableProperties.NewRow();
                    dr["NAME"] = "Columns";
                    dr["VALUE"] = row["TABLE_COLUMNSCOUNT"].ToString();
                    tableProperties.Rows.Add(dr);

                    #endregion

                    #region "Fill Columns"

                    DataRow[] colRows = (ColumnsHelper.Columns.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
                    foreach (DataRow tempDr in colRows)
                    {
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

                    #region "Fill Indexes"

                    DataRow[] foreignKeyRows = (ForeignKeysHelper.ForeignKeys.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
                    foreach (DataRow tempDr in foreignKeyRows)
                    {
                        foreignKeys.ImportRow(tempDr);
                    }

                    #endregion

                    #region "SQL"

                    DataRow[] sqlRows = (tables.Select("TABLE_NAME = '" + row["TABLE_NAME"] + "'"));
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
                    foreignKeys.Clear();
                    tableSQL.Clear();
                }
            }
        }

        private static void GetSMOMetaData(string database)
        {
            //tables.Columns.Add("TABLE_SCRIPT", typeof(String));

            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);
            Database db = server.Databases[database];

            ScriptingOptions so = new ScriptingOptions();
            so.ChangeTracking = true;
            so.ClusteredIndexes = true;
            so.ExtendedProperties = true;

            foreach (Table table in db.Tables)
            {
                StringCollection script = table.Script(so);
                string[] scriptArray = new string[script.Count];
                script.CopyTo(scriptArray, 0);

                DataRow tableRow = (tables.Select("TABLE_NAME = '" + table.Name + "'"))[0];
                for (int i = 0; i < scriptArray.Length; i++)
                    //scriptArray[i] = (scriptArray[i].Length > 150) ? Utility.SplitString(scriptArray[i], 150) :scriptArray[i];
                    scriptArray[i] = scriptArray[i].Replace("@level0type", "\n\t@level0type");

                tableRow["TABLE_SCRIPT"] = string.Join(Environment.NewLine, scriptArray);
            }
        }
    }
}
