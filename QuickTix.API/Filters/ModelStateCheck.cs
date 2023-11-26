using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HouseMate.API.Helpers;

namespace HouseMate.API.Filters
{
    public class ModelStateCheck : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errorMessage = ListModelError(context);

                HttpContext httpContext = context.HttpContext;


                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsJsonAsync(new JsonMessage<string>()
                {
                    status = false,
                    result = errorMessage,
                    status_code = (int)HttpStatusCode.NotImplemented,
                    error_message = "Invalid validation"
                });
                return;
            }
            await next();
        }

        private List<string> ListModelError(ActionContext context) => context.ModelState.SelectMany(x => x.Value.Errors).
                                                                Select(x => x.ErrorMessage).ToList();
    }
}
