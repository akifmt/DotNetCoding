using AutoMapper;
using BlazorAppGenericComponents.Models;
using BlazorAppGenericComponents.Services;
using BlazorAppGenericComponents.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorAppGenericComponents.Components;

public partial class Delete<TModel, TViewModel> : ComponentBase where TModel : BaseModel where TViewModel : BaseViewModel, new()
{
    [Parameter, EditorRequired]
    public int? Id { get; set; }

    [Parameter, EditorRequired]
    public string? NavigateLinkAfterDelete { get; set; }

    [Inject]
    private IService<TModel>? _service { get; set; }

    [Inject]
    private IMapper? Mapper { get; set; }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    private TViewModel? viewModel;

    protected override async Task OnInitializedAsync()
    {
        if (Id == null || _service == null || Mapper == null) return;
        var model = await _service.GetbyId(Id.Value);
        if (model == null) return;
        viewModel = Mapper.Map<TModel, TViewModel>(model);
    }

    private async void DeleteButtonClick()
    {
        bool result = await _service.DeletebyIdAsync(Id.Value);
        if (result)
            NavigationManager.NavigateTo(NavigateLinkAfterDelete);
    }
}