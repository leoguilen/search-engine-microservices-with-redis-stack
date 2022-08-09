namespace QueryApi.Models.Responses;

public abstract class PaginatedResponse<TResponse> where TResponse : struct
{
    public IReadOnlyCollection<TResponse> Data { get; }

    public int CurrentPage { get; }

    public int ItemsPerPage { get; }

    public int TotalItems { get; }

    public int TotalPages { get; }

    protected PaginatedResponse(
        IReadOnlyCollection<TResponse> data,
        int currentPage,
        int itemsPerPage,
        int totalItems)
    {
        Data = data ?? Array.Empty<TResponse>();
        CurrentPage = 0;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = 0;

        var hasRecords = totalItems > 0;
        if (hasRecords)
        {
            CurrentPage = currentPage;
            TotalPages = ((totalItems - 1) / itemsPerPage) + 1;
        }
    }
}
