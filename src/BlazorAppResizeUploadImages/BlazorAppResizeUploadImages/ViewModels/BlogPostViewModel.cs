using System.ComponentModel.DataAnnotations;

namespace BlazorAppResizeUploadImages.ViewModels;

public class BlogPostViewModel
{
    public int Id { get; set; }

    public string PostImage { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;
}