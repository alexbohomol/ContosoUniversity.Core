namespace ContosoUniversity.Mvc.Validators;

using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc.ModelBinding;

internal static class Extensions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState, string key)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError($"{key}.{error.PropertyName}", error.ErrorMessage);
        }
    }
}
