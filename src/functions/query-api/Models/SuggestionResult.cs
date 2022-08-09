namespace QueryApi.Models;

public record SuggestionResult<TValue> where TValue : class
{
    public static SuggestionResult<TValue> Empty = new()
    {
        SuggestedResults = new Dictionary<string, TValue>(),
    };

    public IReadOnlyDictionary<string, TValue> SuggestedResults { get; init; }

    public bool HasResult => SuggestedResults.Count > 0;
}