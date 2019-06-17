using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobStorageDemo.Models;
using BlobStorageDemo.Helpers;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BlobStorageDemo.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;

        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var filePath = Path.GetTempFileName();
                if (customer.Image.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await customer.Image.CopyToAsync(stream);
                    }
                    var dirPath = Path.GetDirectoryName(filePath);
                    var newFilePath = Path.Combine(dirPath, $"{ Guid.NewGuid()}_{customer.Image.FileName}");
                    System.IO.File.Move(filePath, newFilePath);
                    StorageHelper.StorageConnectionString = configuration.GetConnectionString("BlobStorageConnection");
                    ViewBag.ImageUrl = await StorageHelper.SaveFileAsync(newFilePath, "images");
                    System.IO.File.Delete(newFilePath);
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
