namespace BlazorAppSerilogLogging.Models;

public class Log
{
    public int id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
    public string RenderedMessage { get; set; } = string.Empty;
    public string Properties { get; set; } = string.Empty;
}