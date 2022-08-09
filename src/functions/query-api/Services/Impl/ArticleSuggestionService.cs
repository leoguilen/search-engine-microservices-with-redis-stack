namespace QueryApi.Services.Impl;

internal class ArticleSuggestionService : ISuggestionService<ArticleDocument>
{
    private readonly IRedisConnection _redisConnection;

    public ArticleSuggestionService(IRedisConnection redisConnection) => _redisConnection = redisConnection;

    public async Task<SuggestionResult<ArticleDocument>> ExecuteAsync(RedisQuery query)
    {
        var commandArgs = new[] {"articles:autocomplete", query.QueryText, "MAX", "3", "WITHSCORES", "WITHPAYLOADS"};
        var suggestedResults = (await _redisConnection.ExecuteAsync("FT.SUGGET", commandArgs)).ToArray();

        if (suggestedResults is {Length: 0})
        {
            return SuggestionResult<ArticleDocument>.Empty;
        }

        var suggestedArticles = new Dictionary<string, ArticleDocument>(capacity: suggestedResults.Length / 3);
        for (var index = 0; index < suggestedResults.Length; index++)
        {
            if (!float.TryParse(suggestedResults[index], out _))
            {
                continue;
            }

            if (float.Parse(suggestedResults[index].ToString().Replace('.', ',')).CompareTo(0.5F) is < 0)
            {
                continue;
            }

            var suggestedResultKey = suggestedResults[index - 1];
            var suggestedResultPayload = JsonSerializer.Deserialize<ArticleDocument>(suggestedResults[index + 1]);

            if (suggestedArticles.ContainsKey(suggestedResultKey) ||
                suggestedArticles.ContainsValue(suggestedResultPayload))
            {
                continue;
            }

            suggestedArticles.Add(suggestedResultKey, suggestedResultPayload);
        }

        return new()
        {
            SuggestedResults = suggestedArticles
        };
    }
}