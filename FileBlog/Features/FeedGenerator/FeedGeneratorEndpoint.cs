public class FeedGeneratorEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/feed/rss", async (RssFeed rssFeed) =>
        {
            var rss = await rssFeed.RssGenerator();
            return Results.Content(rss, "application/rss+xml");
        });

        app.MapGet("/feed/atom", async (AtomFeed atomFeed) =>
        {
            var atom = await atomFeed.AtomGenerator();
            return Results.Content(atom, "application/atom+xml");
        });
    }
}