using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryFieldCopy : ICloneable
    {
        QueryFieldInfo m_source;
        QueryFieldInfo m_target;
        public QueryFieldCopy(QueryFieldInfo fieldInfo)
        {
            m_source = (QueryFieldInfo)fieldInfo.Clone();
            m_target = (QueryFieldInfo)fieldInfo.Clone();
        }

        public QueryFieldCopy(TableFieldInfo fieldInfo, string aliasName)
        {
            m_source = new QueryFieldInfo(fieldInfo, aliasName);
            m_target = new QueryFieldInfo(fieldInfo, aliasName);
        }

        public TableFieldCopy QueryColumnInfo()
        {
            TableFieldCopy queryColumnInfo = new TableFieldCopy(GetSourceColumnInfo(), GetTargetColumnInfo());

            if (m_source != null)
            {
                queryColumnInfo.ReNameSourceColumn(m_source.AliasName);
            }

            if (m_target != null)
            {
                queryColumnInfo.ReNameTargetColumn(m_target.AliasName);
            }

            return queryColumnInfo;
        }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            bool bSourceIsValid = true;
            bool bTargetIsValid = true;
            if (m_source != null)
            {
                bSourceIsValid = m_source.IsValidInVersion(versCreate);
            }
            if (m_target != null)
            {
                 bTargetIsValid = m_target.IsValidInVersion(versCreate);
            }
            return (bSourceIsValid && bTargetIsValid);
        }

        public QueryFieldInfo GetSourceInfo()
        {
            if (m_source != null)
            {
                return (QueryFieldInfo)m_source.Clone();
            }
            return null;
        }
        public QueryFieldInfo GetTargetInfo()
        {
            if (m_target != null)
            {
                return (QueryFieldInfo)m_target.Clone();
            }
            return null;
        }
        public string SourceAliasName()
        {
            if (m_source != null)
            {
                return m_source.AliasName;
            }
            return "";
        }
        public string TargetAliasName()
        {
            if (m_target != null)
            {
                return m_target.AliasName;
            }
            return "";
        }
        public string SourceColumnInfoSource(string tableAlias)
        {
            if (m_source != null)
            {
                return m_source.ColumnInfoSource(tableAlias);
            }
            return "";
        }
        public string TargetColumnInfoSource(string tableAlias)
        {
            if (m_target != null)
            {
                return m_target.ColumnInfoSource(tableAlias);
            }
            return "";
        }
        public string SourceQueryColumnName()
        {
            if (m_source != null)
            {
                if (m_source.TableColumnInfo == null)
                {
                    return "";
                }
                return m_source.TableColumnInfo.ColumnName;
            }
            return "";
        }
        public string TargetQueryColumnName()
        {
            if (m_target != null)
            {
                if (m_target.TableColumnInfo == null)
                {
                    return "";
                }
                return m_target.TableColumnInfo.ColumnName;
            }
            return "";
        }

        public TableFieldInfo GetSourceColumnInfo()
        {
            if (m_source != null)
            {
                return (TableFieldInfo)m_source.TableColumnInfo.Clone();
            }
            return null;
        }
        public TableFieldInfo GetTargetColumnInfo()
        {
            if (m_target != null)
            {
                return (TableFieldInfo)m_target.TableColumnInfo.Clone();
            }
            return null;
        }
        #region ICloneable Members

        public object Clone()
        {
            QueryFieldCopy other = (QueryFieldCopy)this.MemberwiseClone();
            other.m_source = GetSourceInfo();
            other.m_target = GetTargetInfo();

            return other;
        }
        #endregion

        public bool IsTargetAutoIncrement()
        {
            if (m_target != null)
            {
                return m_target.IsAutoIncrement();
            }
            return false;
        }

        public void ReNameTargetColumn(string newName)
        {
            if (m_target != null)
            {
                m_target.ReNameColumn(newName);
            }
        }
    }
}
