using AutoMapper;
using BlazorAppRadzenMultipleThemesDarkAndLightMode;
using BlazorAppRadzenMultipleThemesDarkAndLightMode.Components;
using BlazorAppRadzenMultipleThemesDarkAndLightMode.Data;
using BlazorAppRadzenMultipleThemesDarkAndLightMode.Models;
using BlazorAppRadzenMultipleThemesDarkAndLightMode.Services;
using BlazorAppRadzenMultipleThemesDarkAndLightMode.ViewModels;
using Microsoft.EntityFrameworkCore;
using Radzen;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseInMemoryDatabase("ConnectionInMemory")
        );
        builder.Services.AddScoped<SeedData>();
        builder.Services.AddScoped<BlogPostService>();

        // Radzen Services
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();


        // ADDED FOR THEME SUPPORT
        builder.Services.AddScoped<ThemeService>();


        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            var profile = new MappingProfile();
            configuration.AddProfile(profile);
        });
        var mapper = mapperConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.CreateDatabase().GetAwaiter().GetResult();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

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

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, BlogPostViewModel>().ReverseMap();
    }
}