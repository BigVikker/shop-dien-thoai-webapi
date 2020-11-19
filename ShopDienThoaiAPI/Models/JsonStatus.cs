using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDienThoaiAPI.Models
{
    public class JsonStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}