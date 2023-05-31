namespace backlogged_api.Helpers
{
    public class PagingParams
    {
        private const int _maxPageSize = 50;
        private int _page = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _page;
            set => _page = (value > _maxPageSize) ? _maxPageSize : value;
        }
    }
}