using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Utils;
using MigrateDataLib.Schema.DefCopyItems;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class RelationDefInfo : ICloneable
    {
        private IList<RelationFieldInfo> m_RelationFields;
        public string RelationName { get; set; }
        public string SourceTableName { get; set; }
        public string ForeignTableName { get; set; }
        public int Attributes { get; set; }
        public int FieldsCount { get; set; }

        public RelationDefInfo(string relationName, string foreignTable, string sourceTable)
        {
            m_RelationFields = new List<RelationFieldInfo>();
            RelationName = relationName;
            SourceTableName = sourceTable;
            ForeignTableName = foreignTable;
            Attributes = 0;
            FieldsCount = 0;
        }
        public IList<RelationFieldInfo> RelationFields()
        {
            return m_RelationFields;
        }

        public RelationFieldInfo RelationFieldByName(string lpszName)
        {
            return m_RelationFields.SingleOrDefault((f) => f.ForeignName.CompareNoCase(lpszName));
        }

        public RelationFieldInfo AppendField(string lpszName)
        {
            RelationFieldInfo fieldInfo = new RelationFieldInfo(lpszName, lpszName);

            m_RelationFields.Add(fieldInfo);
            FieldsCount++;

            return fieldInfo;
        }

        public RelationFieldInfo AppendForeignField(string foreignName, string sourceName)
        {
            RelationFieldInfo fieldInfo = new RelationFieldInfo(foreignName, sourceName);

            m_RelationFields.Add(fieldInfo);
            FieldsCount++;

            return fieldInfo;
        }

        public void ReNameSourceColumnList(IList<TableFieldPipe> columnList)
        {
            var changeList = columnList.Where((c) => (c.AllFieldValid() && c.SourceName().CompareTo(c.TargetName()) != 0)).ToList();

            foreach (var column in changeList)
            {
                RelationFieldInfo relationField = m_RelationFields.SingleOrDefault((f) => (f.SourceName.CompareTo(column.SourceName()) == 0));
                if (relationField != null)
                {
                    relationField.SourceName = column.TargetName();
                }
            }
        }

        public void ReNameSourceColumn(string oldAuroName, string newName)
        {
            RelationFieldInfo relationField = m_RelationFields.SingleOrDefault((f) => (f.SourceName.CompareTo(oldAuroName) == 0));
            if (relationField != null)
            {
                relationField.SourceName = newName;
            }
        }

        public void ReNameForeignColumn(string oldAuroName, string newName)
        {
            RelationFieldInfo relationField = m_RelationFields.SingleOrDefault((f) => (f.ForeignName.CompareTo(oldAuroName) == 0));
            if (relationField != null)
            {
                relationField.ForeignName = newName;
            }
        }

        public string ForeignNamesAllUnique()
        {
            return ForeignTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.ForeignName)).ToList());
        }
        public string SourceNamesAllUnique()
        {
            return SourceTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.SourceName)).ToList());
        }

        public string FieldNamesWithAlias(string foreignAlias, string sourceAlias, string namesJoin)
        {
            IList<string> columnList = m_RelationFields.Select((f) => (string.Format("{0}.{1} = {2}.{3}", foreignAlias, f.ForeignName, sourceAlias, f.SourceName))).ToList();
            return string.Join(namesJoin, columnList);
        }

        public string SourceFieldNameColumnList()
        {
            string strNames = "";
            foreach (var field in m_RelationFields)
            {
                strNames += field.SourceName;
                strNames += (",");
            }
            string retNames = strNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retNames;
        }

        public string ForeignFieldNameColumnList()
        {
            string strFNames = "";
            foreach (var field in m_RelationFields)
            {
                strFNames += field.ForeignName;
                strFNames += (",");
            }
            string retFNames = strFNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retFNames;
        }

        public string SourceFieldNameColumnLnList()
        {
            string strNames = "";
            foreach (var field in m_RelationFields)
            {
                strNames += field.SourceName;
                strNames += (",\n");
            }
            string retNames = strNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retNames;
        }

        public string ForeignFieldNameColumnLnList()
        {
            string strFNames = "";
            foreach (var field in m_RelationFields)
            {
                strFNames += field.ForeignName;
                strFNames += (",\n");
            }
            string retFNames = strFNames.TrimEnd(DatabaseDef.TRIM_CHARS);
            return retFNames;
        }

        public void FieldsDelete()
        {
            m_RelationFields.Clear();
        }

        public string TableCamelName()
        {
            return SourceTableName.Underscore().Camelize();
        }

        public string ConvertTableNameToCamel()
        {
            return SourceTableName.ConvertNameToCamel();
        }
        public string InfoName()
        {
            return SourceTableName + "::" + RelationName;
        }

        public string RelationUniqueForeignName(string nameId)
        {
            return ForeignTableName + "." + m_RelationFields.SingleOrDefault((f) => (f.SourceName.CompareTo(nameId) == 0)).ForeignName;
        }

        public string RelationUniqueSourceAllNames()
        {
            return SourceTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.SourceName)).ToList());
        }
        public string RelationUniqueForeignAllNames()
        {
            return ForeignTableName + "." + string.Join(".", m_RelationFields.Select((f) => (f.ForeignName)).ToList());
        }

        public IList<string> DeepRelationsList(IList<TableDefInfo> tables, IList<string> agrList)
        {
            TableDefInfo tableDef = tables.FirstOrDefault((t) => (t.TableName().Equals(SourceTableName)));

            if (tableDef == null)
            {
                return agrList;
            }
            return tableDef.DeepRelationsList(agrList, tables, true);
        }
        public void SetRelationFields(List<RelationFieldInfo> fieldList)
        {
            m_RelationFields = fieldList;
        }

        public void SetAttributes(int attributes)
        {
            Attributes = attributes;
        }

        public void SetFieldsCount(int fieldCount)
        {
            FieldsCount = fieldCount;
        }

        public void MakeRelationOrmReady(string relForeignColumnName, string relColumnName, string relationName)
        {
            m_RelationFields = new List<RelationFieldInfo>();
            FieldsCount = 0;
            Attributes = 0;
            AppendForeignField(relForeignColumnName, relColumnName);
            RelationName = relationName;
        }


        public object Clone()
        {
            RelationDefInfo other = (RelationDefInfo)this.MemberwiseClone();
            other.RelationName = this.RelationName;
            other.SourceTableName = this.SourceTableName;
            other.ForeignTableName = this.ForeignTableName;
            other.Attributes = this.Attributes;
            other.FieldsCount = FieldsCount;

            other.m_RelationFields = this.m_RelationFields.Select((r) => ((RelationFieldInfo)r.Clone())).ToList();
            return other;
        }
    }
}
