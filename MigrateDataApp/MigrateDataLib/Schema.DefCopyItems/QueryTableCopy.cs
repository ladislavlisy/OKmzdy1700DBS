using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryTableCopy : ICloneable
    {
        private TableDefCopy m_QueryTableInfo;
        public IList<QueryFieldCopy> QueryFields { get; set; }
        public string AliasName { get; set; }
        public string TableName { get; set; }

        public QueryTableCopy(QueryTableInfo defInfo, UInt32 versCreate)
        {
            this.m_QueryTableInfo = new TableDefCopy(defInfo.TableInfo(), versCreate);
            this.QueryFields = defInfo.QueryFieldList().Select((qf) => (new QueryFieldCopy(qf))).ToList();
            this.AliasName = defInfo.AliasName;
            this.TableName = defInfo.TableName;
        }
        public QueryTableCopy(string aliasName, TableDefInfo tableInfo, UInt32 versCreate)
        {
            m_QueryTableInfo = new TableDefCopy(tableInfo, versCreate);
            QueryFields = tableInfo.TableColumns().Select((tc) => (new QueryFieldCopy(tc, tc.ColumnName))).ToList();
            AliasName = aliasName;
            TableName = tableInfo.TableName();
        }


        public QueryTableInfo GetSourceInfo()
        {
            QueryTableInfo defInfo = new QueryTableInfo(this.AliasName, this.m_QueryTableInfo.GetSourceInfo());
            defInfo.SetQueryFieldsList(this.QueryFields.Select((qf) => (qf.GetSourceInfo())).ToList());

            return defInfo;
        }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            return m_QueryTableInfo.IsValidInVersion(versCreate);
        }
        public string OwnerName()
        {
            return m_QueryTableInfo.OwnerName();
        }

        public string UsersName()
        {
            return m_QueryTableInfo.UsersName();
        }
        public UInt32 VersFrom()
        {
            return m_QueryTableInfo.VersFrom();
        }

        public UInt32 VersDrop()
        {
            return m_QueryTableInfo.VersDrop();
        }
        public QueryTableInfo GetTargetInfo()
        {
            QueryTableInfo defInfo = new QueryTableInfo(this.AliasName, this.m_QueryTableInfo.GetTargetInfo());
            defInfo.SetQueryFieldsList(this.QueryFields.Select((qf) => (qf.GetTargetInfo())).ToList());

            return defInfo;
        }

        #region ICloneable Members

        public object Clone()
        {
            QueryTableCopy other = (QueryTableCopy)this.MemberwiseClone();
            other.m_QueryTableInfo = (TableDefCopy)this.m_QueryTableInfo.Clone();
            other.QueryFields = this.QueryFields.Select((qf) => ((QueryFieldCopy)qf.Clone())).ToList();
            other.AliasName = this.AliasName;
            other.TableName = this.TableName;

            return other;
        }
        #endregion

        public TableFieldCopy GetAutoIncrementColumn()
        {
            var columns = m_QueryTableInfo.TableFields().Where((c) => (c.IsAutoIncrement())).ToList();

            return m_QueryTableInfo.TableFields().Where((c) => (c.IsAutoIncrement())).SingleOrDefault();
        }

        public QueryFieldCopy QueryTargetAutoIncrementColumn()
        {
            return QueryFields.Where((c) => (c.IsTargetAutoIncrement())).SingleOrDefault();
        }

        public IndexDefCopy IndexPK()
        {
            return m_QueryTableInfo.IndexPK();
        }

        public IList<IndexDefCopy> IndexesNonPK()
        {
            return m_QueryTableInfo.IndexesNonPK();
        }

        public IList<RelationDefCopy> Relations()
        {
            return m_QueryTableInfo.Relations();
        }

        public TableFieldCopy CreateTargetFAUTO(string lpszName, int nType, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = TableDefInfo.CreateFAUTOInfo(lpszName, nType, versFrom, versDrop);

            TableFieldCopy fieldClone = new TableFieldCopy(null, fieldInfo);

            return m_QueryTableInfo.FieldInsertBeg(fieldClone);
        }
        public void CreateTargetIndexFromXPK(IndexDefCopy primaryKey)
        {
            m_QueryTableInfo.CreateTargetIndexFromXPK(primaryKey);
        }

        public IndexDefCopy CreateTargetIndexFromXPK(IndexDefCopy pkConstraint, string oldAutoName, string newAutoName)
        {
            return m_QueryTableInfo.CreateTargetIndexFromXPK(pkConstraint, oldAutoName, newAutoName);
        }

        public IndexDefCopy CreatePKAutoConstraint(string indexName, string idColName)
        {
            return m_QueryTableInfo.CreatePKAutoConstraint(indexName, idColName);
        }
        public IList<TableFieldCopy> TableColumnsForVersion(UInt32 versCreate)
        {
            return m_QueryTableInfo.TableColumnsForVersion(versCreate);
        }
        public string TableSourceName()
        {
            return string.Join(" ", new string[] { TableName, AliasName });
        }

    }
}
