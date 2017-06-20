using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;

namespace MigrateDataLib.Source.Builder
{
    public interface ISrcCodebaseBuilder
    {
        void CreateTableClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateTableHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateQueryClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateQueryHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateTableConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateQueryConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateBeginConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateCloseConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        void CreateDbSetContxFile(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        string Extension();
        string Prefix();
    }
}
