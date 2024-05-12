using BlazorAppRadzenHtmlEditor.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppRadzenHtmlEditor.ViewModels;

public class BlogPostViewModel : BaseViewModel<BlogPostViewModel, BlogPost>
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;

    public string TitleShort { get => this.Title.Length > 50 ? this.Title.Substring(0, 50) : this.Title; }
    public string ContentShort { get => this.Content.Length > 500 ? this.Content.Substring(0, 500) : this.Content; }

}