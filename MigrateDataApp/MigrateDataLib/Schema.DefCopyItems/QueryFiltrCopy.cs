using System;
using System.Linq;
using System.Collections.Generic;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class QueryFiltrCopy : ICloneable
    {
        private TableDefCopy m_QueryTableInfo;
        public IList<FiltrSpecsCopy> FiltrSpecs;
        public string AliasName;
        public string TableName;

        public QueryFiltrCopy(QueryFiltrInfo defInfo, UInt32 versCreate)
        {
            this.m_QueryTableInfo = new TableDefCopy(defInfo.QueryTableInfo(), versCreate);
            this.FiltrSpecs = defInfo.FiltrSpecs.Select((qf) => new FiltrSpecsCopy(qf)).ToList();
            this.AliasName = defInfo.AliasName;
            this.TableName = defInfo.TableName;
        }

        public QueryFiltrInfo GetSourceInfo()
        {
            QueryFiltrInfo defInfo = new QueryFiltrInfo(this.AliasName, this.m_QueryTableInfo.GetSourceInfo());
            defInfo.SetQueryFiltersList(this.FiltrSpecs.Select((qf) => (qf.GetSourceInfo())).ToList());

            return defInfo;
        }

        public QueryFiltrInfo GetTargetInfo()
        {
            QueryFiltrInfo defInfo = new QueryFiltrInfo(this.AliasName, this.m_QueryTableInfo.GetTargetInfo());
            defInfo.SetQueryFiltersList(this.FiltrSpecs.Select((qf) => (qf.GetTargetInfo())).ToList());

            return defInfo;
        }

        #region ICloneable Members

        public object Clone()
        {
            QueryFiltrCopy other = (QueryFiltrCopy)this.MemberwiseClone();
            other.m_QueryTableInfo = (TableDefCopy)this.m_QueryTableInfo.Clone();
            other.FiltrSpecs = this.FiltrSpecs.Select((idxf) => ((FiltrSpecsCopy)idxf.Clone())).ToList();
            other.AliasName = this.AliasName;
            other.TableName = this.TableName;

            return other;
        }

        #endregion
    }
}
