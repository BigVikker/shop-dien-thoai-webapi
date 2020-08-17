using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShopDienThoaiAPI.Controllers.Api
{
    public class BrandController : ApiController
    {
        public async Task<IEnumerable<BRAND>> GetBrand()
        {
            return await new BrandDAO().LoadData();
        }

        [Route("api/brand/getbrand")]
        public async Task<BRAND> GetBrand(int brandid)
        {
            return await new BrandDAO().LoadByID(brandid);
        }
    }
}
