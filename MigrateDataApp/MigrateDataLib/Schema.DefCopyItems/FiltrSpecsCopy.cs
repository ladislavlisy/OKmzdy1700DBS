using System;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class FiltrSpecsCopy : ICloneable
    {
        FiltrSpecsInfo m_source;
        FiltrSpecsInfo m_target;

        public FiltrSpecsCopy(FiltrSpecsInfo fieldInfo)
        {
            m_source = (FiltrSpecsInfo)fieldInfo.Clone();
            m_target = (FiltrSpecsInfo)fieldInfo.Clone();
        }

        public FiltrSpecsInfo GetSourceInfo()
        {
            return (FiltrSpecsInfo)m_source.Clone();
        }
        public FiltrSpecsInfo GetTargetInfo()
        {
            return (FiltrSpecsInfo)m_target.Clone();
        }
        #region ICloneable Members

        public object Clone()
        {
            FiltrSpecsCopy other = (FiltrSpecsCopy)this.MemberwiseClone();
            other.m_source = (FiltrSpecsInfo)this.m_source.Clone();
            other.m_target = (FiltrSpecsInfo)this.m_target.Clone();

            return other;
        }

        #endregion
    }
}
