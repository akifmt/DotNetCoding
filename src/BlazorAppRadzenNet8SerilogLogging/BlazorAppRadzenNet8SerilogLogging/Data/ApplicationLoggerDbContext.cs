using BlazorAppRadzenNet8SerilogLogging.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppRadzenNet8SerilogLogging.Data;

public class ApplicationLoggerDbContext : DbContext
{
    public ApplicationLoggerDbContext(DbContextOptions<ApplicationLoggerDbContext> options)
    : base(options)
    {
    }

    public DbSet<Log> Logs => Set<Log>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}