using MigrateDataLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class QueryCloseInfo : ICloneable
    {
        public static QueryCloseInfo Create(string closeInfo)
        {
            return new QueryCloseInfo(closeInfo);
        }
        public string CloseInfo { get; set; }

        public QueryCloseInfo(string closeInfo)
        {
            CloseInfo = closeInfo;
        }
        public void ReNameColumn(string aliasName, string oldColumnName, string newColumnName)
        {
            string oldReplaceName = aliasName + "." + oldColumnName;
            string newReplaceName = aliasName + "." + newColumnName;

            CloseInfo = CloseInfo.Replace(oldReplaceName, newReplaceName);
        }
        public bool NonEmpty()
        {
            return (CloseInfo.CompareNoCase("") == false);
        }
        public object Clone()
        {
            QueryCloseInfo other = (QueryCloseInfo)this.MemberwiseClone();
            other.CloseInfo = this.CloseInfo;

            return other;
        }
    }
}
