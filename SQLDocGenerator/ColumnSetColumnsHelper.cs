using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class ColumnSetColumnsHelper
    {
        private static DataTable columnSetColumns = new DataTable("ColumnSetColumns");

        public static DataTable ColumnSetColumns { get { return columnSetColumns; } }

        public static void GetColumnSetColumns()
        {
            columnSetColumns = Utility.DBConnection.GetSchema("ColumnSetColumns");
            //Utility.PrintDatatable(columnSetColumns);
            Utility.WriteXML(columnSetColumns, columnSetColumns.TableName + ".xml");
        }
    }
}
