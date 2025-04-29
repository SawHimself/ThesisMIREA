namespace Services.ProcessingTime;

public class RequestTimingInfo
{
    public string Path { get; set; }
    public long DurationMilliseconds { get; set; }
    public DateTime Timestamp { get; set; }
}