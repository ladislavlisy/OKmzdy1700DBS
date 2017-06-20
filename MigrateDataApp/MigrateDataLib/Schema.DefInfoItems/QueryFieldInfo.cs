using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryFieldInfo : ICloneable
    {
        public QueryFieldInfo(TableFieldInfo fieldInfo, string aliasName)
        {
            TableColumnInfo = (TableFieldInfo)fieldInfo.Clone();
            AliasName = aliasName;
            ValueFunc = DatabaseDef.EMPTY_STRING;
        }

        public QueryFieldInfo(TableFieldInfo fieldInfo, string aliasName, string valueFunc)
        {
            TableColumnInfo = (TableFieldInfo)fieldInfo.Clone();
            AliasName = aliasName;
            ValueFunc = valueFunc;
        }

        public TableFieldInfo TableColumnInfo;
        public string AliasName;
        public string ValueFunc;

        public bool IsValidInVersion(UInt32 versCreate)
        {
            if (TableColumnInfo == null)
            {
                return false;
            }
            return (TableColumnInfo.IsValidInVersion(versCreate));
        }
        public string QueryColumnName()
        {
            if (TableColumnInfo == null)
            {
                return "";
            }
            return TableColumnInfo.ColumnName;
        }

        public TableFieldInfo QueryColumnInfo()
        {
            TableFieldInfo queryColumnInfo = (TableFieldInfo)TableColumnInfo.Clone();

            queryColumnInfo.ColumnName = AliasName;

            return queryColumnInfo;
        }

        public bool IsAutoIncrement()
        {
            if (TableColumnInfo == null)
            {
                return false;
            }
            return TableColumnInfo.IsAutoIncrement();
        }

        public string ColumnInfoSource(string tableAlias)
        {
            string columnFmtx = "";
            string columnName = tableAlias;
            columnName += ".";
            columnName += QueryColumnName();

            if (ValueFunc == "")
            {
                columnFmtx = columnName;
            }
            else
            {
                columnFmtx = string.Format(ValueFunc, columnName);
            }
            columnFmtx += " AS ";
            columnFmtx += AliasName;
            return columnFmtx;
        }

        public void ReNameColumn(string newName)
        {
            if (TableColumnInfo == null)
            {
                return;
            }
            TableColumnInfo.ReNameColumn(newName);
        }

        public object Clone()
        {
            QueryFieldInfo other = (QueryFieldInfo)this.MemberwiseClone();
            other.TableColumnInfo = (TableFieldInfo)this.TableColumnInfo.Clone();
            other.AliasName = (string)this.AliasName.Clone();
            other.ValueFunc = (string)this.ValueFunc.Clone();

            return other;
        }
    }
}
