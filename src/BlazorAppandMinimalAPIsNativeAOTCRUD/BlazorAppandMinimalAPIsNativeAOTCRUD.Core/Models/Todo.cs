namespace BlazorAppandMinimalAPIsNativeAOTCRUD.Core.Models;

public class Todo
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public DateOnly? DueBy { get; set; } = null;
    public bool IsComplete { get; set; } = false;

    public override string ToString() => $"{this.Id} {this.Title} {this.DueBy} {this.IsComplete}";

}