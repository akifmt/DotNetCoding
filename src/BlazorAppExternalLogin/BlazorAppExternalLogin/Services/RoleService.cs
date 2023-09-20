using BlazorAppExternalLogin.Data;
using BlazorAppExternalLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppExternalLogin.Services;

public class RoleService
{
    private readonly ApplicationDbContext _context;

    public RoleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<ApplicationRole>> GetAllRolesAsync()
    {
        return _context.ApplicationRoles.ToListAsync();
    }

    public Task<List<ApplicationRole>> GetAllRoleswithClaimsAsync()
    {
        return _context.ApplicationRoles.Include(x => x.RoleClaims).ToListAsync();
    }
}