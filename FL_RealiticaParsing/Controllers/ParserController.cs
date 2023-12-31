﻿using FL_RealiticaParsing.Models;
using FL_RealiticaParsing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace FL_RealiticaParsing.Controllers
{
    public class ParserController : Controller
    {
        private readonly IParserService _parserService;

        public ParserController(IParserService parserService)
        {
            _parserService = parserService;
        }

        [HttpPost]
        [Route("{controller}")]
        public async Task<IActionResult> ParseDetailsLinks(URL parseUrl)
        {
            if (ModelState.IsValid)
            {
                List<string> links = new();
                List<string> mid = new();
                int i = 0;
                while (true)
                {
                    string pURL = $"{parseUrl.Url}&cur_page={i}";
                    mid = (await _parserService.ParseLinksAndAboutAuthorLinksAsync(pURL)).Where(l => l != "null").ToList();
                    if (mid.Count > 0)
                    {
                        links.AddRange(mid);
                    }
                    else break;
                    i++;
                }
                List<string> distinctResult = links.Distinct().ToList();
                for (int k = 0; k < distinctResult.Count(); k++)
                {
                    distinctResult[k] = $"https://www.realitica.com{distinctResult[k]}";
                }
                List<string> result = new();
                foreach (string d in distinctResult)
                {
                    if (_parserService.ParseCount(d).Result < 5)
                    {
                        result.Add(d);
                    }
                }
                if (result.Count > 0)
                {
                    return View(result);
                }
                else
                {
                    ViewBag.Message = "Ссылки не найдены.";
                    return View();
                }
            }
            else {
                return View();
            }
                
        }
    }
}
