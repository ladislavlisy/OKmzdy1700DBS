using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefCopyItems
{
    public class TableDefPipe : ICloneable
    {
        public const string EMPTY_STRING = "";

        protected string m_TableName;
        protected IndexDefInfo m_PKConstraint;
        protected IndexDefInfo m_AKConstraint;

        protected IList<TableAliasInfo> m_TableSAlias;
        protected IList<TableFieldPipe> m_TableFields;
        protected IList<RelationDefInfo> m_SourceRelat;
        protected IList<RelationDefInfo> m_TargetRelat;
        protected IList<QueryJoinsInfo> m_TableSJoins;
        protected IList<QueryFiltrInfo> m_TableSFiltr;
        protected IList<QueryCloseInfo> m_TableSClose;
        public TableDefPipe(TableDefInfo tableInfo, UInt32 versCreate)
        {
            this.m_TableName = tableInfo.TableName();

            this.m_PKConstraint = null;
            if (tableInfo.IndexPK() != null)
            {
                this.m_PKConstraint = (IndexDefInfo)(tableInfo.IndexPK().Clone());
            }
            this.m_AKConstraint = null;
            if (tableInfo.IndexAK() != null)
            {
                this.m_AKConstraint = (IndexDefInfo)(tableInfo.IndexAK().Clone());
            }
            TableAliasInfo aliasInfo = new TableAliasInfo(tableInfo);

            this.m_TableSAlias = new List<TableAliasInfo>() { aliasInfo };
            this.m_TableFields = tableInfo.TableColumnsForVersion(versCreate).Select((c) => (new TableFieldPipe(c, aliasInfo))).ToList();
            this.m_SourceRelat = tableInfo.Relations().Select((c) => ((RelationDefInfo) c.Clone())).ToList();
            this.m_TargetRelat = tableInfo.Relations().Select((c) => ((RelationDefInfo) c.Clone())).ToList();
            this.m_TableSJoins = new List<QueryJoinsInfo>();
            this.m_TableSFiltr = new List<QueryFiltrInfo>();
            this.m_TableSClose = new List<QueryCloseInfo>();
        }
        public string TableName()
        {
            return m_TableName;
        }

        public IList<string> ColumnAliasNames()
        {
            return m_TableFields.SelectMany((a) => a.SourceAllColumnAliasNames()).ToList();
        }

        public IList<TableFieldPipe> TableColumnList()
        {
            return m_TableFields.ToList();
        }

        public TableFieldPipe ColumnBySourceName(string findName)
        {
            return m_TableFields.SingleOrDefault((c) => (c.SourceName().CompareNoCase(findName)));
        }
        public TableFieldPipe ColumnByTargetName(string findName)
        {
            return m_TableFields.SingleOrDefault((c) => (c.TargetName().CompareNoCase(findName)));
        }

        public string CreateSourceToTargetColumnList()
        {
            string columnsList = "";

            foreach (TableFieldPipe field in m_TableFields)
            {
                if (field.GetSourceInfo() != null && field.GetTargetInfo() != null)
                {
                    TableFieldInfo tableInfo = field.GetSourceInfo();
                    bool bIsValidForXSelect = tableInfo.IncludeColumnType();

                    if (bIsValidForXSelect)
                    {
                        IList<string> columnNames = field.TargetAllColumnNames();

                        foreach (string columnName in columnNames)
                        {
                            columnsList += columnName;
                            columnsList += ", ";
                        }
                    }
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }

        public string CreateTargetColumnList()
        {
            string columnsList = "";

            foreach (TableFieldPipe field in m_TableFields)
            {
                if (field.GetTargetInfo() != null)
                {
                    IList<string> columnNames = field.TargetAllColumnNames();

                    foreach (string columnName in columnNames)
                    {
                        columnsList += columnName;
                        columnsList += ", ";
                    }
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }

        public IndexDefInfo TargetIndexPK()
        {
            return m_PKConstraint;
        }
        public string TargetPKUniqueAllNames()
        {
            return m_TableName + "." + string.Join(".", m_PKConstraint.IndexFields().Select((f) => (f.ColumnName)).ToList());
        }
        public IndexDefInfo TargetIndexAK()
        {
            return m_AKConstraint;
        }
        public string TargetAKUniqueAllNames()
        {
            return m_TableName + "." + string.Join(".", m_AKConstraint.IndexFields().Select((f) => (f.ColumnName)).ToList());
        }
        public IndexDefInfo CreatePKAutoConstraint(string lpszName, string lpszIdName)
        {
            string constraintName = lpszName + m_TableName;

            m_PKConstraint = new IndexDefInfo(constraintName, m_TableName, true);
            m_PKConstraint.AppendField(lpszIdName);

            return m_PKConstraint;
        }
        public IndexDefInfo CreateTargetIndexFromXPK(IndexDefInfo primaryKey)
        {
            m_AKConstraint = null;

            if (primaryKey != null)
            {
                m_AKConstraint = (IndexDefInfo)primaryKey.Clone();

                string indexName = m_AKConstraint.IndexName.Replace("XPK", "XAK");

                m_AKConstraint.IndexName = indexName;

                m_AKConstraint.Primary = false;

                m_AKConstraint.Unique = true;
            }
            return m_AKConstraint;
        }

        public IndexDefInfo CreateTargetIndexFromXPK(IndexDefInfo primaryKey, string oldAutoName, string newAutoName)
        {
            m_AKConstraint = null;

            if (primaryKey != null)
            {
                m_AKConstraint = (IndexDefInfo)primaryKey.Clone();

                string indexName = m_AKConstraint.IndexName.Replace("XPK", "XAK");

                m_AKConstraint.IndexName = indexName;

                m_AKConstraint.Primary = false;

                m_AKConstraint.Unique = true;

                m_AKConstraint.ReNameColumn(oldAutoName, newAutoName);
            }
            return m_AKConstraint;
        }

        public IList<RelationDefInfo> SourceRelations()
        {
            return m_SourceRelat;
        }

        public IList<RelationDefInfo> TargetRelations()
        {
            return m_TargetRelat;
        }

        public IList<RelationDefInfo> SourceForeignRelations(IList<TableDefPipe> tables)
        {
            return tables.SelectMany((m) => (m.SourceRelations().Where((r) => (r.SourceTableName.CompareTo(m_TableName) == 0)))).ToList();
        }

        public IList<RelationDefInfo> TargetForeignRelations(IList<TableDefPipe> tables)
        {
            return tables.SelectMany((m) => (m.TargetRelations().Where((r) => (r.SourceTableName.CompareTo(m_TableName) == 0)))).ToList();
        }

        public IList<QueryJoinsInfo> SourceQueryJoinsList()
        {
            return m_TableSJoins.ToList();
        }
        public IList<QueryFiltrInfo> SourceQueryFiltrList()
        {
            return m_TableSFiltr.ToList();
        }
        public IList<QueryCloseInfo> SourceQueryCloseList()
        {
            return m_TableSClose.ToList();
        }

        public IList<QueryJoinsInfo> SourceTableJoins()
        {
            IList<QueryJoinsInfo> tableJoinList = m_TableSJoins.Where((t) => (t.JoinCondition == true)).ToList();

            return tableJoinList;
        }
        public IList<string> SourceTableFroms()
        {
            return m_TableSAlias.Select((t) => (t.TableAliasName())).ToList();
        }

        public TableAliasInfo TableInfoByAlias(string tableAlias)
        {
            TableAliasInfo aliasTable = m_TableSAlias.Where((c) => (c.AliasName().Equals(tableAlias))).SingleOrDefault();

            return aliasTable;
        }

        public IList<QueryJoinsInfo> SourceWhereJoins(bool bFilterConds)
        {
            IList<QueryJoinsInfo> tableJoinList = m_TableSJoins.Where((t) => (t.QueryTableJoinOpConds(bFilterConds))).ToList();

            return tableJoinList;
        }

        public IList<string> FiltrConsDefinition()
        {
            var tableFiltrList = m_TableSFiltr.SelectMany((t) => t.QueryFilterCondition()).ToList();

            return tableFiltrList;
        }

        public TableFieldPipe CreateFAUTO(string lpszName, int nType)
        {
            TableFieldInfo fieldInfo = TableDefInfo.CreateFAUTOInfo(lpszName, nType);

            return FieldInsertBeg(fieldInfo);
        }

        public TableFieldPipe CreateField(string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = TableDefInfo.CreateFieldInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            return FieldAppend(fieldInfo);
        }


        private TableFieldPipe FieldInsertBeg(TableFieldInfo fieldInfo)
        {
            TableFieldPipe fieldPipe = new TableFieldPipe(null, fieldInfo, new TableAliasInfo(m_TableName));

            m_TableFields.Insert(0, fieldPipe);

            return fieldPipe;
        }

        public TableFieldPipe FieldAppend(TableFieldInfo fieldInfo)
        {
            TableFieldPipe fieldPipe = new TableFieldPipe(null, fieldInfo, new TableAliasInfo(m_TableName));

            m_TableFields.Add(fieldPipe);

            return fieldPipe;
        }

        public TableFieldPipe GetTargetAutoIncrementColumn()
        {
            return m_TableFields.Where((c) => (c.IsTargetAutoIncrement())).SingleOrDefault();
        }

        #region ICloneable Members
        public object Clone()
        {
            TableDefPipe other = (TableDefPipe)this.MemberwiseClone();
            other.m_TableSAlias = this.m_TableSAlias.Select((f) => ((TableAliasInfo)f.Clone())).ToList();
            other.m_TableFields = this.m_TableFields.Select((f) => ((TableFieldPipe)f.CloneWithAlias(other.m_TableSAlias))).ToList();
            other.m_TableSJoins = this.m_TableSJoins.Select((f) => ((QueryJoinsInfo)f.Clone())).ToList();
            other.m_TableSFiltr = this.m_TableSFiltr.Select((f) => ((QueryFiltrInfo)f.Clone())).ToList();
            other.m_TableSClose = this.m_TableSClose.Select((f) => ((QueryCloseInfo)f.Clone())).ToList();
            other.m_SourceRelat = this.m_SourceRelat.Select((f) => ((RelationDefInfo)f.Clone())).ToList();
            other.m_TargetRelat = this.m_TargetRelat.Select((f) => ((RelationDefInfo)f.Clone())).ToList();

            return other;
        }

        #endregion
    }
}
