using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Utils
{
    public class ConfigXmlFileUtil
    {
        static readonly string REG_ROOT_OKM = "OKmzdy_pro_Windows";
        static readonly string REG_DATA_KON = "konfigurace";
        static readonly string REG_DATA_POL = "polozka";
        public static DbsDataConfig LoadOKmzdyMainRegistry(string paramXmlFile)
        {
            #region LOAD_XML_KONFIG
            XmlDocument paramXmlDoc = new XmlDocument();

            try
            {
                paramXmlDoc.Load(paramXmlFile);
            }
            catch (Exception ex)
            {
                string errorDiagnostics = string.Format("Exception loading file: {0}", ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
                return new DbsDataConfig();
            }

            #endregion

            StringBuilder bldNodeXPath = new StringBuilder("/*[local-name()='").Append(REG_ROOT_OKM).Append("']");

            string strNodeXPath = bldNodeXPath.ToString();

            return LoadOKmzdyMainRegistryItem(paramXmlDoc, strNodeXPath, REG_ROOT_OKM);
        }
        public static DbsDataConfig LoadOKmzdyDataRegistry(string paramXmlFile, string dataNode, string itemNode)
        {
            #region LOAD_XML_KONFIG
            XmlDocument paramXmlDoc = new XmlDocument();

            try
            {
                paramXmlDoc.Load(paramXmlFile);
            }
            catch (Exception ex)
            {
                string errorDiagnostics = string.Format("Exception loading file: {0}", ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
                return new DbsDataConfig();
            }

            #endregion

            StringBuilder bldNodeXPath = new StringBuilder("/*[local-name()='")
                .Append(REG_ROOT_OKM).Append("']/*[local-name()='").Append(dataNode)
                .Append("']/*[local-name()='").Append(REG_DATA_KON)
                .Append("' and @name='").Append(itemNode).Append("']");

            string strNodeXPath = bldNodeXPath.ToString();

            return LoadOKmzdyDataRegistryItem(paramXmlDoc, strNodeXPath, itemNode);
        }

        private static DbsDataConfig LoadOKmzdyMainRegistryItem(XmlDocument paramXmlDoc, string strNodeXPath, string dataMainNode)
        {
            DbsDataConfig appMainItem = new DbsDataConfig();

            if (paramXmlDoc == null)
            {
                appMainItem.Init();

                return appMainItem;
            }

            Int32 defaultZero = 0;

            Int32 ShortcutDesktop = GetValueInt32(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_LNKDESK, defaultZero);
            Int32 AccessAceVers = GetValueInt32(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_ACEVERS, defaultZero);

            string ArchivFolder = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_ARCFLDR, DbsDataConfigKeys.EMPTY_STRING);
            string DatabaseFolder = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.DataFileName = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);
            string ParentFolder = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_PARFLDR, DbsDataConfigKeys.EMPTY_STRING);
            string ShortcutFolder = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_LNKFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.SystFileName = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSSYST, DbsDataConfigKeys.EMPTY_STRING);
            string TiskyFolder = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_TISFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.DbServerName = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_ODBCSRV, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.UserName = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSUSER, DbsDataConfigKeys.USER_NAME);
            appMainItem.UserPssw = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSUPSW, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.OwnerName = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSOWNR, DbsDataConfigKeys.OWNER_NAME);
            appMainItem.OwnerPssw = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSOPSW, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSdba = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_SDBUSER, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSdbaPsw = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_SDBPSSW, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlWksArch = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_WKSARCH, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSrvArch = GetValueString(paramXmlDoc, strNodeXPath, dataMainNode, DbsDataConfigKeys.REG_ITEM_SRVARCH, DbsDataConfigKeys.EMPTY_STRING);

            return appMainItem;
        }
        private static DbsDataConfig LoadOKmzdyDataRegistryItem(XmlDocument paramXmlDoc, string strNodeXPath, string dataItemNode)
        {
            DbsDataConfig appDataItem = new DbsDataConfig();

            if (paramXmlDoc == null)
            {
                appDataItem.Init();

                return appDataItem;
            }

            Int32 defaultZero = 0;
            UInt32 defaultUZero = 0u;
            Int32 defaultOnes = 1;
            Int32 defaultNegs = -1;

            appDataItem.ConfigName = dataItemNode;

            Int32 AppUIcolors = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_UICOLOR, defaultZero);
            appDataItem.PlatformType = GetValueUInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSPROV, defaultUZero);
            Int32 DbsProvVers = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_ACEVERS, defaultZero);
            Int32 DbsExclusiv = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBEXCLS, defaultOnes);
            Int32 DbsSysDBShr = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_ENSHARE, defaultNegs);
            Int32 BitmapyTisk = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_TISKBMP, defaultZero);

            appDataItem.DataFileName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.DatabaseName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);
            string DbsDSrcName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCDSN, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.SystFileName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSSYST, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.DbServerName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCSRV, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.UserName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSUSER, DbsDataConfigKeys.USER_NAME);
            appDataItem.UserPssw = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSUPSW, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.OwnerName = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSOWNR, DbsDataConfigKeys.OWNER_NAME);
            appDataItem.OwnerPssw = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSOPSW, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryArch = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSARCH, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryBkup = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSBKUP, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTisk = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_PRNFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTXML = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_XMLFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTPDF = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_PDFFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTXLS = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_XLSFILE, DbsDataConfigKeys.EMPTY_STRING);
            string CmdLineExpt = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSCMDE, DbsDataConfigKeys.EMPTY_STRING);
            string CmdLineImpt = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSCMDI, DbsDataConfigKeys.EMPTY_STRING);
            string DbfDrivImpt = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCDBF, DbsDataConfigKeys.FOXBASE_DRIVER);
            string ExpUMuvozov = GetValueString(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_EXUMUVZ, DbsDataConfigKeys.QUOTES);

            Int32 DbsHRLink = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSHRMG, defaultZero);
            Int32 RemovePrac = GetValueInt32(paramXmlDoc, strNodeXPath, dataItemNode, DbsDataConfigKeys.REG_ITEM_REMPRAC, defaultZero);

            return appDataItem;
        }

        private static Int32 GetValueInt32(XmlDocument xmlConfig, string nodeXPath, string nodeConfig, string keyLabel, Int32 defaultValue)
        {
            StringBuilder bldNodeXPath = new StringBuilder(nodeXPath)
                .Append("/*[local-name()='").Append(REG_DATA_POL)
                .Append("' and @name='").Append(keyLabel).Append("']/@value");

            string strNodeXPath = bldNodeXPath.ToString();

            try
            {
                XmlNode xmlNodeValue = xmlConfig.SelectSingleNode(strNodeXPath);

                if (xmlNodeValue == null)
                {
                    return defaultValue;
                }

                string strNodeValue = xmlNodeValue.InnerText;

                if (strNodeValue == DbsDataConfigKeys.EMPTY_STRING)
                {
                    return defaultValue;
                }
                Int32 numNodeValue = defaultValue;

                if (Int32.TryParse(strNodeValue, out numNodeValue) == false)
                {
                    return defaultValue;
                }
                return numNodeValue;
            }
            catch (Exception ex)
            {
                string errorDiagnostics = string.Format("Exception in NODE {0}:\n{1}", nodeConfig, ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
            }
            return defaultValue;
        }

        private static UInt32 GetValueUInt32(XmlDocument xmlConfig, string nodeXPath, string nodeConfig, string keyLabel, UInt32 defaultValue)
        {
            StringBuilder bldNodeXPath = new StringBuilder(nodeXPath)
                .Append("/*[local-name()='").Append(REG_DATA_POL)
                .Append("' and @name='").Append(keyLabel).Append("']/@value");

            string strNodeXPath = bldNodeXPath.ToString();

            try
            {
                XmlNode xmlNodeValue = xmlConfig.SelectSingleNode(strNodeXPath);

                if (xmlNodeValue == null)
                {
                    return defaultValue;
                }

                string strNodeValue = xmlNodeValue.InnerText;

                if (strNodeValue == DbsDataConfigKeys.EMPTY_STRING)
                {
                    return defaultValue;
                }
                UInt32 numNodeValue = defaultValue;

                if (UInt32.TryParse(strNodeValue, out numNodeValue) == false)
                {
                    return defaultValue;
                }
                return numNodeValue;
            }
            catch (Exception ex)
            {
                string errorDiagnostics = string.Format("Exception in NODE {0}:\n{1}", nodeConfig, ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
            }
            return defaultValue;
        }

        private static string GetValueString(XmlDocument xmlConfig, string nodeXPath, string nodeConfig, string keyLabel, string defaultValue)
        {
            StringBuilder bldNodeXPath = new StringBuilder(nodeXPath)
                .Append("/*[local-name()='").Append(REG_DATA_POL)
                .Append("' and @name='").Append(keyLabel).Append("']/@value");

            string strNodeXPath = bldNodeXPath.ToString();

            try
            {
                XmlNode xmlNodeValue = xmlConfig.SelectSingleNode(strNodeXPath);

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
                string errorDiagnostics = string.Format("Exception in NODE {0}:\n{1}", nodeConfig, ex.ToString());
                System.Diagnostics.Debug.Print(errorDiagnostics);
            }
            return defaultValue;
        }
    }
}
