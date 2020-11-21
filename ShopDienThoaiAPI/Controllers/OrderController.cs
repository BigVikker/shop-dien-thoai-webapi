using ShopDienThoaiAPI.Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using Models.EF;

namespace ShopDienThoaiAPI.Controllers
{
    [HandleError]
    public class OrderController : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CancelOrder(int OrderID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVariable.url + "api/order/cancelorder");

                //HTTP POST
                var response = await client.PutAsJsonAsync("order", new ORDER()
                {
                    OrderID = OrderID
                });

                if (response.IsSuccessStatusCode)
                {

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Success = false, Msg = response.ReasonPhrase }, JsonRequestBehavior.AllowGet);

                }
            }
        }

        [Authorize]
        public async Task<ActionResult> OrderList(int? StatusID)
        {
            var membername = HttpContext.User.Identity.Name;

            string apiurl = GlobalVariable.url + "api/customer/loadbyusername?username=" + membername;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CustomerController.CustomerToken);
                var response = await client.GetStringAsync(apiurl);
                var customer = JsonConvert.DeserializeObject<CUSTOMER>(response);

                if (customer != null)
                {
                    apiurl = GlobalVariable.url + "api/order/getorder?customerid={0}";
                    apiurl = string.Format(apiurl, customer.CustomerID);

                    string json = await client.GetStringAsync(apiurl);
                    var list = JsonConvert.DeserializeObject<List<ORDER>>(json);

                    if (!StatusID.Equals(0))
                    {
                        list = list.Where(x => x.OrderStatusID == StatusID).ToList();
                    }
                    return PartialView("OrderList", list);
                }
            }


            return PartialView("OrderList", null);
        }

        /*
        [Authorize]
        public async Task<ActionResult> OrderProductList(int OrderID)
        {
            return PartialView("OrderProductList", await new OrderDAO().LoadProductOrder(OrderID));
        }

        [Authorize]
        public async Task<JsonResult> GetStatus()
        {
            var status = await new OrderStatusDAO().LoadStatus();
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<JsonResult> GetStatusName(int orderid)
        {
            string apiurl = "https://localhost:44319/api/order/getordername?orderid={0}";
            apiurl = string.Format(apiurl, orderid);
            string token = CustomerController.CustomerToken;

            var request = new HttpRequestMessage(HttpMethod.Get, apiurl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage getResponse = await httpClient.GetAsync(apiurl);
                var json = await getResponse.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<ORDERSTATU>(json);

                return Json(new { name = status }, JsonRequestBehavior.AllowGet);
            }


            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //    var response = client.PostAsJsonAsync(new Uri(apiurl).ToString());

            //string json = await new GlobalVariable().GetApiAsync(apiurl);
            //    return Json(new { Json = json, Token = token }, JsonRequestBehavior.AllowGet);
            //}
        }
        */
    }
}