using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;  

namespace SQLDocGenerator
{
    class HTMLTransfomer
    {
        private string _xmlfile = string.Empty;
        private string _xslFile = string.Empty;
        private string _htmlFile = string.Empty;

        public HTMLTransfomer(string xmlfile, string xslFile, string htmlFile)
        {
            _xmlfile = xmlfile;
            _xslFile = xslFile;
            _htmlFile = htmlFile;
        }

        public void TableTransformer()
        {
           
            XmlTextReader xmlSource = new XmlTextReader(@"xml\" + _xmlfile);
            XPathDocument xpathDoc = new XPathDocument(xmlSource);

            XmlTextReader xslSource = new XmlTextReader(@"xsl\" + _xslFile);
            XslCompiledTransform xsltDoc = new XslCompiledTransform();

            xsltDoc.Load(xslSource);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            XsltArgumentList ar = new XsltArgumentList();

            xsltDoc.Transform(xpathDoc, null, sw);

            File.WriteAllText(@"html\" + _htmlFile, sb.ToString());
        }
    }
}
