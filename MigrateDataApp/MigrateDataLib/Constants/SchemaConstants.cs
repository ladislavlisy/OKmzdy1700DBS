using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Constants
{
    enum DataTypeEnum
    {
        dbBoolean = 1,
        dbByte = 2,
        dbInteger = 3,
        dbLong = 4,
        dbCurrency = 5,
        dbSingle = 6,
        dbDouble = 7,
        dbDate = 8,
        dbBinary = 9,
        dbText = 10,
        dbLongBinary = 11,
        dbMemo = 12,
        dbGUID = 15,
        dbBigInt = 16,
        dbVarBinary = 17,
        dbChar = 18,
        dbNumeric = 19,
        dbDecimal = 20,
        dbFloat = 21,
        dbTime = 22,
        dbTimeStamp = 23
    };

    enum RelationAttributeEnum
    {
        dbRelationUnique = 0x1,
        dbRelationDontEnforce = 0x2,
        dbRelationInherited = 0x4,
        dbRelationUpdateCascade = 0x100,
        dbRelationDeleteCascade = 0x1000,
        dbRelationLeft = 0x1000000,
        dbRelationRight = 0x2000000
    };

    enum FieldAttributeEnum
    {
        dbFixedField = 0x1,
        dbVariableField = 0x2,
        dbAutoIncrField = 0x10,
        dbUpdatableField = 0x20,
        dbSystemField = 0x2000,
        dbHyperlinkField = 0x8000,
        dbDescending = 0x1
    };

    public static class DatabaseDef
    {
        public static readonly char[] TRIM_CHARS = { '\r', '\n', '\t', ' ', ',' };
        public static readonly char[] SPLIT_CHARS = { '\n' };
        public static readonly string SPLIT_STRC = "\n";

        public const string EMPTY_SPACES = " ";
        public const string EMPTY_STRING = "";
        public const string NEW_LINE_STR = "\n";

        public const int MAX_DB_TEXT = 255;
        public const int DB_SINGLE = (int)DataTypeEnum.dbSingle;
        public const int DB_TEXT = (int)DataTypeEnum.dbText;
        public const int DB_BOOLEAN = (int)DataTypeEnum.dbBoolean;
        public const int DB_BYTE = (int)DataTypeEnum.dbByte;
        public const int DB_CURRENCY = (int)DataTypeEnum.dbCurrency;
        public const int DB_DATE = (int)DataTypeEnum.dbDate;
        public const int DB_DATETIME = (int)DataTypeEnum.dbDate;
        public const int DB_DOUBLE = (int)DataTypeEnum.dbDouble;
        public const int DB_INTEGER = (int)DataTypeEnum.dbInteger;
        public const int DB_LONG = (int)DataTypeEnum.dbLong;
        public const int DB_LONGBINARY = (int)DataTypeEnum.dbLongBinary;
        public const int DB_MEMO = (int)DataTypeEnum.dbMemo;
        public const int DB_GUID = (int)DataTypeEnum.dbGUID;
        public const int DB_RELATIONDELETECASCADE = (int)RelationAttributeEnum.dbRelationDeleteCascade;
        public const int DB_RELATIONDONTENFORCE = (int)RelationAttributeEnum.dbRelationDontEnforce;
        public const int DB_RELATIONINHERITED = (int)RelationAttributeEnum.dbRelationInherited;
        public const bool dbNotNullFieldOption = false;
        public const bool dbNullFieldOption = true;
        public const int dbFixedField = (int)FieldAttributeEnum.dbFixedField;
        public const int dbUpdatableField = (int)FieldAttributeEnum.dbUpdatableField;
        public const int dbAutoIncrField = (int)FieldAttributeEnum.dbAutoIncrField;
        public const string COLUMN_NAME_AUTOID = "id";
        public const string NAMEAUTO_REF_ID = "_refid";
        public const string NAMEAUTO_ID = "_id";

    }
}
