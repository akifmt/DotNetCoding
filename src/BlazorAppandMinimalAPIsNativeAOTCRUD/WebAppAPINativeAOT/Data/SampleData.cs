using BlazorAppandMinimalAPIsNativeAOTCRUD.Core.Models;

namespace WebAppAPINativeAOT.Data;

public static class SampleData
{
    public static List<Todo> ToDos { get; set; } = [];

    public static void AddTestData()
    {
        Todo[] sampleToDos =
           [
                new(){ Id = 1, Title = "Walk the dog", IsComplete = true },
                new(){ Id = 2, Title = "Do the dishes", DueBy = DateOnly.FromDateTime(DateTime.Now)},
                new(){ Id = 3, Title = "Do the laundry", DueBy = DateOnly.FromDateTime(DateTime.Now.AddDays(1))},
                new(){ Id = 4, Title = "Clean the bathroom", IsComplete = true },
                new(){ Id = 5, Title = "Clean the car", DueBy = DateOnly.FromDateTime(DateTime.Now.AddDays(2))}
         ];

        ToDos.AddRange(sampleToDos);
    }
}