namespace AspNet5Client
{
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        public IActionResult Index(string error = "")
        {
            ViewBag.error = error;
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Signout()
        {
            //await HttpContext.Authentication.SignOutAsync("DSAUTH");
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("index");
        }

        public async Task<IActionResult> CallApi()
        {
            var token = User.FindFirst("access_token").Value;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetStringAsync("http://localhost:8080/api/user");

                ViewBag.Json = response.ToString();// JArray.Parse(response).ToString();
            }
            catch
            {
                return RedirectToAction("index", new { error = "error in api call" });
            }
            return View();
        }
    }
}