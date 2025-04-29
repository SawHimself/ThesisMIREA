namespace Services.ProcessingTime;

public interface IRequestTimingService
{
    void Add(RequestTimingInfo info);
    IEnumerable<RequestTimingInfo> GetLatest();
    Task<IEnumerable<RequestTimingInfo>> GetAllFromFileAsync();
}