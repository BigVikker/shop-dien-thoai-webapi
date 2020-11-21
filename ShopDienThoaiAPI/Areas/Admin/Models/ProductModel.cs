using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopDienThoaiAPI.Areas.Admin.Models
{
    public class ProductModel
    {
        [Display(Name = "Name")]
        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        [Required]
        public decimal ProductPrice { get; set; }


        [Display(Name = "Decription")]
        public string ProductDescription { get; set; }

        [Display(Name = "Promotion Price")]
        public decimal? PromotionPrice { get; set; }

        [Display(Name = "Stock")]
        public int? ProductStock { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Select Brand")]
        public int BrandID { get; set; }

        [Display(Name = "Product Image")]
        [StringLength(250)]
        public string ProductImage { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ProductStatus { get; set; }
    }
}