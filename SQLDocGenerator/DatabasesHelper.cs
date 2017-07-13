using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class DatabasesHelper
    {
        private static DataTable databases = new DataTable("Databases");
        public static DataTable Databases { get { return databases; } }

        public static void GetDatabases()
        {
            databases = Utility.DBConnection.GetSchema("Databases");

            string xmlfile = databases.TableName + ".xml";
            string xslFile = databases.TableName + ".xsl";
            string htmFile = databases.TableName + ".htm";

            //Utility.PrintDatatable(databases);
            Utility.WriteXML(databases, databases.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);

        }
    }
}
