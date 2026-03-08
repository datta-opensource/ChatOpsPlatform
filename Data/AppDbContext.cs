using Microsoft.EntityFrameworkCore;
using SwiftLux.WhatsApp.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations { get; set; }

    public DbSet<MessageLog> Messages { get; set; }
}