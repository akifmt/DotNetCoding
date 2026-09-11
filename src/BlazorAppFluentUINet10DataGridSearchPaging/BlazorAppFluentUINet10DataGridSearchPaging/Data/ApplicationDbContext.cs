using BlazorAppFluentUINet10DataGridSearchPaging.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppFluentUINet10DataGridSearchPaging.Data;

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