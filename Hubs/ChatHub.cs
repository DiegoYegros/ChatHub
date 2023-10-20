using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs;

public class ChatHub : Hub
{
    private readonly string _botUser;
    private readonly IDictionary<string, UserConnection> _connections;
    public ChatHub(IDictionary<string, UserConnection> connections)
    {
        _botUser = "Almighty";
        _connections = connections;
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            await Clients.Group(userConnection.Room)
            .SendAsync("ReceiveMessage", userConnection.User + " " + DateTime.Now.ToString("HH:mm"), message);
        }
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            _connections.Remove(Context.ConnectionId);
            Clients.Group(userConnection.Room)
            .SendAsync("ReceiveMessage", _botUser + " " + DateTime.Now.ToString("HH:mm"), $"{userConnection.User} has left");
            SendConnectedUsers(userConnection.Room);
        }
        return base.OnDisconnectedAsync(exception);
    }
    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        _connections[Context.ConnectionId] = userConnection;
        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser + " " + DateTime.Now.ToString("HH:mm"), $"{userConnection.User} has joined");
        await SendConnectedUsers(userConnection.Room);
    }
    public Task SendConnectedUsers(string room)
    {
        var users = _connections.Values
        .Where(c => c.Room == room)
        .Select(c => c.User);
        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }
}