using Models.DAO;
using Models.EF;
using Newtonsoft.Json;
using PagedList;
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
        public async Task<ActionResult> Shop(int? page)
        {
            //ViewBag.search = HttpUtility.UrlEncode(search);
            //ViewBag.sort = sort;
            int pageindex = 1;
            int pagesize = 6;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            string url = GlobalVariable.url + "api/product/getall";
            string json = await new GlobalVariable().GetApiAsync(url);
            var list = JsonConvert.DeserializeObject<List<PRODUCT>>(json);

            IPagedList<PRODUCT> pagedlist = list.ToPagedList(pageindex, pagesize);

            return View("Shop", pagedlist);
        }

        [HttpGet]
        [Route("thuong-hieu/{url}-{id:int}")]
        public async Task<ActionResult> ShopCategory(int id, string url, string sort)
        {
            string apiurl = GlobalVariable.url + "api/brand/getbrand?brandid={0}";
            apiurl = string.Format(apiurl, id);
            string json = await new GlobalVariable().GetApiAsync(apiurl);
            var item = JsonConvert.DeserializeObject<BRAND>(json);

            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand = item;
            ViewBag.sort = sort;
            return View();
        }

        [HttpGet]
        [Route("san-pham/{url}-{id:int}")]
        public async Task<ActionResult> Detail(int id, string url)
        {
            try
            {
                string apiurl = GlobalVariable.url + "api/product/detail?id={0}";
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

        [HttpPost]
        public async Task<JsonResult> GetProductName(string prefix)
        {
            string url = GlobalVariable.url + "api/product";
            string json = await new GlobalVariable().GetApiAsync(url);
            var list = JsonConvert.DeserializeObject<List<PRODUCT>>(json);

            list = list.Where(x => x.ProductName.Contains(prefix)).ToList();
            return Json(new { name = list }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SelectTop(string cond)
        {
            string url = GlobalVariable.url + "api/product";
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