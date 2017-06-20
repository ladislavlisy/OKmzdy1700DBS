using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryDefInfo : ICloneable
    {
        protected IList<QueryTableInfo> m_QueryTableInfo;
        protected IList<QueryJoinsInfo> m_QueryJoinsInfo;
        protected IList<QueryFiltrInfo> m_QueryFiltrInfo;
        protected IList<QueryCloseInfo> m_QueryCloseInfo;

        protected string m_strOwnerName;
        protected string m_strUsersName;
        protected UInt32 m_VersFrom = 0;
        protected UInt32 m_VersDrop = 9999;
        public string QueryName;

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

        public QueryDefInfo(string ownerName, string usersName, string queryName, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            this.m_strOwnerName = ownerName;
            this.m_strUsersName = usersName;
            this.QueryName = queryName;
            this.m_VersFrom = versFrom;
            this.m_VersDrop = versDrop;

            this.m_QueryTableInfo = new List<QueryTableInfo>();
            this.m_QueryJoinsInfo = new List<QueryJoinsInfo>();
            this.m_QueryFiltrInfo = new List<QueryFiltrInfo>();
            this.m_QueryCloseInfo = new List<QueryCloseInfo>();
        }
        public void SetQueryTableInfo(IList<QueryTableInfo> tableList)
        {
            m_QueryTableInfo = tableList;
        }

        public void SetQueryJoinsInfo(IList<QueryJoinsInfo> joinsList)
        {
            m_QueryJoinsInfo = joinsList;
        }

        public void SetQueryFiltrInfo(IList<QueryFiltrInfo> filterList)
        {
            m_QueryFiltrInfo = filterList;
        }

        public void SetQueryCloseInfo(IList<QueryCloseInfo> queryClose)
        {
            m_QueryCloseInfo = queryClose;
        }

        public void AddTable(QueryTableInfo tableAliasInfo)
        {
            this.m_QueryTableInfo.Add(tableAliasInfo);
        }
        public void AddTableJoin(QueryJoinsInfo tableJoinInfo)
        {
            this.m_QueryJoinsInfo.Add(tableJoinInfo);
        }

        public void AddFiltr(QueryFiltrInfo tableWhereInfo)
        {
            this.m_QueryFiltrInfo.Add(tableWhereInfo);
        }

        public void AddClose(QueryCloseInfo queryClose)
        {
            this.m_QueryCloseInfo.Add(queryClose);
        }

        public IList<QueryTableInfo> QueryTableList()
        {
            return m_QueryTableInfo.ToList();
        }

        public IList<QueryJoinsInfo> QueryJoinsList()
        {
            return m_QueryJoinsInfo.ToList();
        }
        public IList<QueryFiltrInfo> QueryFiltrList()
        {
            return m_QueryFiltrInfo.ToList();
        }
        public IList<QueryCloseInfo> QueryCloseList()
        {
            return m_QueryCloseInfo.ToList();
        }

        public TableDefInfo GetTableDef()
        {
            TableDefInfo queryTableInfo = new TableDefInfo(m_strOwnerName, m_strUsersName, QueryName, m_VersFrom, m_VersDrop);

            IList<TableFieldInfo> queryColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Select((f) => f.QueryColumnInfo()))).ToList();
            foreach (var columnInfo in queryColumnList)
            {
                queryTableInfo.FieldAppendToQuery(columnInfo);
            }
            return queryTableInfo;
        }

        public IList<string> QueryColumnsNamesForVersion(uint versCreate)
        {
            IList<QueryFieldInfo> tableColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)))).ToList();
            IList<string> queryColumnList = tableColumnList.Select((s) => (s.QueryColumnName())).ToList();

            return queryColumnList;
        }
        public IList<string> QueryAliasNamesForVersion(uint versCreate)
        {
            IList<QueryFieldInfo> tableColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)))).ToList();
            IList<string> queryColumnList = tableColumnList.Select((s) => (s.AliasName)).ToList();

            return queryColumnList;
        }

        public IList<string> TableColumnsSourceForVersion(uint versCreate)
        {
            IList<string> columnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)).
                Select((s) => (s.ColumnInfoSource(t.AliasName))))).ToList();

            return columnList;
        }

        public QueryTableInfo QueryTableByAlias(string tableAlias)
        {
            QueryTableInfo queryTable = m_QueryTableInfo.Where((c) => (c.AliasName.Equals(tableAlias))).SingleOrDefault();

            return queryTable;
        }

        public IList<QueryJoinsInfo> QueryTableJoins()
        {
            IList<QueryJoinsInfo> tableJoinList = m_QueryJoinsInfo.Where((t) => (t.JoinCondition == true)).ToList();

            return tableJoinList;
        }
        public IList<string> QueryTableFroms()
        {
            return m_QueryTableInfo.Select((t) => (t.TableSourceName())).ToList();
        }
        public IList<QueryJoinsInfo> QueryWhereJoins(bool bFilterConds)
        {
            IList<QueryJoinsInfo> tableJoinList = m_QueryJoinsInfo.Where((t) => (t.QueryTableJoinOpConds(bFilterConds))).ToList();

            return tableJoinList;
        }

        public IList<string> FiltrConsDefinition()
        {
            var tableFiltrList = m_QueryFiltrInfo.SelectMany((t) => t.QueryFilterCondition()).ToList();

            return tableFiltrList;
        }

        public string InfoName()
        {
            return QueryName;
        }

        #region ICloneable Members

        public object Clone()
        {
            QueryDefInfo other = (QueryDefInfo)this.MemberwiseClone();
            other.m_VersFrom = this.m_VersFrom;
            other.m_VersDrop = this.m_VersDrop;
            other.QueryName = this.QueryName;
            other.m_QueryTableInfo = this.m_QueryTableInfo.ToList();
            other.m_QueryJoinsInfo = this.m_QueryJoinsInfo.ToList();
            other.m_QueryFiltrInfo = this.m_QueryFiltrInfo.ToList();
            other.m_QueryCloseInfo = this.m_QueryCloseInfo.ToList();

            return other;
        }

        #endregion
    }
}
