using AutoMapper;
using BlazorAppGenericComponents.Models;
using BlazorAppGenericComponents.Services;
using BlazorAppGenericComponents.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorAppGenericComponents.Components;

public partial class List<TModel, TViewModel> : ComponentBase where TModel : BaseModel where TViewModel : BaseViewModel, new()
{
    [Parameter, EditorRequired]
    public string? NavigateLinkItemDetails { get; set; }

    [Parameter, EditorRequired]
    public string? NavigateLinkItemEdit { get; set; }

    [Parameter, EditorRequired]
    public string? NavigateLinkItemDelete { get; set; }

    [Inject]
    private IService<TModel>? _service { get; set; }

    [Inject]
    private IMapper? Mapper { get; set; }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    private IEnumerable<TViewModel>? listViewModel;

    protected override async Task OnInitializedAsync()
    {
        if (_service == null || Mapper == null) return;
        var model = await _service.GetAllAsync();
        if (model == null) return;
        listViewModel = Mapper.Map<IEnumerable<TModel>, IEnumerable<TViewModel>>(model);
    }
}