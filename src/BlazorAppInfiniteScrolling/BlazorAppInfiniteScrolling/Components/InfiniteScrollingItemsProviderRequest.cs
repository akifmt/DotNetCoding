namespace BlazorAppInfiniteScrolling.Components;

public sealed class InfiniteScrollingItemsProviderRequest
{
    public InfiniteScrollingItemsProviderRequest(int startIndex, CancellationToken cancellationToken)
    {
        StartIndex = startIndex;
        CancellationToken = cancellationToken;
    }

    public int StartIndex { get; }
    public CancellationToken CancellationToken { get; }
}

public delegate Task<IEnumerable<int>> ItemsProviderRequestDelegate(InfiniteScrollingItemsProviderRequest request);

public delegate Task<IEnumerable<T>> ItemsProviderRequestDelegate<T>(InfiniteScrollingItemsProviderRequest request);