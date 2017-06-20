using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Utils;

namespace MigrateDataLib.OKmzdy.Schema
{
    public class TableImportCsvKonfig : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_CSV_KONFIG";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportCsvKonfig(lpszOwnerName, lpszUsersName);
        }

        public TableImportCsvKonfig(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("konfig_imp_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("konfig_cis", DB_LONG, dbNotNullFieldOption);
            CreateField("konfig_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("konfig_sep", DB_LONG, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("konfig_imp_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportCsvKonfigCols : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_CSV_KONFIG_COLS";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportCsvKonfigCols(lpszOwnerName, lpszUsersName);
        }

        public TableImportCsvKonfigCols(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("konfig_col_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("konfig_cis", DB_LONG, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_idx", DB_LONG, dbNotNullFieldOption);
            CreateField("hodnota_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("hodnota_kod", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nazev_sloup", DB_TEXT, 255, dbNullFieldOption);
            CreateField("sloupec_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("code_detail", DB_LONG, dbNotNullFieldOption);
            CreateField("code_datbeg", DB_LONG, dbNotNullFieldOption);
            CreateField("code_datend", DB_LONG, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("konfig_col_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportCsvDavka : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_CSV_DAVKA";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportCsvDavka(lpszOwnerName, lpszUsersName);
        }

        public TableImportCsvDavka(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("davka_imp_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("soubor_param", DB_TEXT, 255, dbNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("soubor_nazev", DB_TEXT, 255, dbNullFieldOption);
            CreateField("soubor_datum", DB_DATE, dbNullFieldOption);
            CreateField("davka_imp_stav", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("davka_imp_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportPersRadka : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_PERS_RADKA";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportPersRadka(lpszOwnerName, lpszUsersName);
        }

        public TableImportPersRadka(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("radka_imp_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("column_data", DB_TEXT, 255, dbNullFieldOption).SetMultiple(80);
            CreateFTEXT("errors_radka", DB_TEXT, 255, dbNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("radka_imp_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportPersIdentif : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_PERS_IDENTIF";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportPersIdentif(lpszOwnerName, lpszUsersName);
        }

        public TableImportPersIdentif(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("pers_identif_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("plati_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("sehrani_zpus", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("prac_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateField("prac_idcislo", DB_LONG, dbNotNullFieldOption);
            CreateField("ppom_ppcislo", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("ppom_pppopis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("cislo_pp", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("desc_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_prijmeni", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_jmeno", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_pppopis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("pers_identif_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportPolozkaData : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_POLOZKA_DATA";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportPolozkaData(lpszOwnerName, lpszUsersName);
        }

        public TableImportPolozkaData(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("polozka_data_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("zacat_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("konec_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("zacat_vyp", DB_INTEGER, dbNotNullFieldOption);
            CreateField("konec_vyp", DB_INTEGER, dbNotNullFieldOption);
            CreateField("polozka_idx", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_kod", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_dat", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("polozka_cis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("status_type", DB_INTEGER, dbNotNullFieldOption);
            CreateField("action_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("hodnota_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit01_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit01_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit02_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit02_text", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("goalam_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("datum_zacat", DB_DATE, dbNullFieldOption);
            CreateField("datum_konec", DB_DATE, dbNullFieldOption);
            CreateFTEXT("polozka_ccy", DB_TEXT, 10, dbNullFieldOption);
            CreateField("transac_src", DB_LONG, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("polozka_data_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportProtokolDavka : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_PROTOKOL_DAVKA";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportProtokolDavka(lpszOwnerName, lpszUsersName);
        }

        public TableImportProtokolDavka(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("protokol_davka_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
            CreateField("protokol_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("protokol_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("protokol_datum", DB_DATE, dbNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("protokol_davka_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class TableImportProtokolData : TableDefInfo
    {
        const string TABLE_NAME = "IMPORT_PROTOKOL_DATA";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new TableImportProtokolData(lpszOwnerName, lpszUsersName);
        }

        public TableImportProtokolData(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateFAUTO("protokol_data_id", DB_LONG);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("protokol_davka_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_data_id", DB_LONG, dbNotNullFieldOption);
            CreateField("protokol_level", DB_INTEGER, dbNotNullFieldOption);
            CreateField("protokol_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("oper_result", DB_LONG, dbNotNullFieldOption);
            CreateField("code_result", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("info_result", DB_TEXT, 255, dbNullFieldOption);
            CreateField("oper_action", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("old_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("new_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("imp_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateField("transac_src", DB_LONG, dbNotNullFieldOption);

            IndexDefInfo PKConstraint = CreatePKConstraint("XPK");
            PKConstraint.AppendField("protokol_data_id");
            PKConstraint.AppendField("firma_id");
        }
    }

    public class VyberImportCsvHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_IMPORT_CSV_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberImportCsvHist(lpszOwnerName, lpszUsersName);
        }

        public VyberImportCsvHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("uzivatel_id", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("soubor_param", DB_TEXT, 255, dbNotNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("soubor_nazev", DB_TEXT, 255, dbNullFieldOption);
            CreateField("soubor_datum", DB_DATE, dbNullFieldOption);
            CreateField("davka_imp_stav", DB_LONG, dbNotNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("uzivjmeno", DB_TEXT, 20, dbNotNullFieldOption);
            CreateField("uzivfunkce", DB_BYTE, dbNotNullFieldOption);
        }
    }

    public class VyberPersonsCsvHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_PERSONS_CSV_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberPersonsCsvHist(lpszOwnerName, lpszUsersName);
        }

        public VyberPersonsCsvHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_stav", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);
            CreateField("plati_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("sehrani_zpus", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("prac_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateField("prac_idcislo", DB_LONG, dbNotNullFieldOption);
            CreateField("ppom_ppcislo", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("ppom_pppopis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("cislo_pp", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("desc_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_prijmeni", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_jmeno", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_pppopis", DB_TEXT, 255, dbNullFieldOption);
        }
    }

    public class VyberPersPolCsvHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_PERSPOL_CSV_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberPersPolCsvHist(lpszOwnerName, lpszUsersName);
        }

        public VyberPersPolCsvHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("polozka_data_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("soubor_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_stav", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);
            CreateField("plati_mes", DB_INTEGER, dbNotNullFieldOption);

            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("sehrani_zpus", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("prac_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateField("prac_idcislo", DB_LONG, dbNotNullFieldOption);
            CreateField("ppom_ppcislo", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("ppom_pppopis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("cislo_pp", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("desc_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_prijmeni", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_jmeno", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_pppopis", DB_TEXT, 255, dbNullFieldOption);

            CreateField("poradi_poloz", DB_LONG, dbNotNullFieldOption);
            CreateField("zacat_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("konec_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("zacat_vyp", DB_INTEGER, dbNotNullFieldOption);
            CreateField("konec_vyp", DB_INTEGER, dbNotNullFieldOption);
            CreateField("polozka_idx", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_kod", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_dat", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("polozka_cis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("status_type", DB_INTEGER, dbNotNullFieldOption);
            CreateField("action_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("hodnota_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit01_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit01_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit02_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit02_text", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("goalam_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("datum_zacat", DB_DATE, dbNullFieldOption);
            CreateField("datum_konec", DB_DATE, dbNullFieldOption);
            CreateFTEXT("polozka_ccy", DB_TEXT, 10, dbNullFieldOption);
            CreateField("transac_src", DB_LONG, dbNotNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);
        }
    }

    public class VyberPolozkaCsvHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_POLOZKA_CSV_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberPolozkaCsvHist(lpszOwnerName, lpszUsersName);
        }

        public VyberPolozkaCsvHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_data_id", DB_LONG, dbNotNullFieldOption);
            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("zacat_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("konec_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("polozka_idx", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_typ", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_kod", DB_LONG, dbNotNullFieldOption);
            CreateField("polozka_dat", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("polozka_cis", DB_TEXT, 255, dbNullFieldOption);
            CreateField("status_type", DB_INTEGER, dbNotNullFieldOption);
            CreateField("action_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("hodnota_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit01_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit01_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("unit02_type", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("unit02_text", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("goalam_text", DB_TEXT, 255, dbNullFieldOption);
            CreateField("datum_zacat", DB_DATE, dbNullFieldOption);
            CreateField("datum_konec", DB_DATE, dbNullFieldOption);
            CreateFTEXT("polozka_ccy", DB_TEXT, 10, dbNullFieldOption);
            CreateField("transac_src", DB_LONG, dbNotNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);
        }
    }

    public class VyberProtokolCsvHist : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_PROTOKOL_CSV_HIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberProtokolCsvHist(lpszOwnerName, lpszUsersName);
        }

        public VyberProtokolCsvHist(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("protokol_davka_id", DB_LONG, dbNotNullFieldOption);
            CreateField("protokol_data_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pers_identif_id", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_id", DB_LONG, dbNotNullFieldOption);
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_idruh", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_iveta", DB_LONG, dbNotNullFieldOption);
            CreateField("soubor_separ", DB_LONG, dbNotNullFieldOption);
            CreateField("davka_imp_stav", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("nahran_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("nahran_datum", DB_DATE, dbNullFieldOption);

            CreateField("ulozeno_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateField("poradi_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("import_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("plati_mes", DB_INTEGER, dbNotNullFieldOption);
            CreateField("sehrani_zpus", DB_INTEGER, dbNotNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("cislo_pp", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("desc_oscislo", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_prijmeni", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_jmeno", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("desc_pppopis", DB_TEXT, 255, dbNullFieldOption);

            CreateField("protokol_mesic", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("protokol_wuser", DB_TEXT, 255, dbNullFieldOption);
            CreateField("protokol_datum", DB_DATE, dbNullFieldOption);

            CreateField("polozka_data_id", DB_LONG, dbNotNullFieldOption);
            CreateField("protokol_level", DB_INTEGER, dbNotNullFieldOption);
            CreateField("protokol_cislo", DB_LONG, dbNotNullFieldOption);
            CreateField("oper_result", DB_LONG, dbNotNullFieldOption);
            CreateField("code_result", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("info_result", DB_TEXT, 255, dbNullFieldOption);
            CreateField("oper_action", DB_LONG, dbNotNullFieldOption);
            CreateFTEXT("old_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("new_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("imp_hodnota", DB_TEXT, 255, dbNullFieldOption);
            CreateField("transac_src", DB_LONG, dbNotNullFieldOption);
        }
    }

    public class VyberPracPPomerList : TableDefInfo
    {
        const string TABLE_NAME = "VYBER_PRAC_PPOMER_LIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static TableDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new VyberPracPPomerList(lpszOwnerName, lpszUsersName);
        }

        public VyberPracPPomerList(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME)
        {
            CreateField("firma_id", DB_LONG, dbNotNullFieldOption);
            CreateField("pracovnik_id", DB_LONG, dbNotNullFieldOption);
            CreateField("cislo_pp", DB_INTEGER, dbNotNullFieldOption);
            CreateField("pocitane_obdobi", DB_INTEGER, dbNotNullFieldOption);
            CreateFTEXT("osobni_cislo", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("prijmeni", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("jmeno", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("titul_pred", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("titul_za", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("pompopis", DB_TEXT, 255, dbNullFieldOption);
            CreateFTEXT("funkce", DB_TEXT, 255, dbNullFieldOption);
            CreateField("praczac", DB_DATE, dbNullFieldOption);
            CreateField("prackon", DB_DATE, dbNullFieldOption);
            CreateField("druh", DB_BYTE, dbNotNullFieldOption);
            CreateField("druh07", DB_BYTE, dbNotNullFieldOption);
        } 
    }

}
