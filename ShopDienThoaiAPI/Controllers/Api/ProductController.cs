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
        [AllowAnonymous]
        public async Task<IEnumerable<PRODUCT>> GetProduct()
        {
            return await new ProductDAO().LoadProduct();
        }

        [Authorize, Route("api/product/authorize")]
        public async Task<IEnumerable<PRODUCT>> Authorize()
        {
            var list = await new ProductDAO().LoadProduct();
            list = list.Where(x => x.BrandID == 1).ToList();
            return list;
        }

        public async Task<IEnumerable<PRODUCT>> GetProductList(int brandid, string search, string sort, int pageindex)
        {
            search = HttpUtility.UrlDecode(search);
            int pagesize = 16;
            var list = await new ProductDAO().LoadProduct(brandid, search, sort, pagesize, pageindex);
            return list;
        }

        public async Task<PRODUCT> GetProductDetail(int id)
        {
            return await new ProductDAO().LoadByID(id);
        }
    }
}