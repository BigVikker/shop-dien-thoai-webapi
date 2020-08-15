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
        public async Task<IEnumerable<BRAND>> GetPRODUCTs()
        {
            return await new BrandDAO().LoadData();
        }
    }
}
