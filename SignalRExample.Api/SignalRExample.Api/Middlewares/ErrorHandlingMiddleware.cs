using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SignalRExample.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly JsonSerializerSettings JsonSerializerSettings;


#pragma warning disable S3963 // "static" fields should be initialized inline
        static ErrorHandlingMiddleware()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var json = JsonConvert.SerializeObject(exception, Formatting.None, JsonSerializerSettings);
            var code = HttpStatusCode.InternalServerError;

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            return context.Response.WriteAsync(json);
        }
    }
}