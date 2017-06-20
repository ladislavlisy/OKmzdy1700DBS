using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Config.DbsData;
using MigrateDataLib.SqlData.Adapters;

namespace MigrateDataLib.Schema.Generator
{
    public class ScriptExecutor : IGeneratorWriter
    {
        public ScriptExecutor(string appRootFolder, DbsDataConfig appDataConfig)
        {
            m_DataConfig = appDataConfig;

            string parAppConfig = m_DataConfig.ConfigName;
            string infoFileName = parAppConfig + "_SCHEMA_SCRIPT.TXT";
            string codeFileName = parAppConfig + "_SCHEMA_SCRIPT.SQL";

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
        }

        protected string m_InfoFileName = "unknown";
        protected string m_InfoFilePath = "unknown";
        protected string m_CodeFileName = "unknown";
        protected string m_CodeFilePath = "unknown";

        protected TextWriter m_InfoWriter;

        protected TextWriter m_CodeWriter;

        protected DbsDataConfig m_DataConfig;

        protected ISqlAdapter m_ExecAdapter;
        public UInt32 PlatformType()
        {
            if (m_ExecAdapter == null)
            {
                return m_DataConfig.PlatformType;
            }
            return m_ExecAdapter.PlatformType();
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
        public void DefaultCodeLine(string codeText, string infoName)
        {
            ExecuteCodeLine(codeText, infoName);
        }

        public void PrepareInfo(MigrateOptions buildOptions)
        {
            m_InfoWriter = File.CreateText(m_InfoFilePath);

            m_CodeWriter = File.CreateText(m_CodeFilePath);

        }
        public void PrepareCode(MigrateOptions buildOptions)
        {
            bool createDatabase = buildOptions.CreateDataFiles;

            m_ExecAdapter = SqlAdapterFactory.CreateSqlAdapter(m_DataConfig);

            try
            {
                if (createDatabase)
                {
                    m_ExecAdapter.CreateDatabase();
                }

                m_ExecAdapter.CreateConnection();

                m_ExecAdapter.OpenConnection();
            }
            catch (Exception ex)
            {
                WriteInfoLine("Database Exception: {0}", ex.ToString());
            }
        }

        public void OpenCode(UInt32 sourceType, string codeFilePath)
        {
        }

        public void CloseCode()
        {
        }

        public void ExecuteCodeLine(string codeText, string infoName)
        {
            if (codeText != DatabaseDef.EMPTY_STRING)
            {
                WriteCodeLine(codeText);

                try
                {
                    DbCommand command = m_ExecAdapter.GetCommand(codeText + ";");

                    command.ExecuteNonQuery();
                }
                catch (OleDbException dbex)
                {
                    WriteInfoLine("Database Exception: {0}", infoName);
                    WriteInfoLine("Message: {0}", dbex.ToString());

                    for (int i = 0; i < dbex.Errors.Count; i++)
                    {
                        WriteInfoLine("----------------------------------------------------");
                        WriteInfoLine("Index #{0}", i);
                        WriteInfoLine("Message: {0}", dbex.Errors[i].Message);
                        WriteInfoLine("NativeError: {0}", dbex.Errors[i].NativeError);
                        WriteInfoLine("Source: {0}", dbex.Errors[i].Source);
                        WriteInfoLine("SQLState: {0}", dbex.Errors[i].SQLState);
                        WriteInfoLine("----------------------------------------------------");
                    }
                }
                catch (Exception ex)
                {
                    WriteInfoLine("Database Exception: {0}", infoName);
                    WriteInfoLine("Message: {0}", ex.ToString());
                }
            }
        }
        public void WriteInfo(string infoText)
        {
            if (infoText != DatabaseDef.EMPTY_STRING)
            {
                m_InfoWriter.Write(infoText);
            }
        }
        public void WriteInfo(string format, params object[] args)
        {
            if (format != DatabaseDef.EMPTY_STRING)
            {
                m_InfoWriter.Write(format, args);
            }
        }

        public void WriteInfoLine(string infoText)
        {
            if (infoText != DatabaseDef.EMPTY_STRING)
            {
                m_InfoWriter.WriteLine(infoText);
            }
        }

        public void WriteInfoLine(string format, params object[] args)
        {
            if (format != DatabaseDef.EMPTY_STRING)
            {
                m_InfoWriter.WriteLine(format, args);
            }
        }

        public void WriteCode(string codeText)
        {
            if (codeText != DatabaseDef.EMPTY_STRING)
            {
                m_CodeWriter.Write(codeText);
            }
        }
        public void WriteCode(string format, params object[] args)
        {
            if (format != DatabaseDef.EMPTY_STRING)
            {
                m_CodeWriter.Write(format, args);
            }
        }

        public void WriteCodeLine(string codeText)
        {
            if (codeText != DatabaseDef.EMPTY_STRING)
            {
                m_CodeWriter.WriteLine(codeText);
            }
        }
        public void WriteCodeInBase64Line(string codeText)
        {
            if (codeText != DatabaseDef.EMPTY_STRING)
            {
                m_CodeWriter.WriteLine(Base64Encode(codeText));
            }
        }
        public long GetScriptCount(string countQuery)
        {
            return 0;
        }

        public void Dispose()
        {
            m_InfoWriter.Dispose();

            m_CodeWriter.Dispose();

            m_ExecAdapter.Dispose();
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
