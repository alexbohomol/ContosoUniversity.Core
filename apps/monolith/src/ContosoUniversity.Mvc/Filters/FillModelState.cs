namespace ContosoUniversity.Mvc.Filters;

using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.AspNetCore.Mvc.Filters;

internal class FillModelState<TRequest>(IValidator<TRequest> validator) : IAsyncActionFilter where TRequest : class
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestArgument = context.ActionArguments.Single(x => x.Value is TRequest);

        var validationResult = await validator.ValidateAsync((TRequest)requestArgument.Value);

        foreach (var error in validationResult.Errors)
        {
            context.ModelState.AddModelError(
                $"{requestArgument.Key}.{error.PropertyName}",
                error.ErrorMessage);
        }

        await next();
    }
}
