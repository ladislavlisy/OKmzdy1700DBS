using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.OKmzdy.Schema
{
    class QueryHodnExtMListInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_HODNEXTMLIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryHodnExtMListInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryHodnExtMListInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("EXTML", TableZsestExtMzdlistInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("pomer_id"),
                    SimpleInfo.Create("mesic_opr"),
                    SimpleInfo.Create("skupina"),
                    SimpleInfo.Create("kod"),
                    SimpleInfo.Create("hodnota_numb"),
                    SimpleInfo.Create("hodnota_text")));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("EXTML", TableZsestExtMzdlistInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddConstraints(
                    FiltrSpecsInfo.Create("mesic", "<>", "0"),
                    FiltrSpecsInfo.Create("poradi", "=", "0")));
        }
    }
    class QueryCelkExtMListInfo : QueryDefInfo
    {
        const string TABLE_NAME = "VSEST_CELKEXTMLIST";
        public static string GetNameKey()
        {
            return TABLE_NAME;
        }
        public static QueryDefInfo GetDictValue(string lpszOwnerName, string lpszUsersName)
        {
            return new QueryCelkExtMListInfo(lpszOwnerName, lpszUsersName);
        }
        public QueryCelkExtMListInfo(string lpszOwnerName, string lpszUsersName) :
            base(lpszOwnerName, lpszUsersName, TABLE_NAME, 1600)
        {
            AddTable(QueryTableInfo.GetQueryAliasDefInfo("EXTML", TableZsestExtMzdlistInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddColumns(
                    SimpleInfo.Create("firma_id"),
                    SimpleInfo.Create("kod_data"),
                    SimpleInfo.Create("uzivatel_id"),
                    SimpleInfo.Create("pracovnik_id"),
                    SimpleInfo.Create("pomer_id"),
                    SimpleInfo.Create("mesic_opr"),
                    SimpleInfo.Create("kod"),
                    SimpleInfo.Create("hodnota_numb", "SUM({0})")));

            AddFiltr(QueryFiltrInfo.GetQueryFiltrInfo("EXTML", TableZsestExtMzdlistInfo.GetDictValue(lpszOwnerName, lpszUsersName)).
                AddConstraints(
                    FiltrSpecsInfo.Create("mesic", "<>", "0"),
                    FiltrSpecsInfo.Create("poradi", "=", "0")));

            AddClose(QueryCloseInfo.Create("GROUP BY firma_id, kod_data, uzivatel_id, pracovnik_id, pomer_id, mesic_opr, kod"));
        }
    }
}
