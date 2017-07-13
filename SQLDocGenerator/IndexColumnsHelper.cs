using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class IndexColumnsHelper
    {
        private static DataTable indexColumns = new DataTable("IndexColumns");

        public static DataTable IndexColumns { get { return indexColumns; } }

        public static void GetIndexColumns()
        {
            indexColumns = Utility.DBConnection.GetSchema("IndexColumns");
            //Utility.PrintDatatable(indexColumns);
        }
        public static void WriteIndexColumns()
        {
            Utility.WriteXML(indexColumns, indexColumns.TableName + ".xml");
            //Utility.PrintDatatable(indexColumns);
        }
    }
}
