using System;
using System.Collections.Generic;
using System.Text;

namespace ST.FilterExtension.Attributes
{
    /// <summary>
    /// Use this attribute to specify default property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DefaultValueAttribute: Attribute
    {
        private object defaultValue;

        public DefaultValueAttribute(object defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public object DefaultValue
        {
            get => defaultValue;
        }
    }
}
