using Microsoft.AspNetCore.Components;

namespace BlazorAppExport.Services;

public class ExportService
{
    private readonly NavigationManager navigationManager;

    public ExportService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }

    // only with table and type
    //public void Export(string table, string type)
    //{
    //    navigationManager.NavigateTo($"/export/ApplicationDb/{table}/{type}", true);
    //}

    // Dynamic Query
    public void Export(string table, string type, IQueryCollection? query = null)
    {
        var url = $"/export/ApplicationDb/{table}/{type}";
        if (query is not null)
        {
            string queryString = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));
            url += $"?{queryString}";
        }

        navigationManager.NavigateTo(url, true);
    }

    // Radzen DataTable queries
    //public void Export(string table, string type, Query query = null)
    //{
    //    navigationManager.NavigateTo(query != null ? query.ToUrl($"/export/northwind/{table}/{type}") : $"/export/northwind/{table}/{type}", true);
    //}
}