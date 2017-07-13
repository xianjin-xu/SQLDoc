using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Data;

namespace SQLDocGenerator.Helper.CHMWriter
{
    public class HHCWriter
    {
        public HHCWriter(string tocFileName, string hhpFileName, string chmFileName)
        {
            TOCFileName = tocFileName;      // "Table of Contents.hhc";
            OutputFileName = hhpFileName;        // "AdventureWorks.hhp";
            CompiledFileName = chmFileName;    // "AdventureWorks.chm";
        }

        public string TOCFileName { get; set; }
        public string OutputFileName { get; set; }
        public string CompiledFileName { get; set; }

        List<HTMLFile> files = new List<HTMLFile>();
        public HTMLFile AddFile
        {
            set
            {
                files.Add(value);
            }
        }

        TextWriter tw;
        HHPWriter hhp;

        public void Write()
        {
            #region sample file
            //<!DOCTYPE HTML PUBLIC "-//IETF//DTD HTML//EN">
            //<HTML>
            //<HEAD>
            //<meta name="GENERATOR" content="Microsoft&reg; HTML Help Workshop 4.1">
            //<!-- Sitemap 1.0 -->
            //</HEAD><BODY>
            //<UL>
            //    <LI><OBJECT type="text/sitemap">
            //        <param name="Name" value="amitc\sqlexpress2008">
            //        <param name="Local" value="Root.htm">
            //        </OBJECT>
            //    <UL>
            //        <LI><OBJECT type="text/sitemap">
            //            <param name="Name" value="AdventureWorks">
            //            <param name="Local" value="Root.htm">
            //            </OBJECT>
            //        <UL>
            //            <LI><OBJECT type="text/sitemap">
            //                <param name="Name" value="Object by Types">
            //                <param name="Local" value="Root.htm">
            //                </OBJECT>
            //            <UL>
            //                <LI><OBJECT type="text/sitemap">
            //                    <param name="Name" value="Tables">
            //                    <param name="Local" value="D:/Amit/myWork/SQLDoc/SQLDoc/SQLDoc/bin/Debug/html/TableList.htm">
            //                    </OBJECT>
            //                <UL>
            //                    <LI><OBJECT type="text/sitemap">
            //                        <param name="Name" value="Address">
            //                        <param name="Local" value="D:/Amit/myWork/SQLDoc/SQLDoc/SQLDoc/bin/Debug/html/Address.htm">
            //                        </OBJECT>
            //                    <LI><OBJECT type="text/sitemap">
            //                        <param name="Name" value="AddressType.htm">
            //                        <param name="Local" value="D:/Amit/myWork/SQLDoc/SQLDoc/SQLDoc/bin/Debug/html/AddressType.htm">
            //                        </OBJECT>
            //                </UL>
            //            </UL>
            //        </UL>
            //    </UL>
            //</UL>
            //</BODY></HTML>
            #endregion

            hhp = new HHPWriter();
            hhp.OutputFile = OutputFileName;        // "AdventureWorks.hhp";
            hhp.CompiledFile = CompiledFileName;    // "AdventureWorks.chm";
            hhp.ContentsFile = TOCFileName;         // "AdventureWorks.hhc";

            string location = Assembly.GetExecutingAssembly().Location;
            location = location.Substring(0, location.LastIndexOf("\\")) + "\\html\\";

            using (tw = new StreamWriter(TOCFileName))
            {
                tw.WriteLine(WriteHeader());

                tw.WriteLine(WriteULStartElement());
                tw.WriteLine(WriteLIElement(Utility.DatabaseServer, location + "Root.htm"));
                tw.WriteLine(WriteULStartElement());
                tw.WriteLine(WriteLIElement(Utility.DatabaseName, location + "Root.htm"));
                
                tw.WriteLine(WriteULStartElement());
                tw.WriteLine(WriteLIElement("Server Inforation", location + "Root.htm"));
                tw.WriteLine(WriteULStartElement());
                foreach (string objectType in SQLDoc.ServerInfoToWrite)
                {
                    switch (objectType)
                    {
                        case "InstanceInformation":
                            tw.WriteLine(WriteLIElement("InstanceInformation", location + "InstanceInformation.htm"));
                            hhp.AddFile = new HTMLFile("InstanceInformation", location + "InstanceInformation.htm");
                            break;
                        case "DataSourceInformation":
                            tw.WriteLine(WriteLIElement("DataSourceInformation", location + "DataSourceInformation.htm"));
                            hhp.AddFile = new HTMLFile("DataSourceInformation", location + "DataSourceInformation.htm");
                            break;
                        case "DataTypes":
                            tw.WriteLine(WriteLIElement("DataTypes", location + "DataTypes.htm"));
                            hhp.AddFile = new HTMLFile("DataTypes", location + "DataTypes.htm");
                            break;
                        case "Restrictions":
                            tw.WriteLine(WriteLIElement("Restrictions", location + "Restrictions.htm"));
                            hhp.AddFile = new HTMLFile("Restrictions", location + "Restrictions.htm");
                            break;
                        case "ReservedWords":
                            tw.WriteLine(WriteLIElement("ReservedWords", location + "ReservedWords.htm"));
                            hhp.AddFile = new HTMLFile("ReservedWords", location + "ReservedWords.htm");
                            break;
                        case "Users":
                            tw.WriteLine(WriteLIElement("Users", location + "Users.htm"));
                            hhp.AddFile = new HTMLFile("Users", location + "Users.htm");
                            break;
                        case "Databases":
                            tw.WriteLine(WriteLIElement("Databases", location + "Databases.htm"));
                            hhp.AddFile = new HTMLFile("Databases", location + "Databases.htm");
                            break;
                    }
                }
                tw.WriteLine(WriteULEndElement());
                tw.WriteLine(WriteULEndElement());

                tw.WriteLine(WriteULStartElement());
                tw.WriteLine(WriteLIElement("Object By Type", location + "Root.htm"));
                tw.WriteLine(WriteULStartElement());
                foreach (string objectType in SQLDoc.ObjectByTypesToWrite)
                {
                    switch (objectType)
                    {
                        case "Tables":
                            tw.WriteLine(WriteLIElement("Tables", location + "TableList.htm"));
                            hhp.AddFile = new HTMLFile("Tables", location + "TableList.htm");
                            tw.WriteLine(WriteULWithLIList(TablesHelper.Tables, "TABLE_NAME"));
                            break;
                        case "Views":
                            tw.WriteLine(WriteLIElement("Views", location + "ViewList.htm"));
                            hhp.AddFile = new HTMLFile("Views", location + "ViewList.htm");
                            tw.WriteLine(WriteULWithLIList(ViewsHelper.Views, "TABLE_NAME"));
                            break;
                        case "Procedures":
                            tw.WriteLine(WriteLIElement("Procedures", location + "ProcedureList.htm"));
                            hhp.AddFile = new HTMLFile("Procedures", location + "ProcedureList.htm");
                            tw.WriteLine(WriteULWithLIList(ProceduresHelper.Procedures, "SPECIFIC_NAME"));
                            break;
                        case "Functions":
                            tw.WriteLine(WriteLIElement("Functions", location + "FunctionList.htm"));
                            hhp.AddFile = new HTMLFile("Functions", location + "FunctionList.htm");
                            tw.WriteLine(WriteULWithLIList(ProceduresHelper.Functions, "SPECIFIC_NAME"));
                            break;
                        case "Triggers":
                            tw.WriteLine(WriteLIElement("Triggers", location + "TriggerList.htm"));
                            hhp.AddFile = new HTMLFile("Triggers", location + "TriggerList.htm");
                            tw.WriteLine(WriteULWithLIList(SMOHelper.TriggersHelper.Triggers, "Name"));
                            break;
                        case "UserDefinedDataTypes":
                            tw.WriteLine(WriteLIElement("UserDefinedDataTypes", location + "UserDefinedDataTypeList.htm"));
                            hhp.AddFile = new HTMLFile("UserDefinedDataTypes", location + "UserDefinedDataTypeList.htm");
                            tw.WriteLine(WriteULWithLIList(SMOHelper.UserDefinedDataTypesHelper.UserDefinedDataTypes, "Name"));
                            break;
                    }
                }
                tw.WriteLine(WriteULEndElement());
                tw.WriteLine(WriteULEndElement());

                tw.WriteLine(WriteULEndElement());
                tw.WriteLine(WriteULEndElement());
                tw.WriteLine(WriteFooter());
            }
            hhp.Write();
        }

        private string WriteHeader()
        {
            string header = "<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">" + Environment.NewLine;
            header += "<HTML>" + Environment.NewLine;
            header += "<HEAD>" + Environment.NewLine;
            header += "<meta name=\"GENERATOR\" content=\"Microsoft&reg; HTML Help Workshop 4.1\">" + Environment.NewLine;
            header += "<!-- Sitemap 1.0 -->" + Environment.NewLine;
            header += "</HEAD>" + Environment.NewLine;
            header += "<BODY>";

            return header;
        }
        private string WriteFooter()
        {
            string footer = "</BODY>" + Environment.NewLine;
            footer += "</HTML>";

            return footer;
        }

        private string WriteULStartElement()
        {
            string text = "<UL>" + Environment.NewLine;

            return text;
        }
        private string WriteULEndElement()
        {
            string text = "</UL>";

            return text;
        }
        private string WriteLIElement(string nodeName, string htmlFile)
        {
            string text = "<LI><OBJECT type=\"text/sitemap\">" + Environment.NewLine;
            text += "<param name=\"Name\" value=\"" + nodeName + "\">" + Environment.NewLine;
            text += "<param name=\"Local\" value=\"" + htmlFile + "\">" + Environment.NewLine;
            text += "</OBJECT>";

            return text;
        }
        private string WriteULWithLIList(DataTable dt, string objectName)
        {
            string root = Assembly.GetExecutingAssembly().Location;
            root = root.Substring(0, root.LastIndexOf("\\")) + "\\html\\";

            string text = "<UL>" + Environment.NewLine;

            foreach (DataRow row in dt.Rows)
            {
                string name = row[objectName].ToString();
                string htmFile = name + ".htm";
                string htmLocation = root + htmFile;

                text += "<LI><OBJECT type=\"text/sitemap\">" + Environment.NewLine;
                text += "<param name=\"Name\" value=\"" + name + "\">" + Environment.NewLine;
                text += "<param name=\"Local\" value=\"" + htmLocation + "\">" + Environment.NewLine;
                text += "</OBJECT>" + Environment.NewLine;

                hhp.AddFile = new HTMLFile(name, htmLocation);
            }

            text += "</UL>";
            return text;
        }
    }
}
