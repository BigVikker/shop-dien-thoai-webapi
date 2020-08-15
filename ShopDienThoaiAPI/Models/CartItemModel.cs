using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DAO;
using Models.EF;

namespace ShopDienThoaiAPI.Models
{
    public class CartItemModel
    {
        public PRODUCT product { get; set; }
        public int quantity { get; set; }

        public CartItemModel()
        { }

        public CartItemModel(PRODUCT product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }
    public class CartSession
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public CartSession()
        { }

        public CartSession(int ProductID, int Quantity)
        {
            this.ProductID = ProductID;
            this.Quantity = Quantity;
        }
    }
}