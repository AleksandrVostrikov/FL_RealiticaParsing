using FL_RealiticaParsing.Services;
using Microsoft.AspNetCore.Mvc;

namespace FL_RealiticaParsing.Controllers
{
    public class ParserController : Controller
    {
        private readonly IParserService _parserService;

        public ParserController(IParserService parserService)
        {
            _parserService = parserService;
        }

        [HttpGet]
        [Route("parse")]
        public async Task<IActionResult> ParseDetailsLinks()
        {
            string url = "https://www.realitica.com/index.php?for=Prodaja&opa=Budva&type%5B%5D=&type%5B%5D=Apartment&price-min=30000&price-max=150000&since-day=p-anytime&qry=&lng=hr"; // Замените на нужный URL

            List<string> links = (await _parserService.ParseLinksAndAboutAuthorLinksAsync(url)).Where(l => l != "null").ToList();

            if (links.Count > 0)
            {
                return View(links);
            }
            else
            {
                ViewBag.Message = "Ссылки не найдены.";
                return View();
            }
        }
    }
}
