using Models.DAO;
using Models.EF;
using Newtonsoft.Json;
using ShopDienThoaiAPI.Controllers;
using ShopDienThoaiAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopDienThoaiAPI.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ProductController : BaseController
    {
        public async Task<ActionResult> ProductDetail(int id)
        {
            ViewBag.Brand = await new BrandDAO().LoadData();

            var item = await new ProductDAO().LoadByID(id);
            return View(item);
        }

        public async Task<ActionResult> ProductList()
        {
            return View(await new ProductDAO().LoadProduct());
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductPartial(int brandid)
        {
            return PartialView("CreateProductPartial", await new BrandDAO().LoadByID(brandid));
        }

        [HttpPost]
        public async Task<JsonResult> CreateProduct(PRODUCT model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            string url = GlobalVariable.url + "api/product/create";

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);

                var response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new JsonStatus()
                    {
                        Status = true,
                        Message = "Create Success",
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonStatus()
                    {
                        Status = false,
                        Message = response.ReasonPhrase,
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Error while creating product",
                    StatusCode = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> UpdateProduct(int id, PRODUCT model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            string url = GlobalVariable.url + "api/product/update?id=" + id;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);

                var response = await client.PutAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new JsonStatus()
                    {
                        Status = true,
                        Message = "Update Success",
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonStatus()
                    {
                        Status = false,
                        Message = response.ReasonPhrase,
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Error while updating product",
                    StatusCode = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            string url = GlobalVariable.url + "api/product/delete?id=" + id;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);

                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new JsonStatus()
                    {
                        Status = true,
                        Message = "Delete Success",
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonStatus()
                    {
                        Status = false,
                        Message = response.ReasonPhrase,
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Error while deleting product",
                    StatusCode = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}