using BlazorAppRadzenGoogleMaps.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppRadzenGoogleMaps.ViewModels;

public class MapMarkerViewModel : BaseViewModel<MapMarkerViewModel, MapMarker>
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title can not be empty")]

    [Range(0, double.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Lat for {0} must be between {1} and {2}.")]
    public double Lat { get; set; }

    [Range(0, double.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Lng for {0} must be between {1} and {2}.")]
    public double Lng { get; set; }

}