using Models.DAO;
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
            return View(await GetCartItem());
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
                return View("Checkout", await GetCartItem());
            }
            var item = await new CustomerDAO().LoadByUsername(customer);
            ViewData["Customer"] = item;
            return View("CheckoutCustomer", await GetCartItem());
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
                        foreach (var item in (List<CartSession>)Session["cart"])
                        {
                            await orderdetail.AddOrderDetail(order, item.ProductID, item.Quantity);
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
                    var order = await new OrderDAO().AddOrder(model.CustomerName, model.CustomerAddress, model.CustomerPhone, await GetTotal());
                    if (order != 0)
                    {
                        try
                        {
                            var orderdetail = new OrderDetailDAO();
                            foreach (var item in (List<CartSession>)Session["cart"])
                            {
                                await orderdetail.AddOrderDetail(order, item.ProductID, item.Quantity);
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
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult OrderNow(int prodId, int quantity)
        {
            if (quantity == 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            if (Session["cart"] == null)
            {
                var cart = new List<CartSession>();
                cart.Add(new CartSession(prodId, quantity));
                Session["cart"] = cart;

                return Json(new { Success = true, msg = "null, add moi" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cart = (List<CartSession>)Session["cart"];
                int index = IsExist(prodId);
                if (index == -1)
                {
                    cart.Add(new CartSession(prodId, quantity));
                    return Json(new { Success = true, msg = "add moi" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cart[index].Quantity += quantity;
                    return Json(new { Success = true, msg = "them sl" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete(int id)
        {
            int index = IsExist(id);
            List<CartSession> cart = (List<CartSession>)Session["cart"];
            cart.RemoveAt(index);
            if (cart.Count == 0)
            {
                Session["cart"] = null;
            }
            return RedirectToAction("Cart", "Cart");
        }

        private int IsExist(int id)
        {
            var cart = (List<CartSession>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].ProductID == id)
                    return i;
            return -1;
        }

        public async Task<ActionResult> CartPartial()
        {
            return PartialView("_Cart", await GetCartItem());
        }

        public async Task<List<CartItemModel>> GetCartItem()
        {
            var list = new List<CartItemModel>();
            var session = (List<CartSession>)Session["cart"];
            if (session != null)
            {
                foreach (var item in session)
                {
                    list.Add(new CartItemModel(
                        await new ProductDAO().LoadByID(item.ProductID),
                        item.Quantity));
                }
            }
            return list;
        }

        public async Task<decimal> GetTotal()
        {
            decimal total = 0;

            var cart = (List<CartSession>)Session["cart"];
            if (cart != null)
            {
                var dao = new ProductDAO();
                foreach (var item in cart)
                {
                    var product = await dao.LoadByID(item.ProductID);
                    if (product.PromotionPrice.HasValue)
                    {
                        total += product.PromotionPrice.Value * item.Quantity;
                    }
                    else
                    {
                        total += product.ProductPrice * item.Quantity;
                    }
                }
            }
            return total;
        }
    }
}