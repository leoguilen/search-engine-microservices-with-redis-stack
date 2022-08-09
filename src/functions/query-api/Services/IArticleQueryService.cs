namespace QueryApi.Services;

public interface IArticleQueryService
{
    Task<(IReadOnlyCollection<Article> MatchedArticles, IReadOnlyDictionary<string, Article> SuggestedArticles)> SearchAsync(
        SearchFilter searchFilter,
        PeriodRangeFilter periodRangeFilter,
        PaginationFilter paginationFilter);

    Task<Article> GetSingleAsync(string articleId);
}