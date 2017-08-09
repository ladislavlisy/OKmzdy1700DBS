using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class TableDefInfo : ICloneable
    {
        protected const int DB_SINGLE = DatabaseDef.DB_SINGLE;
        protected const int DB_TEXT = DatabaseDef.DB_TEXT;
        protected const int DB_BOOLEAN = DatabaseDef.DB_BOOLEAN;
        protected const int DB_BYTE = DatabaseDef.DB_BYTE;
        protected const int DB_CURRENCY = DatabaseDef.DB_CURRENCY;
        protected const int DB_DATE = DatabaseDef.DB_DATE;
        protected const int DB_DATETIME = DatabaseDef.DB_DATETIME;
        protected const int DB_DOUBLE = DatabaseDef.DB_DOUBLE;
        protected const int DB_INTEGER = DatabaseDef.DB_INTEGER;

        protected const int DB_LONG = DatabaseDef.DB_LONG;
        protected const int DB_LONGBINARY = DatabaseDef.DB_LONGBINARY;
        protected const int DB_MEMO = DatabaseDef.DB_MEMO;
        protected const int DB_GUID = DatabaseDef.DB_GUID;

        protected static bool dbNotNullFieldOption = DatabaseDef.dbNotNullFieldOption;
        protected static bool dbNullFieldOption = DatabaseDef.dbNullFieldOption;

        protected static int dbFixedField = DatabaseDef.dbFixedField;
        protected static int dbUpdatableField = DatabaseDef.dbUpdatableField;
        protected static int dbAutoIncrField = DatabaseDef.dbAutoIncrField;

        protected IList<TableFieldInfo> m_TableFields;
        protected IndexDefInfo m_PKConstraint;
        protected IndexDefInfo m_AKConstraint;
        protected IList<IndexDefInfo> m_TableIndexs;
        protected IList<RelationDefInfo> m_TableRelations;
        protected int m_nFieldsCount;
        protected string m_strOwnerName;
        protected string m_strUsersName;
        protected string m_strTableName;
        protected UInt32 m_VersFrom = 0;
        protected UInt32 m_VersDrop = 9999;

        public TableDefInfo(string ownerName, string usersName, string tableName, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            this.m_strOwnerName = ownerName;
            this.m_strUsersName = usersName;
            this.m_strTableName = tableName;
            this.m_TableFields = new List<TableFieldInfo>();
            m_PKConstraint = null;
            m_AKConstraint = null;
            this.m_TableIndexs = new List<IndexDefInfo>();
            this.m_TableRelations = new List<RelationDefInfo>();
            this.m_VersFrom = versFrom;
            this.m_VersDrop = versDrop;
        }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            return (m_VersFrom <= versCreate && versCreate < m_VersDrop);
        }

        public string OwnerName()
        {
            return m_strOwnerName;
        }

        public string UsersName()
        {
            return m_strUsersName;
        }

        public string TableName()
        {
            return m_strTableName;
        }

        public IList<TableFieldInfo> TableColumns()
        {
            return m_TableFields;
        }
        public int FieldsCount()
        {
            return m_nFieldsCount;
        }
        public IndexDefInfo IndexPK()
        {
            return m_PKConstraint;
        }

        public IndexDefInfo IndexAK()
        {
            return m_AKConstraint;
        }

        public UInt32 VersFrom()
        {
            return m_VersFrom;
        }

        public UInt32 VersDrop()
        {
            return m_VersDrop;
        }

        public IList<IndexDefInfo> IndexesNonPK()
        {
            return m_TableIndexs;
        }
        public IList<RelationDefInfo> Relations()
        {
            return m_TableRelations;
        }


        public string TableSeqName()
        {
            return ("SEQ_") + m_strTableName;
        }

        public string TableCamelName()
        {
            return m_strTableName.Underscore().Camelize();
        }

        public string ConvertTableNameToCamel()
        {
            return m_strTableName.ConvertNameToCamel();
        }

        public bool EnforceRelation(RelationDefInfo relInfo)
        {
            var relationFields = m_TableFields.Where((f) => relInfo.RelationFieldByName(f.ColumnName) != null);

            return relationFields.Any((rf) => (rf.DbColumnNull())) == false;
        }

        public virtual string PropertyName(string columnName)
        {
            return columnName;
        }

        public void ReNameTable(string tableName)
        {
            m_strTableName = tableName;
        }

        public void ReNameTableColumn(string oldName, string newName)
        {
            foreach (var column in m_TableFields)
            {
                if (column.ColumnName.Equals(oldName))
                {
                    column.ReNameColumn(newName);
                }
            }
        }

        public void CreateIndexFromXPK(IndexDefInfo pkConstraint)
        {
            if (pkConstraint != null)
            {
                IndexDefInfo akConstraint = (IndexDefInfo)pkConstraint.Clone();

                string indexName = akConstraint.IndexName.Replace("XPK", "XAK");

                akConstraint.IndexName = indexName;

                akConstraint.Primary = false;

                akConstraint.Unique = true;

                IndexAppend(akConstraint);
            }
        }
        public IndexDefInfo CreatePKAutoConstraint(string lpszName, string lpszIdName)
        {
            string constraintName = lpszName + m_strTableName;

            m_PKConstraint = new IndexDefInfo(constraintName, m_strTableName, true);
            m_PKConstraint.AppendField(lpszIdName);

            return m_PKConstraint;
        }

        public IndexDefInfo CreateIndexFromXPK(IndexDefInfo pkConstraint, string oldAutoName, string newAutoName)
        {
            m_AKConstraint = null;

            if (pkConstraint != null)
            {
                IndexDefInfo akConstraint = (IndexDefInfo)pkConstraint.Clone();

                string indexName = akConstraint.IndexName.Replace("XPK", "XAK");

                akConstraint.IndexName = indexName;

                akConstraint.Primary = false;

                akConstraint.Unique = true;

                akConstraint.ReNameColumn(oldAutoName, newAutoName);

                m_AKConstraint = akConstraint;
            }
            return m_AKConstraint;
        }

        // Default comparer for Part type.
        public static int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }

        public int CompareTo(TableDefInfo compareDef)
        {
            // A null value means that this object is greater.
            if (compareDef == null)
                return 1;

            else
                return this.m_strTableName.CompareTo(compareDef.m_strTableName);
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            TableDefInfo objAsDef = obj as TableDefInfo;
            if (objAsDef == null) return false;
            else return Equals(objAsDef);
        }
        public override int GetHashCode()
        {
            return m_strTableName.GetHashCode();
        }
        public bool Equals(TableDefInfo other)
        {
            if (other == null) return false;
            return (this.m_strTableName.Equals(other.m_strTableName));
        }

        public string InfoName()
        {
            return m_strTableName;
        }

        public bool IsAutoIncrement(string columnName)
        {
            TableFieldInfo column = m_TableFields.Where((c) => (c.ColumnName.Equals(columnName))).SingleOrDefault();
            if (column != null)
            {
                return column.IsAutoIncrement();
            }
            return false;
        }

        public TableFieldInfo GetAutoIncrementColumn()
        {
            return m_TableFields.Where((c) => (c.IsAutoIncrement())).SingleOrDefault();
        }

        public TableFieldInfo GetAutoIncrementColumnForVersion(UInt32 versCreate)
        {
            IList<TableFieldInfo> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            TableFieldInfo column = columnList.Where((c) => (c.IsAutoIncrement())).SingleOrDefault();

            return column;
        }
        public IList<TableFieldInfo> TableColumnsForVersion(UInt32 versCreate)
        {
            IList<TableFieldInfo> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            return columnList;
        }
        public IList<TableFieldInfo> XPKIdxColumnsForVersion(UInt32 versCreate)
        {
            if (m_PKConstraint == null)
            {
                return new List<TableFieldInfo>();
            }
            IList<TableFieldInfo> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate) && m_PKConstraint.FieldByName(c.ColumnName)!=null)).ToList();

            return columnList;
        }

        public TableFieldInfo FieldByName(string columnName)
        {
            TableFieldInfo column = m_TableFields.Where((c) => (c.ColumnName.Equals(columnName))).SingleOrDefault();

            return column;
        }

        public TableFieldInfo VersionFieldByName(string columnName, UInt32 versCreate)
        {
            IList<TableFieldInfo> columnList = m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).ToList();

            TableFieldInfo column = columnList.Where((c) => (c.ColumnName.CompareNoCase(columnName))).SingleOrDefault();

            return column;
        }

        public bool HasAutoIncrementColumn()
        {
            TableFieldInfo column = GetAutoIncrementColumn();
            if (column != null)
            {
                return true;
            }
            return false;
        }
        public IList<string> PrimaryKeyColumnList()
        {
            if (m_PKConstraint != null)
            {
                return m_PKConstraint.CreateFieldsNamesArray().Select((x) => (x)).ToList();
            }
            return new List<string>();
        }
        public virtual IList<string> OrdinalColumnList()
        {
            if (m_PKConstraint != null)
            {
                return m_PKConstraint.CreateFieldsNamesArray().Select((x) => (x)).ToList();
            }
            return new List<string>();
        }
        public IList<string> AlternateKeyColumnList()
        {
            if (m_AKConstraint != null)
            {
                return m_AKConstraint.CreateFieldsNamesArray().Select((x) => (x)).ToList();
            }
            return new List<string>();
        }
        public IList<string> PrimaryKeyCamelColumnList()
        {
            if (m_PKConstraint != null)
            {
                return m_PKConstraint.CreateFieldsNamesArray().Select((x) => NameConversions.CamelName(x)).ToList();
            }
            return new List<string>();
        }
        public virtual IList<string> OrdinalCamelColumnList()
        {
            if (m_PKConstraint != null)
            {
                return m_PKConstraint.CreateFieldsNamesArray().Select((x) => NameConversions.CamelName(x)).ToList();
            }
            return new List<string>();
        }
        public IList<string> AlternateKeyCamelColumnList()
        {
            if (m_AKConstraint != null)
            {
                return m_AKConstraint.CreateFieldsNamesArray().Select((x) => NameConversions.CamelName(x)).ToList();
            }
            return new List<string>();
        }
        public IList<string> TableDatasColumnList(UInt32 versCreate)
        {
            IList<string> listPKeyColumn = PrimaryKeyColumnList();

            return m_TableFields.Where((c) => (!listPKeyColumn.Contains(c.ColumnName) && c.IsValidInVersion(versCreate))).Select((x) =>(x.ColumnName)).ToList();
        }
        public IList<string> TableDatasColumnList(UInt32 versCreate, IList<string> listPKeyColumn)
        {
            return m_TableFields.Where((c) => (!listPKeyColumn.Contains(c.ColumnName) && c.IsValidInVersion(versCreate))).Select((x) =>(x.ColumnName)).ToList();
        }
        public IList<Tuple<string, Int32>> TableColumnTypesList(UInt32 versCreate)
        {
            return m_TableFields.Where((c) => (c.IsValidInVersion(versCreate))).Select((f) => (new Tuple<string, Int32>(f.ColumnName, f.ColumnType))).ToList();
        }
        public IList<string> RelationForeignTables()
        {
            if (m_TableRelations != null)
            {
                return m_TableRelations.Select((x) => (x.SourceTableName)).ToList();
            }
            return new List<string>();
        }

        public string CreatePKFieldsNameList()
        {
            if (m_PKConstraint == null)
            {
                return "";
            }
            return m_PKConstraint.CreateFieldsNameList();
        }
        public string CreateAKFieldsNameList()
        {
            if (m_AKConstraint == null)
            {
                return "";
            }
            return m_AKConstraint.CreateFieldsNameList();
        }
        public string PKUniqueAllNames()
        {
            return m_strTableName + "." + string.Join(".", m_PKConstraint.IndexFields().Select((f) => (f.ColumnName)).ToList());
        }
        public string AKUniqueAllNames()
        {
            return m_strTableName + "." + string.Join(".", m_AKConstraint.IndexFields().Select((f) => (f.ColumnName)).ToList());
        }

        public TableFieldInfo FieldAppend(TableFieldInfo fieldInfo)
        {
            m_TableFields.Add(fieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }

        private TableFieldInfo FieldInsertBeg(TableFieldInfo fieldInfo)
        {
            m_TableFields.Insert(0, fieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }

        private TableFieldInfo FieldInsertIdx(TableFieldInfo fieldInfo, int index)
        {
            m_TableFields.Insert(Math.Min(index, m_nFieldsCount), fieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }
        public TableFieldInfo FieldAppendToQuery(TableFieldInfo fieldInfo)
        {
            TableFieldInfo queryFieldInfo = (TableFieldInfo)fieldInfo.Clone();

            queryFieldInfo.Attributes = (queryFieldInfo.Attributes & (~DatabaseDef.dbAutoIncrField));

            m_TableFields.Add(queryFieldInfo);
            m_nFieldsCount++;

            return fieldInfo;
        }


        private IndexDefInfo IndexAppend(IndexDefInfo indexInfo)
        {
            m_TableIndexs.Add(indexInfo);

            return indexInfo;
        }

        private RelationDefInfo RelationAppend(RelationDefInfo relationInfo)
        {
            m_TableRelations.Add(relationInfo);

            return relationInfo;
        }

        public static TableFieldInfo CreateFieldInfo(string lpszName, int nType, bool bNullOption, UInt32 versFrom, UInt32 versDrop)
        {
            TableFieldInfo fieldInfo = new TableFieldInfo(versFrom, versDrop);
            fieldInfo.ColumnName = lpszName;
            fieldInfo.ColumnType = nType;

            fieldInfo.AllowZeroLength = false;
            fieldInfo.Required = !bNullOption;
            fieldInfo.CollatingOrder = 0;
            fieldInfo.OrdinalPosition = 0;
            fieldInfo.DefaultValue = "";
            fieldInfo.ValidationRule = "";
            fieldInfo.ValidationText = "";
            fieldInfo.Attributes = DatabaseDef.dbFixedField | DatabaseDef.dbUpdatableField;
            fieldInfo.ColumnSize = FieldSize(nType);
            fieldInfo.DefaultValue = FieldDefaultValue(nType, bNullOption);
            return (fieldInfo);
        }

        public TableFieldInfo CreateField(string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateFieldInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            return FieldAppend(fieldInfo);
        }

        public TableFieldInfo InsertField(int index, string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateFieldInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            return FieldInsertIdx(fieldInfo, index);
        }

        public static TableFieldInfo CreateFTEXTInfo(string lpszName, int nType, int size, bool bNullOption, UInt32 versFrom, UInt32 versDrop)
        {
            TableFieldInfo fieldInfo = new TableFieldInfo(versFrom, versDrop);
            fieldInfo.ColumnName = lpszName;
            fieldInfo.ColumnType = nType;

            fieldInfo.Attributes = DatabaseDef.dbUpdatableField;
            fieldInfo.AllowZeroLength = true;
            fieldInfo.ColumnSize = size;
            fieldInfo.Required = !bNullOption;
            fieldInfo.CollatingOrder = 0;
            fieldInfo.OrdinalPosition = 0;
            fieldInfo.DefaultValue = "";
            fieldInfo.ValidationRule = "";
            fieldInfo.ValidationText = "";

            return (fieldInfo);
        }

        public TableFieldInfo CreateFTEXT(string lpszName, int nType, int size, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateFTEXTInfo(lpszName, nType, size, bNullOption, versFrom, versDrop);

            return FieldAppend(fieldInfo);
        }

        public TableFieldInfo InsertFTEXT(int index, string lpszName, int nType, int size, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateFTEXTInfo(lpszName, nType, size, bNullOption, versFrom, versDrop);

            return FieldInsertIdx(fieldInfo, index);
        }

        public static TableFieldInfo CreateGDATEInfo(string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = new TableFieldInfo(versFrom, versDrop);
            fieldInfo.ColumnName = lpszName;
            fieldInfo.ColumnType = nType;

            fieldInfo.AllowZeroLength = false;
            fieldInfo.Required = !bNullOption;
            fieldInfo.CollatingOrder = 0;
            fieldInfo.OrdinalPosition = 0;
            fieldInfo.DefaultValue = "";
            fieldInfo.ValidationRule = "";
            fieldInfo.ValidationText = "";
            fieldInfo.Attributes = DatabaseDef.dbFixedField | DatabaseDef.dbUpdatableField;
            fieldInfo.ColumnSize = FieldSize(nType);
            if (fieldInfo.Required)
            {
                fieldInfo.DefaultValue = "*";
            }

            return (fieldInfo);
        }

        public TableFieldInfo CreateGDATE(string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateGDATEInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            return FieldAppend(fieldInfo);
        }

        public TableFieldInfo InsertGDATE(int index, string lpszName, int nType, bool bNullOption, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = CreateGDATEInfo(lpszName, nType, bNullOption, versFrom, versDrop);

            return FieldInsertIdx(fieldInfo, index);
        }

        public static TableFieldInfo CreateFAUTOInfo(string lpszName, int nType, UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            TableFieldInfo fieldInfo = new TableFieldInfo(versFrom, versDrop);
            fieldInfo.ColumnName = lpszName;
            fieldInfo.ColumnType = nType;

            fieldInfo.AllowZeroLength = false;
            fieldInfo.Required = true;
            fieldInfo.CollatingOrder = 0;
            fieldInfo.OrdinalPosition = 0;
            fieldInfo.DefaultValue = "";
            fieldInfo.ValidationRule = "";
            fieldInfo.ValidationText = "";
            fieldInfo.Attributes = DatabaseDef.dbFixedField | DatabaseDef.dbAutoIncrField | DatabaseDef.dbUpdatableField;
            fieldInfo.ColumnSize = FieldSize(nType);

            return fieldInfo;
        }

        public TableFieldInfo CreateFAUTO(string lpszName, int nType)
        {
            TableFieldInfo fieldInfo = CreateFAUTOInfo(lpszName, nType);

            return FieldInsertBeg(fieldInfo);
        }

        public IndexDefInfo CreateIndex(string lpszName, bool bUnique = false)
        {
            IndexDefInfo indexInfo = new IndexDefInfo(lpszName, m_strTableName, false);
            indexInfo.Unique = bUnique;

            return IndexAppend(indexInfo);
        }

        public RelationDefInfo CreateRelation(string lpszName, string lpszRelTable)
        {
            RelationDefInfo indexInfo = new RelationDefInfo(lpszName, m_strTableName, lpszRelTable);

            return RelationAppend(indexInfo);
        }

        public IList<RelationDefInfo> ForeignRelations(IList<TableDefInfo> tables)
        {
            return tables.SelectMany((m) => (m.Relations().Where((r) => (r.SourceTableName.CompareTo(m_strTableName) == 0)))).ToList();
        }

        private static string CreateConstraintName(string constraintColl, string keyName, string autoRefId)
        {
            string constraintName;
            string keyNameEx1 = "_" + keyName;
            string keyNameEx2 = keyName + "_";

            if (constraintColl.EndsWith(keyNameEx1))
            {
                constraintName = constraintColl.Replace(keyNameEx1, autoRefId);
            }
            else if (constraintColl.Contains(keyNameEx2))
            {
                constraintName = constraintColl.Replace(keyNameEx2, "") + autoRefId;
            }
            else
            {
                constraintName = constraintColl + autoRefId;
            }

            return constraintName;
        }

        private static int FieldSize(int nType)
        {
            int fieldInfoSize = 0;
            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    fieldInfoSize = 1;
                    break;
                case DatabaseDef.DB_BYTE:
                    fieldInfoSize = 1;
                    break;
                case DatabaseDef.DB_INTEGER:
                    fieldInfoSize = 2;
                    break;
                case DatabaseDef.DB_LONG:
                    fieldInfoSize = 4;
                    break;
                case DatabaseDef.DB_CURRENCY:
                    fieldInfoSize = 8;
                    break;
                case DatabaseDef.DB_SINGLE:
                    fieldInfoSize = 4;
                    break;
                case DatabaseDef.DB_DOUBLE:
                    fieldInfoSize = 8;
                    break;
                case DatabaseDef.DB_DATE:
                    fieldInfoSize = 8;
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    fieldInfoSize = 0;
                    break;
                default:
                    fieldInfoSize = 0;
                    break;
            }
            return fieldInfoSize;
        }

        private static string FieldDefaultValue(int nType, bool bNullOption)
        {
            string fieldInfoDefaultValue = "";
            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    if (!bNullOption)
                    {
                        fieldInfoDefaultValue = "0";
                    }
                    break;
                case DatabaseDef.DB_BYTE:
                    if (!bNullOption)
                    {
                        fieldInfoDefaultValue = "0";
                    }
                    break;
                case DatabaseDef.DB_INTEGER:
                    if (!bNullOption)
                    {
                        fieldInfoDefaultValue = "0";
                    }
                    break;
                case DatabaseDef.DB_LONG:
                    if (!bNullOption)
                    {
                        fieldInfoDefaultValue = "0";
                    }
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    break;
                case DatabaseDef.DB_DOUBLE:
                    break;
                case DatabaseDef.DB_DATE:
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    break;
                default:
                    break;
            }
            return fieldInfoDefaultValue;
        }

        void Clear()
        {
            m_strTableName = "";
            m_TableFields.Clear();
            m_nFieldsCount = 0;
            m_PKConstraint = null;
            m_AKConstraint = null;
            m_TableIndexs.Clear();
            m_TableRelations.Clear();
        }

        public IndexDefInfo CreatePKConstraint(string lpszName)
        {
            string constraintName = lpszName + m_strTableName;

            m_PKConstraint = new IndexDefInfo(constraintName, m_strTableName, true);

            return m_PKConstraint;
        }

        public IndexDefInfo CreateAKConstraint(string lpszName)
        {
            string constraintName = lpszName + m_strTableName;

            m_AKConstraint = new IndexDefInfo(constraintName, m_strTableName, true);

            return m_AKConstraint;
        }

        public IndexDefInfo CreateAKAutoConstraint(string lpszName, string lpszIdName)
        {
            string constraintName = lpszName + m_strTableName;

            m_AKConstraint = new IndexDefInfo(constraintName, m_strTableName, true);
            m_AKConstraint.AppendField(lpszIdName);

            return m_AKConstraint;
        }

        public IndexDefInfo AddTableIndex(string lpszName)
        {
            IndexDefInfo indexInfo = new IndexDefInfo(lpszName, m_strTableName, false);

            return indexInfo;
        }

        public string CreateColumnList(UInt32 versCreate)
        {
            string columnsList = "";

            foreach (TableFieldInfo field in m_TableFields)
            {
                bool bIsValidForVersion = field.IsValidInVersion(versCreate);
                if (field != null && bIsValidForVersion)
                {
                    columnsList += field.ColumnName;
                    columnsList += ", ";
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }

        public string CreateSelectColumnList(UInt32 versCreate)
        {
            string columnsList = "";

            foreach (TableFieldInfo field in m_TableFields)
            {
                bool bIsValidForVersion = field.IsValidInVersion(versCreate);
                bool bIsValidForXSelect = field.IncludeColumnType();
                if (field != null && bIsValidForVersion && bIsValidForXSelect)
                {
                    columnsList += field.ColumnName;
                    columnsList += ", ";
                }
            }
            string columnListRet = columnsList.TrimEnd(DatabaseDef.TRIM_CHARS);

            return columnListRet;
        }

        public IList<string> DeepRelationsList(IList<string> agrList, IList<TableDefInfo> tables, bool addTableName)
        {
            IList<string> retList = agrList.ToList();
            if (addTableName)
            {
                retList = agrList.Concat(new string[] { m_strTableName }).ToList();
            }

            if (m_TableRelations.Count == 0)
            {
                return retList;
            }

            return m_TableRelations.Aggregate(retList, (agr, r) => r.DeepRelationsList(tables, agr));
        }

        #region ICloneable Members

        public object Clone()
        {
            TableDefInfo other = (TableDefInfo)this.MemberwiseClone();
            other.m_VersFrom = this.m_VersFrom;
            other.m_VersDrop = this.m_VersDrop;
            other.m_strTableName = this.m_strTableName;
            other.m_strOwnerName = this.m_strOwnerName;
            other.m_strUsersName = this.m_strUsersName;
            other.m_nFieldsCount = this.m_nFieldsCount;
            if (this.m_PKConstraint != null)
            {
                other.m_PKConstraint = (IndexDefInfo)this.m_PKConstraint.Clone();
            }
            else
            {
                other.m_PKConstraint = this.m_PKConstraint;
            }

            if (this.m_AKConstraint != null)
            {
                other.m_AKConstraint = (IndexDefInfo)this.m_AKConstraint.Clone();
            }
            else
            {
                other.m_AKConstraint = this.m_AKConstraint;
            }

            other.m_TableFields = this.m_TableFields.Select((f) => ((TableFieldInfo)f.Clone())).ToList();
            other.m_TableIndexs = this.m_TableIndexs.Select((i) => ((IndexDefInfo)i.Clone())).ToList();
            other.m_TableRelations = this.m_TableRelations.Select((r) => ((RelationDefInfo)r.Clone())).ToList();
            return other;
        }

        #endregion

        public void SetTableFields(IList<TableFieldInfo> fieldList)
        {
            m_TableFields = fieldList;
        }
        public void SetIndexPK(IndexDefInfo indexInfo)
        {
            m_PKConstraint = indexInfo;
        }
        public void SetIndexAK(IndexDefInfo indexInfo)
        {
            m_AKConstraint = indexInfo;
        }
        public void SetIndexesNonPK(IList<IndexDefInfo> indexList)
        {
            m_TableIndexs = indexList;
        }

        public void SetRelations(IList<RelationDefInfo> relsList)
        {
            m_TableRelations = relsList;
        }

        public void SetFields(int fieldCount)
        {
            m_nFieldsCount = fieldCount;
        }

    }
}
