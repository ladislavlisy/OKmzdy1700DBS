using System;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class JoinsFieldCopy : ICloneable
    {
        JoinsFieldInfo m_source;
        JoinsFieldInfo m_target;

        public JoinsFieldCopy(JoinsFieldInfo fieldInfo)
        {
            m_source = (JoinsFieldInfo)fieldInfo.Clone();
            m_target = (JoinsFieldInfo)fieldInfo.Clone();
        }

        public JoinsFieldInfo GetSourceInfo()
        {
            if (m_source != null)
            {
                return (JoinsFieldInfo)m_source.Clone();
            }
            return null;
        }
        public JoinsFieldInfo GetTargetInfo()
        {
            if (m_target != null)
            {
                return (JoinsFieldInfo)m_target.Clone();
            }
            return null;
        }
        public bool LhrColumnOpValue()
        {
            bool sourceLhrColumnOpValue = false;
            bool targetLhrColumnOpValue = false;
            if (m_source != null)
            {
                sourceLhrColumnOpValue = m_source.LhrColumnOpValue;
            }
            if (m_target != null)
            {
                targetLhrColumnOpValue = m_source.LhrColumnOpValue;
            }
            return(sourceLhrColumnOpValue && targetLhrColumnOpValue);
        }
        
        public bool RhrColumnOpValue()
        {
            bool sourceRhrColumnOpValue = false;
            bool targetRhrColumnOpValue = false;
            if (m_source != null)
            {
                sourceRhrColumnOpValue = m_source.RhrColumnOpValue;
            }
            if (m_target != null)
            {
                targetRhrColumnOpValue = m_source.RhrColumnOpValue;
            }
            return(sourceRhrColumnOpValue && targetRhrColumnOpValue);
        }

        public bool EqualsTargetLeftColumn(string columnName)
        {
            if (m_target != null)
            {
                return m_target.TableLhrColumn.CompareNoCase(columnName);
            }
            return false;
        }
        public bool EqualsTargetRightColumn(string columnName)
        {
            if (m_target != null)
            {
                return m_target.TableRhrColumn.CompareNoCase(columnName);
            }
            return false;
        }
        #region ICloneable Members

        public object Clone()
        {
            JoinsFieldCopy other = (JoinsFieldCopy)this.MemberwiseClone();
            other.m_source = GetSourceInfo();
            other.m_target = GetTargetInfo();

            return other;
        }

        #endregion
        public void ReNameTargetLeftColumn(string oldColumnName, string newColumnName)
        {
            if (m_target != null)
            {
                m_target.ReNameLeftColumnName(oldColumnName, newColumnName);
            }
        }

        public void ReNameTargetRightColumn(string oldColumnName, string newColumnName)
        {
            if (m_target != null)
            {
                m_target.ReNameRightColumnName(oldColumnName, newColumnName);
            }
        }

    }
}
