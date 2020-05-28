using CommandRestfulAPI.MVC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommandRestfulAPI.MVC.Web.Controllers
{
    /// <summary>
    /// Controller to connect to API
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private const string URL = "https://localhost:44303/api/commands/";
        private const string URL = "https://azurecommandapi.azurewebsites.net/api/commands/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Command> commandList = new List<Command>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(URL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    commandList = JsonConvert.DeserializeObject<List<Command>>(apiResponse);
                }
            }
            return View(commandList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommand(int commandId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(URL + commandId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
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
