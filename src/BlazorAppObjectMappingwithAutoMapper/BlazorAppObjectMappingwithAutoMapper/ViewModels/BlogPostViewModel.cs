using BlazorAppObjectMappingwithAutoMapper.Data;
using BlazorAppObjectMappingwithAutoMapper.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppObjectMappingwithAutoMapper.ViewModels;

public class BlogPostViewModel
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Content can not be empty")]
    public string Content { get; set; } = string.Empty;

    public string TitleShort { get; set; } = string.Empty;
    public string ContentShort { get; set; } = string.Empty;

}