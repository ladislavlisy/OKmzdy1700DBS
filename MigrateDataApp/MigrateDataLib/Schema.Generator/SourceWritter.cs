using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Config.DbsData;
using System.IO;

namespace MigrateDataLib.Schema.Generator
{
    public class SourceWritter : IGeneratorWriter
    {
        public SourceWritter(string appRootFolder, DbsDataConfig appDataConfig, string codePref, string codeExts)
        {
            string parAppConfig = appDataConfig.ConfigName;

            string infoFileName = parAppConfig + "_ENTITY_SOURCE.TXT";
            string clazzTablePathName = parAppConfig + "_TableEntity";
            string clazzQueryPathName = parAppConfig + "_QueryEntity";
            string confxTableFileName = parAppConfig + "_TableConfig" + codeExts;
            string confxQueryFileName = parAppConfig + "_QueryConfig" + codeExts;
            string contxFileName = parAppConfig + "_EntityContext" + codeExts;

            m_InfoWriter = null;

            m_CodeWriter = null;

            string appPrefFolder = System.IO.Path.Combine(appRootFolder, codePref);

            if (infoFileName != null)
            {
                m_InfoFileName = infoFileName;
                m_InfoFilePath = System.IO.Path.Combine(appPrefFolder, infoFileName);

            }
            if (clazzTablePathName != null)
            {
                m_TEntFoldName = clazzTablePathName;
                m_TEntFoldPath = System.IO.Path.Combine(appPrefFolder, clazzTablePathName);

            }
            if (clazzQueryPathName != null)
            {
                m_QEntFoldName = clazzQueryPathName;
                m_QEntFoldPath = System.IO.Path.Combine(appPrefFolder, clazzQueryPathName);

            }
            if (confxTableFileName != null)
            {
                m_TCfxFileName = confxTableFileName;
                m_TCfxFilePath = System.IO.Path.Combine(appPrefFolder, confxTableFileName);

            }
            if (confxQueryFileName != null)
            {
                m_QCfxFileName = confxQueryFileName;
                m_QCfxFilePath = System.IO.Path.Combine(appPrefFolder, confxQueryFileName);

            }

            if (contxFileName != null)
            {
                m_ContFileName = contxFileName;
                m_ContFilePath = System.IO.Path.Combine(appPrefFolder, contxFileName);

            }

            m_PlatformType = appDataConfig.PlatformType;
            m_OutputBase64 = false;
        }

        protected UInt32 m_PlatformType;
        protected string m_InfoFileName = "unknown";
        protected string m_InfoFilePath = "unknown";

        protected string m_TEntFoldName = "unknown";
        protected string m_QEntFoldName = "unknown";
        protected string m_TCfxFileName = "unknown";
        protected string m_QCfxFileName = "unknown";
        protected string m_ContFileName = "unknown";

        protected string m_TEntFoldPath = "unknown";
        protected string m_QEntFoldPath = "unknown";
        protected string m_TCfxFilePath = "unknown";
        protected string m_QCfxFilePath = "unknown";
        protected string m_ContFilePath = "unknown";

        protected bool m_OutputBase64;

        protected TextWriter m_InfoWriter;

        protected TextWriter m_CodeWriter;

        public UInt32 PlatformType()
        {
            return m_PlatformType;
        }
        public string InfoFileName()
        {
            return m_InfoFileName;
        }
        public string InfoFilePath()
        {
            return m_InfoFilePath;
        }
        public string TEntFoldName()
        {
            return m_TEntFoldName;
        }
        public string TEntFoldPath()
        {
            return m_TEntFoldPath;
        }

        public string QEntFoldName()
        {
            return m_QEntFoldName;
        }
        public string QEntFoldPath()
        {
            return m_QEntFoldPath;
        }
        public string TCfxFileName()
        {
            return m_TCfxFileName;
        }
        public string TCfxFilePath()
        {
            return m_TCfxFilePath;
        }

        public string QCfxFileName()
        {
            return m_QCfxFileName;
        }
        public string QCfxFilePath()
        {
            return m_QCfxFilePath;
        }

        public string ContFileName()
        {
            return m_ContFileName;
        }
        public string ContFilePath()
        {
            return m_ContFilePath;
        }

        public void PrepareInfo(MigrateOptions buildOptions)
        {
            m_InfoWriter = File.CreateText(m_InfoFilePath);
        }

        public void PrepareCode(MigrateOptions buildOptions)
        {
            m_CodeWriter = File.CreateText(m_ContFilePath);
        }
        public void OpenCode(UInt32 sourceType, string codeFileName)
        {
            string codeFilePath = CodeFileName(sourceType, codeFileName);

            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();

                m_CodeWriter = null;
            }
            m_CodeWriter = File.CreateText(codeFilePath);
        }

        private string CodeFileName(UInt32 sourceType, string codeFileName)
        {
            string codeFilePath = "";
            switch (sourceType)
            {
                case 1:
                    codeFilePath = System.IO.Path.Combine(m_TEntFoldPath, codeFileName);
                    break;
                case 2:
                    codeFilePath = System.IO.Path.Combine(m_QEntFoldPath, codeFileName);
                    break;
                case 3:
                    //codeFilePath = System.IO.Path.Combine(m_TCfxFilePath, codeFileName);
                    codeFilePath = m_TCfxFilePath;
                    break;
                case 4:
                    //codeFilePath = System.IO.Path.Combine(m_QCfxFilePath, codeFileName);
                    codeFilePath = m_QCfxFilePath;
                    break;
                case 5:
                    //codeFilePath = System.IO.Path.Combine(m_ContFilePath, codeFileName);
                    codeFilePath = m_ContFilePath;
                    break;
                default:
                    codeFilePath = m_ContFilePath;
                    break;
            }
            return codeFilePath;
        }

        public void CloseCode()
        {
            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();

                m_CodeWriter = null;
            }
        }
        public void ExecuteCodeLine(string codeText, string infoName)
        {
            if (codeText != DatabaseDef.EMPTY_STRING)
            {
            }
        }

        public void DefaultCodeLine(string codeText, string infoName)
        {
            if (m_OutputBase64)
            {
                WriteCodeInBase64Line(codeText);
            }
            else
            {
                WriteCodeLine(codeText);
            }

        }

        public void WriteInfo(string infoText)
        {
            if (m_InfoWriter != null)
            {
                if (infoText != DatabaseDef.EMPTY_STRING)
                {
                    m_InfoWriter.Write(infoText);
                }
            }
        }
        public void WriteInfo(string format, params object[] args)
        {
            if (m_InfoWriter != null)
            {
                if (format != DatabaseDef.EMPTY_STRING)
                {
                    m_InfoWriter.Write(format, args);
                }
            }
        }

        public void WriteInfoLine(string infoText)
        {
            if (m_InfoWriter != null)
            {
                if (infoText != DatabaseDef.EMPTY_STRING)
                {
                    m_InfoWriter.WriteLine(infoText);
                }
            }
        }

        public void WriteInfoLine(string format, params object[] args)
        {
            if (m_InfoWriter != null)
            {
                if (format != DatabaseDef.EMPTY_STRING)
                {
                    m_InfoWriter.WriteLine(format, args);
                }
            }
        }

        public void WriteCode(string codeText)
        {
            if (m_CodeWriter != null)
            {
                if (codeText != DatabaseDef.EMPTY_STRING)
                {
                    m_CodeWriter.Write(codeText);
                }
            }
        }
        public void WriteCode(string format, params object[] args)
        {
            if (m_CodeWriter != null)
            {
                if (format != DatabaseDef.EMPTY_STRING)
                {
                    m_CodeWriter.Write(format, args);
                }
            }
        }

        public void WriteCodeLine(string codeText)
        {
            if (m_CodeWriter != null)
            {
                if (codeText != DatabaseDef.EMPTY_STRING)
                {
                    m_CodeWriter.WriteLine(codeText);
                }
            }
        }
        public void WriteCodeInBase64Line(string codeText)
        {
            if (m_CodeWriter != null)
            {
                if (codeText != DatabaseDef.EMPTY_STRING)
                {
                    m_CodeWriter.WriteLine(Base64Encode(codeText));
                }
            }
        }
        public long GetScriptCount(string countQuery)
        {
            return 0;
        }

        public void Dispose()
        {
            if (m_InfoWriter != null)
            {
                m_InfoWriter.Dispose();
            }

            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
