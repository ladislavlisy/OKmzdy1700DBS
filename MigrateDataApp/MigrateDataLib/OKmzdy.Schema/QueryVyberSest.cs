using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryVyberSestavyInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VYBERSESTAVY";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryVyberSestavyInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryVyberSestavyInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("SLST", TableSestavyLstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("kod_lst"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("druh"),
                    SimpleInfo.Create("typ_lst"),
                    SimpleInfo.Create("trideni"),
                    SimpleInfo.Create("nazev"),
                    SimpleInfo.Create("soubor"),
                    SimpleInfo.Create("skupina"),
                    SimpleInfo.Create("subjekt_id"),
                    SimpleInfo.Create("informace")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UDATA", TableSestavyUdataInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("vytvor_vyuc"),
                    SimpleInfo.Create("vyuct_dat"),
                    SimpleInfo.Create("mesic_od"),
                    SimpleInfo.Create("mesic_do"),
                    SimpleInfo.Create("rok"),
                    SimpleInfo.Create("vytvor_txt"),
                    SimpleInfo.Create("vytvor_dat")
                 ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("ULST", TableSestavyUlstInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                   SimpleInfo.Create("tisknout"),
                   SimpleInfo.Create("sestavy_id"),
                   SimpleInfo.Create("pg_margins"),
                   SimpleInfo.Create("papir"),
                   SimpleInfo.Create("tridit"),
                   SimpleInfo.Create("exp_cesta"),
                   SimpleInfo.Create("lst_param"),
                   SimpleInfo.Create("txt_param"),
                   SimpleInfo.Create("msg_param"),
                   SimpleInfo.Create("filtr_zobr")
                ));

            AddTable(QueryTableInfo.GetQueryAliasDefInfo("UZ", TableUzivatelInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("uuzivatel_id"),
                    AliasInfo.Create("sestavy_id", "usestavy_id")
                ));

            AddTableJoin(QueryJoinsInfo.GetQueryFirstJoinDefInfo("SLST", "UDATA").
                AddColumn("firma_id", "firma_id").
                AddColumn("kod_data", "kod_data"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("SLST", "ULST").
                AddColumn("firma_id", "firma_id").
                AddColumn("kod_lst", "kod_lst").
                AddRightColumn("uzivatel_id", "=", "UDATA.uzivatel_id"));

            AddTableJoin(QueryJoinsInfo.GetQueryJoinsInfo("ULST", "UZ").
                AddColumn("firma_id", "firma_id").
                AddColumn("uzivatel_id", "uuzivatel_id"));

        }
    }
}
