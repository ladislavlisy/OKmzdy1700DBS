using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Utils;
using MigrateDataLib.Constants;

namespace MigrateDataLib.Config.DbsData
{
    public class DbsDataConfig
    {
        public string ConfigName { get; set; }
        public UInt32 PlatformType { get; set; }
        public string DataFileName { get; set; }
        public string SystFileName { get; set; }
        public string DatabaseName { get; set; }
        public string DbServerName { get; set; }
        public string UserName { get; set; }
        public string UserPssw { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPssw { get; set; }

        public DbsDataConfig()
        {
            Init();
        }

        public void Init()
        {
            PlatformType = 0;

            ConfigName = SchemaDefaults.EMPTY_STRING;

            DataFileName = SchemaDefaults.EMPTY_STRING;
            SystFileName = SchemaDefaults.EMPTY_STRING;
            DatabaseName = SchemaDefaults.EMPTY_STRING;
            DbServerName = SchemaDefaults.EMPTY_STRING;
            UserName = SchemaDefaults.USER_NAME;
            UserPssw = SchemaDefaults.EMPTY_STRING;
            OwnerName = SchemaDefaults.OWNER_NAME;
            OwnerPssw = SchemaDefaults.EMPTY_STRING;
        }

        public string PlainUsersPsw()
        {
            return CryptoUtils.HashToPlainText(UserPssw);
        }
        public string PlainOwnerPsw()
        {
            return CryptoUtils.HashToPlainText(OwnerPssw);
        }

    }
}
