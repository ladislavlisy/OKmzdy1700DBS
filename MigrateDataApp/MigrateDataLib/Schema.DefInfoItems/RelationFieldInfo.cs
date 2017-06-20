using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class RelationFieldInfo : ICloneable
    {
        public string ForeignName { get; set; }
        public string SourceName { get; set; }

        public RelationFieldInfo(string foreignName, string sourceName)
        {
            ForeignName = foreignName;
            SourceName = sourceName;
        }

        public object Clone()
        {
            RelationFieldInfo other = (RelationFieldInfo)this.MemberwiseClone();
            other.ForeignName = this.ForeignName;
            other.SourceName = this.SourceName;
            return other;
        }
    }
}
