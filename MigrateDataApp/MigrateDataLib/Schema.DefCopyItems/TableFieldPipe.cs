using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableFieldPipe : ICloneable
    {
        public const string EMPTY_STRING = "";

        protected TableFieldInfo m_sourceField;
        protected TableFieldInfo m_targetField;
        protected TableAliasInfo m_sourceAlias;

        public TableFieldPipe(TableFieldInfo fieldInfo, TableAliasInfo aliasInfo)
        {
            this.m_sourceField = (TableFieldInfo)fieldInfo.Clone();
            this.m_targetField = (TableFieldInfo)fieldInfo.Clone();
            this.m_sourceAlias = (TableAliasInfo)aliasInfo.Clone();
        }

        public TableFieldPipe(TableFieldInfo sourceInfo, TableFieldInfo targetInfo, TableAliasInfo aliasInfo)
        {
            this.m_sourceField = null;
            if (sourceInfo != null)
            {
                this.m_sourceField =   (TableFieldInfo)sourceInfo.Clone();;
            }
            this.m_targetField = null;
            if (targetInfo != null)
            {
                this.m_targetField = (TableFieldInfo)targetInfo.Clone();
            }
            this.m_sourceAlias = null;
            if (aliasInfo != null)
            {
                this.m_sourceAlias = (TableAliasInfo)aliasInfo.Clone();
            }
        }

        public void ReNameTargetColumn(string newName)
        {
            if (m_targetField != null)
            {
                m_targetField.ReNameColumn(newName);
            }
        }

        public TableFieldInfo GetSourceInfo()
        {
            if (m_sourceField != null)
            {
                return (TableFieldInfo)m_sourceField.Clone();
            }
            return null;
        }

        public TableFieldInfo GetTargetInfo()
        {
            if (m_targetField != null)
            {
                return (TableFieldInfo)m_targetField.Clone();
            }
            return null;
        }
        public bool AllFieldValid()
        {
            if (m_sourceField == null)
            {
                return false;
            }
            if (m_targetField == null)
            {
                return false;
            }
            return (m_sourceField.ColumnName!="" && m_targetField.ColumnName!="");
        }

        public string SourceColumnAliasName(Int32 multiIdx /*=0*/)
        {
            string columnFmtx = "";
            if (m_sourceField != null)
            {
                string columnFunc = EMPTY_STRING;
                string columnTarg = m_sourceField.TableColumnName(multiIdx);
                string columnName = m_sourceAlias.AliasName();
                columnName += ".";
                columnName += m_sourceField.TableColumnName(multiIdx);

                if (columnFunc == EMPTY_STRING)
                {
                    columnFmtx = columnName;
                }
                else
                {
                    columnFmtx = string.Format(columnFunc, columnName);
                }
                columnFmtx += " AS ";
                columnFmtx += columnTarg;
            }
            return columnFmtx;
        }

        public IList<string> SourceAllColumnAliasNames()
        {
            if (m_sourceField != null)
            {
                Int32 multiCount = Math.Max(1, m_sourceField.Multiple());
                return Enumerable.Range(1, multiCount).Select((i,c) => (SourceColumnAliasName(i))).ToList();
            }
            return new List<string>();
        }

        public IList<string> SourceAllColumnNames()
        {
            if (m_sourceField != null)
            {
                return m_sourceField.AllColumnNames();
            }
            return new List<string>();
        }

        public string SourceName()
        {
            if (m_sourceField != null)
            {
                return m_sourceField.ColumnName;
            }
            return EMPTY_STRING;
        }

        public IList<string> TargetAllColumnNames()
        {
            if (m_targetField != null)
            {
                return m_targetField.AllColumnNames();
            }
            return new List<string>();
        }

        public string TargetName()
        {
            if (m_targetField != null)
            {
                return m_targetField.ColumnName;
            }
            return EMPTY_STRING;
        }

        public bool IsSourceToTargetAutoIncrement()
        {
            if (m_sourceField != null && m_targetField != null)
            {
                return m_targetField.IsAutoIncrement();
            }
            return false;   
        }

        public bool IsTargetAutoIncrement()
        {
            if (m_targetField != null)
            {
                return m_targetField.IsAutoIncrement();
            }
            return false;   
        }

        #region ICloneable Members
        public object Clone()
        {
            TableFieldPipe other = (TableFieldPipe)this.MemberwiseClone();
            other.m_sourceField = (TableFieldInfo)this.m_sourceField.Clone();
            other.m_targetField = (TableFieldInfo)this.m_targetField.Clone();
            other.m_sourceAlias = (TableAliasInfo)this.m_sourceAlias.Clone();
            return other;
        }

        public TableFieldPipe CloneWithAlias(IList<TableAliasInfo> aliasList)
        {
            TableFieldPipe other = (TableFieldPipe)this.MemberwiseClone();
            other.m_sourceField = (TableFieldInfo)this.m_sourceField.Clone();
            other.m_targetField = (TableFieldInfo)this.m_targetField.Clone();
            other.m_sourceAlias = aliasList.Where((a) => (a.AliasName().CompareNoCase(this.m_sourceAlias.AliasName()))).SingleOrDefault();
            return other;
        }
        #endregion
    }
}
