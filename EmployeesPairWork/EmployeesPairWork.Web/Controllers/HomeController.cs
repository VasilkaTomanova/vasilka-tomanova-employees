using CsvHelper;
using CsvHelper.Configuration;
using EmployeesPairWork.Models;
using EmployeesPairWork.Services;
using EmployeesPairWork.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace EmployeesPairWork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileReaderService _fileService;
        public HomeController(ILogger<HomeController> logger, IFileReaderService fileService)
        {
            _logger = logger;
            _fileService = fileService; 
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(FileInputModel input)
        {
            List<CsvMappingModel> result = await _fileService.GetAllRowsFromFile(input);

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