namespace BlazorAppResizeUploadImages.Models;

public class BlogPost
{
    public int Id { get; set; }
    public string PostImage { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}