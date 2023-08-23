using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuickTix.Core.Entities;

namespace QuickTix.Service.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class ModelStateCheck : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errorMessage = ListModelError(context);

                HttpContext httpContext = context.HttpContext;


                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsJsonAsync(new ResponseModel()
                {
                    StatusCode = 500,
                    Message = errorMessage,
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
