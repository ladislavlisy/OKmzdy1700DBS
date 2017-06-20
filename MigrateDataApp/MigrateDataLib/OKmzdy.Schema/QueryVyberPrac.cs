using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryVyberUtvaryInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERUTVARY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberUtvaryInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberUtvaryInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("utvnazev"),
                    SimpleInfo.Create("poznamka"),
                    SimpleInfo.Create("zeme_cislo"),
                    SimpleInfo.Create("vyuctgr")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UZIVATEL", TableUzivatelInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uuzivatel_id"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("uzivfunkce"),
                    SimpleInfo.Create("uplne_jmeno")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UTVAR", "UZIVATEL").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uuzivatel_id"));

        }
    }
    class QueryVyberPracZarazeniInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRAC_ZARAZENI";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracZarazeniInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracZarazeniInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("MDAN_UTVAR", TableDanInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("mesic"),
                    AliasInfo.Create("sazba", "cislo_utvar"),
                    AliasInfo.Create("pocjed", "cislo_zdrpoj")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("MDAN_STRCZ", TableDanInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo_zdroj"),
                    SimpleInfo.Create("cislo_stred"),
                    SimpleInfo.Create("cislo_cinnost"),
                    SimpleInfo.Create("cislo_zakazka")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("MUTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("utvnazev", "nazev_utvar"),
                    AliasInfo.Create("zeme_cislo", "cislo_zeme")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("MDAN_UTVAR", "MDAN_STRCZ").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddColumn("mesic", "mesic").
                AddLeftColumn("odkud", "=", "0").
                AddLeftColumn("kod", "=", "6100").
                AddLeftColumn("cislo", "=", "1").
                AddRightColumn("odkud", "=", "0").
                AddRightColumn("kod", "=", "6101").
                AddRightColumn("cislo", "=", "1"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("MDAN_UTVAR", "MUTVAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("sazba", "uutvar_id"));
        }
    }
    class QueryVyberPPomZarazeniInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPPOM_ZARAZENI";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPPomZarazeniInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPPomZarazeniInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("MMZDA_STRCZ", TableMzdaInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("mesic"),
                    AliasInfo.Create("cislo_zdroj", "ppomer_zdroj"),
                    AliasInfo.Create("cislo_stred", "ppomer_stred"),
                    AliasInfo.Create("cislo_cinnost", "ppomer_cinnost"),
                    AliasInfo.Create("cislo_zakazka", "ppomer_zakazka")
                ));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("MMZDA_STRCZ", TableZsestExtMzdlistInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddConstraints(
                    FiltrSpecsInfo.Create("odkud", "=", "0"),
                    FiltrSpecsInfo.Create("kod", "=", "6000"),
                    FiltrSpecsInfo.Create("cislo", "=", "1")
                ));
        }
    }
    class QueryVyberPracOpravneniInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRAC_OPRAVNENI";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracOpravneniInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracOpravneniInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("DAN_UTVAR", TableDanInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("mesic"),
                    AliasInfo.Create("informace", "vyuct_cast")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("uutvar_id"),
                    AliasInfo.Create("utvnazev", "nazev_utvar"),
                    AliasInfo.Create("zeme_cislo", "cislo_zeme"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UPRES", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("uutvar_id", "uupres_id"),
                    AliasInfo.Create("utvnazev", "nazev_upres"),
                    AliasInfo.Create("uzivatel_id", "uziv_pres"),
                    AliasInfo.Create("personal_id", "pers_pres"),
                    AliasInfo.Create("prohlizet_id", "proh_pres"),
                    AliasInfo.Create("omezproh_id", "omez_pres"),
                    AliasInfo.Create("referzp_id", "refz_pres")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UZIVATEL", TableUzivatelInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivjmeno")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("DAN_UTVAR", "UTVAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("sazba", "uutvar_id").
                AddLeftColumn("odkud", "=", "0").
                AddLeftColumn("kod", "=", "6100").
                AddLeftColumn("cislo", "=", "1"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("DAN_UTVAR", "UPRES").
                AddColumn("firma_id", "firma_id").
                AddColumn("hodnota", "uutvar_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("UTVAR", "UZIVATEL").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uuzivatel_id"));

        }
    }
    class QueryVyberPracovnikyFiltrXOInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRACOVNIKY_FILTR_XO";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracovnikyFiltrXOInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracovnikyFiltrXOInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppracovnik_id", "pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPOMER", TablePpomerInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("OPR", QueryVyberPracOpravneniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "zar_mesic"),
                    AliasInfo.Create("mesic", "opr_mesic"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("nazev_utvar"),
                    SimpleInfo.Create("cislo_zeme"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id"),
                    SimpleInfo.Create("uziv_pres"),
                    SimpleInfo.Create("pers_pres"),
                    SimpleInfo.Create("proh_pres"),
                    SimpleInfo.Create("omez_pres"),
                    SimpleInfo.Create("refz_pres"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id"),
                    SimpleInfo.Create("nazev_upres")
               ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "PPOMER").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "OPR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));

        }
    }
    class QueryVyberPracovnikyFiltrXPInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRACOVNIKY_FILTR_XP";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracovnikyFiltrXPInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracovnikyFiltrXPInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppracovnik_id", "pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPOMER", TablePpomerInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("OPR", QueryVyberPracOpravneniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "opr_mesic"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("nazev_utvar"),
                    SimpleInfo.Create("cislo_zeme"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id"),
                    SimpleInfo.Create("uziv_pres"),
                    SimpleInfo.Create("pers_pres"),
                    SimpleInfo.Create("proh_pres"),
                    SimpleInfo.Create("omez_pres"),
                    SimpleInfo.Create("refz_pres"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id"),
                    SimpleInfo.Create("nazev_upres")
               ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPZ", QueryVyberPPomZarazeniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "zar_mesic"),
                    SimpleInfo.Create("ppomer_zdroj"),
                    SimpleInfo.Create("ppomer_stred"),
                    SimpleInfo.Create("ppomer_cinnost"),
                    SimpleInfo.Create("ppomer_zakazka")
               ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "PPOMER").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "OPR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "PPZ").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddColumn("cislo_pp", "cislo_pp"));

        }
    }
    class QueryVyberPracovnikyFiltrXZInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRACOVNIKY_FILTR_XZ";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracovnikyFiltrXZInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracovnikyFiltrXZInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppracovnik_id", "pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPOMER", TablePpomerInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("OPR", QueryVyberPracOpravneniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "opr_mesic"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id"),
                    SimpleInfo.Create("uziv_pres"),
                    SimpleInfo.Create("pers_pres"),
                    SimpleInfo.Create("proh_pres"),
                    SimpleInfo.Create("omez_pres"),
                    SimpleInfo.Create("refz_pres"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id"),
                    SimpleInfo.Create("nazev_upres")
               ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ZAR", QueryVyberPracZarazeniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "zar_mesic"),
                    SimpleInfo.Create("nazev_utvar"),
                    SimpleInfo.Create("cislo_zeme"),
                    SimpleInfo.Create("cislo_utvar"),
                    SimpleInfo.Create("cislo_zdrpoj"),
                    SimpleInfo.Create("cislo_zdroj"),
                    SimpleInfo.Create("cislo_stred"),
                    SimpleInfo.Create("cislo_cinnost"),
                    SimpleInfo.Create("cislo_zakazka")
               ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "PPOMER").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "OPR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "ZAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));
        }
    }
    class QueryVyberPracovnikyFiltrZPInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRACOVNIKY_FILTR_ZP";

        const string QUERY_SQL = "SELECT " +
                    " [PRAC].[firma_id] AS [firma_id], " +
                    " [PRAC].[ppracovnik_id] AS [pracovnik_id],  " +
                    " [ZAR].[mesic] AS [zar_mesic], [OPR].[mesic] AS [opr_mesic], " +
                    " [PRAC].[logicky_zrusen] AS [logicky_zrusen], [PRAC].[logicky_neuplny] AS [logicky_neuplny],  " +
                    " [PRAC].[osobni_cislo] AS [osobni_cislo], [PRAC].[rodne_cislo] AS [rodne_cislo], [PRAC].[nema_rodcis] AS [nema_rodcis], " +
                    " [PRAC].[prijmeni] AS [prijmeni], [PRAC].[jmeno] AS [jmeno], [PRAC].[titul_pred] AS [titul_pred], [PRAC].[titul_za] AS [titul_za], " +
                    " [PPOMER].[cislo_pp] AS [cislo_pp], [PPOMER].[pompopis] AS [pompopis], [PPOMER].[funkce] AS [funkce], " +
                    " [PPOMER].[praczac] AS [praczac], [PPOMER].[prackon] AS [prackon], [PPOMER].[druh] AS [druh], [PPOMER].[druh07] AS [druh07], " +
                    " [OPR].[uzivjmeno] AS [uzivjmeno], [ZAR].[nazev_utvar] AS [nazev_utvar], [ZAR].[cislo_zeme] AS [cislo_zeme], " +
                    " [PRAC].[datum_narozeni] AS [datum_narozeni], [PRAC].[pohlavi] AS [pohlavi], " +
                    " [PPOMER].[ppomer_cislo] AS [ppomer_cislo], " +
                    " [PPOMER].[kzam] AS [kzam], [PPOMER].[platova_trida] AS [platova_trida], [PPOMER].[platovy_stupen] AS [platovy_stupen], " +
                    " [PPOMER].[spraxe_roku] AS [spraxe_roku], [PPOMER].[praxe_dnu] AS [praxe_dnu], " +
                    " [OPR].[uzivatel_id] AS [uzivatel_id], [OPR].[personal_id] AS [personal_id], [OPR].[prohlizet_id] AS [prohlizet_id], " +
                    " [OPR].[omezproh_id] AS [omezproh_id], [OPR].[referzp_id] AS [referzp_id], " +
                    " [OPR].[uziv_pres] AS [uziv_pres], [OPR].[pers_pres] AS [pers_pres], [OPR].[proh_pres] AS [proh_pres], " +
                    " [OPR].[omez_pres] AS [omez_pres], [OPR].[refz_pres] AS [refz_pres], " +
                    " [OPR].[uutvar_id] AS [uutvar_id], [OPR].[vyuctgr] AS [vyuctgr], [OPR].[vyuct_cast] AS [vyuct_cast], " +
                    " [OPR].[uupres_id] AS [uupres_id], [OPR].[nazev_upres] AS [nazev_upres], " +
                    " [ZAR].[cislo_utvar] AS [cislo_utvar], " +
                    " [ZAR].[cislo_zdrpoj] AS [cislo_zdrpoj], " +
                    " [ZAR].[cislo_zdroj] AS [cislo_zdroj], [ZAR].[cislo_stred] AS [cislo_stred], " +
                    " [ZAR].[cislo_cinnost] AS [cislo_cinnost], [ZAR].[cislo_zakazka] AS [cislo_zakazka], " +
                    " [PPZ].[ppomer_zdroj] AS [ppomer_zdroj], [PPZ].[ppomer_stred] AS [ppomer_stred], " +
                    " [PPZ].[ppomer_cinnost] AS [ppomer_cinnost], [PPZ].[ppomer_zakazka] AS [ppomer_zakazka] " +
                    " FROM [PRAC], [PPOMER], [VYBERPRAC_OPRAVNENI] [OPR], [VYBERPRAC_ZARAZENI] [ZAR], [VYBERPPOM_ZARAZENI] [PPZ] " +
                    " WHERE PRAC.firma_id = PPOMER.firma_id " +
                    " AND   OPR.firma_id = PPOMER.firma_id " +
                    " AND   ZAR.firma_id = PPOMER.firma_id " +
                    " AND   PPZ.firma_id = PPOMER.firma_id " +
                    " AND   PRAC.ppracovnik_id = PPOMER.pracovnik_id " +
                    " AND   OPR.pracovnik_id = PPOMER.pracovnik_id " +
                    " AND   ZAR.pracovnik_id = PPOMER.pracovnik_id " +
                    " AND   PPZ.pracovnik_id = PPOMER.pracovnik_id " +
                    " AND   PPZ.cislo_pp = PPOMER.cislo_pp " +
                    " AND   PPZ.mesic = ZAR.mesic ";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracovnikyFiltrZPInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracovnikyFiltrZPInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppracovnik_id", "pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPOMER", TablePpomerInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("OPR", QueryVyberPracOpravneniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "opr_mesic"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id"),
                    SimpleInfo.Create("uziv_pres"),
                    SimpleInfo.Create("pers_pres"),
                    SimpleInfo.Create("proh_pres"),
                    SimpleInfo.Create("omez_pres"),
                    SimpleInfo.Create("refz_pres"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id"),
                    SimpleInfo.Create("nazev_upres")
               ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ZAR", QueryVyberPracZarazeniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    AliasInfo.Create("mesic", "zar_mesic"),
                    SimpleInfo.Create("nazev_utvar"),
                    SimpleInfo.Create("cislo_zeme"),
                    SimpleInfo.Create("cislo_utvar"),
                    SimpleInfo.Create("cislo_zdrpoj"),
                    SimpleInfo.Create("cislo_zdroj"),
                    SimpleInfo.Create("cislo_stred"),
                    SimpleInfo.Create("cislo_cinnost"),
                    SimpleInfo.Create("cislo_zakazka")
               ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPZ", QueryVyberPPomZarazeniInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    SimpleInfo.Create("ppomer_zdroj"),
                    SimpleInfo.Create("ppomer_stred"),
                    SimpleInfo.Create("ppomer_cinnost"),
                    SimpleInfo.Create("ppomer_zakazka")
               ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "PPOMER").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "OPR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "ZAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PPOMER", "PPZ").
                AddColumn("firma_id", "firma_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddColumn("cislo_pp", "cislo_pp").
                AddRightColumn("mesic", "=", "ZAR.mesic"));
        }
    }
    public class QueryVyberPracPocitanyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRAC_POCITANY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracPocitanyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracPocitanyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("VPFZ", TablePracVyberAggrInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    AliasInfo.Create("zar_mesic", "mesic"),
                    SimpleInfo.Create("zar_mesic"),
                    SimpleInfo.Create("opr_mesic"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu"),
                    SimpleInfo.Create("cislo_zdrpoj"),
                    SimpleInfo.Create("cislo_zdroj"),
                    SimpleInfo.Create("cislo_stred"),
                    SimpleInfo.Create("cislo_cinnost"),
                    SimpleInfo.Create("cislo_zakazka"),
                    SimpleInfo.Create("ppomer_zdroj"),
                    SimpleInfo.Create("ppomer_stred"),
                    SimpleInfo.Create("ppomer_cinnost"),
                    SimpleInfo.Create("ppomer_zakazka"),
                    SimpleInfo.Create("cislo_utvar"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id")));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("utvnazev", "nazev_utvar"),
                    AliasInfo.Create("zeme_cislo", "cislo_zeme"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id")));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UPRES", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("utvnazev", "nazev_upres"),
                    AliasInfo.Create("uzivatel_id", "uziv_pres"),
                    AliasInfo.Create("personal_id", "pers_pres"),
                    AliasInfo.Create("prohlizet_id", "proh_pres"),
                    AliasInfo.Create("omezproh_id", "omez_pres"),
                    AliasInfo.Create("referzp_id", "refz_pres")));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UZIVATEL", TableUzivatelInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivjmeno")));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("VPFZ", "UTVAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("cislo_utvar", "uutvar_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("VPFZ", "UPRES").
                AddColumn("firma_id", "firma_id").
                AddColumn("uupres_id", "uutvar_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("UTVAR", "UZIVATEL").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uuzivatel_id"));
        }

    }

    class QueryVyberPracovnikyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPRACOVNIKY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPracovnikyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPracovnikyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("VPFZ", QueryVyberPracovnikyFiltrZPInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    AliasInfo.Create("zar_mesic", "mesic"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("nema_rodcis"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("pompopis"),
                    SimpleInfo.Create("funkce"),
                    SimpleInfo.Create("praczac"),
                    SimpleInfo.Create("prackon"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("druh07"),
                    SimpleInfo.Create("uzivjmeno"),
                    SimpleInfo.Create("nazev_utvar"),
                    SimpleInfo.Create("cislo_zeme"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("pohlavi"),
                    SimpleInfo.Create("ppomer_cislo"),
                    SimpleInfo.Create("kzam"),
                    SimpleInfo.Create("platova_trida"),
                    SimpleInfo.Create("platovy_stupen"),
                    SimpleInfo.Create("spraxe_roku"),
                    SimpleInfo.Create("praxe_dnu"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("personal_id"),
                    SimpleInfo.Create("prohlizet_id"),
                    SimpleInfo.Create("omezproh_id"),
                    SimpleInfo.Create("referzp_id"),
                    SimpleInfo.Create("uziv_pres"),
                    SimpleInfo.Create("pers_pres"),
                    SimpleInfo.Create("proh_pres"),
                    SimpleInfo.Create("omez_pres"),
                    SimpleInfo.Create("refz_pres"),
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("uupres_id"),
                    SimpleInfo.Create("nazev_upres"),
                    SimpleInfo.Create("cislo_utvar"),
                    SimpleInfo.Create("cislo_zdrpoj"),
                    SimpleInfo.Create("cislo_zdroj"),
                    SimpleInfo.Create("cislo_stred"),
                    SimpleInfo.Create("cislo_cinnost"),
                    SimpleInfo.Create("cislo_zakazka"),
                    SimpleInfo.Create("ppomer_zdroj"),
                    SimpleInfo.Create("ppomer_stred"),
                    SimpleInfo.Create("ppomer_cinnost"),
                    SimpleInfo.Create("ppomer_zakazka")
                 ));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("VPFZ", QueryVyberPracovnikyFiltrZPInfo.GetDictValue(lpszOwnerName, lpszUsersName).GetTableDef()).
                AddConstraints(
                    FiltrSpecsInfo.Create("zar_mesic", "=", "opr_mesic")
                ));
        }
    }
}
