namespace QueryApi.Models.Responses;

public class ArticlePaginatedResponse : PaginatedResponse<ArticleResponse>
{
    public ArticlePaginatedResponse(
        IReadOnlyCollection<ArticleResponse> data,
        PaginationFilter paginationFilter,
        int totalItems)
        : base(data, paginationFilter.Page, paginationFilter.ItemsPerPage, totalItems)
    {
    }

    public static ArticlePaginatedResponse From(
        IReadOnlyCollection<ArticleResponse> data,
        PaginationFilter paginationFilter,
        int totalItems) => new(data, paginationFilter, totalItems);
}
