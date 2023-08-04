using System.ComponentModel.DataAnnotations;

namespace BlazorAppGenericComponents.ViewModels;

public class BlogPostViewModel : BaseViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;
}