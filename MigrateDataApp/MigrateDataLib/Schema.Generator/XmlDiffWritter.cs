using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Config.DbsData;
using System.Xml;
using MigrateDataLib.Utils;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace MigrateDataLib.Schema.Generator
{
    class XmlDiffWritter : IGeneratorWriter, IXmlWritter
    {
        public XmlDiffWritter(string appRootFolder, DbsDataConfig appDataConfig)
        {
            string parAppConfig = appDataConfig.ConfigName;
            string infoFileName = parAppConfig + "_SCHEMA_DIFF.TXT";
            string codeFileName = parAppConfig + "_SCHEMA_DIFF.XML";

            m_InfoWriter = null;

            m_CodeWriter = null;

            m_CXmlWriter = null;

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
        }

        protected UInt32 m_PlatformType;
        protected string m_InfoFileName = "unknown";
        protected string m_InfoFilePath = "unknown";
        protected string m_CodeFileName = "unknown";
        protected string m_CodeFilePath = "unknown";

        protected TextWriter m_InfoWriter;

        protected TextWriter m_CodeWriter;

        protected XmlWriter m_CXmlWriter; 


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

            m_CXmlWriter = XmlWriter.Create(m_CodeWriter);

            m_CXmlWriter.WriteStartDocument();
            m_CXmlWriter.WriteStartElement("results");
        }
        public void OpenCode(UInt32 sourceType, string codeFilePath)
        {
            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.Dispose();

                m_CXmlWriter = null;
            }
            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();

                m_CodeWriter = null;
            }
            m_CodeWriter = File.CreateText(codeFilePath);

            m_CXmlWriter = XmlWriter.Create(m_CodeWriter);

            m_CXmlWriter.WriteStartDocument();
            m_CXmlWriter.WriteStartElement("results");
        }
        public void CloseCode()
        {
            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.WriteEndElement();

                m_CXmlWriter.WriteEndDocument();

                m_CXmlWriter.Flush();

                m_CXmlWriter.Close();

                m_CXmlWriter.Dispose();

                m_CXmlWriter = null;
            }
            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();

                m_CodeWriter = null;
            }
        }
        public void ExecuteCodeLine(string codeText, string infoName)
        {
        }

        public void DefaultCodeLine(string codeText, string infoName)
        {
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
        }
        public void WriteCode(string format, params object[] args)
        {
        }

        public void WriteCodeLine(string codeText)
        {
        }
        public void WriteCodeInBase64Line(string codeText)
        {
        }
        public long GetScriptCount(string countQuery)
        {
            return 0;
        }

        public void WriteAttribute(string atLabel, string atValue)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteAttribute(m_CXmlWriter, atLabel, atValue);
            }
        }
        public void WriteElement(string elLabel, string atValue)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteElement(m_CXmlWriter, elLabel, atValue);
            }
        }
        public void WriteElementWithAttribute(string elLabel, string atLabel, string atValue)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteElementWithAttribute(m_CXmlWriter, elLabel, atLabel, atValue);
            }
        }
        public void WriteElementWithAttributeNoEnd(string elLabel, string atLabel, string atValue)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteElementWithAttributeNoEnd(m_CXmlWriter, elLabel, atLabel, atValue);
            }
        }
        public void WriteCountOrErrorAttrib(bool success, Int32 tableCount, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteCountOrErrorAttrib(m_CXmlWriter, success, tableCount, excFunction, excError);
            }
        }
        public void WriteCountOrErrorBlock(string connText, bool success, Int32 tableCount, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteCountOrErrorBlock(m_CXmlWriter, connText, success, tableCount, excFunction, excError);
            }
        }
        public void WriteErrorElement(bool success, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteErrorElement(m_CXmlWriter, success, excFunction, excError);
            }
        }
        public void WriteStartEndErrorElement(bool completed, bool success, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteStartEndErrorElement(m_CXmlWriter, completed, success, excFunction, excError);
            }
        }
        public void WriteBegErrorElement(bool completed, bool success, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteBegErrorElement(m_CXmlWriter, completed, success, excFunction, excError);
            }
        }
        public void WriteEndErrorElement(bool completed, bool success, string excFunction, string excError)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteEndErrorElement(m_CXmlWriter, completed, success, excFunction, excError);
            }
        }
        public void WriteCountElement(bool success, Int32 diffCount)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteCountElement(m_CXmlWriter, success, diffCount);
            }
        }
        public void WriteCountBlock(string connText, Int32 diffCount)
        {
            if (m_CXmlWriter != null)
            {
                XmlResults.WriteCountBlock(m_CXmlWriter, connText, diffCount);
            }
        }
        public void WriteStartElement(string elementText)
        {
            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.WriteStartElement(elementText);
            }
        }

        public void WriteEndElement()
        {
            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.WriteEndElement();
            }
        }

        public void WriteFieldTypeValues(string[] fieldValues, string[] fieldKeys, IDictionary<string, string> fieldDict)
        {
            if (m_CXmlWriter != null)
            {
                int index = 0;
                foreach (var field in fieldKeys)
                {
                    XmlResults.WriteAttribute(m_CXmlWriter, field, fieldValues[index]);
                    index++;
                }
            }
        }

        public void XmlFileToHtmlFile(bool isCompareDetail)
        {
            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.Flush();
                m_CXmlWriter.Close();

                m_CodeWriter.Close();

                string resultXmlsName = m_CodeFilePath;
                string resultHtmlName = m_CodeFilePath.Replace(".XML", ".HTML");
                string resultXsltName = ""; // resultFoldName + "comp_result_2.xslt";

                // Exportuje výsledek transformace XML pomocí šablony XSLT do souboru HTML
                Stream xmlStream = GetXmlStreamForData(resultXmlsName);
                Stream xslStream = GetXslStreamForData(resultXsltName);
                if (xslStream == null)
                {
                    xslStream = GetDefaultXslStreamForData(isCompareDetail);
                }

                bool exportStyle = true;
                ExportToHtmlFile(xmlStream, xslStream, resultHtmlName);
                ExportToHtmlStyle(exportStyle, resultHtmlName);

                xmlStream.Close();
                xslStream.Close();


            }
        }

        protected void ExportToHtmlFile(Stream xmlStream, Stream xslStream, string fileName)
        {
            bool opakovatExportHTML = false;
            string message;
            Stream output = null;
            try
            {
                output = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);

                XslCompiledTransform transform = new XslCompiledTransform();
                XsltSettings transfSetting = new XsltSettings(true, true);
                transform.Load(new XPathDocument(xslStream), transfSetting, null);
                transform.Transform(new XPathDocument(xmlStream), null, output);


            }
            catch (System.Exception)
            {
                opakovatExportHTML = true;
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
            }
            if (opakovatExportHTML)
            {
                try
                {
                    output = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);

                    XslCompiledTransform transform = new XslCompiledTransform();
                    XsltSettings transfSetting = new XsltSettings(true, true);
                    transform.Load(new XPathDocument(xslStream), transfSetting, null);
                    transform.Transform(new XPathDocument(xmlStream), null, output);


                }
                catch (System.Exception exc)
                {
                    message = exc.ToString();
                }
                finally
                {
                    if (output != null)
                    {
                        output.Close();
                    }
                }
            }
        }

        private void ExportToHtmlStyle(bool exportStyle, string fileName)
        {
            // Exportuje obsah souboru se styly z Assembly do adresáře, kde je HTML soubor pod jménem *-style.css
            if (exportStyle)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(GetType());
                Stream styleStream = assembly.GetManifestResourceStream("MigrateDataLib.Schema.Generator.res.export_result.css");

                StreamReader styleReader = new StreamReader(styleStream);

                string styleFileDirs = System.IO.Path.GetDirectoryName(fileName);
                string styleFileStrs = System.IO.Path.GetFileNameWithoutExtension(fileName);
                string styleFileName = styleFileDirs + "//html_export_result.css";

                StreamWriter styleWriter = new StreamWriter(styleFileName, false);
                styleWriter.Write(styleReader.ReadToEnd());
                styleWriter.Close();

                styleReader.Close();

                styleStream.Close();
            }
        }

        protected Stream GetXmlStreamForData(string fileNameXml)
        {
            if (fileNameXml.Length == 0)
            {
                return null;
            }
            try
            {
                Stream stream = new FileStream(fileNameXml, FileMode.Open, FileAccess.Read);
                stream.Position = 0;
                return stream;
            }
            catch (System.Exception exc)
            {
                System.Diagnostics.Debug.Assert(false, exc.ToString());
                return null;
            }
        }
        protected Stream GetDefaultXslStreamForData(bool isCompareDetail)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(GetType());
            string[] resources = assembly.GetManifestResourceNames();
            if (!(isCompareDetail))
            {
                return assembly.GetManifestResourceStream("MigrateDataLib.Schema.Generator.res.export_result.xslt");
            }
            else
            {
                return assembly.GetManifestResourceStream("MigrateDataLib.Schema.Generator.res.export_test_result.xslt");
            }
        }

        protected Stream GetXslStreamForData(string fileNameXslt)
        {
            if (fileNameXslt.Length == 0)
            {
                return null;
            }
            try
            {
                Stream stream = new FileStream(fileNameXslt, FileMode.Open, FileAccess.Read);
                stream.Position = 0;
                return stream;
            }
            catch (System.Exception exc)
            {
                System.Diagnostics.Debug.Assert(false, exc.ToString());
                return null;
            }
        }

        public void Dispose()
        {
            if (m_InfoWriter != null)
            {
                m_InfoWriter.Dispose();
            }

            if (m_CXmlWriter != null)
            {
                m_CXmlWriter.Dispose();

                m_CXmlWriter = null;
            }
            if (m_CodeWriter != null)
            {
                m_CodeWriter.Dispose();
            }
        }
    }
}
