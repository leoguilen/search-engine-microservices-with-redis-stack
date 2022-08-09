namespace QueryApi.Services.Impl;

internal class ArticleQueryService : IArticleQueryService
{
    private const string RedisKeyPrefix = "article:";
    private const string RedisIndexName = "articles-idx";

    private readonly IRedisConnection _redisConnection;
    private readonly ISuggestionService<ArticleDocument> _suggestionService;

    public ArticleQueryService(
        IRedisConnection redisConnection,
        ISuggestionService<ArticleDocument> suggestionService)
    {
        _redisConnection = redisConnection;
        _suggestionService = suggestionService;
    }

    public async Task<Article> GetSingleAsync(string articleId)
        => await Task.FromResult(_redisConnection
            .JsonGet<ArticleDocument>(RedisKeyPrefix + articleId));

    public async Task<(IReadOnlyCollection<Article>, IReadOnlyDictionary<string, Article>)> SearchAsync(
        SearchFilter searchFilter,
        PeriodRangeFilter periodRangeFilter,
        PaginationFilter paginationFilter)
    {
        var query = new RedisQuery(RedisIndexName)
        {
            QueryText = searchFilter.Query,
            Limit = new()
            {
                Offset = paginationFilter.Page,
                Number = paginationFilter.ItemsPerPage,
            }
        };

        var searchResult = (SearchResponse<ArticleDocument>) default;
        var suggestionResult = (SuggestionResult<ArticleDocument>) default;

        await Task.WhenAll(
            Task.Run(async () => searchResult = await _redisConnection.SearchAsync<ArticleDocument>(query)),
            Task.Run(async () => suggestionResult = await _suggestionService.ExecuteAsync(query)));

        return (
            searchResult.Documents.Values
                .Select(doc => (Article) doc)
                .ToArray(),
            suggestionResult.SuggestedResults
                .ToDictionary(
                    res => res.Key,
                    res => (Article) res.Value));
    }
}