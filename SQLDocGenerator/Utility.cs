using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Collections;

namespace SQLDocGenerator
{
    public class Utility
    {
        private static SqlConnection dbConnection;
        private static string database;
        private static string databaseServer;

        public static SqlConnection DBConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; }
        }
        public static string DatabaseServer
        {
            get { return databaseServer; }
            set { databaseServer = value; }
        }
        public static string DatabaseName 
        {
            get { return database; }
            set { database = value; }
        }

        public static void PrintDatatable(DataTable dt)
        {
            TextWriter tw = new StreamWriter("datatable.txt");

            foreach (DataColumn column in dt.Columns)
                tw.Write(column.ToString() + "\t");

            tw.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    tw.Write(row[column] + "\t");
                }
                tw.WriteLine();
            }
            tw.Close();
        }
        public static void WriteXML(DataTable dt, string xmlfile)
        {
            DataSet ds = new DataSet(dt.TableName);
            ds.Tables.Add(dt);
            File.WriteAllText(@"xml\" + xmlfile, ds.GetXml());
        }
        public static void WriteHTML(string xmlfile, string xslFile, string htmlFile)
        {
            HTMLTransfomer ht = new HTMLTransfomer(xmlfile, xslFile, htmlFile);
            ht.TableTransformer();
        }
        public static string SplitString(string input, int splitLength)
        {
            int fullLength = input.Length;
            string piece = string.Empty;
            ArrayList lineArray = new ArrayList();
            while (fullLength > splitLength)
            {
                piece = input.Substring(0, splitLength);
                input = input.Substring(splitLength);
                lineArray.Add(piece);
                fullLength = input.Length;
            }
            lineArray.Add(input);
            return string.Join(Environment.NewLine, (string[])lineArray.ToArray(typeof(string)));
        }

        //public static void OpenConnection()
        //{
        //    string cnnString = ConfigurationManager.AppSettings.Get("ConnectionString");
        //    dbConnection = new SqlConnection(cnnString);
        //    dbConnection.Open();
        //}
        //public static void CloseConnection()
        //{
        //    if (dbConnection.State == ConnectionState.Open)
        //        dbConnection.Close();
        //}
    }
}
