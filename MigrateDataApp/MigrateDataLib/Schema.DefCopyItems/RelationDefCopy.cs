using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class RelationDefCopy : ICloneable
    {
        private IList<RelationFieldCopy> m_RelationFields;
        public string RelationName { get; set; }
        public string SourceTableName { get; set; }
        public string ForeignTableName { get; set; }
        public int Attributes { get; set; }
        public int FieldsCount { get; set; }

        public RelationDefCopy(RelationDefInfo defInfo)
        {
            this.m_RelationFields = defInfo.RelationFields().Select((rf) => new RelationFieldCopy(rf)).ToList();
            this.RelationName = defInfo.RelationName;
            this.SourceTableName = defInfo.SourceTableName;
            this.ForeignTableName = defInfo.ForeignTableName;
            this.Attributes = defInfo.Attributes;
            this.FieldsCount = defInfo.FieldsCount;
        }
        public RelationDefInfo GetSourceInfo()
        {
            RelationDefInfo defInfo = new RelationDefInfo(this.RelationName, this.ForeignTableName, this.SourceTableName);
            defInfo.SetRelationFields(this.m_RelationFields.Select((rf) => (rf.GetSourceInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetAttributes(this.Attributes);
            defInfo.SetFieldsCount(this.FieldsCount);

            return defInfo;
        }

        public RelationDefInfo GetTargetInfo()
        {
            RelationDefInfo defInfo = new RelationDefInfo(this.RelationName, this.ForeignTableName, this.SourceTableName);
            defInfo.SetRelationFields(this.m_RelationFields.Select((rf) => (rf.GetTargetInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetAttributes(this.Attributes);
            defInfo.SetFieldsCount(this.FieldsCount);

            return defInfo;
        }

        public string TargetForeignNamestAllUnique()
        {
            return ForeignTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.TargetForeignName())).ToList());
        }
        public string TargetNamestAllUnique()
        {
            return SourceTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.TargetName())).ToList());
        }


        public void ReNameTableColumn(string oldAuroName, string newName)
        {
            RelationFieldCopy relationField = m_RelationFields.SingleOrDefault((f) => (f.TargetName().CompareTo(oldAuroName) == 0));
            if (relationField != null)
            {
                relationField.SetTargetName(newName);
            }
        }

        public void MakeTargetRelationOrmReady(string relForeignColumnName, string relColumnName)
        {
            m_RelationFields = new List<RelationFieldCopy>();
            FieldsCount = 0;
            Attributes = 0;
            AppendForeignField(relForeignColumnName, relColumnName);

            string newRelationName = relForeignColumnName.Replace("_refid", "").Replace("_id", "");
            RelationName = newRelationName + "_" + ForeignTableName.ToLower();
        }

        public RelationFieldInfo AppendForeignField(string lpszForeignName, string lpszRelName)
        {
            RelationFieldInfo fieldInfo = new RelationFieldInfo(lpszForeignName, lpszRelName);

            RelationFieldCopy fieldClone = new RelationFieldCopy(null, fieldInfo);

            m_RelationFields.Add(fieldClone);
            FieldsCount++;

            return fieldInfo;
        }

        public object Clone()
        {
            RelationDefCopy other = (RelationDefCopy)this.MemberwiseClone();
            other.m_RelationFields = this.m_RelationFields.Select((rf) => ((RelationFieldCopy)rf.Clone())).ToList();
            other.RelationName = this.RelationName;
            other.SourceTableName = this.SourceTableName;
            other.ForeignTableName = this.ForeignTableName;
            other.Attributes = this.Attributes;
            other.FieldsCount = this.FieldsCount;

            return other;
        }
    }
}
