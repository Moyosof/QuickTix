using Microsoft.AspNetCore.Http.Extensions;
using QuickTix.API.Helpers;
using System.Net;

namespace QuickTix.API.Extensions
{
    public class ExceptionMiddlewareExtension
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddlewareExtension(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                var sql = httpContext.RequestServices.GetService<ISqlDBObjects>();
                await sql.StoredProcedures.LogSystemErrorAsync(e.Message, httpContext.Request.GetEncodedPathAndQuery());

                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsJsonAsync(new JsonMessage<string>()
                {
                    status = false,
                    status_code = (int)HttpStatusCode.InternalServerError,
                    error_message = ResponseMessages.InternalServerError
                });
            }
        }
    }
}
