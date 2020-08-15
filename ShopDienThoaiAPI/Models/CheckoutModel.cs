using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopDienThoaiAPI.Models
{
    public class CheckoutModel
    {
        [Required]
        [StringLength(255)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        public string CustomerPhone { get; set; }

        [Required]
        [StringLength(255)]
        public string CustomerAddress { get; set; }
    }
}