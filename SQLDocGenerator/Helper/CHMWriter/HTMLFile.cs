using System;
using System.Collections.Generic;
using System.Text;

namespace SQLDocGenerator.Helper.CHMWriter
{
    public class HTMLFile
    {
        public HTMLFile(string name, string local)
        {
            Name = name;
            Local = local;
        }
        public string Name { get; set; }
        public string Local { get; set; }
    }
}
