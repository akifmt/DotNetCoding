using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BlazorAppExternalLogin.ViewModels;

public class ApplicationUserViewModel
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} can not be empty.")]
    [StringLength(maximumLength: 120, MinimumLength = 10, ErrorMessage = "{0} must be minimum {2} and maximum {1}.")]
    [EmailAddress(ErrorMessage = "Not valid email format")]
    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string GivenName { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string ProfileImage { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} can not be empty.")]
    [StringLength(maximumLength: 120, MinimumLength = 6, ErrorMessage = "{0} must be minimum {2} and maximum {1}.")]
    public string Password { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} can not be empty.")]
    [Compare("Password", ErrorMessage = "Password does not match.")]
    public string RePassword { get; set; } = string.Empty;

    public IList<string> RoleNames { get; set; } = new List<string>();

    public IList<Claim> Claims { get; set; } = new List<Claim>();
}