using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryKrokujPracPocitanyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "KROKUJPRAC_POCITANY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryKrokujPracPocitanyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryKrokujPracPocitanyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracVyberAggrInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("pocitane_obdobi"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za"),
                    AliasInfo.Create("zar_mesic", "mesic"),
                    SimpleInfo.Create("vyuct_cast")
                    ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("utvnazev"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("zeme_cislo"),
                    SimpleInfo.Create("uzivatel_id")
                    ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "UTVAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("uutvar_id", "uutvar_id"));
        }
    }
    class QueryKrokujPracovnikyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "KROKUJPRACOVNIKY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryKrokujPracovnikyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryKrokujPracovnikyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("PRAC", TablePracInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppracovnik_id", "pracovnik_id"),
                    SimpleInfo.Create("logicky_zrusen"),
                    SimpleInfo.Create("logicky_neuplny"),
                    SimpleInfo.Create("pocitane_obdobi"),
                    SimpleInfo.Create("osobni_cislo"),
                    SimpleInfo.Create("datum_narozeni"),
                    SimpleInfo.Create("rodne_cislo"),
                    SimpleInfo.Create("prijmeni"),
                    SimpleInfo.Create("jmeno"),
                    SimpleInfo.Create("titul_pred"),
                    SimpleInfo.Create("titul_za")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UTVAR", TableUtvarInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uutvar_id"),
                    SimpleInfo.Create("utvnazev"),
                    SimpleInfo.Create("vyuctgr"),
                    SimpleInfo.Create("zeme_cislo"),
                    SimpleInfo.Create("uzivatel_id")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("DAN", TableDanInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("mesic"),
                    AliasInfo.Create("informace", "vyuct_cast")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("PRAC", "DAN").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppracovnik_id", "pracovnik_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("DAN", "UTVAR").
                AddColumn("firma_id", "firma_id").
                AddColumn("sazba", "uutvar_id").
                AddLeftColumn("odkud", "=", "0").
                AddLeftColumn("kod", "=", "6100").
                AddLeftColumn("cislo", "=", "1"));
        }
    }
}
