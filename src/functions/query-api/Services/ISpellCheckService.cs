namespace QueryApi.Services;

public interface ISpellCheckService
{
    Task<IReadOnlySet<string>> CheckAsync(string term);
}