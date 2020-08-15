using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopDienThoaiAPI.Models
{
    public class RegisterCustomer
    {
        [Display(Name = "Tên đăng nhập")]
        [StringLength(250)]
        [Required(ErrorMessage = "Không thể trống")]
        public string CustomerUsername { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(250, MinimumLength = 6)]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        [Required(ErrorMessage = "Không thể trống")]
        public string CustomerPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("CustomerPassword", ErrorMessage = "Mật khẩu không giống")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(250)]
        public string CustomerEmail { get; set; }

        [Display(Name = "Họ tên")]
        [StringLength(250)]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string CustomerName { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        public string CustomerPhone { get; set; }
    }
    public class LoginModel
    {
        [Display(Name = "Tên đăng nhập")]
        [StringLength(250)]
        [Required(ErrorMessage = "Không thể trống")]
        public string CustomerUsername { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(250, ErrorMessage = "Quá kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        [Required(ErrorMessage = "Không thể trống")]
        public string CustomerPassword { get; set; }
    }
    public class CustomerSessionModel
    {
        public int CustomerID { get; set; }
        public string CustomerUsername { get; set; }
    }
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}