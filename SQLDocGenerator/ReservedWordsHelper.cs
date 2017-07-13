using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public static class ReservedWordsHelper
    {
        private static DataTable reservedWords = new DataTable("ReservedWords");

        public static DataTable ReservedWords { get { return reservedWords; } }

        public static void GetReservedWords()
        {
            reservedWords = Utility.DBConnection.GetSchema("ReservedWords");
            //Utility.PrintDatatable(reservedWords);
        }

        public static void WriteReservedWords()
        {
            string xmlfile = reservedWords.TableName + ".xml";
            string xslFile = reservedWords.TableName + ".xsl";
            string htmFile = reservedWords.TableName + ".htm";

            Utility.WriteXML(reservedWords, reservedWords.TableName + ".xml");
            Utility.WriteHTML(xmlfile, xslFile, htmFile);

        }
    }
}
