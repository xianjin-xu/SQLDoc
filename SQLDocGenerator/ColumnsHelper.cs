using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class ColumnsHelper
    {
        private static DataTable columns = new DataTable("Columns");

        public static DataTable Columns { get { return columns; } }

        public static void GetColumns()
        {
            columns = Utility.DBConnection.GetSchema("Columns");
            IndexColumnsHelper.GetIndexColumns();
            //Utility.PrintDatatable(columns);
        }
        public static void WriteColumns()
        {
            Utility.WriteXML(columns, columns.TableName + ".xml");
            //Utility.PrintDatatable(columns);
        }
    }
}
