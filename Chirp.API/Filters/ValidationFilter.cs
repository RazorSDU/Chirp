using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chirp.API.Filters
{
    /// <summary>
    /// Converts <see cref="ModelStateDictionary"/> errors into HTTP 400 responses.
    /// Registered globally in <c>Program.cs</c>.
    /// </summary>
    public sealed class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value!.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value!.Errors.Select(e => new { field = kvp.Key, error = e.ErrorMessage }));

                context.Result = new BadRequestObjectResult(new { errors });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

