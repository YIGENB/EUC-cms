using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace DY.Common
{
    public class CSVParser
    {
        string _stream;

        /// <summary>
        /// Creates Parser from Stream
        /// </summary>
        /// <param name="aStream"></param>
        public CSVParser(string aStream)
        {
            _stream = aStream;
            CSVregEx = new System.Text.RegularExpressions.Regex("(\"([^\"]*|\"{2})*\"(,|$))|\"[^\"]*\"(,|$)|[^,]+(,|$)|(,)");
        }

        string delimiter = ",";
        string quotes = "\"";

        System.Text.RegularExpressions.Regex CSVregEx;

        protected string[] BreakCSV(string source)
        {

            MatchCollection matches = CSVregEx.Matches(source);

            string[] res = new string[matches.Count];
            int i = 0;
            foreach (Match m in matches)
            {
                res[i] = m.Groups[0].Value.TrimEnd(delimiter[0]).Trim(quotes[0]);
                i++;
            }
            return res;
        }

        private string _TableName = "CSV";

        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        public DataTable ParseToDataTable()
        {
            StreamReader reader = new StreamReader(_stream, Encoding.Default);

            string firstLine = reader.ReadLine();
            //throw new Exception(firstLine);
            string[] columns = BreakCSV(firstLine);
            DataTable result = new DataTable();
            result.TableName = TableName;

            foreach (string s in columns)
            {
                DataColumn cm = new DataColumn(s, typeof(String));
                cm.ColumnMapping = MappingType.Attribute;
                result.Columns.Add(cm);
            }

            string line = "";
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                string[] data = BreakCSV(line);
                int i = 0;
                DataRow dr = result.NewRow();
                foreach (string d in data)
                {
                    dr[i++] = d;
                }
                result.Rows.Add(dr);
            }

            reader.Close();
            //_stream.Close();
            return result;
        }

    }
}
