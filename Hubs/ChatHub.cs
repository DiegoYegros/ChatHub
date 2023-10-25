using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs;

public class ChatHub : Hub
{
    private readonly string _botUser;
    private readonly IDictionary<string, UserConnection> _connections;

    private const string LOBBY_GROUP_NAME = "LobbyGroup";

    public ChatHub(IDictionary<string, UserConnection> connections)
    {
        _botUser = "Almighty";
        _connections = connections;
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {
            Message message1 = new Message();
            message1.Content = message;
            message1.Instant = DateTime.UtcNow.ToString("o");
            message1.ConnectionId = Context.ConnectionId;
            await Clients.Group(userConnection.Room)
            .SendAsync("ReceiveMessage", userConnection.User, message1);
        }
    }
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, LOBBY_GROUP_NAME);
        var rooms = _connections.Values.GroupBy(c => c.Room).ToList();
        await base.OnConnectedAsync();
        await Clients.Group(LOBBY_GROUP_NAME).SendAsync("RoomsAndAmountOfPeople", rooms);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
        {

            Message message1 = new Message();
            message1.Content = $"{userConnection.User} has left";
            message1.Instant = DateTime.UtcNow.ToString("o");
            message1.ConnectionId = Context.ConnectionId;
            _connections.Remove(Context.ConnectionId);
            Groups.RemoveFromGroupAsync(Context.ConnectionId, LOBBY_GROUP_NAME);
            Clients.Group(userConnection.Room)
            .SendAsync("ReceiveMessage", _botUser, message1);
            SendConnectedUsers(userConnection.Room);
        }
        base.OnDisconnectedAsync(exception);
        return UpdateClientsInLobbyWithRoomList();
    }
    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, LOBBY_GROUP_NAME);
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        Message message1 = new Message();
        message1.Content = $"{userConnection.User} has joined";
        message1.Instant = DateTime.UtcNow.ToString("o");
        message1.ConnectionId = Context.ConnectionId;
        _connections[Context.ConnectionId] = userConnection;
        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, message1);
        await SendConnectedUsers(userConnection.Room);
        await UpdateClientsInLobbyWithRoomList();
    }
    public async Task UpdateClientsInLobbyWithRoomList()
    {
        var rooms = _connections.Values.GroupBy(c => c.Room).ToList();
        await Clients.Group(LOBBY_GROUP_NAME).SendAsync("RoomsAndAmountOfPeople", rooms);
    }
    public Task SendConnectedUsers(string room)
    {
        var users = _connections.Values
        .Where(c => c.Room == room)
        .Select(c => c.User);
        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }

}