using Microsoft.AspNetCore.Mvc;
using AutoLedger.Services;
using AutoLedger.Models;

namespace AutoLedger.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var processor = new DocumentProcessor();
                var transactions = processor.Process(filePath);

                var db = new DbService();
                db.Insert(transactions);

                ViewBag.Message = "File uploaded & processed successfully!";

                return View("Index", transactions);
            }

            ViewBag.Message = "Please select a file.";
            return View("Index");
        }
    }
}