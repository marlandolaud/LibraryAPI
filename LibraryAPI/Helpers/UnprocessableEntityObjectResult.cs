namespace LibraryAPI.Helpers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;

    public class UnprocessableEntityObjectResult : ObjectResult
    {
        /// <summary>
        /// returns 422 unprocessable Entity ObjectResult
        /// </summary>
        /// <param name="modelState"></param>
        public UnprocessableEntityObjectResult(ModelStateDictionary modelState) 
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = 422;
        }
    }
}
