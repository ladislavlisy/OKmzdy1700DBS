using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.Comparator
{
    class StringT2Enumerator
    {
        public StringT2Enumerator(IList<Tuple<string, string>> rows)
        {
            this.RowEnum = rows.GetEnumerator();
            this.RowFieldName = "";
            this.RowEnumIsValid = RowEnum.MoveNext();
        }

        public string NextValues(string[] fieldType, string[] fieldKeys, IDictionary<string, string> fieldDict)
        {
            this.RowFieldName = "";
            this.RowEnumIsValid = RowEnum.MoveNext();
            if (RowEnumIsValid)
            {
                Tuple<string, string> dataRow = ((Tuple<string, string>)RowEnum.Current);
                this.RowFieldName = dataRow.Item1;

                GetFieldTypeRowValues(fieldType, fieldKeys, fieldDict, dataRow);
            }
            return this.RowFieldName;
        }
        private void GetFieldTypeRowValues(string[] fieldType, string[] fieldKeys, IDictionary<string, string> fieldDict, Tuple<string, string> dataRow)
        {
            int index = 0;
            foreach (var field in fieldKeys)
            {
                fieldType[index++] = (index == 0) ? dataRow.Item1 : dataRow.Item2;
            }
        }

        public IEnumerator<Tuple<string, string>> RowEnum { get; private set; }
        public string RowFieldName { get; private set; }
        public bool RowEnumIsValid { get; private set; }
    }

    class StringT3Enumerator
    {
        public StringT3Enumerator(IList<Tuple<string, string, string>> rows)
        {
            this.RowEnum = rows.GetEnumerator();
            this.RowTableName = "";
            this.RowFieldName = "";
            this.RowEnumIsValid = RowEnum.MoveNext();
        }

        public Tuple<string, string> NextValues(string[] fieldType, string[] fieldKeys, IDictionary<string, string> fieldDict)
        {
            this.RowTableName = "";
            this.RowFieldName = "";
            this.RowEnumIsValid = RowEnum.MoveNext();

            if (RowEnumIsValid)
            {
                Tuple<string, string, string> dataRow = ((Tuple<string, string, string>)RowEnum.Current);
                this.RowTableName = dataRow.Item3;
                this.RowFieldName = dataRow.Item1;

                GetFieldTypeRowValues(fieldType, fieldKeys, fieldDict, dataRow);
            }
            return new Tuple<string, string>(this.RowTableName, this.RowFieldName);
        }
        private void GetFieldTypeRowValues(string[] fieldType, string[] fieldKeys, IDictionary<string, string> fieldDict, Tuple<string, string, string> dataRow)
        {
            int index = 0;
            foreach (var field in fieldKeys)
            {
                fieldType[index++] = (index == 0) ? dataRow.Item1 : dataRow.Item2;
            }
        }

        public IEnumerator<Tuple<string, string, string>> RowEnum { get; private set; }
        public string RowTableName { get; private set; }
        public string RowFieldName { get; private set; }
        public bool RowEnumIsValid { get; private set; }
    }

}
