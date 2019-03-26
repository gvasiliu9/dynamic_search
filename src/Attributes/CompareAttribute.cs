using System;
using System.Collections.Generic;
using System.Text;
using static ST.FilterExtension.Attributes.Enums;

namespace ST.FilterExtension.Attributes
{
    /// <summary>
    /// Use this attribute to specify compare type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompareAttribute:  Attribute
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
