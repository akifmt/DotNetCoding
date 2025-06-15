using BlazorAppRadzenNet8DataGridInLineEdit.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppRadzenNet8DataGridInLineEdit.Data;

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