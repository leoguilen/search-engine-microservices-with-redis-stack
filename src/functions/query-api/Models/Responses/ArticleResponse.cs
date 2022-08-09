namespace QueryApi.Models.Responses;

public readonly record struct ArticleResponse(
    string Id,
    string Title,
    string Summary,
    string Content,
    string Author,
    string PublishDate,
    string Link)
{
    public static ArticleResponse From(Article article) => new(
        article.Id,
        article.Title,
        article.Summary,
        article.Content,
        article.Author,
        article.PublishDate,
        article.Link);
}
