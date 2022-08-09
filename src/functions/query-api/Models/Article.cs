namespace QueryApi.Models;

public record Article
{
    public string Id { get; init; }

    public string Title { get; init; }

    public string Summary { get; init; }

    public string Content { get; init; }

    public string Author { get; init; }

    public string PublishDate { get; init; }

    public string Link { get; init; }

    public static implicit operator Article(ArticleDocument articleDoc) => articleDoc is null
        ? null
        : new()
        {
            Id = articleDoc.Id,
            Title = articleDoc.Details.Title,
            Summary = articleDoc.Details.Summary,
            Content = articleDoc.Details.Content,
            Author = articleDoc.Details.Author,
            PublishDate = articleDoc.Details.PublishDate,
            Link = articleDoc.Details.Link,
        };
}
