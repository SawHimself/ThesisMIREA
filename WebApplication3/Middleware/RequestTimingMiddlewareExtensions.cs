using Microsoft.AspNetCore.Builder;
using WebApplication3.Middleware;

namespace YourAppNamespace.Middleware
{
    public static class RequestTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimingMiddleware>();
        }
    }
}