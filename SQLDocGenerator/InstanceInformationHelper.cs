using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class InstanceInformationHelper
    {
        private static DataTable instanceInformation = new DataTable("InstanceInformation");

        public static DataTable InstanceInformation { get { return instanceInformation; } }

        public static void GetInstanceInformation()
        {
            String dataSource = Utility.DBConnection.DataSource;
            Int32 packetSize = Utility.DBConnection.PacketSize;
            String serverVersion = Utility.DBConnection.ServerVersion;
            Boolean statisticsEnabled = Utility.DBConnection.StatisticsEnabled;
            String workstationId = Utility.DBConnection.WorkstationId;

            instanceInformation.Columns.Add("dataSource", dataSource.GetType());
            instanceInformation.Columns.Add("packetSize", packetSize.GetType());
            instanceInformation.Columns.Add("serverVersion", serverVersion.GetType());
            instanceInformation.Columns.Add("statisticsEnabled", statisticsEnabled.GetType());
            instanceInformation.Columns.Add("workstationId", workstationId.GetType());
            instanceInformation.Rows.Add(dataSource, packetSize, serverVersion, statisticsEnabled, workstationId);
            //Utility.PrintDatatable(instanceInformation);
        }

        public static void WriteInstanceInformation()
        {
            string xmlfile = instanceInformation.TableName + ".xml";
            string xslFile = instanceInformation.TableName + ".xsl";
            string htmFile = instanceInformation.TableName + ".htm";

            Utility.WriteXML(instanceInformation, xmlfile);
            Utility.WriteHTML(xmlfile, xslFile, htmFile);
        }
    }
}
