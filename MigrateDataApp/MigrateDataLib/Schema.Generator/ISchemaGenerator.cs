using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Schema.Generator
{
    public interface ISchemaGenerator
    {
        void CreateTableList(MigrateSubsets options);
        void CreateIndexList(MigrateSubsets options);
        void CreateRelatList(MigrateSubsets options);
        void CreateQueryList(MigrateSubsets options);
        void PrepareSchema(IGeneratorWriter writer, MigrateOptions buildOptions);
        IGeneratorWriter CreateWriter(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64);
        IGeneratorWriter CreateExecutor(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64);
        bool Generate(IGeneratorWriter writer, MigrateOptions buildOptions);
    }
}
