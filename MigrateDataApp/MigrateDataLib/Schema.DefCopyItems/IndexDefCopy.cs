using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class IndexDefCopy : ICloneable
    {
        private IList<IndexFieldCopy> m_IndexFields;
        public string IndexName { get; set; }
        public string TableName { get; set; }
        private int FieldsCount { get; set; }
        public bool Unique { get; set; }
        public bool Primary { get; set; }

        public IndexDefCopy(IndexDefInfo defInfo)
        {
            this.m_IndexFields = defInfo.IndexFields().Select((idxf) => new IndexFieldCopy(idxf)).ToList();
            this.IndexName = defInfo.IndexName;
            this.TableName = defInfo.TableName;
            this.FieldsCount = defInfo.FieldsCount;
            this.Unique = defInfo.Unique;
            this.Primary = defInfo.Primary;
        }
        public IndexDefCopy(IndexDefInfo defSource, IndexDefInfo defTarget)
        {
            if (defSource != null)
            {
                this.m_IndexFields = defSource.IndexFields().Select((idxf) => new IndexFieldCopy(idxf, null)).ToList();
                this.IndexName = defSource.IndexName;
                this.TableName = defSource.TableName;
                this.FieldsCount = defSource.FieldsCount;
                this.Unique = defSource.Unique;
                this.Primary = defSource.Primary;
            }
            else if (defTarget != null)
            {
                this.m_IndexFields = defTarget.IndexFields().Select((idxf) => new IndexFieldCopy(null, idxf)).ToList();
                this.IndexName = defTarget.IndexName;
                this.TableName = defTarget.TableName;
                this.FieldsCount = defTarget.FieldsCount;
                this.Unique = defTarget.Unique;
                this.Primary = defTarget.Primary;
            }
        }
        public IndexDefCopy(string lpszName, string lpszTable, bool primary = false)
        {
            m_IndexFields = new List<IndexFieldCopy>();
            this.IndexName = lpszName;
            this.TableName = lpszTable;
            this.FieldsCount = 0;
            this.Primary = primary;
            this.Unique = false;
        }
        public IndexDefInfo GetSourceInfo()
        {
            IndexDefInfo defInfo = new IndexDefInfo(this.IndexName, this.TableName, this.Primary);
            defInfo.SetIndexFields(this.m_IndexFields.Select((idxf) => (idxf.GetSourceInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetFields(this.FieldsCount);
            defInfo.SetUnique(this.Unique);

            return defInfo;
        }

        public IndexDefInfo GetTargetInfo()
        {
            IndexDefInfo defInfo = new IndexDefInfo(this.IndexName, this.TableName, this.Primary);
            defInfo.SetIndexFields(this.m_IndexFields.Select((idxf) => (idxf.GetTargetInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetFields(this.FieldsCount);
            defInfo.SetUnique(this.Unique);

            return defInfo;
        }

        public void AppendTargetField(string lpszName, bool descending = false)
        {
            IndexFieldInfo fieldInfo = new IndexFieldInfo(lpszName, descending);

            IndexFieldCopy fieldClone = new IndexFieldCopy(null, fieldInfo);

            m_IndexFields.Add(fieldClone);

            FieldsCount++;
        }

        public void ReNameTargetColumn(string oldColumnName, string newColumnName)
        {
            IndexFieldCopy indexField = m_IndexFields.SingleOrDefault((f) => (f.SourceName().CompareTo(oldColumnName) == 0));
            if (indexField != null)
            {
                indexField.SetTargetName(newColumnName);
            }
        }

        public IList<IndexFieldCopy> IndexFields()
        {
            return m_IndexFields;
        }

        public object Clone()
        {
            IndexDefCopy other = (IndexDefCopy)this.MemberwiseClone();
            other.m_IndexFields = this.m_IndexFields.Select((idxf) => ((IndexFieldCopy)idxf.Clone())).ToList();
            other.IndexName = this.IndexName;
            other.TableName = this.TableName;
            other.FieldsCount = this.FieldsCount;
            other.Unique = this.Unique;
            other.Primary = this.Primary;

            return other;
        }

    }
}
