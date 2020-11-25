using Models.DAO;
using Models.EF;
using Newtonsoft.Json.Linq;
using ShopDienThoaiAPI.Controllers;
using ShopDienThoaiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopDienThoaiAPI.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class AdminController : Controller
    {
        public static string AdminToken = "";
        // GET: Admin/Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ValidateAdmin(ADMIN model)
        {
            if (ModelState.IsValid)
            {
                string url = GlobalVariable.url + "api/token";

                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", "admin|"+model.UserUsername},
                    {"password", model.UserPassword},
                });

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var obj = JObject.Parse(responseString);

                        AdminToken = (string)obj["access_token"];

                        Session["AdminLogin"] = new ADMIN()
                        {
                            UserName = model.UserUsername,
                        };
                        return Json(new JsonStatus()
                        {
                            Status = true,
                            Message = "Login Success"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new JsonStatus()
                        {
                            Status = false,
                            Message = "Login Fail"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new JsonStatus()
            {
                Status = false,
                Message = "Modelstate invalid",
                StatusCode = 0
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["AdminLogin"] = null;
            AdminToken = "";
            return RedirectToAction("Login", "Admin");
        }
    }
}