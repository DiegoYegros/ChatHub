// YourDbContext.cs
using Microsoft.EntityFrameworkCore;
using ChatService.Models;
namespace ChatService.Data
{
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        { }
        public DbSet<Message> Message { get; set; }
    }
}