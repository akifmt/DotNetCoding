using BlazorAppRadzenGoogleMaps.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppRadzenGoogleMaps.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<MapMarker> MapMarkers => Set<MapMarker>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}