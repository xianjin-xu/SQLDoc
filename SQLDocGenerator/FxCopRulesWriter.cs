using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace SQLDocGenerator
{
    public class FxCopRulesWriter
    {
        public FxCopRulesWriter()
        {
        }

        public static void WriteRules()
        {
            TextWriter tw = new StreamWriter("console.txt");

            string fileName = @"D:\Amit\_projects\titan\standards\xmls\Microsoft.FxCop.Rules.Usage.UsageRules.xml";

            XPathDocument doc = new XPathDocument(fileName);
            XPathNavigator nav1 = doc.CreateNavigator();
            XPathNavigator nav2 = doc.CreateNavigator();
            XPathNavigator nav3 = doc.CreateNavigator();
            XPathNavigator nav4 = doc.CreateNavigator();

            XPathExpression checkid;
            checkid = nav1.Compile("/Rules//Rule[@CheckId]");
            XPathNodeIterator iterator1 = nav1.Select(checkid);
            
            XPathExpression name;
            name = nav2.Compile("/Rules/Rule/Name");
            XPathNodeIterator iterator2 = nav2.Select(name);
            
            XPathExpression desc;
            desc = nav1.Compile("/Rules/Rule/Description");
            XPathNodeIterator iterator3 = nav3.Select(desc);

            XPathExpression url;
            url = nav2.Compile("/Rules/Rule/Url");
            XPathNodeIterator iterator4 = nav4.Select(url);

            int rules = 1;
            while (iterator1.MoveNext() && iterator2.MoveNext() && iterator3.MoveNext() && iterator4.MoveNext())
            {
                //tw.Write(rules.ToString() + "\t");

                XPathNavigator checkid1 = iterator1.Current.Clone();
                tw.Write("Rule Id: " + checkid1.GetAttribute("CheckId", ""));

                XPathNavigator name1 = iterator2.Current.Clone();
                tw.Write("[" + name1.Value + "]" + "\t");

                tw.WriteLine();

                XPathNavigator desc1 = iterator3.Current.Clone();
                tw.Write(desc1.Value);

                tw.WriteLine();
               

                XPathNavigator url1 = iterator4.Current.Clone();
                tw.Write("Reference: " + url1.Value.Replace("@", "http://msdn.microsoft.com/hi-in/library/"));
                tw.WriteLine();

                rules++;
                tw.WriteLine();
            }
            tw.Close();
        }
    }
}
