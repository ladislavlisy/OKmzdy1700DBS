using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class RelationFieldCopy : ICloneable
    {
        RelationFieldInfo m_source;
        RelationFieldInfo m_target;
        public RelationFieldCopy(RelationFieldInfo fieldInfo)
        {
            m_source = (RelationFieldInfo)fieldInfo.Clone();
            m_target = (RelationFieldInfo)fieldInfo.Clone();
        }
        public RelationFieldCopy(RelationFieldInfo sourceField, RelationFieldInfo targetField)
        {
            m_source = null;
            m_target = null;
            if (sourceField != null)
            {
                m_source = (RelationFieldInfo)sourceField.Clone();
            }
            if (targetField != null)
            {
                m_target = (RelationFieldInfo)targetField.Clone();
            }
        }
        public string SourceName()
        {
            if (m_source != null)
            {
                return m_source.SourceName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public string TargetName()
        {
            if (m_target != null)
            {
                return m_target.SourceName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public string SourceForeignName()
        {
            if (m_source != null)
            {
                return m_source.ForeignName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public string TargetForeignName()
        {
            if (m_target != null)
            {
                return m_target.ForeignName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public RelationFieldInfo GetSourceInfo()
        {
            if (m_source != null)
            {
                return (RelationFieldInfo)m_source.Clone();
            }
            return null;
        }
        public RelationFieldInfo GetTargetInfo()
        {
            if (m_target != null)
            {
                return (RelationFieldInfo)m_target.Clone();
            }
            return null;
        }
        public void SetTargetName(string newColumnName)
        {
            if (m_target != null)
            {
                m_target.SourceName = newColumnName;
            }
        }

        public object Clone()
        {
            RelationFieldCopy other = (RelationFieldCopy)this.MemberwiseClone();
            other.m_source = GetSourceInfo();
            other.m_target = GetTargetInfo();

            return other;
        }
    }
}
