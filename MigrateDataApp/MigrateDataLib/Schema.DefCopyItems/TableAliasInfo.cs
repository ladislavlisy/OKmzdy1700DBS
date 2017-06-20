using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableAliasInfo : ICloneable
    {
        protected string m_TableName;
        protected string m_AliasName;

        public TableAliasInfo(string tableName)
        {
            this.m_TableName = tableName;
            this.m_AliasName = tableName;
        }

        public TableAliasInfo(TableDefInfo tableInfo)
        {
            this.m_TableName = tableInfo.TableName();
            this.m_AliasName = tableInfo.TableName();
        }

        public string TableName()
        {
            return m_TableName;
        }

        public string AliasName()
        {
            return m_AliasName;
        }

        public string TableAliasName()
        {
            return string.Join(" ", new string[] { m_TableName, m_AliasName });
        }

        #region ICloneable Members
        public object Clone()
        {
            TableAliasInfo other = (TableAliasInfo)this.MemberwiseClone();
            other.m_TableName = this.m_TableName;
            other.m_AliasName = this.m_AliasName;

            return other;
        }

        #endregion
    }
}
