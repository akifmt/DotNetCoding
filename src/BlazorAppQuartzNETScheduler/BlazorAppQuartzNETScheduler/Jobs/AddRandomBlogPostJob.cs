using BlazorAppQuartzNETScheduler.Data;
using BlazorAppQuartzNETScheduler.Models;
using BlazorAppQuartzNETScheduler.Services;
using Quartz;

namespace BlazorAppQuartzNETScheduler.Jobs;

[DisallowConcurrentExecution]
public class AddRandomBlogPostJob : IJob
{
    private readonly ILogger<AddRandomBlogPostJob> _logger;
    private readonly BlogPostService _blogPostService;

    public AddRandomBlogPostJob(ILogger<AddRandomBlogPostJob> logger, BlogPostService blogPostService)
    {
        _logger = logger;
        _blogPostService = blogPostService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        BlogPost model = SeedData.GetRandomPost();
        await _blogPostService.AddBlogPostAsync(model);
    }
}