namespace QueryApi.Models.Filters;

public readonly record struct PaginationFilter(int Page = 0, int ItemsPerPage = 10)
{
    public static PaginationFilter From(IQueryCollection query) => query is { Count: > 0 }
        ? new(Page: int.Parse(query["page"]), ItemsPerPage: int.Parse(query["itemsPerPage"]))
        : new();
}
