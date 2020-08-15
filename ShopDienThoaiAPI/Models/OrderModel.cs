using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopDienThoaiAPI.Models
{
    public class OrderModel
    {
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Status ID")]
        public int OrderStatusID { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Ordered Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }
    }

    public class OrderProductModel
    {
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}