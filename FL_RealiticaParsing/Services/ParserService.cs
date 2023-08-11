using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace FL_RealiticaParsing.Services
{
    public class ParserService : IParserService
    {
        public async Task<List<string>> ParseLinksAndAboutAuthorLinksAsync(string url)
        {
            List<string> aboutAuthorLinks = new List<string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(url);

            var divNodes = doc.DocumentNode.Descendants()
                .Where(node =>
                    node.Name == "div" &&
                    node.HasClass("thumb_div"));

            foreach (var divNode in divNodes)
            {
                var anchorNode = divNode.Descendants("a").FirstOrDefault();
                if (anchorNode != null)
                {
                    var hrefAttribute = anchorNode.Attributes["href"];
                    if (hrefAttribute != null)
                    {
                        string link = hrefAttribute.Value;
                        string aboutAuthorLink = await GetAboutAuthorLinksAsync(link);
                        if (!string.IsNullOrEmpty(aboutAuthorLink))
                        {
                            aboutAuthorLinks.Add(aboutAuthorLink);
                        }
                    }
                }
            }

            return aboutAuthorLinks;
        }

        private async Task<string> GetAboutAuthorLinksAsync(string link)
        {
            List<string> aboutAuthorLinks = new List<string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(link);

            var aboutAuthorDiv = doc.GetElementbyId("aboutAuthor");
            if (aboutAuthorDiv != null)
            {
                var anchorNodes = aboutAuthorDiv.Descendants("a");

                foreach (var anchorNode in anchorNodes)
                {
                    var hrefAttribute = anchorNode.Attributes["href"];
                    if (hrefAttribute != null)
                    {
                        string hrefValue = hrefAttribute.Value;
                        if (IsDesiredLinkFormat(hrefValue))
                        {
                            aboutAuthorLinks.Add(hrefValue);
                        }
                    }
                }
            }
            if (aboutAuthorLinks.Count > 0)
            {
                return aboutAuthorLinks[0];
            }
            else return "null";
            
        }

        private bool IsDesiredLinkFormat(string link)
        {
            // Define the desired link format using a regular expression
            string pattern = @"^\/\?action=";

            return Regex.IsMatch(link, pattern);
        }
    }
}

