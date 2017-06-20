using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class TableFieldInfo : ICloneable
    {
        public TableFieldInfo(UInt32 versFrom = 0, UInt32 versDrop = 9999)
        {
            ColumnName = "";
            ColumnType = 0;

            CollatingOrder = 0;
            OrdinalPosition = 0;
            DefaultValue = "";
            ValidationRule = "";
            ValidationText = "";
            Attributes = DatabaseDef.dbFixedField | DatabaseDef.dbUpdatableField;
            ColumnSize = 0;
            Required = false;
            AllowZeroLength = false;
            VersFrom = versFrom;
            VersDrop = versDrop;
            Multiplicity = 0;
        }

        public string ColumnName { get; set; }
        public int ColumnType { get; set; }

        public int CollatingOrder { get; set; }
        public int OrdinalPosition { get; set; }
        public string DefaultValue { get; set; }
        public string ValidationRule { get; set; }
        public string ValidationText { get; set; }
        public int Attributes { get; set; }
        public int ColumnSize { get; set; }
        public bool Required { get; set; }
        public bool AllowZeroLength { get; set; }
        protected UInt32 VersFrom { get; set; }
        protected UInt32 VersDrop { get; set; }
        protected Int32 Multiplicity { get; set; }

        public bool IsValidInVersion(UInt32 versCreate)
        {
            return (VersFrom <= versCreate && versCreate < VersDrop);
        }

        public string ColumnCamelName()
        {
            return NameConversions.CamelName(ColumnName);
        }

        public void ReNameColumn(string newName)
        {
            ColumnName = newName;
        }

        public bool IsAutoIncrement()
        {
            return DBPlatform.AutoIncrField(Attributes);
        }

        public Int32 Multiple()
        {
            return Multiplicity;
        }

        public void SetMultiple(Int32 multiple)
        {
            Multiplicity = multiple;
        }

        public bool IsBindColumnInfo()
        {
            return DBPlatform.DefaultBindRequired(Attributes);
        }
        public bool IsBindColumnType()
        {
            return DBPlatform.DefaultBindDataType(ColumnType);
        }
        public bool IsDefaultValue()
        {
            return (DefaultValue.Length != 0);
        }

        public int DbColumnSize()
        {
            return DBPlatform.DataTypeSize(ColumnType, ColumnSize);
        }

        public bool DbColumnNull()
        {
            return !Required;
        }

        public bool IncludeColumnType()
        {
            bool bColumnTypeEx = true;
            switch (ColumnType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_LONG:
                case DatabaseDef.DB_TEXT:
                case DatabaseDef.DB_DATE:
                case DatabaseDef.DB_SINGLE:
                case DatabaseDef.DB_DOUBLE:
                    bColumnTypeEx = true;
                    break;
                case DatabaseDef.DB_CURRENCY:
                case DatabaseDef.DB_LONGBINARY:
                case DatabaseDef.DB_MEMO:
                case DatabaseDef.DB_GUID:
                    bColumnTypeEx = false;
                    break;
                default:
                    break;
            }
            return (bColumnTypeEx);
        }

        public string StringColumnData(string dataItem, UInt32 platformType)
        {
            string dataColumn = "";
            if (dataItem.Length == 0)
            {
                dataColumn += "NULL";
            }
            else
            {
                switch (ColumnType)
                {
                    case DatabaseDef.DB_BOOLEAN:
                        if (dataItem.CompareTo("False") == 0)
                        {
                            dataColumn += "0";
                        }
                        else
                        {
                            dataColumn += "1";
                        }
                        break;
                    case DatabaseDef.DB_BYTE:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_INTEGER:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_LONG:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_CURRENCY:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_SINGLE:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_DOUBLE:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_DATE:
                        dataColumn += DBPlatform.GDateValue(platformType, DateTime.Parse(dataItem));
                        break;
                    case DatabaseDef.DB_LONGBINARY:
                        dataColumn += dataItem;
                        break;
                    case DatabaseDef.DB_TEXT:
                        dataColumn += "'";
                        dataColumn += dataItem;
                        dataColumn += "'";
                        break;
                    default:
                        dataColumn += dataItem;
                        break;
                }
            }
            return dataColumn;
        }

        public object Clone()
        {
            TableFieldInfo other = (TableFieldInfo)this.MemberwiseClone();
            other.ColumnName = this.ColumnName;
            other.ColumnType = this.ColumnType;

            other.CollatingOrder = this.CollatingOrder;
            other.OrdinalPosition = this.OrdinalPosition;
            other.DefaultValue = this.DefaultValue;
            other.ValidationRule = this.ValidationRule;
            other.ValidationText = this.ValidationText;
            other.Attributes = this.Attributes;
            other.ColumnSize = this.ColumnSize;
            other.Required = this.Required;
            other.AllowZeroLength = this.AllowZeroLength;
            other.VersFrom = this.VersFrom;
            other.VersDrop = this.VersDrop;
            other.Multiplicity = this.Multiplicity;

            return other;
        }

        public string TableColumnName(Int32 multiIdx = 0)
        {
            if (Multiplicity > 0)
            {
                return string.Format("{0}{1}", ColumnName, multiIdx.ToString());
            }
            else
            {
                return ColumnName;
            }
        }

        public IList<string> AllColumnNames()
        {
            Int32 multiCount = Math.Max(1, Multiplicity);
            return Enumerable.Range(1, multiCount).Select((i, c) => (TableColumnName(i))).ToList();
        }
    }
}
