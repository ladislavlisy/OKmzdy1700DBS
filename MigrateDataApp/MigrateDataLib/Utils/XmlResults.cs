using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigrateDataLib.Utils
{
    public static class XmlResults
    {
        public static void WriteAttribute(XmlWriter xmlBuilder, string atLabel, string atValue)
        {
            xmlBuilder.WriteStartAttribute(atLabel);
            xmlBuilder.WriteString(atValue);
            xmlBuilder.WriteEndAttribute();
        }
        public static void WriteElement(XmlWriter xmlBuilder, string elLabel, string atValue)
        {
            xmlBuilder.WriteStartElement(elLabel);
            xmlBuilder.WriteString(atValue);
            xmlBuilder.WriteEndElement();
        }
        public static void WriteElementWithAttribute(XmlWriter xmlBuilder, string elLabel, string atLabel, string atValue)
        {
            xmlBuilder.WriteStartElement(elLabel);
            xmlBuilder.WriteStartAttribute(atLabel);
            xmlBuilder.WriteString(atValue);
            xmlBuilder.WriteEndAttribute();
            xmlBuilder.WriteEndElement();
        }
        public static void WriteElementWithAttributeNoEnd(XmlWriter xmlBuilder, string elLabel, string atLabel, string atValue)
        {
            xmlBuilder.WriteStartElement(elLabel);
            xmlBuilder.WriteStartAttribute(atLabel);
            xmlBuilder.WriteString(atValue);
            xmlBuilder.WriteEndAttribute();
        }
        public static void WriteCountOrErrorAttrib(XmlWriter xmlBuilder, bool success, Int32 tableCount, string excFunction, string excError)
        {
            WriteCountElement(xmlBuilder, success, tableCount);
            WriteStartEndErrorElement(xmlBuilder, success, success, excFunction, excError);
        }

        public static void WriteCountOrErrorBlock(XmlWriter xmlBuilder, string connText, bool success, Int32 tableCount, string excFunction, string excError)
        {
            xmlBuilder.WriteStartElement(connText);
            WriteCountOrErrorAttrib(xmlBuilder, success, tableCount, excFunction, excError);
            xmlBuilder.WriteEndElement();
        }

        public static void WriteErrorElement(XmlWriter xmlBuilder, bool success, string excFunction, string excError)
        {
            if (!success)
            {
                xmlBuilder.WriteStartElement("error");
                xmlBuilder.WriteStartAttribute("type");
                xmlBuilder.WriteString(excFunction);
                xmlBuilder.WriteEndAttribute();
                xmlBuilder.WriteStartAttribute("description");
                xmlBuilder.WriteString(excError);
                xmlBuilder.WriteEndAttribute();
                xmlBuilder.WriteEndElement();
            }
        }

        public static void WriteStartEndErrorElement(XmlWriter xmlBuilder, bool completed, bool success, string excFunction, string excError)
        {
            if (!completed)
            {
                xmlBuilder.WriteStartElement("errors");
            }
            WriteErrorElement(xmlBuilder, success, excFunction, excError);
            if (!completed)
            {
                xmlBuilder.WriteEndElement();
            }
        }

        public static void WriteBegErrorElement(XmlWriter xmlBuilder, bool completed, bool success, string excFunction, string excError)
        {
            if (!completed)
            {
                xmlBuilder.WriteStartElement("errors");
            }
            WriteErrorElement(xmlBuilder, success, excFunction, excError);
        }
        public static void WriteEndErrorElement(XmlWriter xmlBuilder, bool completed, bool success, string excFunction, string excError)
        {
            WriteErrorElement(xmlBuilder, success, excFunction, excError);
            if (!completed)
            {
                xmlBuilder.WriteEndElement();
            }
        }

        public static void WriteCountElement(XmlWriter xmlBuilder, bool success, Int32 diffCount)
        {
            if (success)
            {
                xmlBuilder.WriteStartAttribute("count");
                xmlBuilder.WriteString(diffCount.ToString());
                xmlBuilder.WriteEndAttribute();
            }
        }
        public static void WriteCountBlock(XmlWriter xmlBuilder, string connText, Int32 tableCount)
        {
            xmlBuilder.WriteStartElement(connText);
            WriteCountElement(xmlBuilder, true, tableCount);
            xmlBuilder.WriteEndElement();
        }


    }
}
