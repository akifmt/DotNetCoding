using BlazorApp.Components;
using BlazorApp.Services;
using System.Text.Json;

internal class Program
{
    public const string API_URL = "http://localhost:5000";

    public static readonly JsonSerializerOptions TodoApiJsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient("TodoApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri(API_URL);
        });

        // Add services to the container.
        builder.Services.AddScoped<TodoClientService>();
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