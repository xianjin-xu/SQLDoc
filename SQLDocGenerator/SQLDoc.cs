using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Common;

using SQLDocGenerator.Helper.CHMWriter;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace SQLDocGenerator
{
    public partial class SQLDoc : Form
    {
        private static DataSet entireDatabase = new DataSet("EntireDatabase");
        private static SqlConnection dbConnection;
        private static List<string> serverInfoToWrite = new List<string>(new string[] { "InstanceInformation", "DataSourceInformation", "DataTypes", "Restrictions", "ReservedWords", "Users", "Databases" });
        private static List<string> objectByTypesToWrite = new List<string>(new string[] { "Tables", "Views", "Procedures", "Functions", "Triggers", "UserDefinedDataTypes" });

        public static List<string> ServerInfoToWrite { get { return serverInfoToWrite; } }
        public static List<string> ObjectByTypesToWrite { get { return objectByTypesToWrite; } }
        public static SqlConnection DBConnection { get { return dbConnection; } }

        private void randomTry()
        {
            ServerConnection serverConnection = new ServerConnection(Utility.DBConnection);
            Server server = new Server(serverConnection);

            Database northwind = server.Databases["AdventureWorks"];
            Table categories = northwind.Tables["AWBuildVersion"];
            StringCollection script = categories.Script();
            string[] scriptArray = new string[script.Count];

            script.CopyTo(scriptArray, 0);

        }
        private void GetMetaDataCollections()
        {
            DataTable dt = Utility.DBConnection.GetSchema("MetaDataCollections");
            Utility.PrintDatatable(dt);
        }

        /// <summary>
        /// Microsoft SQL Server 2008 Feature Pack, August 2008
        /// http://www.microsoft.com/downloads/details.aspx?FamilyId=C6C3E9EF-BA29-4A43-8D69-A2BED18FE73C&displaylang=en
        /// </summary>
        public SQLDoc()
        {
            InitializeComponent();

            //SqlClientMetaDataCollectionNames
            //randomTry();
            //GetMetaDataCollections();

            //Utility.OpenConnection();

            //SMOHelper.TriggersHelper.GetTriggers("AdventureWorks");
            //SMOHelper.UserDefinedDataTypesHelper.GetUserDefinedDataTypes("AdventureWorks");
            //InstanceInformationHelper.GetInstanceInformation();
            //DataSourceInformationHelper.GetDataSourceInformation();
            //DataTypesHelper.GetDataTypes();
            //RestrictionsHelper.GetRestrictions();
            //ReservedWordsHelper.GetReservedWords();
            //UsersHelper.GetUsers();
            //DatabasesHelper.GetDatabases();
            //ColumnsHelper.GetColumns();
            //ForeignKeysHelper.GetForeignKeys();
            //IndexColumnsHelper.GetIndexColumns();
            //IndexesHelper.GetIndexes();
            //TablesHelper.GetTables();
            //ViewColumnsHelper.GetViewColumns();
            //ViewsHelper.GetViews();
            //ProcedureParametersHelper.GetProcedureParameters();
            //ProceduresHelper.GetProcedures();
            //UserDefinedTypesHelper.GetUserDefinedTypes();

            ////AllColumnsHelper.GetAllColumns();
            ////ColumnSetColumnsHelper.GetColumnSetColumns();
            ////StructuredTypeMembersHelper.GetStructuredTypeMembers();
            ////JoinAllDataTables();
            ////JoinColumnsWithTables();

            //Utility.CloseConnection();

            //WriteCHMHelp();

        }

        private void WriteCHMHelp()
        {
            //HHCWriter hhc = new HHCWriter();
            //hhc.TOCFileName = "Table of Contents.hhc";
            //hhc.Write();

            //CompileCHMHelp();
        }
        private void CompileCHMHelp()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            location = location.Substring(0, location.LastIndexOf("\\"));

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;// false;  
            startInfo.FileName = @"C:\Program Files\HTML Help Workshop\hhc.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;  // ProcessWindowStyle.Normal;
            startInfo.Arguments = location + "\\AdventureWorks.hhp";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JoinColumnsWithTables()
        {
            //foreach (DataRow row in TablesHelper.Tables.Rows)
            //{
            //    string table_Schema = row["TABLE_SCHEMA"].ToString();
            //    string table_Name = row["TABLE_NAME"].ToString();

            //    DataView dv = new DataView(ColumnsHelper.Columns);
            //    dv.RowFilter = "TABLE_SCHEMA = '" + table_Schema + "' AND TABLE_NAME = '" + table_Name + "'";

            //    string xmlfile = table_Schema + "." + table_Name;
            //    string xslFile = ColumnsHelper.Columns.TableName;
            //    string htmlFile = xmlfile;

            //    Utility.WriteDatatable(dv.ToTable(), xmlfile, xslFile, htmlFile);
            //}
        }
        private void JoinAllDataTables()
        {
            //entireDatabase.Tables.Add(instanceInformation);
            //entireDatabase.Tables.Add(dataSourceInformation);
            //entireDatabase.Tables.Add(dataTypes);
            //entireDatabase.Tables.Add(restrictions);
            //entireDatabase.Tables.Add(reservedWords);
            //entireDatabase.Tables.Add(users);
            //entireDatabase.Tables.Add(databases);

            //entireDatabase.Tables.Add(tables);
            //entireDatabase.Tables.Add(columns);
            //entireDatabase.Tables.Add(allColumns);
            //entireDatabase.Tables.Add(columnSetColumns);
            //entireDatabase.Tables.Add(structuredTypeMembers);
            //entireDatabase.Tables.Add(views);
            //entireDatabase.Tables.Add(viewColumns);
            //entireDatabase.Tables.Add(procedureParameters);
            //entireDatabase.Tables.Add(procedures);
            //entireDatabase.Tables.Add(foreignKeys);
            //entireDatabase.Tables.Add(indexColumns);
            //entireDatabase.Tables.Add(indexes);
            //entireDatabase.Tables.Add(userDefinedTypes);

            //File.WriteAllText(@"xml\entireDB.xml", entireDatabase.GetXml());
        }

    }
}
