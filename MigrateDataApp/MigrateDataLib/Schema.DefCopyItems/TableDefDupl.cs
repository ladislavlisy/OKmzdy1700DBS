using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableDefDupl : ICloneable
    {
        public const string EMPTY_STRING = "";

        protected IList<QueryTableCopy> m_QueryTableInfo;
        protected IList<QueryJoinsInfo> m_QueryJoinsInfo;
        protected IList<QueryFiltrInfo> m_QueryFiltrInfo;
        protected IList<QueryCloseInfo> m_QueryCloseInfo;
        protected IList<RelationDefInfo> m_TableRelations;
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
        public IList<TableFieldCopy> DuplColumnsForVersion(UInt32 versCreate)
        {
            return m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((v) => (v.IsValidInVersion(versCreate))).Select((f) => f.QueryColumnInfo()))).ToList();
        }
        public IList<RelationDefInfo> Relations()
        {
            return m_TableRelations;
        }

        public IList<RelationDefInfo> ForeignRelations(IList<TableDefDupl> tables)
        {
            return tables.SelectMany((m) => (m.Relations().Where((r) => (r.SourceTableName.CompareTo(TableName()) == 0)))).ToList();
        }

        public TableFieldCopy TargetFieldByName(string columnName, UInt32 versCreate)
        {
            IList<TableFieldCopy> columnList = m_QueryTableInfo.SelectMany((t) => (t.TableColumnsForVersion(versCreate))).ToList();

            TableFieldCopy column = columnList.Where((c) => (c.TargetColumnName().CompareNoCase(columnName))).SingleOrDefault();

            return column;
        }

        public TableDefDupl(TableDefInfo tableInfo, UInt32 versCreate)
        {
            this.m_QueryTableInfo = new List<QueryTableCopy>()
            {
                new QueryTableCopy(tableInfo.TableName(), tableInfo, versCreate)
            };
            this.m_QueryJoinsInfo = new List<QueryJoinsInfo>();
            this.m_QueryFiltrInfo = new List<QueryFiltrInfo>();
            this.m_QueryCloseInfo = new List<QueryCloseInfo>();
            this.m_TableRelations = new List<RelationDefInfo>();
        }
        public TableDefDupl(string ownerName, string usersName, string queryName, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            this.m_QueryTableInfo = new List<QueryTableCopy>();
            this.m_QueryJoinsInfo = new List<QueryJoinsInfo>();
            this.m_QueryFiltrInfo = new List<QueryFiltrInfo>();
            this.m_QueryCloseInfo = new List<QueryCloseInfo>();
            this.m_TableRelations = new List<RelationDefInfo>();
        }
        public void SetQueryTableInfo(IList<QueryTableCopy> tableList)
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

        public void AddTable(QueryTableCopy tableAliasInfo)
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

        public IList<QueryTableCopy> QueryTableList()
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

        public IList<string> QueryColumnsNamesForVersion(uint versCreate)
        {
            IList<QueryFieldCopy> tableColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)))).ToList();
            IList<string> queryColumnList = tableColumnList.Select((s) => (s.SourceQueryColumnName())).ToList();

            return queryColumnList;
        }
        public IList<string> QueryAliasNamesForVersion(uint versCreate)
        {
            IList<QueryFieldCopy> tableColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)))).ToList();
            IList<string> queryColumnList = tableColumnList.Select((s) => (s.SourceAliasName())).ToList();

            return queryColumnList;
        }

        public IList<string> TableAliasNamesForVersion(uint versCreate)
        {
            IList<string> queryColumnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)).Select((c) => (c.SourceColumnInfoSource(t.AliasName))))).ToList();

            return queryColumnList;
        }

        public IList<string> TableColumnsSourceForVersion(uint versCreate)
        {
            IList<string> columnList = m_QueryTableInfo.SelectMany((t) => (t.QueryFields.Where((f) => f.IsValidInVersion(versCreate)).
                Select((s) => (s.SourceColumnInfoSource(t.AliasName))))).ToList();

            return columnList;
        }

        public QueryTableCopy QueryTableByAlias(string tableAlias)
        {
            QueryTableCopy queryTable = m_QueryTableInfo.Where((c) => (c.AliasName.Equals(tableAlias))).SingleOrDefault();

            return queryTable;
        }

        public IList<QueryJoinsInfo> QueryTableJoins()
        {
            IList<QueryJoinsInfo> tableJoinList = m_QueryJoinsInfo.Where((t) => (t.JoinCondition == true)).ToList();

            return tableJoinList;
        }
        public IList<string> QueryTableFroms()
        {
            return m_QueryTableInfo.Select((t) => (t. TableSourceName())).ToList();
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
            return m_strTableName;
        }

        public string CreateTargetColumnList(UInt32 versCreate)
        {
            IList<TableFieldCopy> columnCopy = DuplColumnsForVersion(versCreate);

            string columnsList = "";

            foreach (TableFieldCopy field in columnCopy)
            {
                if (field != null && field.IsValidInVersion(versCreate))
                {
                    IList<string> columnNames = field.TargetAllColumnNames();

                    foreach (string columnName in columnNames)
                    {
                        columnsList += columnName;
                        columnsList += ", ";
                    }
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }

        #region ICloneable Members

        public object Clone()
        {
            TableDefDupl other = (TableDefDupl)this.MemberwiseClone();
            other.m_QueryTableInfo = this.m_QueryTableInfo.ToList();
            other.m_QueryJoinsInfo = this.m_QueryJoinsInfo.ToList();
            other.m_QueryFiltrInfo = this.m_QueryFiltrInfo.ToList();
            other.m_QueryCloseInfo = this.m_QueryCloseInfo.ToList();
            other.m_TableRelations = this.m_TableRelations.ToList();

            return other;
        }

        #endregion
    }
}
