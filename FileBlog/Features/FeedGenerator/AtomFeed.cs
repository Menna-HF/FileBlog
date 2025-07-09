using System.Xml.Linq;

public class AtomFeed
{
    private readonly IPostStorage _postStorage;

    public AtomFeed(IPostStorage postStorage)
    {
        _postStorage = postStorage;
    }

    public async Task<string> AtomGenerator()
    {
        var posts = await _postStorage.GetAllPostsAsync();
        XNamespace atom = "http://www.w3.org/2005/Atom";

        var feed = new XElement(atom + "feed",
            new XElement(atom + "title", "SK Blog"),
            new XElement(atom + "link", new XAttribute("href", "http://localhost:5054/")),
            new XElement(atom + "updated", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")),
            posts.Select(post =>
                new XElement(atom + "entry",
                new XElement(atom + "title", post.Title),
                new XElement(atom + "link", new XAttribute("href", $"http://localhost:5054/{post.Slug}")),
                new XElement(atom + "summary", post.Description),
                new XElement(atom + "published", post.PublishingDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"))
                )
            )
        );

        return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), feed).ToString();
    }
}