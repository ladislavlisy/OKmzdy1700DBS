using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.OKmzdy.Schema;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Schema.Builder;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Schema.Database;
using MigrateDataLib.Source.Builder;

namespace MigrateDataLib
{
    public class MigrateOptions
    {
        public bool CreateDataFiles = true; 
        public bool CreateTableInit = true; 
        public bool CreateTableStru = true; 
        public bool CreateIndexKeys = true; 
        public bool CreateIUTrigers = false;
        public bool InsertTableData = false;
        public bool InsertTableSest = false;
        public bool CreateQueryView = true; 
        public bool CreateRelations = true;     
        public bool CompareXDetails = false;     

    }
    public class MigrateSubsets
    {
        public IList<string> SubsetTableList {get; set;}
        public IList<string> SubsetIndexList {get; set;}
        public IList<string> SubsetRelatList {get; set;}
        public IList<string> SubsetQueryList {get; set;}

        public IList<string> FilterTableList {get; set;}
        public IList<string> FilterIndexList {get; set;}
        public IList<string> FilterRelatList {get; set;}
        public IList<string> FilterQueryList {get; set;}   
        public MigrateSubsets()
        {
            SubsetTableList = null;
            SubsetIndexList = null;
            SubsetRelatList = null;
            SubsetQueryList = null;
                              
            FilterTableList = null;
            FilterIndexList = null;
            FilterRelatList = null;
            FilterQueryList = null;
        }
    }
    public class MigrateDataTools
    {
        public static void GenerateTargetSqlScript(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {

            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISqlScriptBuilder schemaBuilder = SqlBuilderFactory.CreateSqlBuilder(appDataConfig);

            ISchemaGenerator generator = new SqlTargetGenerator(schemaInfo, appDataConfig, schemaBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }
        public static void GenerateSourceSqlScript(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {

            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISqlScriptBuilder schemaBuilder = SqlBuilderFactory.CreateSqlBuilder(appDataConfig);

            ISchemaGenerator generator = new SqlSourceGenerator(schemaInfo, appDataConfig, schemaBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }
        public static void GenerateTargetDatabase(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {
            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISqlScriptBuilder schemaBuilder = SqlBuilderFactory.CreateSqlBuilder(appDataConfig);

            ISchemaGenerator generator = new SqlTargetGenerator(schemaInfo, appDataConfig, schemaBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateExecutor(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }

        public static void GenerateSourceDatabase(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {
            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISqlScriptBuilder schemaBuilder = SqlBuilderFactory.CreateSqlBuilder(appDataConfig);

            ISchemaGenerator generator = new SqlSourceGenerator(schemaInfo, appDataConfig, schemaBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateExecutor(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }

        public static void CompareDatabaseSchema(string appRootFolder, DbsDataConfig srcDataConfig, DbsDataConfig trgDataConfig, MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {
            ISchemaGenerator generator = new DbSchemaDiffsGenerator(srcDataConfig, trgDataConfig, buildVersion);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, srcDataConfig, false))
            {
                generator.Generate(writer, buildOptions);
            }
        }

        public static void CompareDatabaseData(string appRootFolder, DbsDataConfig srcDataConfig, DbsDataConfig trgDataConfig, MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {
            IDatabaseSchema schemaInfo = new DatabaseSchema(srcDataConfig.UserName, srcDataConfig.OwnerName);

            ISchemaGenerator generator = new DbDataDiffsGenerator(schemaInfo, srcDataConfig, trgDataConfig, buildVersion);

            generator.CreateTableList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, srcDataConfig, false))
            {
                generator.Generate(writer, buildOptions);
            }
        }

        public static void DuplicateDatabase(string appRootFolder, DbsDataConfig srcDataConfig, DbsDataConfig trgDataConfig, MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {
            IDatabaseSchema schemaInfo = new DatabaseSchema(srcDataConfig.UserName, srcDataConfig.OwnerName);

            ISqlScriptBuilder sourceBuilder = SqlBuilderFactory.CreateSqlBuilder(srcDataConfig);

            ISqlScriptBuilder targetBuilder = SqlBuilderFactory.CreateSqlBuilder(trgDataConfig);

            ISchemaGenerator generator = new DbCopyDataGenerator(schemaInfo, srcDataConfig, sourceBuilder, targetBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateExecutor(appRootFolder, trgDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }

        public static void GenerateSourceCpp(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {

            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISrcCodebaseBuilder sourceBuilder = SrcBuilderFactory.CreateSrcBuilder(appDataConfig, "cpp");

            ISchemaGenerator generator = new SrcSourceGenerator(schemaInfo, appDataConfig, sourceBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }
        public static void GenerateSourceCs(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {

            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISrcCodebaseBuilder sourceBuilder = SrcBuilderFactory.CreateSrcBuilder(appDataConfig, "cs");

            ISchemaGenerator generator = new SrcTargetGenerator(schemaInfo, appDataConfig, sourceBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }
        public static void GenerateSourceRb(string appRootFolder, DbsDataConfig appDataConfig, 
            MigrateOptions buildOptions, MigrateSubsets buildSubsets, uint buildVersion)
        {

            IDatabaseSchema schemaInfo = new DatabaseSchema(appDataConfig.UserName, appDataConfig.OwnerName);

            ISrcCodebaseBuilder sourceBuilder = SrcBuilderFactory.CreateSrcBuilder(appDataConfig, "rb");

            ISchemaGenerator generator = new SrcTargetGenerator(schemaInfo, appDataConfig, sourceBuilder, buildVersion);

            generator.CreateTableList(buildSubsets);

            generator.CreateIndexList(buildSubsets);

            generator.CreateRelatList(buildSubsets);

            generator.CreateQueryList(buildSubsets);

            using (IGeneratorWriter writer = generator.CreateWriter(appRootFolder, appDataConfig, false))
            {
                generator.PrepareSchema(writer, buildOptions);

                generator.Generate(writer, buildOptions);
            }
        }
        public static void SetUpSchemaOptions(MigrateSubsets buildOptions)
        {
#if __FILTER_TEST_ITEMS__
            buildOptions.SubsetTableList = new string[] {
                "FIRMA",
                "DIVIZE_STRED",
                "STRED_CINZAK",
                "UZIV_CISELNIK",
                "STAV_DATABAZE"
            };

            buildOptions.SubsetIndexList = new string[] {
                "FIRMA",
                "DIVIZE_STRED",
                "STRED_CINZAK",
                "UZIV_CISELNIK",
                "STAV_DATABAZE"
            };


            buildOptions.SubsetRelatList = new string[] {
                "FIRMA",
                "DIVIZE_STRED",
                "STRED_CINZAK",
                "UZIV_CISELNIK",
                "STAV_DATABAZE"
            };

            buildOptions.SubsetQueryList = new string[] {
            };
#else
            buildOptions.FilterTableList = new string[] { };
            buildOptions.FilterIndexList = new string[] { };
            buildOptions.FilterRelatList = new string[] { };
            buildOptions.FilterQueryList = new string[] { };
#endif
        }
    }
}
