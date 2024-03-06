using System.Text;
using ChatService;
using ChatService.Data;
using ChatService.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add SignalR services
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
});

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(options => new Dictionary<string, UserConnection>());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "issuer",
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"))
        };
    });
var app = builder.Build();

app.UseRouting();

app.UseCors();

app.MapHub<ChatHub>("/chat");

app.MapGet("/", () => "yeah, its working, now go to /chat");

app.Run();