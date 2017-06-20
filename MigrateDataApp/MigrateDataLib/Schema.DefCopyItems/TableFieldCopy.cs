using System;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefInfoItems;
using System.Collections.Generic;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableFieldCopy : ICloneable
    {
        TableFieldInfo m_source;
        TableFieldInfo m_target;

        public TableFieldCopy(TableFieldInfo fieldInfo)
        {
            m_source = (TableFieldInfo)fieldInfo.Clone();
            m_target = (TableFieldInfo)fieldInfo.Clone();
        }

        public TableFieldCopy(TableFieldInfo sourceField, TableFieldInfo targetField)
        {
            m_source = null;
            m_target = null;
            if (sourceField != null)
            {
                m_source = (TableFieldInfo)sourceField.Clone();
            }
            if (targetField != null)
            {
                m_target = (TableFieldInfo)targetField.Clone();
            }
        }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            if (m_source == null)
            {
                return false;
            }
            if (m_target == null)
            {
                return false;
            }
            return (m_source.IsValidInVersion(versCreate) && m_target.IsValidInVersion(versCreate));
        }
        public IList<string> SourceAllColumnNames()
        {
            if (m_source != null)
            {
                return m_source.AllColumnNames();
            }
            return new List<string>();
        }
        public string SourceColumnName()
        {
            if (m_source != null)
            {
                return m_source.ColumnName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public IList<string> TargetAllColumnNames()
        {
            if (m_target != null)
            {
                return m_target.AllColumnNames();
            }
            return new List<string>();
        }
        public string TargetColumnName()
        {
            if (m_target != null)
            {
                return m_target.ColumnName;
            }
            return DatabaseDef.EMPTY_STRING;
        }
        public TableFieldInfo GetSourceInfo()
        {
            if (m_source != null)
            {
                return (TableFieldInfo)m_source.Clone();
            }
            return null;
        }
        public TableFieldInfo GetTargetInfo()
        {
            if (m_target != null)
            {
                return (TableFieldInfo)m_target.Clone();
            }
            return null;
        }
        public bool IsAutoIncrement()
        {
            if (m_source != null)
            {
                return m_source.IsAutoIncrement();
            }
            return false;
        }

        public void ReNameSourceColumn(string newName)
        {
            if (m_source != null)
            {
                m_source.ReNameColumn(newName);
            }
        }

        public void ReNameTargetColumn(string newName)
        {
            if (m_target != null)
            {
                m_target.ReNameColumn(newName);
            }
        }

        public object Clone()
        {
            TableFieldCopy other = (TableFieldCopy)this.MemberwiseClone();
            other.m_source = (TableFieldInfo)this.m_source.Clone();
            other.m_target = (TableFieldInfo)this.m_target.Clone();

            return other;
        }
    }
}