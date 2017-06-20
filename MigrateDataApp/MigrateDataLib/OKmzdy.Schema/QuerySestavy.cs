using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QuerySocialInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_SOCIAL";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QuerySocialInfo(lpszOwnerName, lpszUsersName);
        }
        public QuerySocialInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TableZsestSocialInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("druh_cinnosti", "max_vymzakl"),
                    AliasInfo.Create("zap_prijem", "sum_prijem"),
                    AliasInfo.Create("pojist_org", "pojsum_org"),
                    AliasInfo.Create("pojistne_zam", "pojsum_zam")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PPOM", TableZsestSocialInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("cislo_pp"),
                    SimpleInfo.Create("socsprava_id"),
                    SimpleInfo.Create("info_org"),
                    SimpleInfo.Create("uplne_jmeno"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("ucast_poj"),
                    SimpleInfo.Create("ppom_stav"),
                    SimpleInfo.Create("koef_poj"),
                    SimpleInfo.Create("druh_cinnosti"),
                    SimpleInfo.Create("zap_prijem"),
                    SimpleInfo.Create("pojist_org"),
                    SimpleInfo.Create("pojistne_zam"),
                    SimpleInfo.Create("vyl_doby"),
                    SimpleInfo.Create("absence"),
                    SimpleInfo.Create("nemoc_dny"),
                    SimpleInfo.Create("nemoc_kc"),
                    SimpleInfo.Create("osetrovani_dny"),
                    SimpleInfo.Create("osetrovani_kc"),
                    SimpleInfo.Create("materstvi_kc"),
                    SimpleInfo.Create("materstvi_n"),
                    SimpleInfo.Create("vyrovnani_n"),
                    SimpleInfo.Create("vyrovnani_kc"),
                    SimpleInfo.Create("davka_e_kod"),
                    SimpleInfo.Create("davka_e_kc"),
                    SimpleInfo.Create("davka_f_kod"),
                    SimpleInfo.Create("davka_f_kc")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "PPOM").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uzivatel_id").
                AddColumn("kod_data", "kod_data").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddColumn("socsprava_id", "socsprava_id").
                AddLeftColumn("cislo_pp", "=", "0").
                AddRightColumn("cislo_pp", "<>", "0"));

        }
    }
    class QueryVyplListkySortInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_VYPL_LISTKY_SORT";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyplListkySortInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyplListkySortInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ZSEST_VYPL_LISTKY", TableZsestVyplListkyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("vlist_info"),
                    SimpleInfo.Create("radek"),
                    SimpleInfo.Create("typ"),
                    SimpleInfo.Create("radtext"),
                    SimpleInfo.Create("engtext")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("LISTKYP", TableZsestVyplListkyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("typ", "typ_pr"),
                    AliasInfo.Create("radtext", "text_pr")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("LISTKYU", TableZsestVyplListkyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("typ", "typ_ut"),
                    AliasInfo.Create("radtext", "text_ut")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("ZSEST_VYPL_LISTKY", "LISTKYP").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uzivatel_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddRightColumn("typ", "=", "2"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("ZSEST_VYPL_LISTKY", "LISTKYU").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uzivatel_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddRightColumn("typ", "=", "1"));

        }
    }
    class QueryMzvIIsspPredpisyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VMZV_IISSP_PREDPISY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryMzvIIsspPredpisyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryMzvIIsspPredpisyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("MP", TableMzvIisspPredpisyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("vtrideni"),
                    SimpleInfo.Create("davka_id"),
                    SimpleInfo.Create("rok"),
                    SimpleInfo.Create("mesic"),
                    SimpleInfo.Create("vyuct_cast"),
                    SimpleInfo.Create("mena"),
                    SimpleInfo.Create("castkakc"),
                    SimpleInfo.Create("castka_mena"),
                    SimpleInfo.Create("kurz_mena"),
                    SimpleInfo.Create("datum_kurz"),
                    SimpleInfo.Create("datum_vypl"),
                    SimpleInfo.Create("datum_exp"),
                    SimpleInfo.Create("id_koruny"),
                    SimpleInfo.Create("id_syntet"),
                    SimpleInfo.Create("id_paragraf"),
                    SimpleInfo.Create("id_polozka"),
                    SimpleInfo.Create("id_analyt"),
                    SimpleInfo.Create("stredisko"),
                    SimpleInfo.Create("popis_koruny")
                ));
        }
    }
}
