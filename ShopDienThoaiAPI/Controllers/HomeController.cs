using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Models.EF;
using Newtonsoft.Json;

namespace ShopDienThoaiAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {

            return View();
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> NavbarCategory()
        {
            string url = "https://localhost:44319/api/brand";

            string json = await new GlobalVariable().GetApiAsync(url);

            return PartialView("_NavbarCategory", JsonConvert.DeserializeObject<List<BRAND>>(json));
        }
    }
}