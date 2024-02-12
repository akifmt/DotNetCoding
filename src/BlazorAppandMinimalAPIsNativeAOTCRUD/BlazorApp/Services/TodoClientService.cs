using BlazorAppandMinimalAPIsNativeAOTCRUD.Core.DataModels;
using System.Text.Json;

namespace BlazorApp.Services;

public class TodoClientService(IHttpClientFactory httpClientFactory)
{
    public async Task<TodoDataModelRecord?> GetById(int id)
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.GetAsync($"todos/{id}");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TodoDataModelRecord?>(contentStream, Program.TodoApiJsonOptions);
        }

        return null;
    }

    public async Task<TodoDataModelRecord[]?> GetCompletedAsync()
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.GetAsync("todos/complete");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TodoDataModelRecord[]>(contentStream, Program.TodoApiJsonOptions);
        }

        return [];
    }

    public async Task<TodoDataModelRecord[]?> GetAllAsync()
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.GetAsync("todos");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TodoDataModelRecord[]>(contentStream, Program.TodoApiJsonOptions);
        }

        return [];
    }

    public async Task<bool> AddAsync(TodoDataModel todo)
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.PostAsJsonAsync("todos", todo, Program.TodoApiJsonOptions, default);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateAsync(int id, TodoDataModel todo)
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.PutAsJsonAsync($"todos/{id}", todo, Program.TodoApiJsonOptions, default);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        using var httpClient = httpClientFactory.CreateClient("TodoApi");

        var httpResponseMessage = await httpClient.DeleteAsync($"todos/{id}");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}