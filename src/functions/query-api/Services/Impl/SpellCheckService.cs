namespace QueryApi.Services.Impl;

public class SpellCheckService : ISpellCheckService
{
    private readonly IRedisConnection _redisConnection;

    public SpellCheckService(IRedisConnection redisConnection) => _redisConnection = redisConnection;

    public async Task<IReadOnlySet<string>> CheckAsync(string term)
    {
        var commandArgs = new[] {"articles-idx", term};
        var result = (await _redisConnection.ExecuteAsync("FT.SPELLCHECK", commandArgs)).ToArray();
        if (result is {Length: 0})
        {
            return new HashSet<string>(capacity: 0);
        }

        var correctionTerms = result[0].ToArray()[^1].ToArray();
        if (correctionTerms is {Length: 0})
        {
            return new HashSet<string>(capacity: 0);
        }

        var results = new HashSet<string>(capacity: correctionTerms.Length / 2);
        foreach (var t in correctionTerms)
        {
            results.Add(t.ToArray()[^1]);
        }

        return results;
    }
}