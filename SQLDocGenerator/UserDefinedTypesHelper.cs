using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class UserDefinedTypesHelper
    {
        private static DataTable userDefinedTypes = new DataTable("UserDefinedTypes");

        public static DataTable UserDefinedTypes { get { return userDefinedTypes; } }

        public static void GetUserDefinedTypes()
        {
            userDefinedTypes = Utility.DBConnection.GetSchema("UserDefinedTypes");
            Utility.PrintDatatable(userDefinedTypes);
            Utility.WriteXML(userDefinedTypes, userDefinedTypes.TableName + ".xml");
        }
    }
}
