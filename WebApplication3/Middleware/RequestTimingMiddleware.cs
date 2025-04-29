using Services.ProcessingTime;
using System.Diagnostics;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRequestTimingService _timingService;

    public RequestTimingMiddleware(RequestDelegate next, IRequestTimingService timingService)
    {
        _next = next;
        _timingService = timingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var info = new RequestTimingInfo
        {
            Path = context.Request.Path,
            DurationMilliseconds = stopwatch.ElapsedMilliseconds,
            Timestamp = DateTime.UtcNow
        };

        _timingService.Add(info);
    }
}