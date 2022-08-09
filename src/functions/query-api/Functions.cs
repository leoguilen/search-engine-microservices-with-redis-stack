namespace QueryApi;

public class Functions
{
    private const string BaseFunctionPath = "/api/articles/";
    private const string AllowedHttpMethod = "get";

    private readonly IArticleQueryService _articleQueryService;
    private readonly ISuggestionService<ArticleDocument> _suggestionService;
    private readonly ISpellCheckService _spellCheckService;

    public Functions(
        IArticleQueryService articleQueryService,
        ISpellCheckService spellCheckService,
        ISuggestionService<ArticleDocument> suggestionService)
    {
        _articleQueryService = articleQueryService;
        _spellCheckService = spellCheckService;
        _suggestionService = suggestionService;
    }

    [FunctionName("GetArticleFunction")]
    public async Task<IActionResult> GetSingleAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, AllowedHttpMethod, Route = "articles/{articleId:long}")]
        HttpRequest req,
        ILogger logger)
    {
        logger.LogInformation("C# function GetArticleFunction processed a request");

        var articleId = req.Path.Value
            .AsMemory()[BaseFunctionPath.Length..]
            .ToString();
        var article = await _articleQueryService
            .GetSingleAsync(articleId);

        return article is null
            ? new NotFoundResult()
            : new OkObjectResult(ArticleResponse.From(article));
    }

    [FunctionName("SearchArticlesFunction")]
    public async Task<IActionResult> SearchAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, AllowedHttpMethod, Route = "articles")]
        HttpRequest req,
        ILogger logger)
    {
        logger.LogInformation("C# function SearchArticlesFunction processed a request");

        var searchFilter = new SearchFilter(req.Query["q"]);
        var paginationFilter = PaginationFilter.From(req.Query);

        var (matchedArticles, suggestedArticles) = await _articleQueryService
            .SearchAsync(searchFilter, default, paginationFilter);

        return new OkObjectResult(SearchArticlesResponse.From(matchedArticles, suggestedArticles));
    }

    [FunctionName("SpellCheckFunction")]
    public async Task<IActionResult> CheckAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, AllowedHttpMethod, Route = "spell-check")]
        HttpRequest req,
        ILogger logger)
    {
        logger.LogInformation("C# function SpellCheckFunction processed a request");

        if (!req.Query.TryGetValue("t", out var term))
        {
            return new BadRequestResult();
        }

        var result = await _spellCheckService.CheckAsync(term);

        var response = new SpellCheckResponse(term, result.ToArray());
        return new OkObjectResult(response);
    }
}