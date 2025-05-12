using BlazorAppRadzenNet8waMenuAdaptiveBehaviorMobile.Models;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net.Http.Json;

namespace BlazorAppRadzenNet8waMenuAdaptiveBehaviorMobile.Services;

public class BlogPostService
{
    private readonly HttpClient _httpClient = null!;

    public BlogPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BlogPost?> GetbyId(int id)
    {
        var blogPosts = await _httpClient.GetFromJsonAsync<BlogPost[]?>("sample-data/blogposts.json");
        return blogPosts?.FirstOrDefault(x => x.Id == id);
    }

    public async Task<(IEnumerable<BlogPost> Result, int TotalCount)> GetBlogPostsAsync(string? filter = default, int? top = default, int? skip = default, string? orderby = default, string? expand = default, string? select = default, bool? count = default)
    {
        var blogPosts = await _httpClient.GetFromJsonAsync<BlogPost[]?>("sample-data/blogposts.json");

        var query = blogPosts?.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(orderby))
            query = query.OrderBy(orderby);

        int totalCount = 0;
        if (count == true)
            totalCount = query.Count();

        IEnumerable<BlogPost>? result;
        if (skip == null || top == null)
            result = query.ToList();
        else
            result = query.Skip(skip.Value).Take(top.Value).ToList();

        return (result, totalCount);
    }

    public async Task<bool> AddBlogPostAsync(BlogPost blogPost)
    {
        return true;
    }

    public async Task<bool> UpdateBlogPostAsync(int id, BlogPost blogPost)
    {
        return true;
    }

    public async Task<bool> DeletebyIdAsync(int id)
    {
        return true;
    }
}