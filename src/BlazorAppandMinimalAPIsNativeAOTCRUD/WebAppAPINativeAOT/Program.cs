using BlazorAppandMinimalAPIsNativeAOTCRUD.Core.DataModels;
using BlazorAppandMinimalAPIsNativeAOTCRUD.Core.Models;
using System.Text.Json.Serialization;
using WebAppAPINativeAOT.Data;
using WebAppAPINativeAOT.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        });

        // add test data
        SampleData.AddTestData();

        // Add services to the container.
        builder.Services.AddScoped<TodoService>();

        var app = builder.Build();

        // add endpoints
        app.MapTodoGroup();

        app.Run();
    }
}

[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(TodoDataModel[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

public static class WebApplicationExtensions
{
    public static void MapTodoGroup(this WebApplication app)
    {
        var todosApi = app.MapGroup("/todos");

        // get all
        todosApi.MapGet("/", (TodoService todoService) => todoService.GetAll());

        // get all completed
        todosApi.MapGet("/complete", (TodoService todoService) => todoService.GetCompleted());

        // get by id
        todosApi.MapGet("/{id}", (int id, TodoService todoService) =>
            todoService.GetById(id) is { } todo
                ? Results.Ok(todo)
                : Results.NotFound());

        // add
        todosApi.MapPost("/", (Todo todo, TodoService todoService) =>
        {
            todoService.Add(todo);
            return Results.Created($"/{todo.Id}", todo);
        });

        // update
        todosApi.MapPut("/{id}", (int id, Todo inputTodo, TodoService todoService) =>
        {
            var todo = todoService.GetById(id);

            if (todo is null) return Results.NotFound();

            todoService.Update(id, inputTodo);

            return Results.NoContent();
        });

        // delete
        todosApi.MapDelete("/{id}", (int id, TodoService todoService) =>
        {
            if (todoService.GetById(id) is Todo todo)
            {
                todoService.DeleteById(id);
                return Results.NoContent();
            }

            return Results.NotFound();
        });
    }
}