using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class UsersHelper
    {
        private static DataTable users = new DataTable("Users");

        public static DataTable Users { get { return users; } }

        public static void GetUsers()
        {
            users = Utility.DBConnection.GetSchema("Users");
            //Utility.PrintDatatable(users);
        }

        public static void WriteUsers()
        {
            string xmlfile = users.TableName + ".xml";
            string xslFile = users.TableName + ".xsl";
            string htmFile = users.TableName + ".htm";

            Utility.WriteXML(users, users.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);

        }
    }
}
