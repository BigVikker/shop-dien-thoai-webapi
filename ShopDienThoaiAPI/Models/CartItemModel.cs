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
}