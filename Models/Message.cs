namespace ChatHub.Models;

public class Message
{
    public string? Content { get; set; }
    public required string Instant { get; set; }
    public required string ConnectionId { get; set; }
    public string? ImageData { get; set; }
}