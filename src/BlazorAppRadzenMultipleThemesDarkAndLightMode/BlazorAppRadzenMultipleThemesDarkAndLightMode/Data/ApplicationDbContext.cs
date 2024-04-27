using BlazorAppRadzenMultipleThemesDarkAndLightMode.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppRadzenMultipleThemesDarkAndLightMode.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}