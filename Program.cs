using ChatService;
using ChatService.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(options => new Dictionary<string, UserConnection>());
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://chathub.diegoyegros.com")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/chat");
app.UseCors();
app.Run();
