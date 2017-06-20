using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryVyberReldp09DavkaInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERRELDP09DAVKA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberReldp09DavkaInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberReldp09DavkaInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UD", TableUzivReldpDavkaInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ddavka_reldp_id", "davka_reldp_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    AliasInfo.Create("vydano_dat", "vydano_dat", "MAX({0})"),
                    AliasInfo.Create("info_davka", "info_davka", "MAX({0})")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PP", TablePracReldp09DataPracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("eldp_rok", "eldp_rok", "MAX({0})")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UD", "PP").
                AddColumn("firma_id", "firma_id").
                AddColumn("ddavka_reldp_id", "davka_reldp_id").
                AddColumn("uzivatel_id", "uzivatel_id"));

            AddClose(QueryCloseInfo.Create("GROUP BY UD.firma_id, UD.ddavka_reldp_id, UD.uzivatel_id"));
        }
    }
    class QueryVyberReldp09DataInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERRELDP09DATA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberReldp09DataInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberReldp09DataInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UD", TableUzivReldpDavkaInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("ddavka_reldp_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("vydano_dat"),
                    SimpleInfo.Create("info_davka")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("EPRAC", TablePracReldp09DataPracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("sprava_id"),
                    SimpleInfo.Create("eldp_rok"),
                    SimpleInfo.Create("eldp_oprava"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul"),
                    SimpleInfo.Create("rodne_prijmeni"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("ulice"),
                    SimpleInfo.Create("cislo_domu"),
                    SimpleInfo.Create("obec"),
                    SimpleInfo.Create("posta"),
                    SimpleInfo.Create("psc"),
                    SimpleInfo.Create("stat"),
                    SimpleInfo.Create("misto_narozeni")
                   ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("EPOJ", TablePracReldp09DataPojInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("pomer_id"),
                    SimpleInfo.Create("eldp_typ"),
                    SimpleInfo.Create("stranka"),
                    SimpleInfo.Create("radek"),
                    SimpleInfo.Create("kod"),
                    SimpleInfo.Create("maly_rozsah"),
                    SimpleInfo.Create("cinn_od"),
                    SimpleInfo.Create("cinn_do"),
                    SimpleInfo.Create("dny"),
                    SimpleInfo.Create("vyd_cinn_od"),
                    SimpleInfo.Create("s1"),
                    SimpleInfo.Create("s2"),
                    SimpleInfo.Create("s3"),
                    SimpleInfo.Create("s4"),
                    SimpleInfo.Create("s5"),
                    SimpleInfo.Create("s6"),
                    SimpleInfo.Create("s7"),
                    SimpleInfo.Create("s8"),
                    SimpleInfo.Create("s9"),
                    SimpleInfo.Create("s10"),
                    SimpleInfo.Create("s11"),
                    SimpleInfo.Create("s12"),
                    SimpleInfo.Create("s1_12"),
                    SimpleInfo.Create("vylouc_doby1"),
                    SimpleInfo.Create("vymer_zaklad1"),
                    SimpleInfo.Create("doby_odecet1"),
                    SimpleInfo.Create("vylouc_doby2"),
                    SimpleInfo.Create("vymer_zaklad2"),
                    SimpleInfo.Create("doby_odecet2"),
                    SimpleInfo.Create("vylouc_doby3"),
                    SimpleInfo.Create("vymer_zaklad3"),
                    SimpleInfo.Create("doby_odecet3"),
                    SimpleInfo.Create("vylouc_doby4"),
                    SimpleInfo.Create("vymer_zaklad4"),
                    SimpleInfo.Create("doby_odecet4"),
                    SimpleInfo.Create("vylouc_doby5"),
                    SimpleInfo.Create("vymer_zaklad5"),
                    SimpleInfo.Create("doby_odecet5"),
                    SimpleInfo.Create("vylouc_doby6"),
                    SimpleInfo.Create("vymer_zaklad6"),
                    SimpleInfo.Create("doby_odecet6"),
                    SimpleInfo.Create("vylouc_doby7"),
                    SimpleInfo.Create("vymer_zaklad7"),
                    SimpleInfo.Create("doby_odecet7"),
                    SimpleInfo.Create("vylouc_doby8"),
                    SimpleInfo.Create("vymer_zaklad8"),
                    SimpleInfo.Create("doby_odecet8"),
                    SimpleInfo.Create("vylouc_doby9"),
                    SimpleInfo.Create("vymer_zaklad9"),
                    SimpleInfo.Create("doby_odecet9"),
                    SimpleInfo.Create("vylouc_doby10"),
                    SimpleInfo.Create("vymer_zaklad10"),
                    SimpleInfo.Create("doby_odecet10"),
                    SimpleInfo.Create("vylouc_doby11"),
                    SimpleInfo.Create("vymer_zaklad11"),
                    SimpleInfo.Create("doby_odecet11"),
                    SimpleInfo.Create("vylouc_doby12"),
                    SimpleInfo.Create("vymer_zaklad12"),
                    SimpleInfo.Create("doby_odecet12")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UD", "EPRAC").
                AddColumn("firma_id", "firma_id").
                AddColumn("ddavka_reldp_id", "davka_reldp_id").
                AddColumn("uzivatel_id", "uzivatel_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("EPRAC", "EPOJ").
                AddColumn("firma_id", "firma_id").
                AddColumn("davka_reldp_id", "davka_reldp_id").
                AddColumn("uzivatel_id", "uzivatel_id").
                AddColumn("pracovnik_id", "pracovnik_id").
                AddColumn("sprava_id", "sprava_id"));

        }
    }
    class QueryVyberReldpDavkaInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERRELDPDAVKA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberReldpDavkaInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberReldpDavkaInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UD", TableUzivReldpDavkaInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ddavka_reldp_id", "davka_reldp_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    AliasInfo.Create("vydano_dat", "vydano_dat", "MAX({0})"),
                    AliasInfo.Create("info_davka", "info_davka", "MAX({0})")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PL", TablePracReldpListekInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    AliasInfo.Create("eldprok", "eldprok", "MAX({0})")
                 ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UD", "PL").
                AddColumn("firma_id", "firma_id").
                AddColumn("ddavka_reldp_id", "davka_reldp_id").
                AddColumn("uzivatel_id", "uzivatel_id"));

            AddClose(QueryCloseInfo.Create("GROUP BY UD.firma_id, UD.ddavka_reldp_id, UD.uzivatel_id"));
        }
    }
    class QueryVyberReldpDataInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERRELDPDATA";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberReldpDataInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberReldpDataInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UD", TableUzivReldpDavkaInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("vydano_dat"),
                    SimpleInfo.Create("neprijata"),
                    SimpleInfo.Create("info_davka")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PL", TablePracReldpListekInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("davka_reldp_id"),
                    SimpleInfo.Create("eldppgn"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("info_eldp"),
                    SimpleInfo.Create("eldprok"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("rodne_prijmeni"),
                    SimpleInfo.Create("uplne_jmeno"),
                    SimpleInfo.Create("misto_narozeni"),
                    SimpleInfo.Create("posledni_prijmeni"),
                    SimpleInfo.Create("adresa1_3"),
                    SimpleInfo.Create("eldptyp"),
                    SimpleInfo.Create("zacatek"),
                    SimpleInfo.Create("eldpvydzedne"),
                    SimpleInfo.Create("eldpoprzedne")
                    ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PD", TablePracReldpDataInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("listek_reldp_id"),
                    SimpleInfo.Create("pomer_id"),
                    SimpleInfo.Create("eldpkod"),
                    SimpleInfo.Create("eldpod"),
                    SimpleInfo.Create("eldpdo"),
                    SimpleInfo.Create("eldppoj"),
                    SimpleInfo.Create("eldpdnym01"),
                    SimpleInfo.Create("eldpdnym02"),
                    SimpleInfo.Create("eldpdnym03"),
                    SimpleInfo.Create("eldpdnym04"),
                    SimpleInfo.Create("eldpdnym05"),
                    SimpleInfo.Create("eldpdnym06"),
                    SimpleInfo.Create("eldpdnym07"),
                    SimpleInfo.Create("eldpdnym08"),
                    SimpleInfo.Create("eldpdnym09"),
                    SimpleInfo.Create("eldpdnym10"),
                    SimpleInfo.Create("eldpdnym11"),
                    SimpleInfo.Create("eldpdnym12"),
                    SimpleInfo.Create("zaklvym01"),
                    SimpleInfo.Create("zaklvym02"),
                    SimpleInfo.Create("zaklvym03"),
                    SimpleInfo.Create("zaklvym04"),
                    SimpleInfo.Create("zaklvym05"),
                    SimpleInfo.Create("zaklvym06"),
                    SimpleInfo.Create("zaklvym07"),
                    SimpleInfo.Create("zaklvym08"),
                    SimpleInfo.Create("zaklvym09"),
                    SimpleInfo.Create("zaklvym10"),
                    SimpleInfo.Create("zaklvym11"),
                    SimpleInfo.Create("zaklvym12"),
                    SimpleInfo.Create("eldpvcm")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UD", "PL").
                AddColumn("firma_id", "firma_id").
                AddColumn("ddavka_reldp_id", "davka_reldp_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("PL", "PD").
                AddColumn("firma_id", "firma_id").
                AddColumn("llistek_reldp_id", "listek_reldp_id").
                AddColumn("eldpPgn", "eldpPgn"));

        }
    }
}
