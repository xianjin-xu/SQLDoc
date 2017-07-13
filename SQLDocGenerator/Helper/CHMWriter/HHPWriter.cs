using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;

namespace SQLDocGenerator.Helper.CHMWriter
{
    public class HHPWriter
    {
        public string OutputFile { get; set; }
        public string Compatibility { get; set; }
        public string CompiledFile { get; set; }
        public string ContentsFile { get; set; }
        public string DefaultTopicHTMLFile { get; set; }
        public string IndexFile { get; set; }
        public string Language { get; set; }

        List<HTMLFile> files = new List<HTMLFile>();
        public HTMLFile AddFile
        {
            set
            {
                files.Add(value);
            }
        }

        List<HTMLFile> infoTypes = new List<HTMLFile>();
        public HTMLFile AddInfoType
        {
            set
            {
                infoTypes.Add(value);
            }
        }

        TextWriter tw;

        public void Write()
        {
            #region sample file
            //[OPTIONS]
            //Compatibility=1.1 or later
            //Compiled file=AdventureWorks.chm
            //Contents file=Table of Contents.hhc
            //Default topic=D:\Amit\myWork\SQLDoc\SQLDoc\SQLDoc\bin\Debug\html\Root.htm
            //Index file=Index.hhk
            //Language=0x409 English (United States)

            //[FILES]
            //D:\Amit\myWork\SQLDoc\SQLDoc\SQLDoc\bin\Debug\html\TableList.htm
            //D:\Amit\myWork\SQLDoc\SQLDoc\SQLDoc\bin\Debug\html\Address.htm

            //[INFOTYPES]
            #endregion

            using (tw = new StreamWriter(OutputFile))
            {
                tw.WriteLine(WriteOptions());
                tw.WriteLine(WriteFiles());
                tw.WriteLine(WriteInfoTypes());

                tw.Flush();
            }
        }

        private string WriteOptions()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            location = location.Substring(0, location.LastIndexOf("\\")) + "\\html\\";

            string options = "[OPTIONS]" + Environment.NewLine;

            options += "Compatibility = " + (String.IsNullOrEmpty(Compatibility) ? "1.1 or later" : Compatibility) + Environment.NewLine;
            options += "Compiled file=" + (String.IsNullOrEmpty(CompiledFile) ? "AdventureWorks.chm" : CompiledFile) + Environment.NewLine;
            options += "Contents file=" + (String.IsNullOrEmpty(ContentsFile) ? "Table of Contents.hhc" : ContentsFile) + Environment.NewLine;
            options += "Default topic=" + (String.IsNullOrEmpty(DefaultTopicHTMLFile) ? location + "Root.htm" : DefaultTopicHTMLFile) + Environment.NewLine;
            //options += "Index file=" + (String.IsNullOrEmpty(IndexFile) ? "Index.hhk" : IndexFile) + Environment.NewLine;
            options += "Language=" + (String.IsNullOrEmpty(Language) ? "0x409 English (United States)" : Language) + Environment.NewLine;

            return options + Environment.NewLine;
        }

        private string WriteFiles()
        {
            string fileName = "[FILES]" + Environment.NewLine;

            foreach (HTMLFile file in files)
            {
                fileName += file.Local + Environment.NewLine;
            }
            return fileName + Environment.NewLine;
        }

        private string WriteInfoTypes()
        {
            string infoTypeName = "[INFOTYPES]" + Environment.NewLine;

            foreach (HTMLFile infoType in infoTypes)
            {
                infoTypeName += infoType.Local + Environment.NewLine;
            }
            return infoTypeName + Environment.NewLine;
        }
    }
}
