using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Utils
{
    public class ConfigWinRegsUtil
    {
        public static string REG_SOFTWARE = "Software";
        public static string REG_OKSYSTEM = "OKsystem";
        public static string REG_OKMZDY_PRO_WINDOWS = "OKmzdy pro Windows";
        public static DbsDataConfig LoadOKmzdyMainRegistry()
        {
            using (RegistryKey regSoftwareNode = Registry.LocalMachine.OpenSubKey(REG_SOFTWARE))
            {
                if (regSoftwareNode != null)
                {
                    using (RegistryKey regOKsystemNode = regSoftwareNode.OpenSubKey(REG_OKSYSTEM))
                    {
                        if (regOKsystemNode != null)
                        {
                            using (RegistryKey regOKmzdyNode = regOKsystemNode.OpenSubKey(REG_OKMZDY_PRO_WINDOWS))
                            {
                                if (regOKmzdyNode != null)
                                {
                                    return LoadOKmzdyMainRegistryItem(regOKmzdyNode);
                                }
                            }
                        }
                    }
                }
            }
            return new DbsDataConfig();
        }
        public static DbsDataConfig LoadOKmzdyDataRegistry(string dataNode, string itemNode)
        {
            using (RegistryKey regSoftwareNode = Registry.CurrentUser.OpenSubKey(REG_SOFTWARE))
            {
                if (regSoftwareNode != null)
                {
                    using (RegistryKey regOKsystemNode = regSoftwareNode.OpenSubKey(REG_OKSYSTEM))
                    {
                        if (regOKsystemNode != null)
                        {
                            using (RegistryKey regOKmzdyNode = regOKsystemNode.OpenSubKey(REG_OKMZDY_PRO_WINDOWS))
                            {
                                if (regOKmzdyNode != null)
                                {
                                    using (RegistryKey regDataNode = regOKmzdyNode.OpenSubKey(dataNode))
                                    {
                                        if (regDataNode != null)
                                        {
                                            using (RegistryKey regItemNode = regDataNode.OpenSubKey(itemNode))
                                            {
                                                return LoadOKmzdyDataRegistryItem(regItemNode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new DbsDataConfig();
        }
        private static DbsDataConfig LoadOKmzdyMainRegistryItem(RegistryKey dataMainNode)
        {
            DbsDataConfig appMainItem = new DbsDataConfig();

            if (dataMainNode == null)
            {
                appMainItem.Init();

                return appMainItem;
            }

            Int32 defaultZero = 0;

            Int32 ShortcutDesktop = GetValueInt32(dataMainNode, DbsDataConfigKeys.REG_ITEM_LNKDESK, defaultZero);
            Int32 AccessAceVers = GetValueInt32(dataMainNode, DbsDataConfigKeys.REG_ITEM_ACEVERS, defaultZero);

            string ArchivFolder = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_ARCFLDR, DbsDataConfigKeys.EMPTY_STRING);
            string DatabaseFolder = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.DataFileName = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);
            string ParentFolder = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_PARFLDR, DbsDataConfigKeys.EMPTY_STRING);
            string ShortcutFolder = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_LNKFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.SystFileName = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSSYST, DbsDataConfigKeys.EMPTY_STRING);
            string TiskyFolder = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_TISFLDR, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.DbServerName = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_ODBCSRV, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.UserName = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSUSER, DbsDataConfigKeys.USER_NAME);
            appMainItem.UserPssw = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSUPSW, DbsDataConfigKeys.EMPTY_STRING);
            appMainItem.OwnerName = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSOWNR, DbsDataConfigKeys.OWNER_NAME);
            appMainItem.OwnerPssw = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_DBSOPSW, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSdba = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_SDBUSER, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSdbaPsw = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_SDBPSSW, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlWksArch = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_WKSARCH, DbsDataConfigKeys.EMPTY_STRING);
            string MsSqlSrvArch = GetValueString(dataMainNode, DbsDataConfigKeys.REG_ITEM_SRVARCH, DbsDataConfigKeys.EMPTY_STRING);

            return appMainItem;
        }

        private static DbsDataConfig LoadOKmzdyDataRegistryItem(RegistryKey dataItemNode)
        {
            DbsDataConfig appDataItem = new DbsDataConfig();

            if (dataItemNode == null)
            {
                appDataItem.Init();

                return appDataItem;
            }

            Int32 defaultZero = 0;
            Int32 defaultOnes = 1;
            Int32 defaultNegs = -1;

            appDataItem.ConfigName = dataItemNode.Name;

            Int32 AppUIcolors = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_UICOLOR, defaultZero);
            Int32 DbsProvider = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSPROV, defaultZero);
            Int32 DbsProvVers = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_ACEVERS, defaultZero);
            Int32 DbsExclusiv = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBEXCLS, defaultOnes);
            Int32 DbsSysDBShr = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_ENSHARE, defaultNegs);
            Int32 BitmapyTisk = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_TISKBMP, defaultZero);

            appDataItem.DataFileName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.DatabaseName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSNAME, DbsDataConfigKeys.EMPTY_STRING);;
            string DbsDSrcName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCDSN, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.SystFileName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSSYST, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.DbServerName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCSRV, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.UserName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSUSER, DbsDataConfigKeys.USER_NAME);
            appDataItem.UserPssw = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSUPSW, DbsDataConfigKeys.EMPTY_STRING);
            appDataItem.OwnerName = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSOWNR, DbsDataConfigKeys.OWNER_NAME);
            appDataItem.OwnerPssw = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSOPSW, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryArch = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSARCH, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryBkup = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSBKUP, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTisk = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_PRNFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTXML = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_XMLFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTPDF = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_PDFFILE, DbsDataConfigKeys.EMPTY_STRING);
            string SouboryTXLS = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_XLSFILE, DbsDataConfigKeys.EMPTY_STRING);
            string CmdLineExpt = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSCMDE, DbsDataConfigKeys.EMPTY_STRING);
            string CmdLineImpt = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSCMDI, DbsDataConfigKeys.EMPTY_STRING);
            string DbfDrivImpt = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_ODBCDBF, DbsDataConfigKeys.FOXBASE_DRIVER);
            string ExpUMuvozov = GetValueString(dataItemNode, DbsDataConfigKeys.REG_ITEM_EXUMUVZ, DbsDataConfigKeys.QUOTES);

            Int32 DbsHRLink = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_DBSHRMG, defaultZero);
            Int32 RemovePrac = GetValueInt32(dataItemNode, DbsDataConfigKeys.REG_ITEM_REMPRAC, defaultZero);

            return appDataItem;
        }

        private static Int32 GetValueInt32(RegistryKey dataItemNode, string dataItemName, Int32 defaultValue)
        {
            Int32? dataItemValue = dataItemNode.GetValue(dataItemName, defaultValue) as Int32?;

            if (dataItemValue.HasValue)
            {
                return dataItemValue.Value;
            }
            return defaultValue;
        }
        private static string GetValueString(RegistryKey dataItemNode, string dataItemName, string defaultValue)
        {
            return dataItemNode.GetValue(dataItemName, defaultValue) as string;
        }
    }
}
