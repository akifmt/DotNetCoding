using AutoMapper;
using BlazorAppAuth.Areas.Identity;
using BlazorAppAuth.Data;
using BlazorAppAuth.Models;
using BlazorAppAuth.Services;
using BlazorAppAuth.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ConnectionInMemory")
         );

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = false;

            // Default Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;

            // Default SignIn settings.
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // Default User settings.
            options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.Cookie.Name = "BlazorAppAuth";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/Identity/Account/Login";
            // ReturnUrlParameter requires
            //using Microsoft.AspNetCore.Authentication.Cookies;
            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.SlidingExpiration = true;
        });

        builder.Services.Configure<PasswordHasherOptions>(option =>
        {
            option.IterationCount = 12000;
        });

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoles<ApplicationRole>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        builder.Services.AddScoped<SeedData>();
        builder.Services.AddScoped<BlogPostService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<RoleService>();

        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy(ProjectPolicies.BlogPostCreateClaimPolicy.Name, policy =>
            {
                foreach (var claim in ProjectPolicies.BlogPostCreateClaimPolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(ProjectPolicies.BlogPostReadClaimPolicy.Name, policy =>
            {
                foreach (var claim in ProjectPolicies.BlogPostReadClaimPolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(ProjectPolicies.BlogPostUpdateClaimPolicy.Name, policy =>
            {
                foreach (var claim in ProjectPolicies.BlogPostUpdateClaimPolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
            opts.AddPolicy(ProjectPolicies.BlogPostDeleteClaimPolicy.Name, policy =>
            {
                foreach (var claim in ProjectPolicies.BlogPostDeleteClaimPolicy.RequiredClaims)
                    policy.RequireClaim(claim.Type, new[] { claim.Value });
            });
        });

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            var profile = new MappingProfile();
            configuration.AddProfile(profile);
        });
        var mapper = mapperConfiguration.CreateMapper();
        builder.Services.AddSingleton(mapper);

        var app = builder.Build();

        builder.Services.CreateDatabase().GetAwaiter().GetResult();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
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
        CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
        CreateMap<BlogPost, BlogPostViewModel>().ReverseMap();
    }
}