using BlazorAppExternalLogin.Data;
using BlazorAppExternalLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppExternalLogin.Services;

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

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        var query = _context.BlogPosts.AsQueryable();
        IEnumerable<BlogPost>? result = await query.ToListAsync();
        return result;
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