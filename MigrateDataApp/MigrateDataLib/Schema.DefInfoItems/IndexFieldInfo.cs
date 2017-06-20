using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class IndexFieldInfo : ICloneable
    {
        public string ColumnName { get; set; }
        public bool Descending { get; set; }

        public IndexFieldInfo(string lpszName, bool descending = false)
        {
            ColumnName = lpszName;
            Descending = descending;
        }

        public string FieldInfo(bool primary)
        {
            string strFieldInfo = ColumnName;
            if (!primary)
            {
                if (Descending)
                {
                    strFieldInfo += (" DESC");
                }
                else
                {
                    strFieldInfo += (" ASC");
                }
            }
            return strFieldInfo;
        }
        public object Clone()
        {
            IndexFieldInfo other = (IndexFieldInfo)this.MemberwiseClone();
            other.ColumnName = this.ColumnName;
            other.Descending = this.Descending;

            return other;
        }
    }
}
