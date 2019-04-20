namespace LibraryAPI.Helpers
{
    using Library.Domain;

    public class AuthorResouceParameter : IAuthorsRepositoryParameters
    {
        const int MaxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                if (_pageSize <= 0 || _pageSize >= MaxPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public string Genre { get; set; }

        public string Search { get; set; }

        public string OrderBy { get; set; } = "Name";

        public bool Descending { get; set; }

        public string Fields { get; set; }
    }
}
