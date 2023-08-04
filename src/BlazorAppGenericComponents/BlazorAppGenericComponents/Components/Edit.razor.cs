using AutoMapper;
using BlazorAppGenericComponents.Models;
using BlazorAppGenericComponents.Services;
using BlazorAppGenericComponents.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorAppGenericComponents.Components;

public partial class Edit<TModel, TViewModel> : ComponentBase where TModel : BaseModel where TViewModel : BaseViewModel, new()
{
    [Parameter, EditorRequired]
    public int? Id { get; set; }

    [Parameter, EditorRequired]
    public string? NavigateLinkAfterSubmit { get; set; }

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

    private async void HandleValidSubmit()
    {
        var model = Mapper.Map<TViewModel, TModel>(viewModel);
        bool result = await _service.UpdateAsync(Id.Value, model);
        if (result)
            NavigationManager.NavigateTo(NavigateLinkAfterSubmit);
    }

    private void HandleValueChanged(ChangeEventArgs e, string propertyName)
    {
        var value = e.Value as string;
        var property = viewModel?.GetType().GetProperty(propertyName);
        if (property?.GetValue(viewModel) as string != value)
        {
            property?.SetValue(viewModel, value);
        }
    }
}