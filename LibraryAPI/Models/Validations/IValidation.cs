namespace LibraryAPI.Models.Validations
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public interface IValidation<TModel>
    {
        void Validate(TModel model, ModelStateDictionary modelState);
    }
}