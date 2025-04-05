using Flurl;
using HtmlAgilityPack.CssSelectors.NetCore;
using Totoro.Plugins.Anime.Contracts;
using Totoro.Plugins.Contracts.Optional;
using Totoro.Plugins.Helpers;
using Totoro.Plugins.Options;

namespace Totoro.Plugins.Anime.AnimeFlv;

public class Catalog : IAnimeCatalog
{
    class SearchResult : ICatalogItem, IHaveImage
    {
        required public string Title { get; init; }
        required public string Url { get; init; }
        required public string Image { get; init; }
    }

    public async IAsyncEnumerable<ICatalogItem> Search(string query)
    {
        var baseUrl = ConfigManager<Config>.Current.Url;
        var doc = await Path.Combine($"{baseUrl}/browse")
            .SetQueryParams(new
            {
                q = query
            })
            .GetHtmlDocumentAsync();

        foreach (var item in doc.QuerySelectorAll("div.Container ul.ListAnimes li article"))
        {
            var imageNode = item.QuerySelector("a div.Image figure img");
            string image;
            if (imageNode.Attributes["src"] != null)
            {
                image = imageNode.Attributes["src"].Value;
            }
            else
            {
                image = imageNode.Attributes["src"].Value;
            }

            var title = item.QuerySelector("a h3").InnerText.Trim();
            var url = Path.Combine(baseUrl, item.QuerySelector("div.Description a.Button").Attributes["href"].Value);
            yield return new SearchResult
            {
                Title = title,
                Image = image,
                Url = url
            };
        }
    }
}
