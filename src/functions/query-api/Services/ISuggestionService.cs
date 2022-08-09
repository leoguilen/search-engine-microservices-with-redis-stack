namespace QueryApi.Services;

public interface ISuggestionService<TOut> where TOut : class
{
    Task<SuggestionResult<TOut>> ExecuteAsync(RedisQuery query);
}
