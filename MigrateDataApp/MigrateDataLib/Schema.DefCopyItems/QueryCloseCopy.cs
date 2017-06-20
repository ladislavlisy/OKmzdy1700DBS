using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryCloseCopy : ICloneable
    {
        QueryCloseInfo m_source;
        QueryCloseInfo m_target;
        public QueryCloseCopy(QueryCloseInfo fieldInfo)
        {
            m_source = fieldInfo;
            m_target = fieldInfo;
        }
        public QueryCloseInfo GetSourceInfo()
        {
            return m_source;
        }
        public QueryCloseInfo GetTargetInfo()
        {
            return m_target;
        }
        public object Clone()
        {
            QueryCloseCopy other = (QueryCloseCopy)this.MemberwiseClone();
            other.m_source = (QueryCloseInfo)this.GetSourceInfo().Clone();
            other.m_target = (QueryCloseInfo)this.GetTargetInfo().Clone();

            return other;
        }

        public bool ContainsTargetColumn(string aliasName, string columnName)
        {
            if (m_target != null)
            {
                return m_target.CloseInfo.Contains(aliasName + "." + columnName);
            }
            return false;
        }

        public void ReNameTargetColumn(string aliasName, string oldColumnName, string newColumnName)
        {
            if (m_target != null)
            {
                m_target.ReNameColumn(aliasName, oldColumnName, newColumnName);
            }
        }
    }
}