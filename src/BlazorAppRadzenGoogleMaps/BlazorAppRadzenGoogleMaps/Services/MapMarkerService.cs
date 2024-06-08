using BlazorAppRadzenGoogleMaps.Data;
using BlazorAppRadzenGoogleMaps.Models;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace BlazorAppRadzenGoogleMaps.Services;

public class MapMarkerService
{
    private readonly ApplicationDbContext _context;

    public MapMarkerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<MapMarker?> GetbyId(int id)
    {
        return _context.MapMarkers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<(IEnumerable<MapMarker> Result, int TotalCount)> GetMapMarkersAsync(string? filter = default, int? top = default, int? skip = default, string? orderby = default, string? expand = default, string? select = default, bool? count = default)
    {
        var query = _context.MapMarkers.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(orderby))
            query = query.OrderBy(orderby);

        int totalCount = 0;
        if (count == true)
            totalCount = query.Count();

        IEnumerable<MapMarker>? result;
        if (skip == null || top == null)
            result = await query.ToListAsync();
        else
            result = await query.Skip(skip.Value).Take(top.Value).ToListAsync();

        return (result, totalCount);
    }

    public async Task<bool> AddMapMarkerAsync(MapMarker mapMarker)
    {
        try
        {
            await _context.MapMarkers.AddAsync(mapMarker);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> UpdateMapMarkerAsync(int id, MapMarker mapMarker)
    {
        try
        {
            var oldMapMarker = _context.MapMarkers.FirstOrDefault(x => x.Id == id);
            if (oldMapMarker == null) return false;

            oldMapMarker.Title = mapMarker.Title;
            oldMapMarker.Lat = mapMarker.Lat;
            oldMapMarker.Lng = mapMarker.Lng;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeletebyIdAsync(int id)
    {
        var mapMarker = await _context.MapMarkers.FirstOrDefaultAsync(x => x.Id == id);
        if (mapMarker == null)
            return false;

        _context.MapMarkers.Remove(mapMarker);
        await _context.SaveChangesAsync();

        return true;
    }
}