namespace Library.Domain
{
    public interface IRepositorySorter
    {
        string OrderBy { get; set; }

        bool Descending { get; set; }
    }
}
