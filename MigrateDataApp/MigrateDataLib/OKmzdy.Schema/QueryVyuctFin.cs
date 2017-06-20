using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryHodnVyuctFinInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_HODNVYUCTFIN";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryHodnVyuctFinInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryHodnVyuctFinInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("VFIN", TableZsestPrehvyuctfinInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("skupina"),
                    SimpleInfo.Create("kod"),
                    AliasInfo.Create("hodnota_numb", "hodnota_numb", "SUM({0})")
                ));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("VFIN", TableZsestPrehvyuctfinInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddConstraints(
                    FiltrSpecsInfo.Create("mesic", "<>", "0"),
                    FiltrSpecsInfo.Create("poradi", "=", "0")
                ));

            AddClose(QueryCloseInfo.Create("GROUP BY firma_id, kod_data, uzivatel_id, skupina, kod"));
        }
    }
    class QueryCelkVyuctFinInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_CELKVYUCTFIN";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryCelkVyuctFinInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryCelkVyuctFinInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("VFIN", TableZsestPrehvyuctfinInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("kod"),
                    AliasInfo.Create("hodnota_numb", "hodnota_numb", "SUM({0})")
                ));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("VFIN", TableZsestPrehvyuctfinInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddConstraints(
                    FiltrSpecsInfo.Create("mesic", "<>", "0"),
                    FiltrSpecsInfo.Create("poradi", "=", "0")
                ));

            AddClose(QueryCloseInfo.Create("GROUP BY firma_id, kod_data, uzivatel_id, kod"));
        }
    }
}
