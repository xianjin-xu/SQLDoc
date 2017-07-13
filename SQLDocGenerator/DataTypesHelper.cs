using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class DataTypesHelper
    {
        private static DataTable dataTypes = new DataTable("DataTypes");

        public static DataTable DataTypes { get { return dataTypes; } }

        public static void GetDataTypes()
        {
            dataTypes = Utility.DBConnection.GetSchema("DataTypes");
            //Utility.PrintDatatable(dataTypes);
        }

        public static void WriteDataTypes()
        {
            string xmlfile = dataTypes.TableName + ".xml";
            string xslFile = dataTypes.TableName + ".xsl";
            string htmFile = dataTypes.TableName + ".htm";

            Utility.WriteXML(dataTypes, dataTypes.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);
        }
    }
}
