using System.Collections.Concurrent;
using System.Text.Json;

namespace Services.ProcessingTime
{
    public class RequestTimingService : IRequestTimingService
    {
        private readonly ConcurrentQueue<RequestTimingInfo> _requests = new();
        private readonly string _filePath;
        private readonly int _maxInMemory = 15;

        public RequestTimingService(string filePath)
        {
            _filePath = filePath;
            LoadFromFile();
        }

        public void Add(RequestTimingInfo info)
        {
            _requests.Enqueue(info);

            while (_requests.Count > _maxInMemory)
            {
                _requests.TryDequeue(out _);
            }

            SaveToFileAsync(info);
        }

        public IEnumerable<RequestTimingInfo> GetLatest()
        {
            return _requests.ToArray();
        }

        public async Task<IEnumerable<RequestTimingInfo>> GetAllFromFileAsync()
        {
            if (!File.Exists(_filePath))
                return Enumerable.Empty<RequestTimingInfo>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<RequestTimingInfo>>(json) ?? new List<RequestTimingInfo>();
        }

        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var items = JsonSerializer.Deserialize<List<RequestTimingInfo>>(json) ?? new List<RequestTimingInfo>();
                foreach (var item in items.TakeLast(_maxInMemory))
                {
                    _requests.Enqueue(item);
                }
            }
        }

        private async void SaveToFileAsync(RequestTimingInfo newEntry)
        {
            List<RequestTimingInfo> allEntries;

            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath);
                allEntries = JsonSerializer.Deserialize<List<RequestTimingInfo>>(json) ?? new List<RequestTimingInfo>();
            }
            else
            {
                allEntries = new List<RequestTimingInfo>();
            }

            allEntries.Add(newEntry);

            var updatedJson = JsonSerializer.Serialize(allEntries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, updatedJson);
        }
    }
}