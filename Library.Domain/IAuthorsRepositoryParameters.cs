﻿namespace Library.Domain
{
    public interface IAuthorsRepositoryParameters : IRepositoryPager, IAuthorRepositoryFilter, IRepositorySearch, IRepositorySorter
    {
    }
}