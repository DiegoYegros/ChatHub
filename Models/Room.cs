namespace ChatService;

public class Room
{
    public required String Id { get; set; }
    public required String Name { get; set; }
    public required int ConnectedUsers { get; set; }
}