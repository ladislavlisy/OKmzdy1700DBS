using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.OKmzdy.Schema
{
    public class TablePopisAuditText : TableDefInfo
    {
        const string TABLE_NAME = "POPIS_AUDIT_TEXT";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TablePopisAuditText(lpszOwnerName, lpszUsersName);
        }
        override public string PropertyName(string columnName)
        {
            Tuple<string, string>[] changeNames = new Tuple<string, string>[] {
                new Tuple<string, string>("ffirma_id", "firma_id"),
            };
            return NameConversions.TransformName(changeNames, columnName);
        }

        public TablePopisAuditText(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("formxsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateField("fieldsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("field_text_cz", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("field_text_en", DB_TEXT, 255, dbNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("firma_id");
            PKConstraint.AppendField("fieldsrc_id");
        }
    }
    public class TableAuditUdalost : TableDefInfo
    {
        const string TABLE_NAME = "AUDIT_UDALOST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableAuditUdalost(lpszOwnerName, lpszUsersName);
        }
        override public string PropertyName(string columnName)
        {
            Tuple<string, string>[] changeNames = new Tuple<string, string>[] {
                new Tuple<string, string>("ffirma_id", "firma_id"),
            };
            return NameConversions.TransformName(changeNames, columnName);
        }

        public TableAuditUdalost(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
		    CreateFAUTO("audit_udalost_id", DB_LONG);
		    CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
		    CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
		    CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
		    CreateField("udalost_level", DB_INTEGER, dbNotNullFieldOption);
		    CreateField("udalost_tagid", DB_LONG, dbNotNullFieldOption);
		    CreateField("udalost_regid", DB_LONG, dbNotNullFieldOption);
		    CreateFTEXT("udalost_popis", DB_TEXT, 255, dbNullFieldOption);
		    CreateFTEXT("udalost_wuser", DB_TEXT, 255, dbNullFieldOption);
		    CreateField("udalost_datum", DB_DATE, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("audit_udalost_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableAuditZaznamHist : TableDefInfo
    {
        const string TABLE_NAME = "AUDIT_ZAZNAM_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableAuditZaznamHist(lpszOwnerName, lpszUsersName);
        }
        override public string PropertyName(string columnName)
        {
            Tuple<string, string>[] changeNames = new Tuple<string, string>[] {
                new Tuple<string, string>("ffirma_id", "firma_id"),
            };
            return NameConversions.TransformName(changeNames, columnName);
        }

        public TableAuditZaznamHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("audit_zaznam_hist_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("zaznam_level", DB_INTEGER, dbNotNullFieldOption);
            CreateField("zaznam_tagid", DB_LONG, dbNotNullFieldOption);
            CreateField("record_uid", DB_LONG, dbNotNullFieldOption);
            CreateField("record_pom", DB_INTEGER, dbNotNullFieldOption);
            CreateField("record_kod", DB_INTEGER, dbNotNullFieldOption);
            CreateField("record_cis", DB_INTEGER, dbNotNullFieldOption);
            CreateField("formxsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateField("fieldsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateField("obdobi_akt", DB_INTEGER, dbNotNullFieldOption);
            CreateField("obdobi_min", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("hodnota_old", DB_TEXT, 255, dbNotNullFieldOption);
            CreateFTEXT("hodnota_new", DB_TEXT, 255, dbNotNullFieldOption);
            CreateField("zaznam_datum", DB_DATE, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("audit_zaznam_hist_id");
            PKConstraint.AppendField("firma_id");
        }
    }
    public class VyberAuditZaznamHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_AUDIT_ZAZNAM_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberAuditZaznamHist(lpszOwnerName, lpszUsersName);
        }
        override public string PropertyName(string columnName)
        {
            Tuple<string, string>[] changeNames = new Tuple<string, string>[] {
                new Tuple<string, string>("ffirma_id", "firma_id"),
            };
            return NameConversions.TransformName(changeNames, columnName);
        }

        public VyberAuditZaznamHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("audit_zaznam_hist_id", DB_LONG, dbNotNullFieldOption);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("zaznam_level", DB_INTEGER, dbNotNullFieldOption);
            CreateField("zaznam_tagid", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("formx_text_cz", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("formx_text_en", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("field_text_cz", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("field_text_en", DB_TEXT, 255, dbNullFieldOption);
            CreateField("record_uid", DB_LONG, dbNotNullFieldOption);
            CreateField("record_pom", DB_INTEGER, dbNotNullFieldOption);
            CreateField("record_kod", DB_INTEGER, dbNotNullFieldOption);
            CreateField("record_cis", DB_INTEGER, dbNotNullFieldOption);
            CreateField("formxsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateField("fieldsrc_id", DB_LONG, dbNotNullFieldOption);
            CreateField("obdobi_akt", DB_INTEGER, dbNotNullFieldOption);
            CreateField("obdobi_min", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("hodnota_old", DB_TEXT, 255, dbNotNullFieldOption);
            CreateFTEXT("hodnota_new", DB_TEXT, 255, dbNotNullFieldOption);
            CreateField("zaznam_datum", DB_DATE, dbNotNullFieldOption);
        }
    }
}
