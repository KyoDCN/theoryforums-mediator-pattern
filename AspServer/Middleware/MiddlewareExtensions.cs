using Microsoft.AspNetCore.Builder;

namespace AspServer.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRestExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RestErrorHandler>();
        }
    }
}
