using MigrateDataLib.Schema.DefCopyItems;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryJoinsInfo : ICloneable
    {
        public static QueryJoinsInfo GetQueryFirstJoinDefInfo(string leftAlias, string rightAlias)
        {
            return new QueryJoinsInfo(leftAlias, rightAlias, true, true);
        }
        public static QueryJoinsInfo GetQueryJoinsInfo(string leftAlias, string rightAlias)
        {
            return new QueryJoinsInfo(leftAlias, rightAlias, true, false);
        }
        public static QueryJoinsInfo GetWhereJoinDefInfo(string leftAlias, string rightAlias)
        {
            return new QueryJoinsInfo(leftAlias, rightAlias, false, false);
        }
        public QueryJoinsInfo AddColumn(string leftColumnName, string rightColumnName)
        {
            QueryJoinsInfo other = (QueryJoinsInfo)this.MemberwiseClone();
            other.LhrAliasName = this.LhrAliasName;
            other.RhrAliasName = this.RhrAliasName;
            JoinsFieldInfo addJoinColumns = new JoinsFieldInfo(leftColumnName, rightColumnName);
            other.m_JoinsFieldInfo = this.m_JoinsFieldInfo.Concat(new List<JoinsFieldInfo>() { addJoinColumns }).ToList();

            return other;
        }
        public QueryJoinsInfo AddLeftColumn(string leftColumnName, string leftOp, string leftValue)
        {
            QueryJoinsInfo other = (QueryJoinsInfo)this.MemberwiseClone();
            other.LhrAliasName = this.LhrAliasName;
            other.RhrAliasName = this.RhrAliasName;
            JoinsFieldInfo addJoinColumns = new JoinsFieldInfo(true, leftColumnName, leftOp, leftValue);
            other.m_JoinsFieldInfo = this.m_JoinsFieldInfo.Concat(new List<JoinsFieldInfo>() { addJoinColumns }).ToList();

            return other;
        }
        public QueryJoinsInfo AddRightColumn(string rightColumnName, string rightOp, string rightValue)
        {
            QueryJoinsInfo other = (QueryJoinsInfo)this.MemberwiseClone();
            other.LhrAliasName = this.LhrAliasName;
            other.RhrAliasName = this.RhrAliasName;
            JoinsFieldInfo addJoinColumns = new JoinsFieldInfo(false, rightColumnName, rightOp, rightValue);
            other.m_JoinsFieldInfo = this.m_JoinsFieldInfo.Concat(new List<JoinsFieldInfo>() { addJoinColumns }).ToList();

            return other;
        }
        public QueryJoinsInfo(string leftAlias, string rightAlias, bool joinCondition, bool leftCondition)
        {
            m_JoinsFieldInfo = new List<JoinsFieldInfo>();
            JoinCondition = joinCondition;
            LeftCondition = leftCondition;
            LhrAliasName = leftAlias;
            RhrAliasName = rightAlias;
        }

        private IList<JoinsFieldInfo> m_JoinsFieldInfo;
        public bool JoinCondition;
        public bool LeftCondition;
        public string LhrAliasName;
        public string RhrAliasName;

        public IList<JoinsFieldInfo> QueryTableJoinFields()
        {
            IList<JoinsFieldInfo> joinFieldList = m_JoinsFieldInfo.ToList();

            return joinFieldList;
        }

        public void SetQueryJoinFieldList(IList<JoinsFieldInfo> fieldList)
        {
            m_JoinsFieldInfo = fieldList;
        }
        public bool QueryTableJoinOpConds(bool bFilterConds)
        {
            bool bOperatorConds = m_JoinsFieldInfo.Any((f) => (f.LhrColumnOpValue || f.RhrColumnOpValue));

            return (JoinCondition == false) || (bFilterConds && bOperatorConds);
        }

        public string QueryTableJoinConditions(QueryDefInfo queryInfo, bool bTbJoinConds, bool bFilterConds)
        {
            string strFieldNames = "";

            QueryTableInfo rightTableInfo = queryInfo.QueryTableByAlias(RhrAliasName);

            if (LeftCondition)
            {
                QueryTableInfo leftTableInfo = queryInfo.QueryTableByAlias(LhrAliasName);
                strFieldNames += leftTableInfo.TableSourceName();
                strFieldNames += " ";
            }

            strFieldNames += "INNER JOIN ";
            strFieldNames += rightTableInfo.TableSourceName();
            strFieldNames += " ON ";
            strFieldNames += QueryFieldJoinConditions(bTbJoinConds, bFilterConds);

            return strFieldNames;
        }

        public string QueryTableJoinConditions(TableDefPipe tableInfo, bool bTbJoinConds, bool bFilterConds)
        {
            string strFieldNames = "";

            TableAliasInfo rightTableInfo = tableInfo.TableInfoByAlias(RhrAliasName);

            if (LeftCondition)
            {
                TableAliasInfo leftTableInfo = tableInfo.TableInfoByAlias(LhrAliasName);
                strFieldNames += leftTableInfo.TableAliasName();
                strFieldNames += " ";
            }

            strFieldNames += "INNER JOIN ";
            strFieldNames += rightTableInfo.TableAliasName();
            strFieldNames += " ON ";
            strFieldNames += QueryFieldJoinConditions(bTbJoinConds, bFilterConds);

            return strFieldNames;
        }

        public string QueryFieldJoinConditions(bool bTbJoinConds, bool bFilterConds)
        {
            IList<string> joinConditions = m_JoinsFieldInfo.Select((f) => f.JoinCondition(LhrAliasName, RhrAliasName, bTbJoinConds, bFilterConds)).ToList();

            return string.Join(" AND ", joinConditions.Where((s) => (s.CompareNoCase("") == false)));
        }
        public string QueryTableFilterConditions(bool bTbJoinConds, bool bFilterConds)
        {
            IList<string> joinConditions = m_JoinsFieldInfo.Select((f) => f.JoinCondition(LhrAliasName, RhrAliasName, bTbJoinConds, bFilterConds)).ToList();

            return string.Join(" AND ", joinConditions.Where((s) => (s.CompareNoCase("") == false)));
        }
        public object Clone()
        {
            QueryJoinsInfo other = (QueryJoinsInfo)this.MemberwiseClone();
            other.JoinCondition = this.JoinCondition;
            other.LeftCondition = this.LeftCondition;
            other.LhrAliasName = this.LhrAliasName;
            other.RhrAliasName = this.RhrAliasName;

            return other;
        }

    }
}
