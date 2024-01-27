using BlazorAppObjectMappingwithMapster.Data;
using BlazorAppObjectMappingwithMapster.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppObjectMappingwithMapster.ViewModels;

public class BlogPostViewModel : BaseDTO<BlogPostViewModel, BlogPost>
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;

    public string TitleShort { get; set; } = string.Empty;
    public string ContentShort { get; set; } = string.Empty;

    public override void AddCustomMappings()
    {
        // from BlogPostViewModel to BlogPost
        SetCustomMappings().Map(dst => dst.Title,
                                src => src.Title);

        // from BlogPost to BlogPostViewModel
        SetCustomMappingsInverse().Map(dst => dst.TitleShort,
                                       src => src.Title.Substring(0, Math.Min(src.Title.Count(), 10)))
                                  .Map(dst => dst.ContentShort,
                                       src => src.Content.Substring(0, Math.Min(src.Content.Count(), 50)));

        // base.AddCustomMappings();
    }

}