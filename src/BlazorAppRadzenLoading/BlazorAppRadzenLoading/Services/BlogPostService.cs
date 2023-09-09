using BlazorAppRadzenLoading.Data;
using BlazorAppRadzenLoading.Models;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace BlazorAppRadzenLoading.Services;

public class BlogPostService
{
    private readonly ApplicationDbContext _context;

    public BlogPostService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<BlogPost?> GetbyId(int id)
    {
        return _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<(IEnumerable<BlogPost> Result, int TotalCount)> GetBlogPostsAsync(string? filter = default, int? top = default, int? skip = default, string? orderby = default, string? expand = default, string? select = default, bool? count = default)
    {
        var query = _context.BlogPosts.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(orderby))
            query = query.OrderBy(orderby);

        int totalCount = 0;
        if (count == true)
            totalCount = query.Count();

        IEnumerable<BlogPost>? result;
        if (skip == null || top == null)
            result = await query.ToListAsync();
        else
            result = await query.Skip(skip.Value).Take(top.Value).ToListAsync();

        return (result, totalCount);
    }

    public async Task<bool> AddBlogPostAsync(BlogPost blogPost)
    {
        try
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateBlogPostAsync(int id, BlogPost blogPost)
    {
        try
        {
            var oldBlogPost = _context.BlogPosts.FirstOrDefault(x => x.Id == id);
            if (oldBlogPost == null) return false;

            oldBlogPost.Title = blogPost.Title;
            oldBlogPost.Content = blogPost.Content;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeletebyIdAsync(int id)
    {
        var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
        if (blogPost == null)
            return false;

        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();

        return true;
    }
}