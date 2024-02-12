using BlazorAppandMinimalAPIsNativeAOTCRUD.Core.Models;
using WebAppAPINativeAOT.Data;

namespace WebAppAPINativeAOT.Services;

public class TodoService(ILogger<TodoService> logger)
{
    public Todo? GetById(int id)
    {
        logger.LogInformation("Called GetById: {id}", id);
        return SampleData.ToDos.FirstOrDefault(x => x.Id == id);
    }

    public Todo[]? GetCompleted()
    {
        logger.LogInformation("Called GetCompleted");
        return SampleData.ToDos.Where(x => x.IsComplete == true).ToArray();
    }

    public Todo[]? GetAll()
    {
        logger.LogInformation("Called GetAll");
        return [.. SampleData.ToDos];
    }

    public bool Add(Todo todo)
    {
        logger.LogInformation("Called Add {todo}", todo);

        try
        {
            var last = SampleData.ToDos.LastOrDefault();
            todo.Id = last is not null ? last.Id + 1 : 1;
            SampleData.ToDos.Add(todo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Called Add Error {todo}", todo);
            return false;
        }
        return true;
    }

    public bool Update(int id, Todo todo)
    {
        logger.LogInformation("Called Update {todo}", todo);
        try
        {
            var oTodo = SampleData.ToDos.FirstOrDefault(x => x.Id == id);
            if (oTodo == null) return false;

            oTodo.Title = todo.Title;
            oTodo.DueBy = todo.DueBy;
            oTodo.IsComplete = todo.IsComplete;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Called Update Error {todo}", todo);
            return false;
        }
        return true;
    }

    public bool DeleteById(int id)
    {
        logger.LogInformation("Called DeleteById {id}", id);
        var oTodo = SampleData.ToDos.FirstOrDefault(x => x.Id == id);
        if (oTodo == null)
            return false;

        SampleData.ToDos.Remove(oTodo);

        return true;
    }
}