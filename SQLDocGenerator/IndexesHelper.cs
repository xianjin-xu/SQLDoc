﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SQLDocGenerator
{
    public class IndexesHelper
    {
        private static DataTable indexes = new DataTable("Indexes");

        public static DataTable Indexes { get { return indexes; } }

        public static void GetIndexes()
        {
            indexes = Utility.DBConnection.GetSchema("Indexes");
            AddColumns();

            //Utility.PrintDatatable(indexes);
        }
        public static void WriteIndexes()
        {
            Utility.WriteXML(indexes, indexes.TableName + ".xml");
            //Utility.PrintDatatable(indexes);
        }
        private static void AddColumns()
        {
            indexes.Columns.Add("column_name", typeof(string));
            indexes.Columns.Add("KeyType", typeof(string));

            foreach (DataRow row in indexes.Rows)
            {
                DataRow[] rows = (IndexColumnsHelper.IndexColumns.Select("index_name = '" + row["index_name"] + "'"));
                string column = string.Empty, keyType = string.Empty;

                foreach (DataRow dr in rows)
                {
                    column += ", " + Convert.ToString(dr["column_name"]);
                    keyType += ", " + Convert.ToString(dr["KeyType"]);
                }

                row["column_name"] = column.Substring(1);
                row["KeyType"] = keyType.Substring(1);
            }
        }
    }
}
