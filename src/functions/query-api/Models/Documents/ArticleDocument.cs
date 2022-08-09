namespace QueryApi.Models.Documents;

[Document(IndexName = "articles-idx", StorageType = StorageType.Json)]
public record ArticleDocument
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("value")]
    public ArticleDetailsDocument Details { get; init; }
}

public record ArticleDetailsDocument
{
    [JsonPropertyName("title")]
    [Indexed(JsonPath = "$.value.title")]
    [Searchable(JsonPath = "$.value.title")]
    public string Title { get; init; }

    [JsonPropertyName("summary")]
    [Indexed(JsonPath = "$.value.summary")]
    [Searchable(JsonPath = "$.value.summary")]
    public string Summary { get; init; }

    [JsonPropertyName("content")]
    [Indexed(JsonPath = "$.value.content")]
    [Searchable(JsonPath = "$.value.content")]
    public string Content { get; init; }

    [JsonPropertyName("author")]
    [Indexed(JsonPath = "$.value.author")]
    public string Author { get; init; }

    [JsonPropertyName("published_at")]
    public string PublishDate { get; init; }

    [JsonPropertyName("link")]
    public string Link { get; init; }
}