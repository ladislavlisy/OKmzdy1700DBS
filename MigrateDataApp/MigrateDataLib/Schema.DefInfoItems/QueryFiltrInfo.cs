using MigrateDataLib.Schema.DefInfoItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryFiltrInfo : ICloneable
    {
        public static QueryFiltrInfo GetQueryFiltrInfo(string aliasName, TableDefInfo tableInfo)
        {
            return new QueryFiltrInfo(aliasName, tableInfo);
        }
        public QueryFiltrInfo AddConstraint(FiltrSpecsInfo constraint)
        {
            QueryFiltrInfo other = (QueryFiltrInfo)this.MemberwiseClone();
            other.m_QueryTableInfo = this.m_QueryTableInfo;
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;
            other.FiltrSpecs = this.FiltrSpecs.Concat(new List<FiltrSpecsInfo>() { constraint }).ToList();

            return other;
        }
        public QueryFiltrInfo AddConstraints(params FiltrSpecsInfo[] constraints)
        {
            QueryFiltrInfo other = (QueryFiltrInfo)this.MemberwiseClone();
            other.m_QueryTableInfo = this.m_QueryTableInfo;
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;

            other.FiltrSpecs = this.FiltrSpecs.Concat(constraints).ToList();
            return other;
        }
        public QueryFiltrInfo(string aliasName, TableDefInfo tableInfo)
        {
            m_QueryTableInfo = tableInfo;
            FiltrSpecs = new List<FiltrSpecsInfo>();
            AliasName = aliasName;
            TableName = tableInfo.TableName();
        }

        private TableDefInfo m_QueryTableInfo;
        public IList<FiltrSpecsInfo> FiltrSpecs;
        public string AliasName;
        public string TableName;

        public bool IsValidInVersion(UInt32 versCreate)
        {
            if (m_QueryTableInfo == null)
            {
                return false;
            }
            return (m_QueryTableInfo.IsValidInVersion(versCreate));
        }
        public TableDefInfo QueryTableInfo()
        {
            return (TableDefInfo)m_QueryTableInfo.Clone();
        }

        public IList<FiltrSpecsInfo> FiltrSpecsList()
        {
            return FiltrSpecs.ToList();
        }

        public void SetQueryFiltersList(IList<FiltrSpecsInfo> specsList)
        {
            FiltrSpecs = specsList;
        }

        public string TableSourceName()
        {
            return string.Join(" ", new string[] { TableName, AliasName });
        }

        public IList<string> QueryFilterCondition()
        {
            return FiltrSpecs.Select((f) => (f.QueryFilterCondition(AliasName))).ToList();
        }

        public object Clone()
        {
            QueryFiltrInfo other = (QueryFiltrInfo)this.MemberwiseClone();
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;
            other.FiltrSpecs = this.FiltrSpecs.ToList();

            return other;
        }

    }
}
