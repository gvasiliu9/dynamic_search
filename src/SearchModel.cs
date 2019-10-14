using Utmdev.DynamicSearch.Attributes;

namespace Utmdev.DynamicSearch
{
    public class SearchModel
    {
        private int _pageSize = 20;

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
