using ST.FilterExtension.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ST.FilterExtension
{
    public class BaseSearchViewModel
    {
        private int _pageSize = 21;

        [Exclude]
        public int Page { get; set; } = 1;

        [Exclude]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 100) ? 100 : value;
        }
    }
}
