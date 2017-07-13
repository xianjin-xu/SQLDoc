using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class ViewColumnsHelper
    {
        private static DataTable viewColumns = new DataTable("ViewColumns");

        public static DataTable ViewColumns { get { return viewColumns; } }

        public static void GetViewColumns()
        {
            viewColumns = Utility.DBConnection.GetSchema("ViewColumns");
            //Utility.PrintDatatable(viewColumns);
        }
        public static void WriteViewColumns()
        {
            Utility.WriteXML(viewColumns, viewColumns.TableName + ".xml");
            //Utility.PrintDatatable(viewColumns);
        }
    }
}
