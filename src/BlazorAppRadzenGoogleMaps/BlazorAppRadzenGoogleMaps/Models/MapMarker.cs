namespace BlazorAppRadzenGoogleMaps.Models;

public class MapMarker
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lng { get; set; }
}