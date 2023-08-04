using AutoMapper;
using BlazorAppGenericComponents.Models;
using BlazorAppGenericComponents.Services;
using BlazorAppGenericComponents.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorAppGenericComponents.Components;

public partial class Create<TModel, TViewModel> : ComponentBase where TModel : BaseModel where TViewModel : BaseViewModel, new()
{
    [Parameter, EditorRequired]
    public string? NavigateLinkAfterSubmit { get; set; }

    [Inject]
    private IService<TModel>? _service { get; set; }

    [Inject]
    private IMapper? Mapper { get; set; }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    private TViewModel? viewModel;

    protected override void OnInitialized() => viewModel = new();

    private async void HandleValidSubmit()
    {
        var model = Mapper.Map<TViewModel, TModel>(viewModel);
        bool result = await _service.AddAsync(model);
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