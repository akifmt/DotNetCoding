using BlazorAppSerilogLogging.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppSerilogLogging.Data;

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