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
using Newtonsoft.Json;

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
        public async Task<JsonResult> LoginCustomer(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string url = GlobalVariable.url + "api/customer/login";

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

                        FormsAuthentication.SetAuthCookie(model.CustomerUsername, true);
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

        [HttpPost]
        public async Task<JsonResult> RegisterCustomer(RegisterCustomer model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            string url = GlobalVariable.url + "api/customer/register";

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
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return Json(new JsonStatus()
                    {
                        Status = false,
                        Message = "Username has already exist",
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
                    Message = "Error while creating account",
                    StatusCode = 0
                }, JsonRequestBehavior.AllowGet);
            }
            //var dao = new CustomerDAO();
            //if (!await dao.CheckUser(model.CustomerUsername))
            //{
            //    try
            //    {
            //        int result = await new CustomerDAO().Register(new CUSTOMER()
            //        {
            //            CustomerUsername = model.CustomerUsername,
            //            CustomerPassword = model.CustomerPassword,
            //            CustomerName = model.CustomerName,
            //            CustomerPhone = model.CustomerPhone,
            //            CustomerEmail = model.CustomerEmail,
            //            CreatedDate = DateTime.Now
            //        });
            //        return Json(new JsonStatus()
            //        {
            //            Status = true,
            //            Message = "Create Success",
            //            StatusCode = 1
            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch
            //    {
            //        return Json(new JsonStatus()
            //        {
            //            Status = false,
            //            Message = "Create Fail",
            //            StatusCode = 2
            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //    return Json(new JsonStatus()
            //    {
            //        Status = false,
            //        Message = "Username has already exist",
            //        StatusCode = 0
            //    }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            CustomerToken = null;
            return RedirectToAction("Login", "Customer");
        }

        [Authorize]
        [Route("profile")]
        public async Task<ActionResult> CustomerProfile()
        {
            var membername = HttpContext.User.Identity.Name;
            return View(await new CustomerDAO().LoadByUsername(membername));
        }
    }
}