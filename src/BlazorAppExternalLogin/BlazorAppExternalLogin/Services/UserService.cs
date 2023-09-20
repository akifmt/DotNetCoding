using AutoMapper;
using BlazorAppExternalLogin.Data;
using BlazorAppExternalLogin.Models;
using BlazorAppExternalLogin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppExternalLogin.Services;

public class UserService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserService(IMapper mapper, ApplicationDbContext context,
                        UserManager<ApplicationUser> userManager,
                        IUserStore<ApplicationUser> userStore,
                        SignInManager<ApplicationUser> signInManager,
                        IEmailSender emailSender,
                        RoleManager<ApplicationRole> roleManager)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _emailSender = emailSender;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser?> GetUserbyIdAsync(string userId)
    {
        return await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<(ApplicationUser? user, ApplicationUserViewModel? userVM)?> GetUserbyUserNameAsync(string userName)
    {
        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null) return null;
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles == null) return null;

        var userVM = _mapper.Map<ApplicationUserViewModel>(user);
        userVM.RoleNames = userRoles;

        foreach (var roleName in userVM.RoleNames)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                // add user s role claims
                var claims = await _roleManager.GetClaimsAsync(role);
                for (int i = 0; i < claims.Count; i++)
                {
                    userVM.Claims.Add(claims[i]);
                }
            }
        }

        return (user, userVM);
    }

    public Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return _context.ApplicationUsers.ToListAsync();
    }

    public async Task<List<ApplicationUserViewModel>> GetAllUserswithRolesandClaimsAsync()
    {
        List<ApplicationUserViewModel> applicationUserViewModels = new List<ApplicationUserViewModel>();
        foreach (var user in _context.ApplicationUsers)
        {
            var appUser = _mapper.Map<ApplicationUserViewModel>(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            // add user roles
            appUser.RoleNames = userRoles;
            foreach (var roleName in appUser.RoleNames)
            {
                var role = _context.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role != null)
                {
                    // add user s role claims
                    var claims = await _roleManager.GetClaimsAsync(role);
                    for (int i = 0; i < claims.Count; i++)
                    {
                        appUser.Claims.Add(claims[i]);
                    }
                }
            }
            applicationUserViewModels.Add(appUser);
        }
        return applicationUserViewModels;
    }

    public async Task<bool> AddUser(ApplicationUser applicationUser)
    {
        await _context.ApplicationUsers.AddAsync(applicationUser);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddUser(ApplicationUser applicationUser, string[] roles, string password)
    {
        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, applicationUser.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, applicationUser.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            foreach (var role in roles)
                await _userManager.AddToRoleAsync(user, role);

            return true;
        }

        return false;
    }

    public async Task<bool> UpdateUser(string id, ApplicationUser applicationUser, string password)
    {
        var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
        if (user != null)
        {
            user.Name = applicationUser.Name;
            user.GivenName = applicationUser.GivenName;
            user.Surname = applicationUser.Surname;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool?> DeleteById(string id)
    {
        ApplicationUser? user = await _context.ApplicationUsers.FindAsync(id);
        if (user == null)
            return null;

        try
        {
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> CheckUserPassword(ApplicationUser user, string password)
    {
        bool result = await _userManager.CheckPasswordAsync(user, password);
        if (result)
        {
            return true;
        }
        return false;
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}