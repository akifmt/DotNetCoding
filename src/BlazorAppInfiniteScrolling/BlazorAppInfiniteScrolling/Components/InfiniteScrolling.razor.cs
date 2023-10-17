using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorAppInfiniteScrolling.Components;

public partial class InfiniteScrolling<T> : ComponentBase, IAsyncDisposable
{
    private List<T> _items = new();
    private ElementReference _lastItemIndicator;
    private DotNetObjectReference<InfiniteScrolling<T>>? _currentComponentReference;
    private IJSObjectReference? _module;
    private IJSObjectReference? _instance;
    private bool _loading = false;
    private bool _enumerationCompleted = false;
    private CancellationTokenSource? _loadItemsCts;

    [Inject]
    private IJSRuntime? JSRuntime { get; set; }

    [Parameter]
    public ItemsProviderRequestDelegate<T>? ItemsProvider { get; set; }

    [Parameter]
    public RenderFragment<T>? ItemTemplate { get; set; }

    [Parameter]
    public RenderFragment? LoadingTemplate { get; set; }

    [JSInvokable]
    public async Task LoadMoreItems()
    {
        if (_loading)
            return;

        _loading = true;
        try
        {
            _loadItemsCts ??= new CancellationTokenSource();

            StateHasChanged();
            try
            {
                var newItems = await ItemsProvider(new InfiniteScrollingItemsProviderRequest(_items.Count, _loadItemsCts.Token));

                var previousCount = _items.Count;
                _items.AddRange(newItems);

                if (_items.Count == previousCount)
                {
                    _enumerationCompleted = true;
                }
                else
                {
                    await _instance.InvokeVoidAsync("onNewItems");
                }
            }
            catch (OperationCanceledException oce) when (oce.CancellationToken == _loadItemsCts.Token)
            {
            }
        }
        finally
        {
            _loading = false;
        }

        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./infinite-scrolling.js");
            _currentComponentReference = DotNetObjectReference.Create(this);
            _instance = await _module.InvokeAsync<IJSObjectReference>("initialize", _lastItemIndicator, _currentComponentReference);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_loadItemsCts != null)
        {
            _loadItemsCts.Dispose();
            _loadItemsCts = null;
        }

        if (_instance != null)
        {
            await _instance.InvokeVoidAsync("dispose");
            await _instance.DisposeAsync();
            _instance = null;
        }

        if (_module != null)
        {
            await _module.DisposeAsync();
        }

        _currentComponentReference?.Dispose();
    }
}