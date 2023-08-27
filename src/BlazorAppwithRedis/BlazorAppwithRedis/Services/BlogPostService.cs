using BlazorAppwithRedis.Cache;
using BlazorAppwithRedis.Data;
using BlazorAppwithRedis.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppwithRedis.Services;

public class BlogPostService
{
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private static object _lock = new object();

    public BlogPostService(ApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<(BlogPost? Data, string From)> GetbyIdAsync(int id)
    {
        BlogPost? filteredData;
        var data = _cacheService.GetDataSlave<IEnumerable<BlogPost>>("blogpost");
        if (data is not null)
        {
            filteredData = data.Where(x => x.Id == id).FirstOrDefault();
            return (Data: filteredData, From: "cache");
        }
        data = await SetBlogPostData();
        filteredData = data.Where(x => x.Id == id).FirstOrDefault();
        return (Data: filteredData, From: "database");
    }

    public async Task<(IEnumerable<BlogPost> Data, string From)> GetAllAsync()
    {
        var data = _cacheService.GetDataSlave<IEnumerable<BlogPost>>("blogpost");
        if (data is not null)
            return (Data: data, From: "cache");
        data = await SetBlogPostData();
        return (Data: data, From: "database");
    }

    public async Task<bool> AddBlogPostAsync(BlogPost blogPost)
    {
        try
        {
            var result = await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            _cacheService.RemoveDataMaster("blogpost");
            await SetBlogPostData();
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
            var oldBlogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (oldBlogPost == null) return false;
            oldBlogPost.Title = blogPost.Title;
            oldBlogPost.Content = blogPost.Content;
            await _context.SaveChangesAsync();
            _cacheService.RemoveDataMaster("blogpost");
            await SetBlogPostData();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeletebyIdAsync(int id)
    {
        try
        {
            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost is null)
                return false;
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            _cacheService.RemoveDataMaster("blogpost");
            await SetBlogPostData();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    private async Task<IEnumerable<BlogPost>> SetBlogPostData()
    {
        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        var data = await _context.BlogPosts.ToListAsync();
        lock (_lock)
        {
            _cacheService.SetDataMaster<IEnumerable<BlogPost>>("blogpost", data, expirationTime);
        }
        return data;
    }
}