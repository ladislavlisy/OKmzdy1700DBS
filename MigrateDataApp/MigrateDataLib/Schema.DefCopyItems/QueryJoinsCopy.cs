using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryJoinsCopy : ICloneable
    {
        private IList<JoinsFieldCopy> m_JoinsFieldInfo;
        public bool JoinCondition;
        public bool LeftCondition;
        public string LhrAliasName;
        public string RhrAliasName;

        public QueryJoinsCopy(QueryJoinsInfo defInfo)
        {
            this.m_JoinsFieldInfo = defInfo.QueryTableJoinFields().Select((jf) => (new JoinsFieldCopy(jf))).ToList();
            this.JoinCondition = defInfo.JoinCondition;
            this.LeftCondition = defInfo.LeftCondition;
            this.LhrAliasName = defInfo.LhrAliasName;
            this.RhrAliasName = defInfo.RhrAliasName;
        }

        public QueryJoinsInfo GetSourceInfo()
        {
            QueryJoinsInfo defInfo = new QueryJoinsInfo(this.LhrAliasName, this.RhrAliasName, this.JoinCondition, this.LeftCondition);
            defInfo.SetQueryJoinFieldList(this.m_JoinsFieldInfo.Select((qj) => (qj.GetSourceInfo())).ToList());

            return defInfo;
        }

        public QueryJoinsInfo GetTargetInfo()
        {
            QueryJoinsInfo defInfo = new QueryJoinsInfo(this.LhrAliasName, this.RhrAliasName, this.JoinCondition, this.LeftCondition);
            defInfo.SetQueryJoinFieldList(this.m_JoinsFieldInfo.Select((qj) => (qj.GetTargetInfo())).ToList());

            return defInfo;
        }

        public IList<JoinsFieldCopy> JoinsFieldInfo()
        {
            return m_JoinsFieldInfo;
        }

        public bool QueryTableJoinOpConds(bool bFilterConds)
        {
            bool bOperatorConds = m_JoinsFieldInfo.Any((f) => (f.LhrColumnOpValue() || f.RhrColumnOpValue()));

            return (JoinCondition == false) || (bFilterConds && bOperatorConds);
        }

        #region ICloneable Members

        public object Clone()
        {
            QueryJoinsCopy other = (QueryJoinsCopy)this.MemberwiseClone();
            other.m_JoinsFieldInfo = this.m_JoinsFieldInfo.Select((jf) => ((JoinsFieldCopy)jf.Clone())).ToList();
            other.JoinCondition = this.JoinCondition;
            other.LeftCondition = this.LeftCondition;
            other.LhrAliasName = this.LhrAliasName;
            other.RhrAliasName = this.RhrAliasName;

            return other;
        }

        #endregion
    }
}
