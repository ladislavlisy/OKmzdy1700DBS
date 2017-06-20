using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class IndexFieldCopy : ICloneable
    {
        IndexFieldInfo m_source;
        IndexFieldInfo m_target;
        public IndexFieldCopy(IndexFieldInfo fieldInfo)
        {
            m_source = (IndexFieldInfo)fieldInfo.Clone();
            m_target = (IndexFieldInfo)fieldInfo.Clone();
        }
        public IndexFieldCopy(IndexFieldInfo sourceField, IndexFieldInfo targetField)
        {
            m_source = null;
            m_target = null;
            if (sourceField != null)
            {
                m_source = (IndexFieldInfo)sourceField.Clone();
            }
            if (targetField != null)
            {
                m_target = (IndexFieldInfo)targetField.Clone();
            }
        }
        public string SourceName()
        {
            if (m_source != null)
            {
                return m_source.ColumnName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public string TargetName()
        {
            if (m_target != null)
            {
                return m_target.ColumnName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public IndexFieldInfo GetSourceInfo()
        {
            if (m_source != null)
            {
                return (IndexFieldInfo)m_source.Clone();
            }
            return null;
        }
        public IndexFieldInfo GetTargetInfo()
        {
            if (m_target != null)
            {
                return (IndexFieldInfo)m_target.Clone();
            }
            return null;
        }
        public void SetTargetName(string newColumnName)
        {
            if (m_target != null)
            {
                m_target.ColumnName = newColumnName;
            }
        }

        public object Clone()
        {
            IndexFieldCopy other = (IndexFieldCopy)this.MemberwiseClone();
            other.m_source = GetSourceInfo();
            other.m_target = GetTargetInfo();

            return other;
        }
    }
}
