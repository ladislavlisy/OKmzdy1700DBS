using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Config.DbsData;

namespace MigrateDataLib.Schema.Generator
{
    public class ScriptWritter : IGeneratorWriter
    {
        public ScriptWritter(string appRootFolder, DbsDataConfig appDataConfig, bool outBase64)
        {
            string parAppConfig = appDataConfig.ConfigName;
            string infoFileName = parAppConfig + "_SCHEMA_SCRIPT.TXT";
            string codeFileName = parAppConfig + "_SCHEMA_SCRIPT.SQL";

            m_InfoWriter = null;

            m_CodeWriter = null;

            if (infoFileName != null)
            {
                m_InfoFileName = infoFileName;
                m_InfoFilePath = System.IO.Path.Combine(appRootFolder, infoFileName);

            }

            if (codeFileName != null)
            {
                m_CodeFileName = codeFileName;
                m_CodeFilePath = System.IO.Path.Combine(appRootFolder, codeFileName);

            }

            m_PlatformType = appDataConfig.PlatformType;
            m_OutputBase64 = outBase64;
        }

        protected UInt32 m_PlatformType;
        protected string m_InfoFileName = "unknown";
        protected string m_InfoFilePath = "unknown";
        protected string m_CodeFileName = "unknown";
        protected string m_CodeFilePath = "unknown";
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
        public string CodeFileName()
        {
            return m_CodeFileName;
        }
        public string CodeFilePath()
        {
            return m_CodeFilePath;
        }

        public void PrepareInfo(MigrateOptions buildOptions)
        {
            m_InfoWriter = File.CreateText(m_InfoFilePath);
        }

        public void PrepareCode(MigrateOptions buildOptions)
        {
            m_CodeWriter = File.CreateText(m_CodeFilePath);
        }
        public void OpenCode(UInt32 sourceType, string codeFilePath)
        {
            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();

                m_CodeWriter = null;
            }
            m_CodeWriter = File.CreateText(codeFilePath);
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
