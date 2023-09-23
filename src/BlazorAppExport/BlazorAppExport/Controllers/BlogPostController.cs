using BlazorAppExport.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppExport.Controllers;

public partial class BlogPostController : ExportController
{
    private BlogPostService _blogPostService;

    public BlogPostController(BlogPostService blogPostService)
    {
        this._blogPostService = blogPostService;
    }

    [HttpGet("/export/ApplicationDb/BlogPost/csv")]
    public async Task<FileStreamResult> ExportBlogPostToCSV()
    {
        var result = await _blogPostService.GetAllAsync();
        var query = result.AsQueryable();

        return ToCSV(ApplyQuery(query, Request.Query));
    }

    [HttpGet("/export/ApplicationDb/BlogPost/excel")]
    public async Task<FileStreamResult> ExportBlogPostToExcel()
    {
        var result = await _blogPostService.GetAllAsync();
        var query = result.AsQueryable();

        return ToExcel(ApplyQuery(query, Request.Query));
    }

    [HttpGet("/export/ApplicationDb/BlogPost/pdf")]
    public async Task<FileStreamResult> ExportBlogPostToPdf()
    {
        var result = await _blogPostService.GetAllAsync();
        var query = result.AsQueryable();

        return ToPdf(ApplyQuery(query, Request.Query));
    }

    [HttpGet("/export/ApplicationDb/BlogPost/word")]
    public async Task<FileStreamResult> ExportBlogPostToWord()
    {
        var result = await _blogPostService.GetAllAsync();
        var query = result.AsQueryable();

        return ToWord(ApplyQuery(query, Request.Query));
    }
}