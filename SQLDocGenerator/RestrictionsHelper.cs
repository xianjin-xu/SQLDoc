using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class RestrictionsHelper
    {
        private static DataTable restrictions = new DataTable("Restrictions");

        public static DataTable Restrictions { get { return restrictions; } }

        public static void GetRestrictions()
        {
            restrictions = Utility.DBConnection.GetSchema("Restrictions");
            //Utility.PrintDatatable(restrictions);
        }

        public static void WriteRestrictions()
        {
            string xmlfile = restrictions.TableName + ".xml";
            string xslFile = restrictions.TableName + ".xsl";
            string htmFile = restrictions.TableName + ".htm";

            Utility.WriteXML(restrictions, restrictions.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);
        }
    }
}
