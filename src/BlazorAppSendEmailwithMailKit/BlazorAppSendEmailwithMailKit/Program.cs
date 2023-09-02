using BlazorAppSendEmailwithMailKit.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //builder.Services.AddTransient<IEmailSender, EmailSenderSystem>();
        builder.Services.AddTransient<IEmailSender, EmailSenderMailKit>();
        builder.Services.Configure<EmailConfiguration>(options =>
        {
            builder.Configuration.GetSection("Email").Bind(options);

            //options.Host = builder.Configuration.GetValue<string>("Email:Host");
            //options.Port = builder.Configuration.GetValue<int>("Email:Port");
            //options.Username = builder.Configuration.GetValue<string>("Email:Username");
            //options.Password = builder.Configuration.GetValue<string>("Email:Password");
            //options.From = builder.Configuration.GetValue<string>("Email:From");
            //options.Name = builder.Configuration.GetValue<string>("Email:Name");
            //options.EnableSSL = builder.Configuration.GetValue<bool>("Email:EnableSSL");
        });

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        var app = builder.Build();

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