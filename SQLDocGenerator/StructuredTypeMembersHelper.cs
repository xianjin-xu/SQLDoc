using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class StructuredTypeMembersHelper
    {
        private static DataTable structuredTypeMembers = new DataTable("StructuredTypeMembers");

        public static DataTable StructuredTypeMembers { get { return structuredTypeMembers; } }

        public static void GetStructuredTypeMembers()
        {
            structuredTypeMembers = Utility.DBConnection.GetSchema("StructuredTypeMembers");
            Utility.PrintDatatable(structuredTypeMembers);
            Utility.WriteXML(structuredTypeMembers, structuredTypeMembers.TableName + ".xml");
        }
    }
}
