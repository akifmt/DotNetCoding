using BlazorAppRadzenNet8UpgradeRadzen4to5.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppRadzenNet8UpgradeRadzen4to5.ViewModels;

public class BlogPostViewModel : BaseViewModel<BlogPostViewModel, BlogPost>
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;

    public string TitleShort { get; set; } = string.Empty;
    public string ContentShort { get; set; } = string.Empty;

}