using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace BlazorAppRadzenAuthIdentityAutoLogoutTimer.Services;

public class AuthCheckService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CookieAuthenticationOptions _options;

    public AuthCheckService(IHttpContextAccessor httpContextAccessor, IOptions<CookieAuthenticationOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public Task<TimeSpan?> GetAuthExpiration()
    {
        TimeSpan? timeSpan = null;
        string? cookie = _httpContextAccessor?.HttpContext?.Request.Cookies[Program.COOKIE_NAME];
        if (cookie is null)
            return Task.FromResult(timeSpan);

        IDataProtectionProvider? provider = _options.DataProtectionProvider;
        if (provider is null)
            return Task.FromResult(timeSpan);

        IDataProtector protector = provider.CreateProtector(
            "Microsoft.AspNetCore.Authentication.Cookies." +
            "CookieAuthenticationMiddleware",
            "Identity.Application",
            "v2");

        TicketDataFormat format = new TicketDataFormat(protector);
        AuthenticationTicket? authTicket = format.Unprotect(cookie);
        if (authTicket is null)
            return Task.FromResult(timeSpan);

        AuthenticationProperties property = authTicket.Properties;
        DateTimeOffset? expiresUtc = property.ExpiresUtc;

        timeSpan = expiresUtc - DateTimeOffset.UtcNow;

        return Task.FromResult(timeSpan);
    }
}