using Application.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspServer.Middleware
{
    public class RestErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RestErrorHandler> _logger;

        public RestErrorHandler(RequestDelegate next, ILogger<RestErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<RestErrorHandler> logger)
        {
            object errors = null;

            switch (ex)
            {
                case RestException re:
                    logger.LogError(ex, "REST ERROR");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    logger.LogError(ex, "SERVER ERROR");
                    errors = e.Message ?? "Error";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (errors != null) 
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
        }
    }
}
