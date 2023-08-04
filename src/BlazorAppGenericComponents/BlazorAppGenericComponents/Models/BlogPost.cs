namespace BlazorAppGenericComponents.Models;

public class BlogPost : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}