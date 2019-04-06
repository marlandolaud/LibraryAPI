namespace LibraryAPI.Helpers
{
    public class AuthorsResourceParameters
    {
        const int MaxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value < MaxPageSize ? value : MaxPageSize;
            }
        }



    }
}
