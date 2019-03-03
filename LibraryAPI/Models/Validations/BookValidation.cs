namespace LibraryAPI.Models.Validations
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class BookValidation : IBookValidation
    {
        public void Validate(BookForCreationDto book, ModelStateDictionary modelState)
        {
            TitleAndDescriptionNotSame(book, modelState);
        }

        public void Validate(BookForUpdateDto book, ModelStateDictionary modelState)
        {
            TitleAndDescriptionNotSame(book, modelState);
        }

        private void TitleAndDescriptionNotSame(BookForManipulationDto book, ModelStateDictionary modelState)
        {
            if (book.Description == book.Title)
            {
                modelState.AddModelError(nameof(BookForCreationDto), "title can not be the same as description");
            }
        }
    }
}
