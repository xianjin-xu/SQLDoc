using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class ProcedureParametersHelper
    {
        private static DataTable procedureParameters = new DataTable("ProcedureParameters");

        public static DataTable ProcedureParameters { get { return procedureParameters; } }

        public static void GetProcedureParameters()
        {
            procedureParameters = Utility.DBConnection.GetSchema("ProcedureParameters");
            //Utility.PrintDatatable(procedureParameters);
        }
        public static void WriteProcedureParameters()
        {
            Utility.WriteXML(procedureParameters, procedureParameters.TableName + ".xml");
            //Utility.PrintDatatable(procedureParameters);
        }
    }
}
