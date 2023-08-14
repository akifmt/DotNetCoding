using AutoMapper;
using BlazorAppSerilogLogging.Data;
using BlazorAppSerilogLogging.Models;
using BlazorAppSerilogLogging.Services;
using BlazorAppSerilogLogging.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var currentDir = Directory.GetCurrentDirectory();

        // get connection string from configuration file (appsettings.json)
        string? sqliteLoggerConnectionString = builder.Configuration.GetConnectionString("SqliteLogger");
        SqliteConnectionStringBuilder sqliteLoggerConnectionStringBuilder = new SqliteConnectionStringBuilder(sqliteLoggerConnectionString);
        sqliteLoggerConnectionStringBuilder.DataSource = Path.Combine(currentDir, sqliteLoggerConnectionStringBuilder.DataSource);
        string sqliteDbFilePath = sqliteLoggerConnectionStringBuilder.DataSource;

        // file logger path
        string serilogFileLoggerFilePath = Path.Combine(currentDir, "LogsFolder", "logs.log");

        builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Information()
            //.WriteTo.Console(new JsonFormatter(), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo.Seq("http://localhost:5001", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo.File(serilogFileLoggerFilePath,
                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
                            rollingInterval: RollingInterval.Hour,
                            encoding: System.Text.Encoding.UTF8)
            .WriteTo.SQLite(sqliteDbFilePath,
                            tableName: "Logs",
                            restrictedToMinimumLevel:
                                builder.Environment.IsDevelopment() ? Serilog.Events.LogEventLevel.Information : Serilog.Events.LogEventLevel.Warning,
                            storeTimestampInUtc: false,
                            batchSize:
                                builder.Environment.IsDevelopment() ? (uint)1 : (uint)100,
                            retentionPeriod: new TimeSpan(0, 1, 0, 0, 0),
                            maxDatabaseSize: 10)
        );

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationLoggerDbContext>(options =>
                options.UseSqlite(sqliteLoggerConnectionStringBuilder.ConnectionString)
        );

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ConnectionInMemory")
         );

        builder.Services.AddScoped<SeedData>();
        builder.Services.AddScoped<BlogPostService>();
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            var profile = new MappingProfile();
            configuration.AddProfile(profile);
        });
        var mapper = mapperConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.CreateDatabase().GetAwaiter().GetResult();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddScoped<LoggerService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();

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
    public static async Task CreateDatabase(this IServiceCollection services)
    {
        using (IServiceScope tmp = services.BuildServiceProvider().CreateScope())
        {
            await using var _context = tmp.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedData = tmp.ServiceProvider.GetRequiredService<SeedData>();
            await seedData.CreateInitialData();
        }
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, BlogPostViewModel>().ReverseMap();
        CreateMap<BlazorAppSerilogLogging.Models.Log, LogViewModel>().ReverseMap();
    }
}