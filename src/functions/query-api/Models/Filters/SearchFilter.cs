#nullable enable
namespace QueryApi.Models.Filters;

//public readonly record struct SearchFilter(string? Query, string? Title, string? Summary, string? Content, string? Author);
public readonly record struct SearchFilter(string? Query)
{
    public static implicit operator SearchFilter(StringValues query) => new(query);
}