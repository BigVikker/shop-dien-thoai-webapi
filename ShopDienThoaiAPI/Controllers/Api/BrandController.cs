using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        [HttpPost]
        [Route("api/brand/create")]
        public async Task<IHttpActionResult> CreateBrand(BRAND brand)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                int result = await new BrandDAO().CreateBrand(brand);
                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    var item = Content(HttpStatusCode.BadRequest, "Create Fail");
                    return item;
                }
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Create Fail");
            }
        }

        [HttpPut]
        [Route("api/brand/update")]
        public async Task<IHttpActionResult> UpdateBrand(int id, BRAND brand)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                int result = await new BrandDAO().UpdateBrand(id, brand);
                if (result == 0)
                {
                    return Content(HttpStatusCode.Conflict, "Not updated");
                }
                return Ok();
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Update Fail");
            }
        }

        [HttpDelete]
        [Route("api/brand/delete")]
        public async Task<IHttpActionResult> DeleteBrand(int id)
        {
            try
            {
                bool result = await new BrandDAO().DeleteBrand(id);
                if (result)
                {
                    return Ok();
                }
                return Content(HttpStatusCode.Conflict, "Not deleted");
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Delete Fail");
            }
        }
    }
}
