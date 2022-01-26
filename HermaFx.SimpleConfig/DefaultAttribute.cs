using System;

namespace HermaFx.SimpleConfig
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultAttribute : Attribute
    {
// ReSharper disable MemberCanBePrivate.Global
        public object DefaultValue { get; set; }
// ReSharper restore MemberCanBePrivate.Global

        public DefaultAttribute():this(null)
        {
        }

        private DefaultAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public virtual object Default()
        {
            return DefaultValue;
        }
    }
}
