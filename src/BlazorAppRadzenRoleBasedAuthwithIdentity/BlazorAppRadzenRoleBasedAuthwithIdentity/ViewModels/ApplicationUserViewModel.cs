using BlazorAppRadzenRoleBasedAuthwithIdentity.Models.Identity;

namespace BlazorAppRadzenRoleBasedAuthwithIdentity.ViewModels;

public class ApplicationUserViewModel : BaseViewModel<ApplicationUserViewModel, ApplicationUser>
{
    public string Id { get; set; }
    public string ProfileImage { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string CustomTag { get; set; } = String.Empty;

    public string? UserName { get; set; } = String.Empty;
    public string? NormalizedUserName { get; set; } = String.Empty;
    public string? Email { get; set; } = String.Empty;
    public string? NormalizedEmail { get; set; } = String.Empty;
    public bool EmailConfirmed { get; set; }
    public string? PasswordHash { get; set; } = String.Empty;
    public string? SecurityStamp { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = String.Empty;
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }

    public virtual IEnumerable<ApplicationUserClaim>? Claims { get; set; }
    public virtual IEnumerable<ApplicationUserLogin>? Logins { get; set; }
    public virtual IEnumerable<ApplicationUserToken>? Tokens { get; set; }
    public virtual IEnumerable<ApplicationUserRole>? UserRoles { get; set; }
}