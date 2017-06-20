using MigrateDataLib.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.DefInfoItems
{
    public abstract class NameAliasInfo
    {
        public abstract string OrigsName { get; }
        public abstract string AliasName { get; }
        public abstract string Functions { get; }
    }

    public sealed class SimpleInfo : NameAliasInfo, ICloneable
    {
        private string m_OrigsName;

        private string m_Functions;
        private SimpleInfo(string name)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_OrigsName = name;

            this.m_Functions = DatabaseDef.EMPTY_STRING;
        }

        private SimpleInfo(string name, string functions)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_OrigsName = name;

            if (functions == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_Functions = functions;
        }

        public static NameAliasInfo Create(string name)
        {
            return new SimpleInfo(name);
        }

        public static NameAliasInfo Create(string name, string function)
        {
            return new SimpleInfo(name, function);
        }

        public override string OrigsName
        {
            get { return m_OrigsName; }
        }
        public override string AliasName
        {
            get { return m_OrigsName; }
        }
        public override string Functions
        {
            get { return m_Functions; }
        }
        public object Clone()
        {
            SimpleInfo other = (SimpleInfo)this.MemberwiseClone();
            other.m_OrigsName = this.m_OrigsName;
            other.m_Functions = this.m_Functions;

            return other;
        }
    }

    public sealed class AliasInfo : NameAliasInfo, ICloneable
    {
        private string m_OrigsName;

        private string m_AliasName;

        private string m_Functions;

        private AliasInfo(string name, string alias)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_OrigsName = name;

            if (alias == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_AliasName = alias;

            this.m_Functions = DatabaseDef.EMPTY_STRING;
        }

        private AliasInfo(string name, string alias, string functions)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }
            this.m_OrigsName = name;

            if (alias == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }
            this.m_AliasName = alias;

            if (functions == null)
            {
                throw new System.ArgumentNullException("value", "Some value was null, use None instead");
            }

            this.m_Functions = functions;
        }

        public static AliasInfo Create(string name, string alias)
        {
            return new AliasInfo(name, alias);
        }

        public static AliasInfo Create(string name, string alias, string functions)
        {
            return new AliasInfo(name, alias, functions);
        }

        public override string OrigsName
        {
            get { return m_OrigsName; }
        }
        public override string AliasName
        {
            get { return m_AliasName; }
        }
        public override string Functions
        {
            get { return m_Functions; }
        }
        public object Clone()
        {
            AliasInfo other = (AliasInfo)this.MemberwiseClone();
            other.m_OrigsName = this.m_OrigsName;
            other.m_AliasName = this.m_AliasName;
            other.m_Functions = this.m_Functions;

            return other;
        }
    }
}
