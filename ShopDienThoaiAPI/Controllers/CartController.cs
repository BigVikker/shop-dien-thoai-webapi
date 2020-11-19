using Models.DAO;
using Models.EF;
using Newtonsoft.Json;
using ShopDienThoaiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopDienThoaiAPI.Controllers
{
    public class CartController : Controller
    {
        [AllowAnonymous]
        [Route("cart")]
        public async Task<ActionResult> Cart()
        {
            return View((List<CartItemModel>)Session["cart"]);
        }

        [Route("checkout")]
        public async Task<ActionResult> Checkout()
        {
            if (Session["cart"] == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            var customer = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(customer))
            {
                return View("Checkout", (List<CartItemModel>)Session["cart"]);
            }
            var item = await new CustomerDAO().LoadByUsername(customer);
            ViewData["Customer"] = item;
            return View("CheckoutCustomer", (List<CartItemModel>)Session["cart"]);
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SubmitCheckoutCustomer(CheckoutModel model)
        {
            try
            {
                var customer = await new CustomerDAO().LoadByUsername(HttpContext.User.Identity.Name);
                var order = await new OrderDAO().AddOrderCustomer(customer.CustomerID, model.CustomerName, model.CustomerAddress, model.CustomerPhone, await GetTotal());
                if (order != 0)
                {
                    try
                    {
                        var orderdetail = new OrderDetailDAO();
                        foreach (var item in (List<CartItemModel>)Session["cart"])
                        {
                            await orderdetail.AddOrderDetail(order, item.product.ProductID, item.quantity);
                        }
                        Session["cart"] = null;
                        return Json(new { Success = true, ID = order }, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        var result = await new OrderDAO().DeleteOrder(order);
                        return Json(new { Success = false, error = "error creating order details" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitCheckout(CheckoutModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var orderid = await new OrderDAO().AddOrder(model.CustomerName, model.CustomerAddress, model.CustomerPhone, await GetTotal());
                    if (orderid != 0)
                    {
                        try
                        {
                            var orderdetail = new OrderDetailDAO();
                            foreach (var item in (List<CartItemModel>)Session["cart"])
                            {
                                await orderdetail.AddOrderDetail(orderid, item.product.ProductID, item.quantity);
                            }
                            Session["cart"] = null;
                            return Json(new JsonStatus()
                            {
                                Status = true,
                                Message = orderid.ToString()
                            }, JsonRequestBehavior.AllowGet);
                            //return Json(new { Success = true, ID = orderid }, JsonRequestBehavior.AllowGet);
                        }
                        catch
                        {
                            var result = await new OrderDAO().DeleteOrder(orderid);
                            return Json(new JsonStatus()
                            {
                                Status = false,
                                Message = "error creating order details"
                            }, JsonRequestBehavior.AllowGet);
                            //return Json(new { Success = false, error = "error creating order details" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new JsonStatus()
                    {
                        Status = false,
                        Message = "Fail create order"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Model State no valid"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Error"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("addtocart")]
        public async Task<JsonResult> OrderNow(int prodId, int quantity)
        {
            //get product by id
            var prod = await GetData(prodId);
            //check null
            if (prod == null)
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "Not available"
                }, JsonRequestBehavior.AllowGet);
            }
            //check quantity 
            if (quantity <= 0)
            {
                return Json(new JsonStatus()
                {
                    Status = false,
                    Message = "So luong khong the nho hon 0"
                }, JsonRequestBehavior.AllowGet);
            }
            //check session exist
            if (Session["cart"] == null)
            {
                //add new if null
                var cart = new List<CartItemModel>();
                cart.Add(new CartItemModel(prod, quantity));
                Session["cart"] = cart;

                return Json(new JsonStatus()
                {
                    Status = true,
                    Message = "Add moi"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cart = (List<CartItemModel>)Session["cart"];
                //check product exist in session
                int index = IsExist(prodId);
                if (index == -1)
                {
                    //add new if product not exist
                    cart.Add(new CartItemModel(prod, quantity));
                    return Json(new JsonStatus()
                    {
                        Status = true,
                        Message = "Add moi"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //append quantity if product exist
                    cart[index].quantity += quantity;
                    return Json(new JsonStatus()
                    {
                        Status = true,
                        Message = "Add so luong"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public async Task<PRODUCT> GetData(int id)
        {
            try
            {
                string apiurl = GlobalVariable.url + "api/product/detail?id={0}";
                apiurl = string.Format(apiurl, id);

                string json = await new GlobalVariable().GetApiAsync(apiurl);
                var prod = JsonConvert.DeserializeObject<PRODUCT>(json);

                return prod;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Delete(int id)
        {
            int index = IsExist(id);
            List<CartItemModel> cart = (List<CartItemModel>)Session["cart"];
            cart.RemoveAt(index);
            if (cart.Count == 0)
            {
                Session["cart"] = null;
            }
            return RedirectToAction("Cart", "Cart");
        }

        private int IsExist(int id)
        {
            var cart = (List<CartItemModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].product.ProductID == id)
                    return i;
            return -1;
        }

        public async Task<ActionResult> CartPartial()
        {
            return PartialView("_Cart", (List<CartItemModel>)Session["cart"]);
        }

        public async Task<decimal> GetTotal()
        {
            decimal total = 0;

            var cart = (List<CartItemModel>)Session["cart"];
            if (cart != null)
            {
                var dao = new ProductDAO();
                foreach (var item in cart)
                {
                    var product = await dao.LoadByID(item.product.ProductID);
                    if (product.PromotionPrice.HasValue)
                    {
                        total += product.PromotionPrice.Value * item.quantity;
                    }
                    else
                    {
                        total += product.ProductPrice * item.quantity;
                    }
                }
            }
            return total;
        }
    }
}