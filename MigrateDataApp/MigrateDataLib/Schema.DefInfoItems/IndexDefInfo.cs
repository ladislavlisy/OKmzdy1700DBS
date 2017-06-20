using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class IndexDefInfo : ICloneable
    {
        private IList<IndexFieldInfo> m_IndexFields;
        public string IndexName { get; set; }
        public string TableName { get; set; }
        public int FieldsCount { get; set; }
        public bool Unique { get; set; }
        public bool Primary { get; set; }

        public IndexDefInfo()
        {
            m_IndexFields = new List<IndexFieldInfo>();
            IndexName = "";
            TableName = "";
            FieldsCount = 0;
            Primary = false;
            Unique = false;
        }

        public IndexDefInfo(string lpszName, string lpszTable, bool primary = false)
        {
            m_IndexFields = new List<IndexFieldInfo>();
            IndexName = lpszName;
            TableName = lpszTable;
            FieldsCount = 0;
            Primary = primary;
            Unique = false;
        }
        public IList<IndexFieldInfo> IndexFields()
        {
            return m_IndexFields;
        }

        public void AppendField(string lpszName, bool descending = false)
        {
            IndexFieldInfo fieldInfo = new IndexFieldInfo(lpszName, descending);

            m_IndexFields.Add(fieldInfo);
            FieldsCount++;
        }

        public IndexFieldInfo FieldByName(string columnName)
        {
            IndexFieldInfo column = m_IndexFields.Where((c) => (c.ColumnName.Equals(columnName))).SingleOrDefault();

            return column;
        }

        public string CreateFieldsNameList()
        {
            string strNames = "";

            foreach (var field in m_IndexFields)
            {
                strNames += field.FieldInfo(Primary);
                strNames += (",");
            }
            return strNames.TrimEnd(DatabaseDef.TRIM_CHARS);
        }
        public string CreateFieldsNameLnList()
        {
            string strNames = "";

            foreach (var field in m_IndexFields)
            {
                strNames += field.FieldInfo(Primary);
                strNames += (",\n");
            }
            return strNames.TrimEnd(DatabaseDef.TRIM_CHARS);
        }

        public string[] CreateFieldsNamesArray()
        {
            List<string> strNames = new List<string>();
            foreach (var field in m_IndexFields)
            {
                strNames.Add(field.ColumnName);
            }
            return strNames.ToArray();
        }

        public void Delete()
        {
            m_IndexFields.Clear();
        }


        public string InfoName()
        {
            return TableName + "::" + IndexName;
        }

        public void ReNameColumn(string oldColumnName, string newColumnName)
        {
            IndexFieldInfo indexField = m_IndexFields.SingleOrDefault((f) => (f.ColumnName.CompareTo(oldColumnName) == 0));
            if (indexField != null)
            {
                indexField.ColumnName = newColumnName;
            }
        }

        public void SetIndexFields(List<IndexFieldInfo> fieldList)
        {
            m_IndexFields = fieldList;
        }

        public void SetFields(int fieldCount)
        {
            FieldsCount = fieldCount;
        }

        public void SetUnique(bool uniqueIndex)
        {
            Unique = uniqueIndex;
        }

        public object Clone()
        {
            IndexDefInfo other = (IndexDefInfo)this.MemberwiseClone();
            other.IndexName = this.IndexName;
            other.TableName = this.TableName;
            other.FieldsCount = this.FieldsCount;
            other.Unique = this.Unique;
            other.Primary = this.Primary;

            other.m_IndexFields = this.m_IndexFields.Select((f) => ((IndexFieldInfo)f.Clone())).ToList();
            return other;
        }
    }
}
