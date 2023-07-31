﻿using BlazorAppEFMultipleDBProviders.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppEFMultipleDBProviders.Data;

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