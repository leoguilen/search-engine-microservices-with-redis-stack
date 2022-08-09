namespace QueryApi.Models.Responses;

public readonly record struct SearchArticlesResponse(ArticleResponse[] MatchedArticles, ArticleResponse[] SuggestedArticles)
{
    public static SearchArticlesResponse From(
        IReadOnlyCollection<Article> articles,
        IReadOnlyDictionary<string, Article> suggestions) => new(
        articles.Select(ArticleResponse.From).ToArray(),
        suggestions.Values.Select(sug => ArticleResponse.From(sug)).ToArray());
}