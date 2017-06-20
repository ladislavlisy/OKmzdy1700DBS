//#define __CREATE_NEXT_DATABASE_SCHEMA__
//#define __CREATE_DATABASE_SCHEMA__
//#define __COMPARE_DATABASE_SCHEMA__
//#define __COMPARE_DATABASE_DATA__
//#define __CREATE_DATABASE_DATA_CLONE__
#define __CREATE_SOURCE_FOR_ENTITY_CPP__

using MigrateDataLib;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace MigrateDataApp
{
    class Program
    {
        public const string EMPTY_STRING = "";
        static void Main(string[] args)
        {
            string appRootFolder = ExecutableAppFolder();

            string dataFile = "OKMZDY_PARAMS.XML";
            string dataNode = "DATA";
            string itemNode = "OKMZDY_MSSQL";
            string fromNode = "OKMZDY_MDB";
            string targNode = "OKMZDY_MSSQL";

            string paramXmlFile = Path.Combine(appRootFolder, dataFile);

            DbsDataConfig srcDataConfig = ConfigXmlFileUtil.LoadOKmzdyDataRegistry(paramXmlFile, dataNode, fromNode);
            DbsDataConfig trgDataConfig = ConfigXmlFileUtil.LoadOKmzdyDataRegistry(paramXmlFile, dataNode, targNode);

            DbsDataConfig appDataConfig = ConfigXmlFileUtil.LoadOKmzdyDataRegistry(paramXmlFile, dataNode, itemNode);

            MigrateOptions buildOptions = new MigrateOptions();
            if (appDataConfig.ConfigName =="OKMZDY_MDB")
            {
                buildOptions.CreateDataFiles = false;
                buildOptions.CreateTableInit = true;
                buildOptions.CreateTableStru = true;
                buildOptions.CreateIndexKeys = true;
                buildOptions.CreateIUTrigers = false;
                buildOptions.InsertTableData = false;
                buildOptions.InsertTableSest = false;
                buildOptions.CreateQueryView = true;
                buildOptions.CreateRelations = true;
                buildOptions.CompareXDetails = false;
            }
            else if (appDataConfig.ConfigName =="OKMZDY_MSSQL")
            {
                buildOptions.CreateDataFiles = false;
                buildOptions.CreateTableInit = true;
                buildOptions.CreateTableStru = true;
                buildOptions.CreateIndexKeys = true;
                buildOptions.CreateIUTrigers = false;
                buildOptions.InsertTableData = false;
                buildOptions.InsertTableSest = false;
                buildOptions.CreateQueryView = true;
                buildOptions.CreateRelations = true;
                buildOptions.CompareXDetails = false;
            }
            else if (appDataConfig.ConfigName =="OKMZDY_SQLITE")
            {
                buildOptions.CreateDataFiles = true;
                buildOptions.CreateTableInit = true;
                buildOptions.CreateTableStru = true;
                buildOptions.CreateIndexKeys = true;
                buildOptions.CreateIUTrigers = false;
                buildOptions.InsertTableData = false;
                buildOptions.InsertTableSest = false;
                buildOptions.CreateQueryView = true;
                buildOptions.CreateRelations = true;
                buildOptions.CompareXDetails = false;
            }

            MigrateSubsets buildSubsets = new MigrateSubsets();
            MigrateDataTools.SetUpSchemaOptions(buildSubsets);

            uint buildVersion = 1700;

#if __CREATE_SCHEMA_SCRIPT__ 
            MigrateDataTools.GenerateSqlScript(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif

#if __CREATE_NEXT_DATABASE_SCHEMA__ 
            appDataConfig.ConfigName = "NEXT_EMPTY";
            appDataConfig.PlatformType = 18;
            appDataConfig.DatabaseName = "NEXT_EMPTY";
            appDataConfig.DataFileName = "NEXT_EMPTY";
            appDataConfig.SystFileName = EMPTY_STRING;
            appDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            appDataConfig.OwnerName = "oksystem";
            appDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            appDataConfig.UserName = "okmzdy";
            appDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            //appDataConfig.ConfigName = "SQLITE_EMPTY";
            //appDataConfig.PlatformType = 2;
            //appDataConfig.DatabaseName = "..\\..\\okmzdysq.sqlite";
            //appDataConfig.DataFileName = "..\\..\\okmzdysq.sqlite";
            //appDataConfig.SystFileName = EMPTY_STRING;
            //appDataConfig.DbServerName = EMPTY_STRING;
            ////appDataConfig.OwnerName = "admin";
            ////appDataConfig.OwnerPssw = "2C1A313A0F528234";
            //appDataConfig.OwnerName = "oksystem";
            //appDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            //appDataConfig.UserName = "okmzdy";
            //appDataConfig.UserPssw = "7355F5E53BC55173";

            buildOptions.CreateDataFiles = true;
            buildOptions.InsertTableData = false;
            MigrateDataTools.GenerateTargetDatabase(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __CREATE_DATABASE_SCHEMA__ 
            appDataConfig.ConfigName = "SQLITE_EMPTY";
            appDataConfig.PlatformType = 2;
            appDataConfig.DatabaseName = "okmzdysq";
            appDataConfig.DataFileName = "..\\..\\okmzdysq.sqlite";
            appDataConfig.SystFileName = EMPTY_STRING;
            appDataConfig.DbServerName = EMPTY_STRING;
            //appDataConfig.OwnerName = "admin";
            //appDataConfig.OwnerPssw = "2C1A313A0F528234";
            appDataConfig.OwnerName = "oksystem";
            appDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            appDataConfig.UserName = "okmzdy";
            appDataConfig.UserPssw = "7355F5E53BC55173";

            buildOptions.InsertTableData = false;
            MigrateDataTools.GenerateSourceDatabase(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __CREATE_DATABASE_DATA_CLONE__ 
            //srcDataConfig.ConfigName = "OK_EMPTY";
            //srcDataConfig.PlatformType = 18;
            //srcDataConfig.DatabaseName = "OK_EMPTY";
            //srcDataConfig.DataFileName = "OK_EMPTY";
            //srcDataConfig.SystFileName = EMPTY_STRING;
            //srcDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            //srcDataConfig.OwnerName = "oksystem";
            //srcDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            //srcDataConfig.UserName = "okmzdy";
            //srcDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            //srcDataConfig.ConfigName = "OK_MDB_EMPTY";
            //srcDataConfig.PlatformType = 1;
            //srcDataConfig.DatabaseName = "okmzdyw";
            //srcDataConfig.DataFileName = "..\\..\\okmzdye.mdb";
            //srcDataConfig.SystFileName = "..\\..\\okmzdye.mdw";
            //srcDataConfig.DbServerName = EMPTY_STRING;
            //srcDataConfig.OwnerName = "admin";
            //srcDataConfig.OwnerPssw = "2C1A313A0F528234";
            //srcDataConfig.OwnerName = "oksystem";
            //srcDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            //srcDataConfig.UserName = "okmzdy";
            //srcDataConfig.UserPssw = "7355F5E53BC55173";

            //trgDataConfig.ConfigName = "NEXT_EMPTY";
            //trgDataConfig.PlatformType = 18;
            //trgDataConfig.DatabaseName = "NEXT_EMPTY";
            //trgDataConfig.DataFileName = "NEXT_EMPTY";
            //trgDataConfig.SystFileName = EMPTY_STRING;
            //trgDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            //trgDataConfig.OwnerName = "oksystem";
            //trgDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            //trgDataConfig.UserName = "okmzdy";
            //trgDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            //trgDataConfig.ConfigName = "NEW_EMPTY";
            //trgDataConfig.PlatformType = 18;
            //trgDataConfig.DatabaseName = "NEW_EMPTY";
            //trgDataConfig.DataFileName = "NEW_EMPTY";
            //trgDataConfig.SystFileName = EMPTY_STRING;
            //trgDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            //trgDataConfig.OwnerName = "oksystem";
            //trgDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            //trgDataConfig.UserName = "okmzdy";
            //trgDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            srcDataConfig.ConfigName = "X17ROK_2017";
            srcDataConfig.PlatformType = 18;
            srcDataConfig.DatabaseName = "X17ROK_2017";
            srcDataConfig.DataFileName = "X17ROK_2017";
            srcDataConfig.SystFileName = EMPTY_STRING;
            srcDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            srcDataConfig.OwnerName = "oksystem";
            srcDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            srcDataConfig.UserName = "okmzdy";
            srcDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            trgDataConfig.ConfigName = "OK_MDB_EMPTY";
            trgDataConfig.PlatformType = 1;
            trgDataConfig.DatabaseName = "okmzdyw";
            trgDataConfig.DataFileName = "..\\..\\X17ROK_2017\\okmzdyw.mdb";
            trgDataConfig.SystFileName = "..\\..\\X17ROK_2017\\okmzdyw.mdw";
            trgDataConfig.DbServerName = EMPTY_STRING;
            trgDataConfig.OwnerName = "admin";
            trgDataConfig.OwnerPssw = "2C1A313A0F528234";
            trgDataConfig.OwnerName = "oksystem";
            trgDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            trgDataConfig.UserName = "okmzdy";
            trgDataConfig.UserPssw = "7355F5E53BC55173";

            buildOptions.InsertTableData = true;
            MigrateDataTools.DuplicateDatabase(appRootFolder, srcDataConfig, trgDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __COMPARE_DATABASE_SCHEMA__ 
            //srcDataConfig.ConfigName = "OK_EMPTY";
            //srcDataConfig.PlatformType = 18;
            //srcDataConfig.DatabaseName = "OK_EMPTY";
            //srcDataConfig.DataFileName = "OK_EMPTY";
            //srcDataConfig.SystFileName = EMPTY_STRING;
            //srcDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            //srcDataConfig.OwnerName = "oksystem";
            //srcDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            //srcDataConfig.UserName = "okmzdy";
            //srcDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            srcDataConfig.ConfigName = "OK_MDB_EMPTY";
            srcDataConfig.PlatformType = 1;
            srcDataConfig.DatabaseName = "okmzdyw";
            srcDataConfig.DataFileName = "..\\..\\okmzdye.mdb";
            srcDataConfig.SystFileName = "..\\..\\okmzdye.mdw";
            srcDataConfig.DbServerName = EMPTY_STRING;
            srcDataConfig.OwnerName = "admin";
            srcDataConfig.OwnerPssw = "2C1A313A0F528234";
            srcDataConfig.OwnerName = "oksystem";
            srcDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            srcDataConfig.UserName = "okmzdy";
            srcDataConfig.UserPssw = "7355F5E53BC55173";

            trgDataConfig.ConfigName = "NEW_EMPTY";
            trgDataConfig.PlatformType = 18;
            trgDataConfig.DatabaseName = "NEW_EMPTY";
            trgDataConfig.DataFileName = "NEW_EMPTY";
            trgDataConfig.SystFileName = EMPTY_STRING;
            trgDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            trgDataConfig.OwnerName = "oksystem";
            trgDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            trgDataConfig.UserName = "okmzdy";
            trgDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            buildOptions.CompareXDetails = false;
            MigrateDataTools.CompareDatabaseSchema(appRootFolder, srcDataConfig, trgDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __COMPARE_DATABASE_DATA__ 
            //srcDataConfig.ConfigName = "NEW_EMPTY";
            //srcDataConfig.PlatformType = 18;
            //srcDataConfig.DatabaseName = "NEW_EMPTY";
            //srcDataConfig.DataFileName = "NEW_EMPTY";
            //srcDataConfig.SystFileName = EMPTY_STRING;
            //srcDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            //srcDataConfig.OwnerName = "oksystem";
            //srcDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            //srcDataConfig.UserName = "okmzdy";
            //srcDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            srcDataConfig.ConfigName = "OK_MDB_EMPTY";
            srcDataConfig.PlatformType = 1;
            srcDataConfig.DatabaseName = "okmzdyw";
            srcDataConfig.DataFileName = "..\\..\\okmzdye.mdb";
            srcDataConfig.SystFileName = "..\\..\\okmzdye.mdw";
            srcDataConfig.DbServerName = EMPTY_STRING;
            srcDataConfig.OwnerName = "admin";
            srcDataConfig.OwnerPssw = "2C1A313A0F528234";
            srcDataConfig.OwnerName = "oksystem";
            srcDataConfig.OwnerPssw = "D84A56202ED0ED46440DDDCD5B5C701C";
            srcDataConfig.UserName = "okmzdy";
            srcDataConfig.UserPssw = "7355F5E53BC55173";

            trgDataConfig.ConfigName = "OK_EMPTY";
            trgDataConfig.PlatformType = 18;
            trgDataConfig.DatabaseName = "OK_EMPTY";
            trgDataConfig.DataFileName = "OK_EMPTY";
            trgDataConfig.SystFileName = EMPTY_STRING;
            trgDataConfig.DbServerName = ".\\SQLEXPRESS2014";
            trgDataConfig.OwnerName = "oksystem";
            trgDataConfig.OwnerPssw = "DA0E8433E89531A05C0F38DFCAF1ACCB";
            trgDataConfig.UserName = "okmzdy";
            trgDataConfig.UserPssw = "55FCC22DE2E2049B00367EAA821B6B73";

            buildOptions.CompareXDetails = false;
            MigrateDataTools.CompareDatabaseData(appRootFolder, srcDataConfig, trgDataConfig, buildOptions, buildSubsets, buildVersion);
#endif

#if __CREATE_SOURCE_FOR_ENTITY_CPP__ 
            MigrateDataTools.GenerateSourceCpp(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __CREATE_SOURCE_FOR_ENTITY_CS__ 
            MigrateDataTools.GenerateSourceCs(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
#if __CREATE_SOURCE_FOR_ENTITY_RB__ 
            MigrateDataTools.GenerateSourceRb(appRootFolder, appDataConfig, buildOptions, buildSubsets, buildVersion);
#endif
        }
        private static string ExecutableAppFolder()
        {
            string[] args = Environment.GetCommandLineArgs();

            string appExecutableFileNm = args[0];

            return System.IO.Path.GetDirectoryName(appExecutableFileNm);
        }

        private static void ValidateXML()
        {
            string fileXML = "..\\..\\SEPA043x022017.XML";
            string fileXSD = "..\\..\\pain.001.001.03.xsd";

            XmlDocument asset = new XmlDocument();

            XmlTextReader schemaReader = new XmlTextReader(fileXSD);
            XmlSchema schema = XmlSchema.Read(schemaReader, SchemaValidationHandler);

            asset.Schemas.Add(schema);

            asset.Load(fileXML);
            asset.Validate(DocumentValidationHandler);
        }

        private static void SchemaValidationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);
        }
        private static void DocumentValidationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
            {
                Console.WriteLine("\tValidation error: " + args.Message);

                XmlSchemaValidationException exceptionSource = args.Exception as XmlSchemaValidationException;
                if (exceptionSource != null)
                {
                    XmlElement argsElement = exceptionSource.SourceObject as XmlElement;
                    if (argsElement != null)
                    {
                        XmlNode parentElement = argsElement.ParentNode;
                        while (parentElement != null)
                        {
                            XmlNode compareNode = parentElement;

                            if (compareNode.Name.CompareNoCase("CdtTrfTxInf"))
                            {
                                break;
                            }
                            if (compareNode.Name.CompareNoCase("PmtInf"))
                            {
                                break;
                            }
                            if (compareNode.Name.CompareNoCase("GrpHdr"))
                            {
                                break;
                            }
                            if (compareNode.Name.CompareNoCase("CstmrCdtTrfInitn"))
                            {
                                break;
                            }
                            parentElement = compareNode.ParentNode;
                        }



                        if (parentElement != null)
                        {
                            StringBuilder bldReferXPath = new StringBuilder("./*[local-name()='").Append("PmtId")
                                .Append("']/*[local-name()='").Append("InstrId").Append("']");
                            StringBuilder bldCNameXPath = new StringBuilder("./*[local-name()='").Append("Cdtr")
                                .Append("']/*[local-name()='").Append("Nm").Append("']");
                            StringBuilder bldCUstrXPath = new StringBuilder("./*[local-name()='").Append("RmtInf")
                                .Append("']/*[local-name()='").Append("Ustrd").Append("']");
                            string ReferXPath = bldReferXPath.ToString();
                            string CNameXPath = bldCNameXPath.ToString();
                            string CUstrXPath = bldCUstrXPath.ToString();

                            string ElementErrs = argsElement.Name;
                            string ReferParent = "";
                            string CNameParent = "";
                            string CUstrParent = "";
                            switch (parentElement.Name)
                            {
                                case "CdtTrfTxInf":
                                    ReferParent = GetXPathString(parentElement, parentElement.Name, ReferXPath, EMPTY_STRING);
                                    CNameParent = GetXPathString(parentElement, parentElement.Name, CNameXPath, EMPTY_STRING);
                                    CUstrParent = GetXPathString(parentElement, parentElement.Name, CUstrXPath, EMPTY_STRING);
                                    break;
                                case "PmtInf":
                                    break;
                                case "GrpHdr":
                                    break;
                                case "CstmrCdtTrfInitn":
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static string GetXPathString(XmlNode xmlConfig, string nodeExpeption, string nodeXPath, string defaultValue)
        {
            try
            {
                XmlNode xmlNodeValue = xmlConfig.SelectSingleNode(nodeXPath);

                if (xmlNodeValue == null)
                {
                    return defaultValue;
                }

                string strNodeValue = xmlNodeValue.InnerText;

                if (strNodeValue == DbsDataConfigKeys.EMPTY_STRING)
                {
                    return defaultValue;
                }
                return xmlNodeValue.InnerText;
            }
            catch (Exception ex)
            {
                string errorDiagnostics = string.Format("Exception in NODE {0}:\n{1}", nodeExpeption, ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
            }
            return defaultValue;
        }

    }
}
