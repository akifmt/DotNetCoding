using BlazorAppRadzenNet8SerilogLogging.Components;
using BlazorAppRadzenNet8SerilogLogging.Data;
using BlazorAppRadzenNet8SerilogLogging.Services;
using BlazorAppRadzenNet8SerilogLogging.ViewModels;
using Mapster;
using MapsterMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using System.Reflection;

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

        builder.Services.AddDbContext<ApplicationLoggerDbContext>(options =>
                options.UseSqlite(sqliteLoggerConnectionStringBuilder.ConnectionString)
        );

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseInMemoryDatabase("ConnectionInMemory")
        );
        builder.Services.AddScoped<SeedData>();
        builder.Services.AddScoped<BlogPostService>();
        builder.Services.AddScoped<LoggerService>();

        // Radzen Services
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();

        // Add mapster mapper
        builder.Services.AddMapster();

        builder.Services.CreateDatabase().GetAwaiter().GetResult();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}

public static class ServiceCollectionExtensions
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

public static class MapsterConfiguration
{
    public static void AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        Assembly applicationAssembly = typeof(BaseViewModel<,>).Assembly;
        typeAdapterConfig.Scan(applicationAssembly);

        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
    }
}