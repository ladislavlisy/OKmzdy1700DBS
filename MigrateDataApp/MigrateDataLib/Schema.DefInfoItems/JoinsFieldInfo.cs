using MigrateDataLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public class JoinsFieldInfo : ICloneable
    {
        public JoinsFieldInfo(string lhrColumnName)
        {
            LhrColumnOpValue = false;
            RhrColumnOpValue = false;
            TableLhrColumn = lhrColumnName;
            TableRhrColumn = lhrColumnName;
        }

        public JoinsFieldInfo(string lhrColumnName, string rhrColumnName)
        {
            LhrColumnOpValue = false;
            TableLhrColumn = lhrColumnName;
            TableRhrColumn = rhrColumnName;
        }

        public JoinsFieldInfo(bool bLeftColumn, string columnName, string columnOper, string columnValue)
        {
            LhrColumnOpValue = false;
            RhrColumnOpValue = false;
            if (bLeftColumn)
            {
                LhrColumnOpValue = true;
                TableLhrColumn = columnName;
                TableRhrColumn = columnOper + columnValue;
            }
            else
            {
                RhrColumnOpValue = true;
                TableRhrColumn = columnName;
                TableLhrColumn = columnOper + columnValue;
            }
        }

        public bool LhrColumnOpValue;
        public string TableLhrColumn;
        public bool RhrColumnOpValue;
        public string TableRhrColumn;

        public string LeftColumnName()
        {
            return TableLhrColumn;
        }

        public string RightColumnName()
        {
            return TableRhrColumn;
        }

        public string JoinCondition(string lhrSourceAlias, string rhrSourceAlias, bool bJoinConds, bool bFilterConds)
        {
            string strFieldNames = "";

            if (LhrColumnOpValue)
            {
                if (bFilterConds)
                {
                    strFieldNames += lhrSourceAlias;
                    strFieldNames += ".";
                    strFieldNames += TableLhrColumn;
                    strFieldNames += TableRhrColumn;
                }
            }
            else if (RhrColumnOpValue)
            {
                if (bFilterConds)
                {
                    strFieldNames += rhrSourceAlias;
                    strFieldNames += ".";
                    strFieldNames += TableRhrColumn;
                    strFieldNames += TableLhrColumn;
                }
            }
            else
            {
                if (bJoinConds)
                {
                    strFieldNames += lhrSourceAlias;
                    strFieldNames += ".";
                    strFieldNames += TableLhrColumn;
                    strFieldNames += " = ";
                    strFieldNames += rhrSourceAlias;
                    strFieldNames += ".";
                    strFieldNames += TableRhrColumn;
                }
            }

            return strFieldNames;
        }

        public void ReNameLeftColumnName(string oldColumnName, string newColumnName)
        {
            if (TableLhrColumn.CompareNoCase(oldColumnName))
            {
                TableLhrColumn = newColumnName;
            }
        }

        public void ReNameRightColumnName(string oldColumnName, string newColumnName)
        {
            if (TableRhrColumn.CompareNoCase(oldColumnName))
            {
                TableRhrColumn = newColumnName;
            }
        }

        public object Clone()
        {
            JoinsFieldInfo other = (JoinsFieldInfo)this.MemberwiseClone();
            other.LhrColumnOpValue = this.LhrColumnOpValue;
            other.TableLhrColumn = this.TableLhrColumn;
            other.RhrColumnOpValue = this.RhrColumnOpValue;
            other.TableRhrColumn = this.TableRhrColumn;

            return other;
        }

    }
}
