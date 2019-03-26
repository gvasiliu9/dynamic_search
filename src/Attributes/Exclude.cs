using System;
using System.Collections.Generic;
using System.Text;

namespace ST.FilterExtension.Attributes
{
    /// <summary>
    /// Use this attribute to exclude a property from search
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExcludeAttribute: Attribute
    {
    }
}
