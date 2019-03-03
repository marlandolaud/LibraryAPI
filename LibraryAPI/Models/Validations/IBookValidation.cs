namespace LibraryAPI.Models.Validations
{
    public interface IBookValidation: IValidation<BookForCreationDto>, IValidation<BookForUpdateDto>
    {

    }
}