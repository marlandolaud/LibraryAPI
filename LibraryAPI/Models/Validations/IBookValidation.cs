namespace LibraryAPI.Models.Validations
{
    using Library.Contracts.Request.Book;

    public interface IBookValidation: IValidation<BookForCreationDto>, IValidation<BookForUpdateDto>
    {

    }
}