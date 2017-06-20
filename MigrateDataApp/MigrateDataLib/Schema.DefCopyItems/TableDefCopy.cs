using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableDefCopy : ICloneable
    {
        protected IList<TableFieldCopy> m_TableFields;
        protected IndexDefCopy m_PKConstraint;
        protected IndexDefCopy m_AKConstraint;
        protected IList<IndexDefCopy> m_TableIndexs;
        protected IList<RelationDefCopy> m_TableRelations;
        protected int m_nFieldsCount;
        protected string m_strOwnerName;
        protected string m_strUsersName;
        protected string m_strTableName;
        protected UInt32 m_VersFrom = 0;
        protected UInt32 m_VersDrop = 9999;

        public bool IsValidInVersion(UInt32 versCreate)
        {
            return (m_VersFrom <= versCreate && versCreate < m_VersDrop);
        }
        public string OwnerName()
        {
            return m_strOwnerName;
        }

        public string UsersName()
        {
            return m_strUsersName;
        }
        public UInt32 VersFrom()
        {
            return m_VersFrom;
        }

        public UInt32 VersDrop()
        {
            return m_VersDrop;
        }

        public string TableName()
        {
            return m_strTableName;
        }

        public TableDefCopy(string ownerName, string usersName, string tableName, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            this.m_strOwnerName = ownerName;
            this.m_strUsersName = usersName;
            this.m_strTableName = tableName;
            this.m_TableFields = new List<TableFieldCopy>();
            m_PKConstraint = null;
            m_AKConstraint = null;
            this.m_TableIndexs = new List<IndexDefCopy>();
            this.m_TableRelations = new List<RelationDefCopy>();
            this.m_VersFrom = versFrom;
            this.m_VersDrop = versDrop;
        }

        public TableDefCopy(TableDefInfo defInfo, UInt32 versCreate)
        {
            this.m_TableFields = defInfo.TableColumnsForVersion(versCreate).Select((tf) => new TableFieldCopy(tf)).ToList();
            this.m_PKConstraint = CloneIndex(defInfo.IndexPK());
            this.m_AKConstraint = CloneIndex(defInfo.IndexAK());
            this.m_TableIndexs = defInfo.IndexesNonPK().Select((idx) => new IndexDefCopy(idx)).ToList();
            this.m_TableRelations = defInfo.Relations().Select((rel) => new RelationDefCopy(rel)).ToList();
            this.m_nFieldsCount = defInfo.FieldsCount();
            this.m_strOwnerName = defInfo.OwnerName();
            this.m_strUsersName = defInfo.UsersName();
            this.m_strTableName = defInfo.TableName();
            this.m_VersFrom = defInfo.VersFrom();
            this.m_VersDrop = defInfo.VersDrop();
        }
        private IndexDefCopy CloneIndex(IndexDefInfo defInfo)
        {
            if (defInfo == null)
            {
                return null;
            }
            return new IndexDefCopy(defInfo);
        }

        private IndexDefInfo CloneSourceIndex(IndexDefCopy defInfo)
        {
            if (defInfo == null)
            {
                return null;
            }
            return defInfo.GetSourceInfo();
        }

        private IndexDefInfo CloneTargetIndex(IndexDefCopy defInfo)
        {
            if (defInfo == null)
            {
                return null;
            }
            return defInfo.GetTargetInfo();
        }


        public TableDefInfo GetSourceInfo()
        {
            TableDefInfo defInfo = new TableDefInfo(this.m_strOwnerName, this.m_strUsersName, this.m_strTableName, this.m_VersFrom, this.m_VersDrop);
            defInfo.SetTableFields(this.m_TableFields.Select((tbf) => (tbf.GetSourceInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetIndexPK(CloneSourceIndex(this.m_PKConstraint));
            defInfo.SetIndexAK(CloneSourceIndex(this.m_AKConstraint));
            defInfo.SetIndexesNonPK(this.m_TableIndexs.Select((idx) => (idx.GetSourceInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetRelations(this.m_TableRelations.Select((rel) => (rel.GetSourceInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetFields(this.m_nFieldsCount);

            return defInfo;
        }

        public TableDefInfo GetTargetInfo()
        {
            TableDefInfo defInfo = new TableDefInfo(this.m_strOwnerName, this.m_strUsersName, this.m_strTableName, this.m_VersFrom, this.m_VersDrop);
            defInfo.SetTableFields(this.m_TableFields.Select((tbf) => (tbf.GetTargetInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetIndexPK(CloneTargetIndex(this.m_PKConstraint));
            defInfo.SetIndexAK(CloneTargetIndex(this.m_AKConstraint));
            defInfo.SetIndexesNonPK(this.m_TableIndexs.Select((idx) => (idx.GetTargetInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetRelations(this.m_TableRelations.Select((rel) => (rel.GetTargetInfo())).Where((fi) => (fi != null)).ToList());
            defInfo.SetFields(this.m_nFieldsCount);

            return defInfo;
        }
        public IList<TableFieldCopy> TableColumnsForVersion(UInt32 versCreate)
        {
            IList<TableFieldCopy> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            return columnList;
        }
        public TableFieldCopy SourceFieldByName(string columnName, UInt32 versCreate)
        {
            IList<TableFieldCopy> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            TableFieldCopy column = columnList.Where((c) => (c.SourceColumnName().CompareNoCase(columnName))).SingleOrDefault();

            return column;
        }

        public TableFieldCopy TargetFieldByName(string columnName, UInt32 versCreate)
        {
            IList<TableFieldCopy> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            TableFieldCopy column = columnList.Where((c) => (c.TargetColumnName().CompareNoCase(columnName))).SingleOrDefault();

            return column;
        }

        public string TargetPKUniqueAllNames()
        {
            return m_strTableName + "." + string.Join(".", m_PKConstraint.IndexFields().Select((f) => (f.TargetName())).ToList());
        }
        public string TargetAKUniqueAllNames()
        {
            return m_strTableName + "." + string.Join(".", m_AKConstraint.IndexFields().Select((f) => (f.TargetName())).ToList());
        }

        public TableFieldCopy GetAutoIncrementColumn()
        {
            return m_TableFields.Where((c) => (c.IsAutoIncrement())).SingleOrDefault();
        }

        public IndexDefCopy IndexPK()
        {
            return m_PKConstraint;
        }
        public IList<TableFieldCopy> TableFields()
        {
            return m_TableFields;
        }
        public IList<RelationDefCopy> Relations()
        {
            return m_TableRelations;
        }
        public IList<RelationDefCopy> ForeignRelations(IList<TableDefCopy> tables)
        {
            return tables.SelectMany((m) => (m.Relations().Where((r) => (r.SourceTableName.CompareTo(m_strTableName) == 0)))).ToList();
        }

        public TableFieldCopy CreateTargetFAUTO(string lpszName, int nType, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = TableDefInfo.CreateFAUTOInfo(lpszName, nType, versFrom, versDrop);

            TableFieldCopy fieldClone = new TableFieldCopy(null, fieldInfo);

            return FieldInsertBeg(fieldClone);
        }
        public TableFieldCopy CreateTargetField(string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = TableDefInfo.CreateFieldInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            TableFieldCopy fieldClone = new TableFieldCopy(null, fieldInfo);

            return FieldAppend(fieldClone);
        }

        public TableFieldCopy FieldInsertBeg(TableFieldCopy fieldInfo)
        {
            m_TableFields.Insert(0, fieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }

        public TableFieldCopy FieldAppend(TableFieldCopy fieldInfo)
        {
            m_TableFields.Add(fieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }

        public void CreateTargetIndexFromXPK(IndexDefCopy primaryKey)
        {
            if (primaryKey != null)
            {
                IndexDefInfo indexInfo = (IndexDefInfo)primaryKey.GetTargetInfo();

                string indexName = indexInfo.IndexName.Replace("XPK", "XAK");

                indexInfo.IndexName = indexName;

                indexInfo.Primary = false;

                indexInfo.Unique = true;

                IndexDefCopy indexClone = new IndexDefCopy(null, indexInfo);

                IndexAppend(indexClone);
            }
        }

        public IndexDefCopy CreateTargetIndexFromXPK(IndexDefCopy pkConstraint, string oldAutoName, string newAutoName)
        {
            m_AKConstraint = null;

            if (pkConstraint != null)
            {
                IndexDefInfo indexInfo = (IndexDefInfo)pkConstraint.GetTargetInfo();

                m_AKConstraint = new IndexDefCopy(null, indexInfo);

                string indexName = m_AKConstraint.IndexName.Replace("XPK", "XAK");

                m_AKConstraint.IndexName = indexName;

                m_AKConstraint.Primary = false;

                m_AKConstraint.Unique = true;

                m_AKConstraint.ReNameTargetColumn(oldAutoName, newAutoName);
            }
            return m_AKConstraint;
        }

        public IndexDefCopy CreatePKAutoConstraint(string lpszName, string lpszIdName)
        {
            string constraintName = lpszName + m_strTableName;

            m_PKConstraint = new IndexDefCopy(constraintName, m_strTableName, true);
            m_PKConstraint.AppendTargetField(lpszIdName);

            return m_PKConstraint;
        }

        public IndexDefCopy IndexAppend(IndexDefCopy indexInfo)
        {
            m_TableIndexs.Add(indexInfo);

            return indexInfo;
        }

        public IList<IndexDefCopy> IndexesNonPK()
        {
            return m_TableIndexs;
        }
        public object Clone()
        {
            TableDefCopy other = (TableDefCopy)this.MemberwiseClone();
            other.m_TableFields = this.m_TableFields.Select((tf) => ((TableFieldCopy)tf.Clone())).ToList();
            other.m_PKConstraint = (IndexDefCopy)(this.m_PKConstraint.Clone());
            other.m_AKConstraint = (IndexDefCopy)(this.m_AKConstraint.Clone());
            other.m_TableIndexs = this.m_TableIndexs.Select((idx) => ((IndexDefCopy)idx.Clone())).ToList();
            other.m_TableRelations = this.m_TableRelations.Select((rel) => ((RelationDefCopy)rel.Clone())).ToList();
            other.m_nFieldsCount = this.m_nFieldsCount;
            other.m_strOwnerName = this.m_strOwnerName;
            other.m_strUsersName = this.m_strUsersName;
            other.m_strTableName = this.m_strTableName;
            other.m_VersFrom = this.m_VersFrom;
            other.m_VersDrop = this.m_VersDrop;

            return other;
        }
    }
}
