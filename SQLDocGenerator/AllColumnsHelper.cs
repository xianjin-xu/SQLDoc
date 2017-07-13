using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class AllColumnsHelper
    {
        private static DataTable allColumns = new DataTable("AllColumns");

        public static DataTable AllColumns { get { return allColumns; } }

        public static void GetAllColumns()
        {
            allColumns = Utility.DBConnection.GetSchema("AllColumns");
            //Utility.PrintDatatable(allColumns);
            Utility.WriteXML(allColumns, allColumns.TableName + ".xml");
        }
    }
}
