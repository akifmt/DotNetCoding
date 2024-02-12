namespace BlazorAppandMinimalAPIsNativeAOTCRUD.Core.DataModels;

public record TodoDataModelRecord(int Id = 0, string? Title = "", DateOnly? DueBy = null, bool IsComplete = false);

public class TodoDataModel
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public DateOnly? DueBy { get; set; } = null;
    public bool IsComplete { get; set; } = false;
}