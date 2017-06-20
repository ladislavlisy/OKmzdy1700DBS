using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryDefCopy : ICloneable
    {
        protected IList<QueryTableCopy> m_QueryTableInfo;
        protected IList<QueryJoinsCopy> m_QueryJoinsInfo;
        protected IList<QueryFiltrCopy> m_QueryFiltrInfo;
        protected IList<QueryCloseCopy> m_QueryCloseInfo;

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

        public IList<QueryTableCopy> QueryTableInfo()
        {
            return m_QueryTableInfo;
        }
        public IList<QueryCloseCopy> QueryCloseInfo()
        {
            return m_QueryCloseInfo;
        }

        public QueryDefCopy(QueryDefInfo defInfo, UInt32 versCreate)
        {
            this.m_VersFrom = defInfo.VersFrom();
            this.m_VersDrop = defInfo.VersDrop();
            this.QueryName = defInfo.QueryName;
            this.m_QueryTableInfo = defInfo.QueryTableList().Select((qt) => (new QueryTableCopy(qt, versCreate))).ToList();
            this.m_QueryJoinsInfo = defInfo.QueryJoinsList().Select((qj) => (new QueryJoinsCopy(qj))).ToList();
            this.m_QueryFiltrInfo = defInfo.QueryFiltrList().Select((qf) => (new QueryFiltrCopy(qf, versCreate))).ToList();
            this.m_QueryCloseInfo = defInfo.QueryCloseList().Select((qc) => (new QueryCloseCopy(qc))).ToList();
        }

        public QueryDefInfo GetSourceInfo()
        {
            QueryDefInfo defInfo = new QueryDefInfo(this.m_strOwnerName, this.m_strUsersName, this.QueryName, this.m_VersFrom, this.m_VersDrop);
            defInfo.SetQueryTableInfo(this.m_QueryTableInfo.Select((qt) => (qt.GetSourceInfo())).ToList());
            defInfo.SetQueryJoinsInfo(this.m_QueryJoinsInfo.Select((qj) => (qj.GetSourceInfo())).ToList());
            defInfo.SetQueryFiltrInfo(this.m_QueryFiltrInfo.Select((qf) => (qf.GetSourceInfo())).ToList());
            defInfo.SetQueryCloseInfo(this.m_QueryCloseInfo.Select((qc) => (qc.GetSourceInfo())).ToList());

            return defInfo;
        }

        public QueryDefInfo GetTargetInfo()
        {
            QueryDefInfo defInfo = new QueryDefInfo(this.m_strOwnerName, this.m_strUsersName, this.QueryName, this.m_VersFrom, this.m_VersDrop);
            defInfo.SetQueryTableInfo(this.m_QueryTableInfo.Select((qt) => (qt.GetTargetInfo())).ToList());
            defInfo.SetQueryJoinsInfo(this.m_QueryJoinsInfo.Select((qj) => (qj.GetTargetInfo())).ToList());
            defInfo.SetQueryFiltrInfo(this.m_QueryFiltrInfo.Select((qf) => (qf.GetTargetInfo())).ToList());
            defInfo.SetQueryCloseInfo(this.m_QueryCloseInfo.Select((qc) => (qc.GetTargetInfo())).ToList());

            return defInfo;
        }
        #region ICloneable Members

        public object Clone()
        {
            QueryDefCopy other = (QueryDefCopy)this.MemberwiseClone();
            other.m_VersFrom = this.m_VersFrom;
            other.m_VersDrop = this.m_VersDrop;
            other.QueryName = this.QueryName;
            other.m_QueryTableInfo = this.m_QueryTableInfo.Select((qt) => ((QueryTableCopy)qt.Clone())).ToList();
            other.m_QueryJoinsInfo = this.m_QueryJoinsInfo.Select((qj) => ((QueryJoinsCopy)qj.Clone())).ToList();
            other.m_QueryFiltrInfo = this.m_QueryFiltrInfo.Select((qf) => ((QueryFiltrCopy)qf.Clone())).ToList();
            other.m_QueryCloseInfo = this.m_QueryCloseInfo.Select((qc) => ((QueryCloseCopy)qc.Clone())).ToList();

            return other;
        }

        #endregion
        public IList<RelationDefCopy> ForeignRelations()
        {
            return m_QueryTableInfo.SelectMany((m) => (m.Relations().Where((r) => (r.SourceTableName.CompareTo(QueryName) == 0)))).ToList();
        }

        public IList<JoinsFieldCopy> LeftQueryJoinFieldInfo(string aliasName, string columnName)
        {
            IList<QueryJoinsCopy> tableJoins = m_QueryJoinsInfo.Where((qj) => (qj.LhrAliasName.CompareTo(aliasName) == 0)).ToList();

            return tableJoins.SelectMany((fj) => (fj.JoinsFieldInfo().Where((wj) => (wj.EqualsTargetLeftColumn(columnName))))).ToList();
        }
        public IList<JoinsFieldCopy> RightQueryJoinFieldInfo(string aliasName, string columnName)
        {
            IList<QueryJoinsCopy> tableJoins = m_QueryJoinsInfo.Where((qj) => (qj.RhrAliasName.CompareTo(aliasName) == 0)).ToList();

            return tableJoins.SelectMany((fj) => (fj.JoinsFieldInfo().Where((wj) => (wj.EqualsTargetRightColumn(columnName))))).ToList();
        }
        public IList<QueryCloseCopy> QueryEndClausesInfo(string aliasName, string columnName)
        {
            IList<QueryCloseCopy> tableCluses = m_QueryCloseInfo.Where((qc) => (qc.ContainsTargetColumn(aliasName, columnName))).ToList();

            return tableCluses;
        }

    }
}
