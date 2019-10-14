using System;

namespace Utmdev.DynamicSearch.Attributes
{
    /// <summary>
    /// Use this attribute to specify default property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DefaultValueAttribute : Attribute
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
