using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Utils
{
    public static class NameConversions
    {
        public static string TransformName(Tuple<string, string>[] transformations, string name)
        {
            return transformations.Aggregate(name, (agr, x) => agr.Replace(x.Item1, x.Item2));
        }

        public static string CamelName(string name)
        {
            return name.Underscore().Camelize();
        }
        public static string ConvertCamelName(string name)
        {
            return name.ConvertNameToCamel();
        }
    }
    public static class DBPlatform
    {
        public const UInt32 DATA_PROVIDER_JET35 = 1;
        public const UInt32 DATA_PROVIDER_SQLITE = 2;
        public const UInt32 DATA_PROVIDER_ODBC_ORACLE = 6;
        public const UInt32 DATA_PROVIDER_ODBC_MSSQL = 10;
        public const UInt32 DATA_PROVIDER_ODBC_IMSSQL = 18;

        public static bool IsMsJetType(UInt32 type)
        {
            return (type == DATA_PROVIDER_JET35);
        }

        public static bool IsOracleType(UInt32 type)
        {
            return (type == DATA_PROVIDER_ODBC_ORACLE);
        }

        public static bool IsMsSQLType(UInt32 type)
        {
            return (type == DATA_PROVIDER_ODBC_MSSQL || type == DATA_PROVIDER_ODBC_IMSSQL);
        }

        public static bool IsSQLiteType(UInt32 type)
        {
            return (type == DATA_PROVIDER_SQLITE);
        }

        public static string GDateDefault(UInt32 type)
        {
            if (IsMsJetType(type))
            {
                return ""; // "DEFAULT GETDATE()";
            }
            else if (IsMsSQLType(type))
            {
                return "DEFAULT GETDATE()";
            }
            else if (IsOracleType(type))
            {
                return "DEFAULT SYSDATE";
            }
            else if (IsSQLiteType(type))
            {
                return "DEFAULT CURRENT_TIMESTAMP";
            }
            return DatabaseDef.EMPTY_STRING;
        }

        public static string GDateValue(UInt32 type, DateTime? value)
        {
            string dataColumn = DatabaseDef.EMPTY_STRING;
            if (value.HasValue)
            {
                if (IsMsJetType(type))
                {
                    dataColumn += "DateValue('";
                    dataColumn += value.Value.ToString("yyyy-MM-dd"); //String.Format("{0:YYYY-MM-DD}", datum.Value)
                    dataColumn += "')";
                }
                else if (IsMsSQLType(type))
                {
                    dataColumn += "PARSE('";
                    dataColumn += value.Value.ToString("yyyy-MM-dd"); //String.Format("{0:YYYY-MM-DD}", datum.Value)
                    dataColumn += "' AS date USING 'en-US')";
                }
                else if (IsOracleType(type))
                {
                    dataColumn += "TO_DATE('";
                    dataColumn += value.Value.ToString("dd.MM.yyyy");
                    dataColumn += "')";
                }
                else if (IsSQLiteType(type))
                {
                    dataColumn += "datetime('";
                    dataColumn += value.Value.ToString("yyyy-MM-dd"); //String.Format("{0:YYYY-MM-DD}", datum.Value)
                    dataColumn += "')";
                }
            }
            else
            {
                dataColumn += "NULL";
            }
            return dataColumn;
        }

        public static string NumberDefault(UInt32 type)
        {
            if (IsMsJetType(type))
            {
                return "DEFAULT 0";
            }
            else if (IsMsSQLType(type))
            {
                return "DEFAULT (0)";
            }
            else if (IsOracleType(type))
            {
                return "DEFAULT (0)";
            }
            else if (IsSQLiteType(type))
            {
                return "DEFAULT (0)";
            }
            return DatabaseDef.EMPTY_STRING;
        }

        public static bool DefaultBindRequired(int lAttributes)
        {
            int lAutoIncrField = (lAttributes & DatabaseDef.dbAutoIncrField);

            return (lAutoIncrField == 0);
        }

        public static bool DefaultBindDataType(int nType)
        {
            bool bBindDefault = false;
            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_LONG:
                    bBindDefault = true;
                    break;
            }
            return (bBindDefault);
        }

        public static bool AutoIncrField(int lAttributes)
        {
            int lAutoIncrField = (lAttributes & DatabaseDef.dbAutoIncrField);

            return (lAutoIncrField == DatabaseDef.dbAutoIncrField);
        }

        public static string OracleConvertDataType(int nType, int nSize)
        {
            string strFieldType = "";
            string strFieldTempl;
            string strFieldS2Add;

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "NUMBER(1)";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "NUMBER(3)";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "NUMBER(5)";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "NUMBER(11)";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldTempl = "varchar2({{0} CHAR)";
                    strFieldS2Add = String.Format(strFieldTempl, nSize);
                    strFieldType += strFieldS2Add;
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "DATE";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "BLOB";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string MsSQLConvertDataType(int nType, int nSize)
        {
            string strFieldType = "";
            string strFieldTempl;
            string strFieldS2Add;

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "bit";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "tinyint";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "smallint";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "int";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldTempl = "varchar({0})";
                    strFieldS2Add = String.Format(strFieldTempl, nSize);
                    strFieldType = strFieldS2Add;
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "datetime";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "varbinary(MAX)";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string MsJetConvertDataType(int nType, int nSize)
        {
            string strFieldType = "";
            string strFieldTempl;
            string strFieldS2Add;

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "BIT";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "BYTE";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "SHORT";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "LONG";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldTempl = "VARCHAR({0})";
                    strFieldS2Add = String.Format(strFieldTempl, nSize);
                    strFieldType = strFieldS2Add;
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "DATETIME";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "SINGLE";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "DOUBLE";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "LONGBINARY";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string SqliteConvertDataType(int nType, int nSize)
        {
            string strFieldType = "";
            string strFieldTempl;
            string strFieldS2Add;

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "INT2";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "TINYINT";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "SMALLINT";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "INT";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldTempl = "VARCHAR({0})";
                    strFieldS2Add = String.Format(strFieldTempl, nSize);
                    strFieldType = strFieldS2Add;
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "DATETIME";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "FLOAT";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "FLOAT";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "BLOB";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string EntityConvertDataType(int nType, int nSize, bool bRequired)
        {
            string strFieldType = "";

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "bool";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "byte";
                    break;
                case DatabaseDef.DB_INTEGER:
                    if (bRequired)
                    {
                        strFieldType = "Int16";
                    }
                    else
                    {
                        strFieldType = "Int16?";
                    }
                    break;
                case DatabaseDef.DB_LONG:
                    if (bRequired)
                    {
                        strFieldType = "Int32";
                    }
                    else
                    {
                        strFieldType = "Int32?";
                    }
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldType = "string";
                    break;
                case DatabaseDef.DB_DATE:
                    if (bRequired)
                    {
                        strFieldType = "DateTime";
                    }
                    else
                    {
                        strFieldType = "DateTime?";
                    }
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "double";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "double";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "byte[]";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string DaoCppConvertDataType(int nType, int nSize, bool bRequired)
        {
            string strFieldType = "";

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "DB_BOOLEAN";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "DB_BYTE";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "DB_INTEGER";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "DB_LONG";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldType = "DB_TEXT";
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "DB_DATE";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    strFieldType = "DB_CURRENCY";
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "DB_SINGLE";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "DB_DOUBLE";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "DB_LONGBINARY";
                    break;
                case DatabaseDef.DB_MEMO:
                    strFieldType = "DB_MEMO";
                    break;
                case DatabaseDef.DB_GUID:
                    strFieldType = "DB_GUID";
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string AdoCppConvertDataType(int nType, int nSize, bool bRequired)
        {
            string strFieldType = "";

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "VARIANT_BOOL";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "BYTE";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "SHORT";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "LONG";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldType = "TCHAR";
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "DBTIMESTAMP";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "double";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "double";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "ISequentialStream*";
                    break;
                case DatabaseDef.DB_MEMO:
                    strFieldType = "ISequentialStream*";
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }

        public static string AdoCppConvertDataAttr(int nType, int nSize, bool bRequired, string columnName)
        {
            string strFieldAttr = "";

            Int32 int2Str = nSize + 1;

            switch (nType)
            {
                case DatabaseDef.DB_TEXT:
                    strFieldAttr = "[" + columnName + "Count ]";
                    break;
                default:
                    break;
            }
            return strFieldAttr;
        }

        public static string ModelConvertDataType(int nType, int nSize, bool bRequired)
        {
            string strFieldType = "";

            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    strFieldType = "boolean";
                    break;
                case DatabaseDef.DB_BYTE:
                    strFieldType = "integer";
                    break;
                case DatabaseDef.DB_INTEGER:
                    strFieldType = "integer";
                    break;
                case DatabaseDef.DB_LONG:
                    strFieldType = "integer";
                    break;
                case DatabaseDef.DB_TEXT:
                    strFieldType = "string";
                    break;
                case DatabaseDef.DB_DATE:
                    strFieldType = "datetime";
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_DOUBLE:
                    strFieldType = "float";
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    strFieldType = "byte[]";
                    break;
                case DatabaseDef.DB_MEMO:
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return strFieldType;
        }


        public static int DataTypeSize(int nType, int columnSize)
        {
            int nDbTypeSize = 0;
            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                    nDbTypeSize = 1;
                    break;
                case DatabaseDef.DB_BYTE:
                    nDbTypeSize = 1;
                    break;
                case DatabaseDef.DB_INTEGER:
                    nDbTypeSize = 8;
                    break;
                case DatabaseDef.DB_LONG:
                    nDbTypeSize = 8;
                    break;
                case DatabaseDef.DB_TEXT:
                    nDbTypeSize = columnSize;
                    break;
                case DatabaseDef.DB_DATE:
                    nDbTypeSize = 8;
                    break;
                case DatabaseDef.DB_CURRENCY:
                    break;
                case DatabaseDef.DB_SINGLE:
                    nDbTypeSize = 4;
                    break;
                case DatabaseDef.DB_DOUBLE:
                    nDbTypeSize = 8;
                    break;
                case DatabaseDef.DB_LONGBINARY:
                    nDbTypeSize = columnSize;
                    break;
                case DatabaseDef.DB_MEMO:
                    nDbTypeSize = columnSize;
                    break;
                case DatabaseDef.DB_GUID:
                    break;
                default:
                    break;
            }
            return nDbTypeSize;
        }
        public static bool TypeIsNumber(int nType)
        {
            switch (nType)
            {
                case DatabaseDef.DB_BOOLEAN:
                case DatabaseDef.DB_BYTE:
                case DatabaseDef.DB_INTEGER:
                case DatabaseDef.DB_LONG:
                    return true;
                default:
                    break;
            }
            return false;
        }
        public static bool TypeIsDate(int nType)
        {
            if (nType == DatabaseDef.DB_DATE)
            {
                return true;
            }
            return false;
        }
    }
}
