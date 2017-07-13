using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class DataSourceInformationHelper
    {
        private static DataTable dataSourceInformation = new DataTable("DataSourceInformation");

        public static DataTable DataSourceInformation { get { return dataSourceInformation; } }

        public static void GetDataSourceInformation()
        {
            dataSourceInformation = Utility.DBConnection.GetSchema("DataSourceInformation");
            //Utility.PrintDatatable(dataSourceInformation);
        }

        public static void WriteDataSourceInformation()
        {
            string xmlfile = dataSourceInformation.TableName + ".xml";
            string xslFile = dataSourceInformation.TableName + ".xsl";
            string htmFile = dataSourceInformation.TableName + ".htm";

            Utility.WriteXML(dataSourceInformation, dataSourceInformation.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);
        }
    }
}
