using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Models.DAO;
using Models.EF;

namespace ShopDienThoaiAPI.Controllers.Api
{
    public class ProductController : ApiController
    {
        // GET: api/Product
        [Route("api/product/getall")]
        public async Task<IEnumerable<PRODUCT>> GetProduct()
        {
            return await new ProductDAO().LoadProduct();
        }

        [Route("api/product/detail")]
        public async Task<PRODUCT> GetProductDetail(int id)
        {
            return await new ProductDAO().LoadByID(id);
        }

        //[Route("api/product/getname")]
        //public async Task<IEnumerable<PRODUCT>> GetProductName(string prefix)
        //{
        //    return await new ProductDAO().LoadName(prefix);
        //}
    }
}