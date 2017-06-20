using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryVyberUcetPrPolozkyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERUCET_PRPOLOZKY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberUcetPrPolozkyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberUcetPrPolozkyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UCPREDP", TableUcetniPredpisyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    AliasInfo.Create("ppredpis_id", "predpis_id"),
                    SimpleInfo.Create("predpis_uckod"),
                    SimpleInfo.Create("predpis_nazev"),
                    SimpleInfo.Create("predpis_druh")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UCPOLOZ", TableUcetniPolozkyInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("cislo"),
                    SimpleInfo.Create("poloz_nazev"),
                    SimpleInfo.Create("poloz_druh"),
                    SimpleInfo.Create("poloz_skup"),
                    SimpleInfo.Create("poloz_synt"),
                    SimpleInfo.Create("poloz_anal"),
                    SimpleInfo.Create("poloz_text1"),
                    SimpleInfo.Create("poloz_inf1"),
                    SimpleInfo.Create("poloz_delka1"),
                    SimpleInfo.Create("poloz_fmt1"),
                    SimpleInfo.Create("poloz_text2"),
                    SimpleInfo.Create("poloz_inf2"),
                    SimpleInfo.Create("poloz_delka2"),
                    SimpleInfo.Create("poloz_fmt2"),
                    SimpleInfo.Create("poloz_text3"),
                    SimpleInfo.Create("poloz_inf3"),
                    SimpleInfo.Create("poloz_delka3"),
                    SimpleInfo.Create("poloz_fmt3"),
                    SimpleInfo.Create("poloz_text4"),
                    SimpleInfo.Create("poloz_inf4"),
                    SimpleInfo.Create("poloz_delka4"),
                    SimpleInfo.Create("poloz_fmt4"),
                    SimpleInfo.Create("poloz_text5"),
                    SimpleInfo.Create("poloz_inf5"),
                    SimpleInfo.Create("poloz_delka5"),
                    SimpleInfo.Create("poloz_fmt5")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("UCPREDP", "UCPOLOZ").
                AddColumn("firma_id", "firma_id").
                AddColumn("ppredpis_id", "predpis_id"));

        }
    }
}
