using System.Xml.Linq;

public class RssFeed
{
    private readonly IPostStorage _postStorage;

    public RssFeed(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }

    public async Task<string> RssGenerator()
    {
        var posts = await _postStorage.GetAllPostsAsync();

        var channel = new XElement("channel",
        new XElement("title", "SK Blog"),
        new XElement("link", "http://localhost:5054/"),
        new XElement("description", "The latest posts of SK Blog."),
        posts.Select(post =>
            new XElement("item",
                new XElement("title", post.Title),
                new XElement("link", $"http://localhost:5054/{post.Slug}"),
                new XElement("description", post.Description),
                new XElement("pubDate", post.PublishingDate?.ToString("r"))
                )
            )
        );

        var rss = new XElement("rss", new XAttribute("version", "2.0"), channel);
        return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), rss).ToString();
    }
}