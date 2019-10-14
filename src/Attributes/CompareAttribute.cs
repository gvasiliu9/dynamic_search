using System;
using static Utmdev.DynamicSearch.Attributes.Enums;

namespace Utmdev.DynamicSearch.Attributes
{
    /// <summary>
    /// Use this attribute to specify compare type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompareAttribute : Attribute
    {
        private CompareType compareType;

        public CompareAttribute(CompareType compareType)
        {
            this.compareType = compareType;
        }

        public CompareType CompareType
        {
            get => compareType;
        }
    }
}
