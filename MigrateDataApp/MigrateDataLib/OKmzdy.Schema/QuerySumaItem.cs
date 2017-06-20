using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QuerySumaDanInfo : QueryDefInfo
    {
        const string TABLE_NAME = "SUMA_DAN";

        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QuerySumaDanInfo(lpszOwnerName, lpszUsersName);
        }
        public QuerySumaDanInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("DAN", TableDanInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("mesic"),
                    SimpleInfo.Create("odkud"),
                    SimpleInfo.Create("kod"),
                    AliasInfo.Create("hodnota", "hodnota", "SUM({0})"),
                    AliasInfo.Create("pocjed", "pocjed", "SUM({0})"),
                    AliasInfo.Create("pocdal", "pocdal", "SUM({0})"),
                    AliasInfo.Create("sazba", "sazba", "SUM({0})")
               ));

            AddClose(QueryCloseInfo.Create("GROUP BY firma_id, mesic, odkud, kod"));
        }
    }
}
