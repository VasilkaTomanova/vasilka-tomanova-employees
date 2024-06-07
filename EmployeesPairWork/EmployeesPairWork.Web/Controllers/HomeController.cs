using CsvHelper;
using CsvHelper.Configuration;
using EmployeesPairWork.Models;
using EmployeesPairWork.Services;
using EmployeesPairWork.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Web.Helpers;

namespace EmployeesPairWork.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileReaderService _fileService;
        private readonly IRenderViewService _renderService;
        public HomeController(ILogger<HomeController> logger, IFileReaderService fileService, IRenderViewService renderService)
        {
            _logger = logger;
            _fileService = fileService;
            _renderService = renderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(FileInputModel input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                List<CsvMappingModel> fileResult = await _fileService.GetAllRowsFromFile(input);
                List<PairViewModel> viewResult = await _renderService.GetFilteredEmpoyees(fileResult);
                input.Employees = viewResult;
                return View(input);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(HomeController.Error));
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}