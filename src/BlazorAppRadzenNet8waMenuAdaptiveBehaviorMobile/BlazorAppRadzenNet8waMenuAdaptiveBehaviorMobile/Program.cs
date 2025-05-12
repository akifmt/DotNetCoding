using BlazorAppRadzenNet8waMenuAdaptiveBehaviorMobile;
using BlazorAppRadzenNet8waMenuAdaptiveBehaviorMobile.Services;
using BlazorAppRadzenNet8waMenuAdaptiveBehaviorMobile.ViewModels;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Add services to the container.
        builder.Services.AddScoped<BlogPostService>();

        // Radzen Services
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();

        // Add mapster mapper
        builder.Services.AddMapster();

        builder.Services.AddRadzenComponents();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
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