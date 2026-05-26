using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCpractices.Models;
using MVCpractices.Services;

namespace MVCpractices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AiService _aiService;

        public HomeController(ILogger<HomeController> logger,AiService aiService)
        {
            _logger = logger;
            _aiService = aiService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetPeopleData()
        {
            var people = new List<Person>
    {
        new Person { FirstName="John", LastName="Doe", Country="USA" },   // Index 0
        new Person { FirstName="Jane", LastName="Smith", Country="UK" },  // Index 1
        new Person { FirstName="Bob", LastName="Johnson", Country="USA" } // Index 2
    };

            // Logic: Get the indices of all people from the USA
            var highlightedIndices = people
                .Select((person, index) => new { person, index })
                .Where(x => x.person.Country == "USA")
                .Select(x => x.index)
                .ToList();

            return Json(new
            {
                data = people,
                highlightRows = highlightedIndices
            });
        }
        [HttpPost]
        public IActionResult SavePeopleData([FromBody] List<Person> updatedPeople)
        {
            if (updatedPeople == null || updatedPeople.Count == 0)
            {
                return BadRequest("No data received.");
            }

            // Process your data here (e.g., save to Database)
            foreach (var person in updatedPeople)
            {
                // Debug: Console.WriteLine(person.FirstName);
            }

            return Ok(new { message = "Success" });
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

        [HttpPost]
        public IActionResult GetAiResponse(string prompt)
        {
            string aiResponse = _aiService.GetAiResponse(prompt).Result;
            return Json(aiResponse);
        }

    }
}
