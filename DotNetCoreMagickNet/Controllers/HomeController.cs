using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreMagickNet.Models;
using ImageMagick;
using Microsoft.Extensions.Hosting;

namespace DotNetCoreMagickNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _rootPath;

        public HomeController(ILogger<HomeController> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _rootPath = hostEnvironment.ContentRootPath;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Download()
        {
            _logger.LogInformation("start create image");

            MagickNET.SetGhostscriptDirectory(_rootPath);
            var settings = new MagickReadSettings { Density = new Density(300, 300) };

            byte[] img;
            using (var images = new MagickImageCollection())
            {
                images.Read(Path.Combine(_rootPath, "sample.pdf"), settings);
                img = images.First().ToByteArray(MagickFormat.Png);
            }
            return File(img, MediaTypeNames.Application.Octet, "sample.png");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
