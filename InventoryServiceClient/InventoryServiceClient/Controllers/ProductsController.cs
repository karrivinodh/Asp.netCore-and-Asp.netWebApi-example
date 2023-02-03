using Microsoft.AspNetCore.Mvc;

//Added...
using InventoryServiceClient.Models; //for Products
using System.Net.Http; //for HttpClient
using System.Net.Http.Headers; //for GetStringAsync
using Microsoft.AspNetCore.Http; //for Session
using Newtonsoft.Json; //for JsonConvert

namespace InventoryServiceClient.Controllers
{
    public class ProductsController : Controller
    {
        public static string baseURL = "http://localhost:5015/api/Products";

        public async Task<IActionResult> Index()
        {
            return View(await GetProducts());
        }

        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string JsonStr = await client.GetStringAsync(baseURL);

            var result = JsonConvert.DeserializeObject<List<Product>>(JsonStr).ToList();
            
            return result;
        }
    }
}
