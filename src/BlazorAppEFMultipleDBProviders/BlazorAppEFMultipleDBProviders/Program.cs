using AutoMapper;
using BlazorAppEFMultipleDBProviders.Data;
using BlazorAppEFMultipleDBProviders.Data.Models;
using BlazorAppEFMultipleDBProviders.Models;
using BlazorAppEFMultipleDBProviders.Services;
using Microsoft.EntityFrameworkCore;
using static BlazorAppEFMultipleDBProviders.Provider;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Set the active provider via configuration
        var configuration = builder.Configuration;
        var provider = configuration.GetValue("DatabaseProvider", string.Empty);
        if (!string.IsNullOrEmpty(provider))
        {
            var connectionString = configuration.GetValue($"ConnectionStrings:{provider}", string.Empty);
            builder.Services.ConfigureServices(provider, connectionString);

            if (provider == "InMemory")
                builder.Services.CheckAndCreateDatabase(isInMemoryDatabase: true).GetAwaiter().GetResult();
        }

        // Add services to the container.
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var app = builder.Build();

        // this seeding is only for the template to bootstrap the DB and users.
        // in production you will likely want a different approach.
        if (args.Contains("/seed"))
        {
            Console.WriteLine("seeding...");
            // DATABASE CREATE AND INITIAL DATA
            builder.Services.CheckAndCreateDatabase().GetAwaiter().GetResult();

            return;
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}

public static class ServiceCollectionExtensitions
{
    public static void ConfigureServices(this IServiceCollection services, string provider, string connectionstring)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => _ = provider switch
            {
                "InMemory" => options.UseInMemoryDatabase("ConnectionInMemory"),
                "Sqlite" => options.UseSqlite(connectionstring,
                    x => x.MigrationsAssembly(Sqlite.Assembly)),
                "SqlServer" => options.UseSqlServer(connectionstring,
                x => x.MigrationsAssembly(SqlServer.Assembly)),

                _ => throw new Exception($"Unsupported provider: {provider}")
            });

        services.AddScoped<SeedData>();
        services.AddScoped<BlogPostService>();

        services.AddAutoMapper(typeof(Program));

        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            var profile = new MappingProfile();
            configuration.AddProfile(profile);
        });
        var mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);
    }

    public static async Task CheckAndCreateDatabase(this IServiceCollection services, bool isInMemoryDatabase = false)
    {
        using (IServiceScope tmp = services.BuildServiceProvider().CreateScope())
        {
            await using var _context = tmp.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!isInMemoryDatabase)
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    // apply migrations
                    await _context.Database.MigrateAsync();
                    await _context.SaveChangesAsync();
                }
            }

            var seedData = tmp.ServiceProvider.GetRequiredService<SeedData>();
            // add initial data
            await seedData.CreateInitialData();
        }
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, BlogPostViewModel>().ReverseMap();
    }
}