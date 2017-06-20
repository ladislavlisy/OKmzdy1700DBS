using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.Generator
{
    public interface IXmlWritter
    {
        void WriteAttribute(string atLabel, string atValue);
        void WriteElement(string elLabel, string atValue);
        void WriteElementWithAttribute(string elLabel, string atLabel, string atValue);
        void WriteElementWithAttributeNoEnd(string elLabel, string atLabel, string atValue);
        void WriteCountOrErrorAttrib(bool success, Int32 tableCount, string excFunction, string excError);
        void WriteCountOrErrorBlock(string connText, bool success, Int32 tableCount, string excFunction, string excError);
        void WriteErrorElement(bool success, string excFunction, string excError);
        void WriteStartEndErrorElement(bool completed, bool success, string excFunction, string excError);
        void WriteBegErrorElement(bool completed, bool success, string excFunction, string excError);
        void WriteEndErrorElement(bool completed, bool success, string excFunction, string excError);
        void WriteCountElement(bool success, Int32 diffCount);
        void WriteCountBlock(string connText, Int32 diffCount);
        void XmlFileToHtmlFile(bool isCompareDetail);
        void WriteStartElement(string elementText);
        void WriteEndElement();
        void WriteFieldTypeValues(string[] fieldValues, string[] fieldKeys, IDictionary<string, string> fieldDict);
    }
}
