using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

using SQLDocGenerator.Helper;
using SQLDocGenerator.Properties;
using SQLDocGenerator.Helper.CHMWriter;
using SQLDocGenerator;
using System.ComponentModel;
using System.Threading;

namespace SQLDocUI
{
    public partial class SQLDocSharp : Form
    {
        private static DataSet entireDatabase = new DataSet("EntireDatabase");
        private static SqlConnection dbConnection;
        private static List<string> serverInfoToWrite = new List<string>(new string[] { "InstanceInformation", "DataSourceInformation", "DataTypes", "Restrictions", "ReservedWords", "Users", "Databases" });
        private static List<string> objectByTypesToWrite = new List<string>(new string[] { "Tables", "Views", "Procedures", "Functions", "Triggers", "UserDefinedDataTypes" });
        public static List<string> ServerInfoToWrite { get { return serverInfoToWrite; } }
        public static List<string> ObjectByTypesToWrite { get { return objectByTypesToWrite; } }
        public static SqlConnection DBConnection { get { return dbConnection; } }

        #region "Initialization"

        public SQLDocSharp()
        {
            InitializeComponent();

            //string program = GetProgramFiles();

            //SqlClientMetaDataCollectionNames
            //randomTry();
            //GetMetaDataCollections();

            ////UserDefinedTypesHelper.GetUserDefinedTypes();
            ////AllColumnsHelper.GetAllColumns();
            ////ColumnSetColumnsHelper.GetColumnSetColumns();
            ////StructuredTypeMembersHelper.GetStructuredTypeMembers();
            ////JoinAllDataTables();
            ////JoinColumnsWithTables();            

        }

        private void FilIncludeExcludeObjects()
        {
            SetStatus("Populating Tables...");
            FillTables();

            SetStatus("Populating Views...");
            FillViews();

            SetStatus("Populating Procedures...");
            FillProcedures();

            SetStatus("Populating Function...");
            FillFunction();

            SetStatus("Populating Triggers...");
            FillTriggers();

            SetStatus("Populating UDDTs...");
            FillUDDTs();
        }

        private void FillDatabases()
        {
            DatabasesHelper.GetDatabases();
            FillComboBox(ddlDatabase, DatabasesHelper.Databases, "database_name", "dbid");
        }
        private void FillTables()
        {
            FillListView(listViewTables, TablesHelper.Tables, "TABLE_NAME");
        }
        private void FillViews()
        {
            FillListView(listViewViews, ViewsHelper.Views, "TABLE_NAME");
        }
        private void FillProcedures()
        {
            FillListView(listViewProcedures, ProceduresHelper.Procedures, "SPECIFIC_NAME");
        }
        private void FillFunction()
        {
            FillListView(listViewFunctions, ProceduresHelper.Functions, "SPECIFIC_NAME");
        }
        private void FillTriggers()
        {
            FillListView(listViewTriggers, SMOHelper.TriggersHelper.Triggers, "Name");
        }
        private void FillUDDTs()
        {
            FillListView(listViewProcedures, SMOHelper.UserDefinedDataTypesHelper.UserDefinedDataTypes, "Name");
        }

        private void FillComboBox(ComboBox cb, DataTable dt, string nameField, string valueField)
        {
            cb.DataSource = dt;
            cb.ValueMember = valueField;
            cb.DisplayMember = nameField;
        }

        private void FillListView(ListView lv, DataTable dt, string name)
        {
            lv.View = View.List;

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(Convert.ToString(row[name]));
                item.Checked = true;
                lv.Items.Add(item);
            }
        }

        #endregion

        private void WriteCHMHelp()
        {
            HHCWriter hhc = new HHCWriter(Utility.DatabaseName + ".hhc", Utility.DatabaseName + ".hhp", Utility.DatabaseName + ".chm");
            hhc.Write();

            CompileCHMHelp();
        }

        private void CompileCHMHelp()
        {
            StreamReader rd = null;
            string location = Assembly.GetExecutingAssembly().Location;
            location = location.Substring(0, location.LastIndexOf("\\"));

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;// false;  
            startInfo.FileName =  GetProgramFiles() + @"\HTML Help Workshop\hhc.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;  // ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = "\"" + location + "\\" + Utility.DatabaseName + ".hhp" + "\"";
            //MessageBox.Show(startInfo.Arguments);
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                    rd = exeProcess.StandardOutput;
                    //MessageBox.Show("StandardOutput: " + rd.ReadToEnd());

                    File.Copy(location + "\\" + Utility.DatabaseName + ".chm", txtFileName.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + rd != null ? rd.ReadToEnd() : string.Empty);
            }
        }

        private string GetProgramFiles()
        {
            string programFiles = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (programFiles == null)
                programFiles = Environment.GetEnvironmentVariable("ProgramFiles");

            return programFiles;
        }

        private OSVersion GetOSVersion()
        {
            OSVersion version = OSVersion.Unknown;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                    version = OSVersion.Windows3_1;
                    break;
                case PlatformID.Win32Windows:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            version = OSVersion.Windows95;
                            break;
                        case 10:
                            if (Environment.OSVersion.Version.Revision.ToString() == "2222A")
                                version = OSVersion.Windows98SecondEdition;
                            else
                                version = OSVersion.Windows98;
                            break;
                        case 90:
                            version = OSVersion.WindowsME;
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (Environment.OSVersion.Version.Major)
                    {
                        case 3:
                            version = OSVersion.WindowsNT3_51;
                            break;
                        case 4:
                            version = OSVersion.WindowsNT4_0;
                            break;
                        case 5:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    version = OSVersion.Windows2000;
                                    break;
                                case 1:
                                    version = OSVersion.WindowsXP;
                                    break;
                                case 2:
                                    version = OSVersion.Windows2003;
                                    break;
                            }
                            break;
                        case 6:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    version = OSVersion.WindowsVista;
                                    break;
                                case 1:
                                    version = OSVersion.Windows7;
                                    break;
                            }
                            break;
                    }
                    break;
                case PlatformID.WinCE:
                    version = OSVersion.WindowsCE;
                    break;
                case PlatformID.Unix:
                    version = OSVersion.Unix;
                    break;
            }
            return version;
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

        private string BuildConnectionString(string serverName, bool isIntegratedSecurity, string userName, string password, string databaseName)
        {
            string conString;

            conString = "Data Source=";
            conString += serverName + ";";
            conString += "Initial Catalog=";
            conString += databaseName + ";";

            if (!isIntegratedSecurity)
                conString += "User Id=" + userName + ";Password=" + password + ";";
            else
                conString += "Trusted_Connection=True;";

            return conString;
        }

        private bool OpenConnection(string serverName, bool isIntegratedSecurity, string userName, string password, string databaseName)
        {
            try
            {
                dbConnection = new SqlConnection();
                dbConnection.ConnectionString = BuildConnectionString(serverName, isIntegratedSecurity, userName, password, databaseName);
                dbConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CloseConnection()
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox frm = new AboutBox();
            frm.Show();
        }

        private void chkIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = !chkIntegratedSecurity.Checked;
            txtUserPassword.Enabled = !chkIntegratedSecurity.Checked;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.OpenConnection(txtSQLServer.Text, chkIntegratedSecurity.Checked, txtUserName.Text, txtUserPassword.Text, "Master"))
            {
                btnConnect.Text = "Connected";
                btnConnect.ForeColor = Color.Green;
                Utility.DBConnection = dbConnection;
                EnableGenerateUI(true);
                FillDatabases();
                lblMessage.Text = "Select the database from list, and click Fetch button.";
            }
            else
            {
                btnConnect.Text = "Failed";
                btnConnect.ForeColor = Color.Red;
                EnableGenerateUI(false);
            }
            this.CloseConnection();
        }

        public void EnableGenerateUI(bool state)
        {
            ddlDatabase.Enabled = state;
            txtFileName.Enabled = state;
            ddlFormat.Enabled = state;
            txtFileName.Enabled = state;
            btnFileName.Enabled = state;
            chkExportMetadata.Enabled = state;
            btnFetch.Enabled = state;
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            btnFetch.Enabled = false;
            txtFileName.Text = "C:\\" + ddlDatabase.Text + ".chm";
            ddlFormat.SelectedIndex = 0;

            this.OpenConnection(txtSQLServer.Text, chkIntegratedSecurity.Checked, txtUserName.Text, txtUserPassword.Text, ddlDatabase.Text);
            Utility.DBConnection = DBConnection;
            Utility.DatabaseServer = txtSQLServer.Text;
            Utility.DatabaseName = ddlDatabase.Text;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SetStatus("Fetching Metadata...");
            GetMetaData();
        }

        private void GetMetaData()
        {
            SetStatus("Fetching Columns...");
            ColumnsHelper.GetColumns();
            backgroundWorker1.ReportProgress(5, ObjectTypes.Columns);

            SetStatus("Fetching ForeignKeys...");
            ForeignKeysHelper.GetForeignKeys();
            backgroundWorker1.ReportProgress(10, ObjectTypes.ForeignKeys);

            SetStatus("Fetching IndexColumns...");
            IndexColumnsHelper.GetIndexColumns();
            backgroundWorker1.ReportProgress(15, ObjectTypes.IndexColumns);

            SetStatus("Fetching Indexes...");
            IndexesHelper.GetIndexes();
            backgroundWorker1.ReportProgress(20, ObjectTypes.Indexes);

            SetStatus("Fetching Tables...");
            TablesHelper.GetTables();
            backgroundWorker1.ReportProgress(25, ObjectTypes.Tables);

            SetStatus("Fetching ViewColumns...");
            ViewColumnsHelper.GetViewColumns();
            backgroundWorker1.ReportProgress(30, ObjectTypes.ViewColumns);

            SetStatus("Fetching Views...");
            ViewsHelper.GetViews();
            backgroundWorker1.ReportProgress(35, ObjectTypes.Views);

            SetStatus("Fetching ProcedureParameters...");
            ProcedureParametersHelper.GetProcedureParameters();
            backgroundWorker1.ReportProgress(40, ObjectTypes.ProcedureParameters);

            SetStatus("Fetching Procedures...");
            ProceduresHelper.GetProcedures();
            backgroundWorker1.ReportProgress(45, ObjectTypes.Procedures);

            SetStatus("Fetching Triggers...");
            SMOHelper.TriggersHelper.GetTriggers(Utility.DatabaseName);
            backgroundWorker1.ReportProgress(50, ObjectTypes.Triggers);

            SetStatus("Fetching UserDefinedDataTypes...");
            SMOHelper.UserDefinedDataTypesHelper.GetUserDefinedDataTypes(Utility.DatabaseName);
            backgroundWorker1.ReportProgress(55, ObjectTypes.UserDefinedDataTypes);

            SetStatus("Fetching InstanceInformation...");
            InstanceInformationHelper.GetInstanceInformation();
            backgroundWorker1.ReportProgress(60, ObjectTypes.InstanceInformation);

            SetStatus("Fetching DataSourceInformation...");
            DataSourceInformationHelper.GetDataSourceInformation();
            backgroundWorker1.ReportProgress(65, ObjectTypes.DataSourceInformation);

            SetStatus("Fetching DataTypes...");
            DataTypesHelper.GetDataTypes();
            backgroundWorker1.ReportProgress(70, ObjectTypes.DataTypes);

            SetStatus("Fetching Restrictions...");
            RestrictionsHelper.GetRestrictions();
            backgroundWorker1.ReportProgress(75, ObjectTypes.Restrictions);

            SetStatus("Fetching ReservedWords...");
            ReservedWordsHelper.GetReservedWords();
            backgroundWorker1.ReportProgress(80, ObjectTypes.ReservedWords);

            SetStatus("Fetching Users...");
            UsersHelper.GetUsers();
            backgroundWorker1.ReportProgress(85, ObjectTypes.Users);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ObjectTypes objectTypes = (ObjectTypes)e.UserState;
            switch (objectTypes)
            {
                case ObjectTypes.Columns:
                    SetStatus("Columns Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.DataSourceInformation:
                    SetStatus("DataSourceInformation Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.DataTypes:
                    SetStatus("DataTypes Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ForeignKeys:
                    SetStatus("ForeignKeys Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.IndexColumns:
                    SetStatus("IndexColumns Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Indexes:
                    SetStatus("Indexes Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.InstanceInformation:
                    SetStatus("InstanceInformation Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ProcedureParameters:
                    SetStatus("ProcedureParameters Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Procedures:
                    SetStatus("Procedures Fetched...", e.ProgressPercentage);
                    FillProcedures();
                    FillFunction();
                    break;
                case ObjectTypes.ReservedWords:
                    SetStatus("ReservedWords Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Restrictions:
                    SetStatus("Restrictions Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Tables:
                    SetStatus("Tables Fetched...", e.ProgressPercentage);
                    FillTables();
                    break;
                case ObjectTypes.Triggers:
                    SetStatus("Triggers Fetched...", e.ProgressPercentage);
                    FillTriggers();
                    break;
                case ObjectTypes.UserDefinedDataTypes:
                    SetStatus("UserDefinedDataTypes Fetched...", e.ProgressPercentage);
                    FillUDDTs();
                    break;
                case ObjectTypes.Users:
                    SetStatus("Users Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ViewColumns:
                    SetStatus("ViewColumns Fetched...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Views:
                    FillViews();
                    break;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnFetch.Enabled = true;
            btnGenerate.Enabled = true;
            lblMessage.Text = "Provide Documentation details, and click Generate button.";

            SetStatus("Idle...", 100);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            backgroundWorker2.RunWorkerAsync();

            if (chkExportMetadata.Checked)
                WriteMetaDataInXML();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            SetStatus("Writing Metadata...");
            WriteMetaDataInCHM();
        }

        private void WriteMetaDataInCHM()
        {
            SetStatus("Writing Columns...");
            ColumnsHelper.WriteColumns();
            backgroundWorker2.ReportProgress(5, ObjectTypes.Columns);

            SetStatus("Writing ForeignKeys...");
            ForeignKeysHelper.WriteForeignKeys();
            backgroundWorker2.ReportProgress(10, ObjectTypes.ForeignKeys);

            SetStatus("Writing IndexColumns...");
            IndexColumnsHelper.WriteIndexColumns();
            backgroundWorker2.ReportProgress(15, ObjectTypes.IndexColumns);

            SetStatus("Writing Indexes...");
            IndexesHelper.WriteIndexes();
            backgroundWorker2.ReportProgress(20, ObjectTypes.Indexes);

            SetStatus("Writing Tables...");
            TablesHelper.WriteTables();
            backgroundWorker2.ReportProgress(25, ObjectTypes.Tables);

            SetStatus("Writing ViewColumns...");
            ViewColumnsHelper.WriteViewColumns();
            backgroundWorker2.ReportProgress(30, ObjectTypes.ViewColumns);

            SetStatus("Writing Views...");
            ViewsHelper.WriteViews();
            backgroundWorker2.ReportProgress(35, ObjectTypes.Views);

            SetStatus("Writing ProcedureParameters...");
            ProcedureParametersHelper.WriteProcedureParameters();
            backgroundWorker2.ReportProgress(40, ObjectTypes.ProcedureParameters);

            SetStatus("Writing Procedures...");
            ProceduresHelper.WriteProcedures();
            backgroundWorker2.ReportProgress(45, ObjectTypes.Procedures);

            SetStatus("Writing Triggers...");
            SMOHelper.TriggersHelper.WriteTriggers();
            backgroundWorker2.ReportProgress(50, ObjectTypes.Triggers);

            SetStatus("Writing UserDefinedDataTypes...");
            SMOHelper.UserDefinedDataTypesHelper.WriteUserDefinedDataTypes();
            backgroundWorker2.ReportProgress(55, ObjectTypes.UserDefinedDataTypes);

            SetStatus("Writing InstanceInformation...");
            InstanceInformationHelper.WriteInstanceInformation();
            backgroundWorker2.ReportProgress(60, ObjectTypes.InstanceInformation);

            SetStatus("Writing DataSourceInformation...");
            DataSourceInformationHelper.WriteDataSourceInformation();
            backgroundWorker2.ReportProgress(65, ObjectTypes.DataSourceInformation);

            SetStatus("Writing DataTypes...");
            DataTypesHelper.WriteDataTypes();
            backgroundWorker2.ReportProgress(70, ObjectTypes.DataTypes);

            SetStatus("Writing Restrictions...");
            RestrictionsHelper.WriteRestrictions();
            backgroundWorker2.ReportProgress(75, ObjectTypes.Restrictions);

            SetStatus("Writing ReservedWords...");
            ReservedWordsHelper.WriteReservedWords();
            backgroundWorker2.ReportProgress(80, ObjectTypes.ReservedWords);

            SetStatus("Writing Users...");
            UsersHelper.WriteUsers();
            backgroundWorker2.ReportProgress(85, ObjectTypes.Users);
        }

        private void WriteMetaDataInXML()
        {
            entireDatabase.Tables.Add(InstanceInformationHelper.InstanceInformation.Copy());
            entireDatabase.Tables.Add(DataSourceInformationHelper.DataSourceInformation.Copy());
            entireDatabase.Tables.Add(DataTypesHelper.DataTypes.Copy());
            entireDatabase.Tables.Add(RestrictionsHelper.Restrictions.Copy());
            entireDatabase.Tables.Add(ReservedWordsHelper.ReservedWords.Copy());
            entireDatabase.Tables.Add(UsersHelper.Users.Copy());
            entireDatabase.Tables.Add(DatabasesHelper.Databases.Copy());

            entireDatabase.Tables.Add(TablesHelper.Tables.Copy());
            entireDatabase.Tables.Add(ColumnsHelper.Columns.Copy());
            entireDatabase.Tables.Add(AllColumnsHelper.AllColumns.Copy());
            entireDatabase.Tables.Add(ColumnSetColumnsHelper.ColumnSetColumns.Copy());
            entireDatabase.Tables.Add(StructuredTypeMembersHelper.StructuredTypeMembers.Copy());
            entireDatabase.Tables.Add(ViewsHelper.Views.Copy());
            entireDatabase.Tables.Add(ViewColumnsHelper.ViewColumns.Copy());
            entireDatabase.Tables.Add(ProcedureParametersHelper.ProcedureParameters.Copy());
            entireDatabase.Tables.Add(ProceduresHelper.Procedures.Copy());
            entireDatabase.Tables.Add(ProceduresHelper.Functions.Copy());
            entireDatabase.Tables.Add(ForeignKeysHelper.ForeignKeys.Copy());
            entireDatabase.Tables.Add(IndexColumnsHelper.IndexColumns.Copy());
            entireDatabase.Tables.Add(IndexesHelper.Indexes.Copy());
            entireDatabase.Tables.Add(UserDefinedTypesHelper.UserDefinedTypes.Copy());
            entireDatabase.Tables.Add(SMOHelper.TriggersHelper.Triggers.Copy());
            entireDatabase.Tables.Add(SMOHelper.UserDefinedDataTypesHelper.UserDefinedDataTypes.Copy());

            File.WriteAllText(txtExportXMLFileName.Text, entireDatabase.GetXml());
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ObjectTypes objectTypes = (ObjectTypes)e.UserState;
            switch (objectTypes)
            {
                case ObjectTypes.Columns:
                    SetStatus("Columns Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.DataSourceInformation:
                    SetStatus("DataSourceInformation Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.DataTypes:
                    SetStatus("DataTypes Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ForeignKeys:
                    SetStatus("ForeignKeys Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.IndexColumns:
                    SetStatus("IndexColumns Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Indexes:
                    SetStatus("Indexes Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.InstanceInformation:
                    SetStatus("InstanceInformation Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ProcedureParameters:
                    SetStatus("ProcedureParameters Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Procedures:
                    SetStatus("Procedures Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ReservedWords:
                    SetStatus("ReservedWords Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Restrictions:
                    SetStatus("Restrictions Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Tables:
                    SetStatus("Tables Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Triggers:
                    SetStatus("Triggers Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.UserDefinedDataTypes:
                    SetStatus("UserDefinedDataTypes Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Users:
                    SetStatus("Users Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.ViewColumns:
                    SetStatus("ViewColumns Written...", e.ProgressPercentage);
                    break;
                case ObjectTypes.Views:
                    break;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnGenerate.Enabled = true;
            SetStatus("Idle...", 90);

            WriteCHMHelp();
            lblMessage.Text = "Documentation is generated, please refer to CHM file.";

            SetStatus("Idle...", 100);
        }

        delegate void aDelegate(string t);

        private void SetStatus(string message)
        {
            this.Invoke(new aDelegate(SetToolStripText), message);
        }

        private void SetStatus(string message, int percentage)
        {
            this.Invoke(new aDelegate(SetToolStripText), message);
            pbCopy.Value = percentage;
        }

        private void chkExportMetadata_CheckedChanged(object sender, EventArgs e)
        {
            txtExportXMLFileName.Enabled = chkExportMetadata.Checked;
            txtExportXMLFileName.Text = "C:\\" + ddlDatabase.Text + ".xml";
            btnExportXMLFileName.Enabled = chkExportMetadata.Checked;
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CloseConnection();
            this.Close();
        }

        private void SetToolStripText(string text)
        {
            tsStatus.Text = text;
        }

        #region Radio button handling for Check/Uncheck/Toggle

        private void AdjustListViewCheckboxes(ListView lv, bool state, bool toggle)
        {
            foreach (ListViewItem item in lv.Items)
            {
                if (!toggle)
                    item.Checked = state;
                else
                    item.Checked = !item.Checked;
            }
        }

        private void radCheckTables_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTables, radCheckTables.Checked, false);
        }
        private void radUncheckTables_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTables, !radUncheckTables.Checked, false);
        }
        private void radToggleTables_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTables, radToggleTables.Checked, true);
        }

        private void radCheckViews_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewViews, radCheckViews.Checked, false);

        }
        private void radUncheckViews_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewViews, !radUncheckViews.Checked, false);

        }
        private void radToggleViews_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewViews, radToggleViews.Checked, true);

        }

        private void radCheckProcedured_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewProcedures, radCheckProcedured.Checked, false);

        }
        private void radUncheckProcedures_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewProcedures, !radUncheckProcedures.Checked, false);

        }
        private void radToggleProcedures_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewProcedures, radToggleProcedures.Checked, true);

        }

        private void radCheckFunctions_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewFunctions, radCheckFunctions.Checked, false);

        }
        private void radUncheckFunctions_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewFunctions, !radUncheckFunctions.Checked, false);

        }
        private void radToggleFunctions_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewFunctions, radToggleFunctions.Checked, true);

        }

        private void radCheckTriggers_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTriggers, radCheckTriggers.Checked, false);
        }
        private void radUvcheckTriggers_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTriggers, !radUncheckTriggers.Checked, false);

        }
        private void radToggleTriggers_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listViewTriggers, radToggleTriggers.Checked, true);

        }

        private void radCheckUDDTs_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listviewUDDT, radCheckUDDTs.Checked, false);
        }
        private void radUncheckUDDTs_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listviewUDDT, !radUncheckUDDTs.Checked, false);
        }
        private void radToggleUDDTs_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)(sender)).Checked)
                AdjustListViewCheckboxes(listviewUDDT, radToggleUDDTs.Checked, true);
        }

        #endregion

        private void btnFileName_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Compiled HTML Help|*.CHM";
            saveFileDialog1.Title = "Save an CHM File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = saveFileDialog1.FileName;
            }
        }

        private void btnExportXMLFileName_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "Extensible Markup Language|*.XML";
            saveFileDialog2.Title = "Save an XML File";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtExportXMLFileName.Text = saveFileDialog2.FileName;
            }
        }
    }
}


