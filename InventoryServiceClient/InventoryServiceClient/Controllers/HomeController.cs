using InventoryServiceClient.Models;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Text;

//Installed and Added..
using Newtonsoft.Json;

namespace InventoryServiceClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() //Make Index action method to show Login Page
        {
            return View();  //Replace the default code of index.cshtml with your UI Code)
        }

        public async Task<IActionResult> LoginUser(UserInfo user)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var httpClient = new HttpClient(clientHandler))
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:5015/api/token", stringContent))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if (token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect Email and Password";
                        return Redirect("~/Home/Index");
                    }

                    HttpContext.Session.SetString("JWToken", token);
                }
                return Redirect("~/Products/Index");
            }
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