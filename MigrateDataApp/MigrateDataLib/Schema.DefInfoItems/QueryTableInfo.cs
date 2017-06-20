using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryTableInfo : ICloneable
    {
        public static QueryTableInfo GetQueryAliasDefInfo(string aliasName, TableDefInfo tableDef)
        {
            return new QueryTableInfo(aliasName, tableDef);
        }
        public QueryTableInfo AddColumn(NameAliasInfo columnName)
        {
            QueryTableInfo other = (QueryTableInfo)this.MemberwiseClone();
            other.m_QueryTableInfo = this.m_QueryTableInfo;
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;
            TableFieldInfo tabColumn = other.m_QueryTableInfo.FieldByName(columnName.OrigsName);
            QueryFieldInfo addColumn = new QueryFieldInfo(tabColumn, columnName.AliasName, columnName.Functions);
            other.QueryFields = this.QueryFields.Concat(new List<QueryFieldInfo>() { addColumn }).ToList();

            return other;
        }
        public QueryTableInfo AddColumns(params NameAliasInfo[] columnNames)
        {
            QueryTableInfo other = (QueryTableInfo)this.MemberwiseClone();
            other.m_QueryTableInfo = this.m_QueryTableInfo;
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;

            IList<QueryFieldInfo> listQueryFields = this.QueryFieldList();
            foreach (var columnName in columnNames)
            {
                TableFieldInfo tabColumn = other.m_QueryTableInfo.FieldByName(columnName.OrigsName);
                QueryFieldInfo addColumn = new QueryFieldInfo(tabColumn, columnName.AliasName, columnName.Functions);
                listQueryFields.Add(addColumn);
            }
            other.QueryFields = listQueryFields;
            return other;
        }
        public QueryTableInfo(string aliasName, TableDefInfo tableInfo)
        {
            m_QueryTableInfo = tableInfo;
            QueryFields = new List<QueryFieldInfo>();
            AliasName = aliasName;
            TableName = tableInfo.TableName();
        }

        public QueryTableInfo(string aliasName, TableDefInfo tableInfo, IList<TableFieldInfo> tableColumns)
        {
            m_QueryTableInfo = tableInfo;
            QueryFields = tableColumns.Select((tc) => (new QueryFieldInfo(tc, tc.ColumnName))).ToList();
            AliasName = aliasName;
            TableName = tableInfo.TableName();
        }

        private TableDefInfo m_QueryTableInfo;
        public IList<QueryFieldInfo> QueryFields { get; set; }
        public string AliasName { get; set; }
        public string TableName { get; set; }

        public TableDefInfo TableInfo()
        {
            return (TableDefInfo)m_QueryTableInfo.Clone();
        }
        public IList<QueryFieldInfo> QueryFieldList()
        {
            return QueryFields.ToList();
        }

        public void SetQueryFieldsList(IList<QueryFieldInfo> fieldList)
        {
            QueryFields = fieldList;
        }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            if (m_QueryTableInfo == null)
            {
                return false;
            }
            return (m_QueryTableInfo.IsValidInVersion(versCreate));
        }
        public string TableSourceName()
        {
            return string.Join(" ", new string[] { TableName, AliasName });
        }

        public object Clone()
        {
            QueryTableInfo other = (QueryTableInfo)this.MemberwiseClone();
            other.TableName = this.TableName;
            other.AliasName = this.AliasName;

            return other;
        }
    }
}
