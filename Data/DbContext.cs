// YourDbContext.cs
using Microsoft.EntityFrameworkCore;
using ChatHub.Models;
namespace ChatHub.Data
{
    public class SqlServerDbContext : DbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        { }
        public DbSet<Message> Message { get; set; }
    }
}