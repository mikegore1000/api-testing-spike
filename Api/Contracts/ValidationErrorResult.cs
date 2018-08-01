using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Contracts
{
    public class ValidationErrorResult : ObjectResult
    {
        public ValidationErrorResult(ModelStateDictionary modelState) : base(FromModelState(modelState))
        {
            this.StatusCode = 400;
        }

        private static IEnumerable<ValidationError> FromModelState(ModelStateDictionary modelState)
        {
            foreach (var x in modelState)
            foreach (var e in x.Value.Errors)
            {
                string[] errorMessageParts = e.ErrorMessage.Split(':');

                yield return new ValidationError(x.Key, errorMessageParts[0], errorMessageParts[1]);
            }
        }
    }
}