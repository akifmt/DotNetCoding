using Microser.BlazorAppClient;
using Microser.BlazorAppClient.Components;
using Microser.BlazorAppClient.HttpHandlers;
using Microser.BlazorAppClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddTransient<AuthenticationDelegatingHandler>();
        builder.Services.AddScoped<IWeatherForecastApiService, WeatherForecastApiService>();

        builder.Services.AddOIDCAuthentication();

        builder.Services.AddHttpClients();

        builder.Services.AddPolicies();

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapPost("/account/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        });

        app.MapGet("/account/login", async (string redirectUri, HttpContext context) =>
        {
            await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUri });
        });

        app.Run();
    }
}

public static class StartupExtensions
{
    public static void AddOIDCAuthentication(this IServiceCollection services)
    {
        services
            .AddAntiforgery(options => options.Cookie.Name = "ClientBlazorAppAntiForgeryCookie")
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
        .AddCookie(options =>
        {
            options.Cookie.Name = "ClientBlazorAppAuthCookie";
            options.AccessDeniedPath = "/AccessDenied";
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = "https://localhost:5001/";
            options.ClientId = "dotnet_blazor_serverapp";
            options.ClientSecret = "E8C65E41BB0E4E519D409023CF5112F4";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.UseTokenLifetime = false;

            //options.SignedOutRedirectUri = "/";

            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("address");
            options.Scope.Add("email");
            options.Scope.Add("roles");

            options.Scope.Add("microser_api_weather");

            options.ClaimActions.MapJsonKey("role", "role");
            //options.ClaimActions.MapUniqueJsonKey("role", "role");
            //options.ClaimActions.MapAll();

            options.TokenValidationParameters = new
                TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            options.Events = new OpenIdConnectEvents
            {
                OnAccessDenied = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("WeatherForecastAPIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:6001/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        // added for get user info
        services.AddHttpClient("IDPClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        });
    }

    public static void AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(nameof(ProjectPolicies.UserRolePolicy), policy =>
            {
                foreach (var role in ProjectPolicies.UserRolePolicy.RequiredRoles)
                    policy.RequireRole(role.RoleName);
            });
            opts.AddPolicy(nameof(ProjectPolicies.IdentityAdminRolePolicy), policy =>
            {
                foreach (var role in ProjectPolicies.IdentityAdminRolePolicy.RequiredRoles)
                    policy.RequireRole(role.RoleName);
            });
            opts.AddPolicy(nameof(ProjectPolicies.AdminRolePolicy), policy =>
            {
                foreach (var role in ProjectPolicies.AdminRolePolicy.RequiredRoles)
                    policy.RequireRole(role.RoleName);
            });
        });

        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(nameof(ProjectPolicies.WeatherForecastCreatePolicy), policy =>
            {
                foreach (var claim in ProjectPolicies.WeatherForecastCreatePolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(nameof(ProjectPolicies.WeatherForecastReadPolicy), policy =>
            {
                foreach (var claim in ProjectPolicies.WeatherForecastReadPolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(nameof(ProjectPolicies.WeatherForecastUpdatePolicy), policy =>
            {
                foreach (var claim in ProjectPolicies.WeatherForecastUpdatePolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(nameof(ProjectPolicies.WeatherForecastDeletePolicy), policy =>
            {
                foreach (var claim in ProjectPolicies.WeatherForecastDeletePolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
        });
    }
}