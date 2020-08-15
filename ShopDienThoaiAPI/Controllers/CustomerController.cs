using Models.DAO;
using Models.EF;
using ShopDienThoaiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace ShopDienThoaiAPI.Controllers
{
    public class CustomerController : Controller
    {
        public static string CustomerToken = "";

        [Route("login")]
        public ActionResult Login()
        {
            var name = HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(name))
            {
                return RedirectToAction("CustomerProfile", "Customer", new { username = name });
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ValidateUser(LoginModel model, bool RememberMe)
        {
            if (ModelState.IsValid)
            {
                string url = "https://localhost:44319/api/customer/login";

                //using (var client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                //    var response = await client.GetStringAsync(url);

                //}

                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", model.CustomerUsername},
                    {"password", model.CustomerPassword},
                });

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(url, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var obj = JObject.Parse(responseString);

                        CustomerToken = (string)obj["access_token"];

                        FormsAuthentication.SetAuthCookie(model.CustomerUsername, RememberMe);
                        return Json(new
                        {
                            Success = true,
                            Username = model.CustomerUsername,
                            Status = response.StatusCode,
                            Token = CustomerToken
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Success = false, Status = response.StatusCode }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> RegisterCustomer(RegisterCustomer model)
        {
            var dao = new CustomerDAO();
            if (!await dao.CheckUser(model.CustomerUsername))
            {
                try
                {
                    int result = await new CustomerDAO().Register(new CUSTOMER()
                    {
                        CustomerUsername = model.CustomerUsername,
                        CustomerPassword = model.CustomerPassword,
                        CustomerName = model.CustomerName,
                        CustomerPhone = model.CustomerPhone,
                        CustomerEmail = model.CustomerEmail,
                        CreatedDate = DateTime.Now
                    });
                    return Json(new { ReturnID = 1 }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { ReturnID = 2 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { ReturnID = 0 }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Customer");
        }

        [Authorize]
        [Route("profile/{username}")]
        public async Task<ActionResult> CustomerProfile(string username)
        {
            var membername = HttpContext.User.Identity.Name;
            if (!membername.Equals(username))
            {
                return View(await new CustomerDAO().LoadByUsername(membername));
            }
            return View(await new CustomerDAO().LoadByUsername(username));
        }
    }
}