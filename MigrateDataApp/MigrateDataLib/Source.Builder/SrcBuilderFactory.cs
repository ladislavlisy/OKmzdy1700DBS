using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Source.Builder
{
    public class SrcBuilderFactory
    {
        public static ISrcCodebaseBuilder CreateSrcBuilder(DbsDataConfig config, string ext)
        {
            if (ext == "cpp")
            {
                return new SourceBuilderAdo(config) as ISrcCodebaseBuilder;
            }
            else if (ext == "rb")
            {
                return new SourceBuilderRuby(config) as ISrcCodebaseBuilder;
            }
            else if (ext == "cs")
            {
                return new SourceBuilderCsharp(config) as ISrcCodebaseBuilder;
            }
            else
            {
                return new SourceBuilderCsharp(config) as ISrcCodebaseBuilder;
            }
        }
    }
}

