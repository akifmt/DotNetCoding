using BlazorAppRadzenRoleBasedAuthwithIdentity.Data;
using BlazorAppRadzenRoleBasedAuthwithIdentity.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace BlazorAppRadzenRoleBasedAuthwithIdentity.Services;

public class ApplicationUserService
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ApplicationUser?> GetbyId(string id)
    {
        return _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<(IEnumerable<ApplicationUser> Result, int TotalCount)> GetUsersAsync(string? filter = default, int? top = default, int? skip = default, string? orderby = default, string? expand = default, string? select = default, bool? count = default)
    {
        var query = _context.Users.AsQueryable();

        query = query.Include(user => user.UserRoles);
        query = query.Include("UserRoles.Role");

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(orderby))
            query = query.OrderBy(orderby);

        int totalCount = 0;
        if (count == true)
            totalCount = query.Count();

        IEnumerable<ApplicationUser>? result;
        if (skip == null || top == null)
            result = await query.ToListAsync();
        else
            result = await query.Skip(skip.Value).Take(top.Value).ToListAsync();

        return (result, totalCount);
    }
}