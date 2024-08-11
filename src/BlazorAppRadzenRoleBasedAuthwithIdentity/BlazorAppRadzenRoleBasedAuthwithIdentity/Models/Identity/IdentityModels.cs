using Microsoft.AspNetCore.Identity;

namespace BlazorAppRadzenRoleBasedAuthwithIdentity.Models.Identity;

public class ApplicationUser : IdentityUser<string>
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
    }

    public override string Id { get; set; }
    public string ProfileImage { get; set; } = String.Empty;

    [PersonalData]
    public string FirstName { get; set; } = String.Empty;

    [PersonalData]
    public string LastName { get; set; } = String.Empty;

    public string CustomTag { get; set; } = String.Empty;

    public virtual IEnumerable<ApplicationUserClaim>? Claims { get; set; }
    public virtual IEnumerable<ApplicationUserLogin>? Logins { get; set; }
    public virtual IEnumerable<ApplicationUserToken>? Tokens { get; set; }
    public virtual IEnumerable<ApplicationUserRole>? UserRoles { get; set; }
}

public class ApplicationRole : IdentityRole<string>
{
    public ApplicationRole()
    {
        Id = Guid.NewGuid().ToString();
    }

    public override string Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public virtual IEnumerable<ApplicationUserRole>? UserRoles { get; set; }
    public virtual IEnumerable<ApplicationRoleClaim>? RoleClaims { get; set; }
}

public class ApplicationUserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser? User { get; set; }
    public virtual ApplicationRole? Role { get; set; }
}

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public virtual ApplicationUser? User { get; set; }
}

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser? User { get; set; }
}

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole? Role { get; set; }
}

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser? User { get; set; }
}