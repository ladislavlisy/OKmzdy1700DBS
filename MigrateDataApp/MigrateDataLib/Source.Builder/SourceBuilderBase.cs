using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Schema.DefInfoItems;
using MigrateDataLib.Schema.Generator;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.Utils;

namespace MigrateDataLib.Source.Builder
{
    public abstract class SourceBuilderBase : ISrcCodebaseBuilder
    {
        protected static readonly char[] TRIM_CHARS = { '\r', '\n', '\t', ' ', ',' };
        protected static readonly char[] SPLIT_CHARS = { '\n' };
        protected static readonly string SPLIT_STRC = "\n";

        protected const string EMPTY_SPACES = " ";
        protected const string EMPTY_STRING = "";
        protected const string NEW_LINE_STR = "\n";

        protected const string TAB_INDENT0 = "";
        protected const string TAB_INDENT1 = "\t";
        protected const string TAB_INDENT2 = "\t\t";
        protected const string TAB_INDENT3 = "\t\t\t";

        protected const string SP_INDENT0 = "";
        protected const string SP_INDENT1 = "    ";
        protected const string SP_INDENT2 = "        ";
        protected const string SP_INDENT3 = "            ";

        readonly string CONTEXT_MAIN_NAME = "_DATA_LIB";
        readonly string CONTEXT_PART_NAME = "_DATA_CTX";
        readonly string CONTEXT_PROJ_NAME = "_DATA_LIB._DATA_PRX._IMPL";

        protected DbsDataConfig _config;

        protected IList<Tuple<string, string>> m_ChangeNames;

        protected UInt32 PlatformType { get; set; }
        protected string ProjectNamespace { get; set; }

        protected string ContextProj()
        {
            return CONTEXT_PROJ_NAME;
        }

        protected string ContextName()
        {
            return CONTEXT_MAIN_NAME;
        }

        protected string ContextPart()
        {
            return CONTEXT_PART_NAME;
        }

        public SourceBuilderBase(DbsDataConfig config)
        {
            this._config = config;

            this.PlatformType = _config.PlatformType;

            this.ProjectNamespace = _config.ConfigName.ConvertNameToCamel();

            m_ChangeNames = new Tuple<string, string>[] {
                new Tuple<string, string>("Ppom", "PPom"),
                new Tuple<string, string>("Vrep", "VRep"),
                new Tuple<string, string>("Uimp", "UImp"),
                new Tuple<string, string>("Ulst", "ULst"),
                new Tuple<string, string>("SestavyUdata", "SestavyData"),
                new Tuple<string, string>("Pmedia", "PMedia"),
                new Tuple<string, string>("PrijmySsp", "PrijmySSP"),
                new Tuple<string, string>("HlaseniZp", "HlaseniZP"),
                new Tuple<string, string>("Krokujprac", "KrokujPrac"),
                new Tuple<string, string>("Vyberprac", "VyberPrac"),
                new Tuple<string, string>("Vyberppom", "VyberPPom"),
                new Tuple<string, string>("VyberReldpdata", "VyberReldpData"),
                new Tuple<string, string>("Vyberreldpdavka", "VyberReldpDavka"),
                new Tuple<string, string>("VyberReldp09data", "VyberReldp09Data"),
                new Tuple<string, string>("Vyberreldp09davka", "VyberReldp09Davka"),
                new Tuple<string, string>("Vybersbmedia", "VyberSbMedia"),
                new Tuple<string, string>("Vyberplmedia", "VyberPlMedia"),
                new Tuple<string, string>("Vyberucmedia", "VyberUcMedia"),
                new Tuple<string, string>("Vybersestavy", "VyberSestavy"),
                new Tuple<string, string>("Vyberutvary", "VyberUtvary"),
                new Tuple<string, string>("Vyberucetprpolozky", "VyberUcetPrPolozky")
            };
        }

        public abstract void CreateTableClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateTableHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateQueryClazzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateQueryHeadzFile(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateTableConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateQueryConfxCode(TableDefInfo tableInfo, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateBeginConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateCloseConfxCode(IList<TableDefInfo> tableList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract void CreateDbSetContxFile(IList<TableDefInfo> tableList, IList<QueryDefInfo> queryList, UInt32 buildVersion, IGeneratorWriter scriptWriter);
        public abstract string Extension();
        public abstract string Prefix();
    }
}
