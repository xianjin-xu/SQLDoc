using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class ForeignKeysHelper
    {
        private static DataTable foreignKeys = new DataTable("ForeignKeys");

        public static DataTable ForeignKeys { get { return foreignKeys; } }

        public static void GetForeignKeys()
        {
            foreignKeys = Utility.DBConnection.GetSchema("ForeignKeys");
            //Utility.PrintDatatable(foreignKeys);
        }
        public static void WriteForeignKeys()
        {
            Utility.WriteXML(foreignKeys, foreignKeys.TableName + ".xml");
            //Utility.PrintDatatable(foreignKeys);
        }
    }
}
