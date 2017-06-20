using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryVyberSbMediaInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERSBMEDIA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberSbMediaInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberSbMediaInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("SM", TableSberneMediumInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("smedium_id"),
                    SimpleInfo.Create("info_medium"),
                    SimpleInfo.Create("nazev_klienta"),
                    SimpleInfo.Create("soubor_start"),
                    SimpleInfo.Create("soubor_stop"),
                    SimpleInfo.Create("soubor_next"),
                    SimpleInfo.Create("soub_cis1"),
                    SimpleInfo.Create("soub_cis2"),
                    SimpleInfo.Create("soub_cis3"),
                    SimpleInfo.Create("soub_txt1"),
                    SimpleInfo.Create("soub_txt2"),
                    SimpleInfo.Create("soub_txt3"),
                    SimpleInfo.Create("datum_predani"),
                    SimpleInfo.Create("datum_transakce"),
                    SimpleInfo.Create("datum_splatnosti"),
                    SimpleInfo.Create("datum_prevodu"),
                    SimpleInfo.Create("datum_popprev"),
                    SimpleInfo.Create("bezny_bkspoj_id"),
                    SimpleInfo.Create("debet_bkspoj_id"),
                    SimpleInfo.Create("cpopl_bkspoj_id"),
                    SimpleInfo.Create("dpopl_bkspoj_id"),
                    SimpleInfo.Create("par_kod_obr11"),
                    SimpleInfo.Create("par_kod_obr32"),
                    SimpleInfo.Create("crc"),
                    SimpleInfo.Create("prior_prevodu"),
                    SimpleInfo.Create("prior_popprev"),
                    SimpleInfo.Create("cislo_klienta"),
                    SimpleInfo.Create("telefon"),
                    SimpleInfo.Create("ucet_dal"),
                    SimpleInfo.Create("ucet_md"),
                    SimpleInfo.Create("ucetni_zdr"),
                    SimpleInfo.Create("ucetni_str"),
                    SimpleInfo.Create("ucetni_zak"),
                    SimpleInfo.Create("ucetni_cin"),
                    SimpleInfo.Create("prijem_id")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("SLST", TableSestavyLstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("nazev"),
                    SimpleInfo.Create("soubor"),
                    SimpleInfo.Create("trideni"),
                    SimpleInfo.Create("kod_lst"),
                    SimpleInfo.Create("kod_data")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ULST", TableSestavyUlstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("sestavy_id"),
                    SimpleInfo.Create("exp_cesta")
                    ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("SM", "SLST").
                AddColumn("firma_id", "firma_id").
                AddColumn("smedium_id", "subjekt_id").
                AddRightColumn("typ_lst", "=", "2"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("SLST", "ULST").
                AddColumn("firma_id", "firma_id").
                AddColumn("kod_lst", "kod_lst"));

        }
    }
    class QueryVyberPlMediaInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERPLMEDIA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberPlMediaInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberPlMediaInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PM", TablePlatebniMediumInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pmedium_id"),
                    SimpleInfo.Create("info_medium"),
                    SimpleInfo.Create("nazev_klienta"),
                    SimpleInfo.Create("soubor_start"),
                    SimpleInfo.Create("soubor_stop"),
                    SimpleInfo.Create("soubor_next"),
                    SimpleInfo.Create("datum_predani"),
                    SimpleInfo.Create("pol_cis1"),
                    SimpleInfo.Create("pol_cis2"),
                    SimpleInfo.Create("pol_cis3"),
                    SimpleInfo.Create("pol_txt1"),
                    SimpleInfo.Create("pol_txt2"),
                    SimpleInfo.Create("pol_txt3"),
                    SimpleInfo.Create("soub_cis1"),
                    SimpleInfo.Create("soub_cis2"),
                    SimpleInfo.Create("soub_cis3"),
                    SimpleInfo.Create("soub_txt1"),
                    SimpleInfo.Create("soub_txt2"),
                    SimpleInfo.Create("soub_txt3"),
                    SimpleInfo.Create("mena"),
                    SimpleInfo.Create("ustav"),
                    SimpleInfo.Create("cislo_pobocky"),
                    SimpleInfo.Create("prideleny_kod"),
                    SimpleInfo.Create("banka_id")
            ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("SLST", TableSestavyLstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("nazev"),
                    SimpleInfo.Create("soubor"),
                    SimpleInfo.Create("trideni"),
                    SimpleInfo.Create("kod_lst"),
                    SimpleInfo.Create("kod_data")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ULST", TableSestavyUlstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("sestavy_id"),
                    SimpleInfo.Create("exp_cesta")
                    ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PM", "SLST").
                AddColumn("firma_id", "firma_id").
                AddColumn("pmedium_id", "subjekt_id").
                AddRightColumn("typ_lst", "=", "3"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("SLST", "ULST").
                AddColumn("firma_id", "firma_id").
                AddColumn("kod_lst", "kod_lst"));

        }
    }
    class QueryVyberUcMediaInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERUCMEDIA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberUcMediaInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberUcMediaInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UM", TableUcetniMediumInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("umedium_id"),
                    SimpleInfo.Create("info_medium"),
                    SimpleInfo.Create("doklad_druh"),
                    SimpleInfo.Create("soub_cis1"),
                    SimpleInfo.Create("soub_cis2"),
                    SimpleInfo.Create("soub_cis3"),
                    SimpleInfo.Create("soub_txt1"),
                    SimpleInfo.Create("soub_txt2"),
                    SimpleInfo.Create("soub_txt3"),
                    SimpleInfo.Create("doklad_cis1"),
                    SimpleInfo.Create("doklad_cis2"),
                    SimpleInfo.Create("doklad_cis3"),
                    SimpleInfo.Create("doklad_txt1"),
                    SimpleInfo.Create("doklad_txt2"),
                    SimpleInfo.Create("doklad_txt3"),
                    SimpleInfo.Create("ucto_klicmd"),
                    SimpleInfo.Create("ucto_klicdal"),
                    SimpleInfo.Create("znam_plus"),
                    SimpleInfo.Create("znam_minus"),
                    SimpleInfo.Create("domaci_mena"),
                    SimpleInfo.Create("spoj_ucet_md"),
                    SimpleInfo.Create("spoj_ucet_dal"),
                    SimpleInfo.Create("bezny_ucet_md"),
                    SimpleInfo.Create("bezny_ucet_dal"),
                    SimpleInfo.Create("korekce_zdr"),
                    SimpleInfo.Create("korekce_str"),
                    SimpleInfo.Create("korekce_zak"),
                    SimpleInfo.Create("korekce_cin")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("SLST", TableSestavyLstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("nazev"),
                    SimpleInfo.Create("soubor"),
                    SimpleInfo.Create("trideni"),
                    SimpleInfo.Create("kod_lst"),
                    SimpleInfo.Create("kod_data")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ULST", TableSestavyUlstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("sestavy_id"),
                    SimpleInfo.Create("exp_cesta")
                    ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UM", "SLST").
                AddColumn("firma_id", "firma_id").
                AddColumn("umedium_id", "subjekt_id").
                AddRightColumn("typ_lst", "=", "1"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("SLST", "ULST").
                AddColumn("firma_id", "firma_id").
                AddColumn("kod_lst", "kod_lst"));
        }
    }
}
