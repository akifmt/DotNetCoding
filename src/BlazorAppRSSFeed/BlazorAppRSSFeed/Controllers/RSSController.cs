using BlazorAppRSSFeed.Data;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BlazorAppRSSFeed.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RSSController : ControllerBase
{
    private readonly MockData _mockData;

    public RSSController(MockData mockData)
    {
        _mockData = mockData;
    }

    // GET: api/<RSSController>
    [HttpGet]
    public IActionResult Get()
    {
        var output = new MemoryStream();
        string xml;

        var Feeds = _mockData.GetAllBlogPosts();

        List<SyndicationItem> items = new List<SyndicationItem>();
        var feed = new SyndicationFeed("RSS Feed Title", "Feed Description",
                                        new Uri("https://localhost:5001/posts"));
        feed.ImageUrl = new Uri("https://picsum.photos/600/400");
        feed.Authors.Add(new SyndicationPerson("asd@asd.com", "asdname", "https://picsum.photos/600/600"));
        feed.BaseUri = new Uri("https://localhost:5001");
        feed.Categories.Add(new SyndicationCategory("Feed Category 1 Base"));
        feed.LastUpdatedTime = DateTime.Now;
        feed.Language = "Lang1";
        feed.Copyright = new TextSyndicationContent("Copy1");
        foreach (var post in Feeds)
        {
            var solutionfeed = new SyndicationItem(post.Title, post.Content,
                new Uri($"https://localhost:5001/postsingle/{post.Id}"), post.Id.ToString(), DateTime.Now);
            solutionfeed.Authors.Add(new SyndicationPerson("post@user.com", "postuser", "https://picsum.photos/600/600"));
            solutionfeed.BaseUri = new Uri("https://localhost:5001/posts");
            solutionfeed.Categories.Add(new SyndicationCategory("Feed Category 1 feed"));
            solutionfeed.Contributors.Add(new SyndicationPerson("postCont@user.com", "postcontuser", "https://picsum.photos/600/600"));
            solutionfeed.Copyright = new TextSyndicationContent("feed copy");
            solutionfeed.ElementExtensions.Add(new XElement("enclosure",
                new XAttribute("type", ""),
                new XAttribute("url", "https://picsum.photos/600/400"),
                new XAttribute("width", 200),
                new XAttribute("height", 200)).CreateReader());
            solutionfeed.PublishDate = DateTime.Now;
            solutionfeed.Summary = new TextSyndicationContent(post.Content.Substring(0, 5));
            items.Add(solutionfeed);
        }

        feed.Items = items;
        var formatter = new Rss20FeedFormatter(feed);
        var xws = new XmlWriterSettings { Encoding = Encoding.UTF8 };
        using (var xmlWriter = XmlWriter.Create(output, xws))
        {
            formatter.WriteTo(xmlWriter);
            xmlWriter.Flush();
        }
        using (var sr = new StreamReader(output))
        {
            output.Position = 0;
            xml = sr.ReadToEnd();
            sr.Close();
        }

        ContentResult result = Content(xml, "application/xml", Encoding.UTF8);
        return result;
    }
}