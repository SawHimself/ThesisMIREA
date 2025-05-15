namespace WebApplication3.Models
{
    public class SecretData
    {
        public string Id { get; set; } = null!;
        public string? UserId { get; set; } = null!;
        public string Data { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
