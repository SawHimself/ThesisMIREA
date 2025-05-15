using System.Diagnostics;
using Services.ProcessingTime;

namespace WebApplication3.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, IRequestTimingService timingService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        var info = new RequestTimingInfo
        {
            Path = context.Request.Path,
            DurationMilliseconds = stopwatch.ElapsedMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        if (!info.Path.Contains("/api/admin"))
        {
            timingService.Add(info);   
        }
    }
}