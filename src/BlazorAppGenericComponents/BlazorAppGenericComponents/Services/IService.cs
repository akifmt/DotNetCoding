namespace BlazorAppGenericComponents.Services;

internal interface IService<TModel> where TModel : class
{
    Task<TModel?> GetbyId(int id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<bool> AddAsync(TModel blogPost);

    Task<bool> UpdateAsync(int id, TModel model);

    Task<bool> DeletebyIdAsync(int id);
}