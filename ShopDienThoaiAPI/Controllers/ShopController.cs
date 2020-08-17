using Models.DAO;
using Models.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopDienThoaiAPI.Controllers
{
    public class ShopController : Controller
    {
        [HttpGet]
        [Route("shop")]
        public ActionResult Shop(string search, string sort)
        {
            ViewBag.search = HttpUtility.UrlEncode(search);
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("thuong-hieu/{url}-{id:int}")]
        public async Task<ActionResult> ShopCategory(int id, string url, string sort)
        {
            
            var brand = await new BrandDAO().LoadByID(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand = brand;
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("san-pham/{url}-{id:int}")]
        public async Task<ActionResult> Detail(int id, string url)
        {
            try
            {
                string apiurl = "https://localhost:44319/api/product?id={0}";
                apiurl = string.Format(apiurl, id);

                string json = await new GlobalVariable().GetApiAsync(apiurl);
                var prod = JsonConvert.DeserializeObject<PRODUCT>(json);
                if (prod == null)
                {
                    return HttpNotFound();
                }
                return View(prod);
            }
            catch
            {
                return HttpNotFound();
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> ConfigDetail(int id)
        {
            try
            {
                var config = await new ConfigurationDAO().LoadConfig(id);
                if (config == null)
                {
                    return HttpNotFound();
                }
                return PartialView("ConfigDetail", config);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShopPartial(int? brandid, string search, string sort, int pageindex)
        {
            string url = "https://localhost:44319/api/product";
            string json = await new GlobalVariable().GetApiAsync(url);
            var list = JsonConvert.DeserializeObject<List<PRODUCT>>(json);
            int pagesize = 16;

            //filter category
            if (!string.IsNullOrEmpty(brandid.ToString()))
            {
                list = list.Where(x => x.BrandID == brandid).ToList();
            }

            //filter search
            if (!String.IsNullOrEmpty(search))
            {
                list = list.Where(x => x.ProductName.Contains(search)).ToList();
            }
            //sort
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName).ToList();
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.ProductName).ToList();
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.ProductPrice).ToList();
                    break;
                case "price_asc":
                    list = list.OrderBy(s => s.ProductPrice).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.ProductID).ToList();
                    break;
            }
            list = list.Skip(pageindex * pagesize).Take(pagesize).ToList();

            return PartialView("ShopPartial", list);
        }

        [HttpPost]
        public async Task<JsonResult> GetProductName(string prefix)
        {
            return Json(new { name = await new ProductDAO().LoadName(prefix) }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SelectTop(string cond)
        {
            string url = "https://localhost:44319/api/product";
            string json = await new GlobalVariable().GetApiAsync(url);
            var list = JsonConvert.DeserializeObject<List<PRODUCT>>(json);
            int number = 8;

            if (cond.Equals("top"))
                list = list.OrderBy(x => x.Viewcount).Take(number).ToList();
            else if (cond.Equals("newest"))
                list = list.OrderByDescending(x => x.CreatedDate).Take(number).ToList();

            return PartialView("SelectTop", list);
        }
    }
}