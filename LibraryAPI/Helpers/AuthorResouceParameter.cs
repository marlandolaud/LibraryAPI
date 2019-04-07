using Library.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Helpers
{
    public class AuthorResouceParameter : IRepositoryPager
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
    }
}
