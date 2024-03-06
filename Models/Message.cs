namespace ChatService.Models;

public class Message
{
    public string Content { get; set; }
    public string Instant { get; set; }
    public string ConnectionId { get; set; }
    public string ImageData { get; set; }
}