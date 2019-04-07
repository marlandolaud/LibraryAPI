namespace Library.Domain
{
    public interface IRepositoryPager
    {
        int PageSize { get; set; }

        int PageNumber { get; set; }
    }
}