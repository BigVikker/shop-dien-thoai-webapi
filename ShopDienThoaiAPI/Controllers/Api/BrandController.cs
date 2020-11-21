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

        [HttpPost]
        [Route("api/brand/create")]
        public async Task<IHttpActionResult> CreateBrand(BRAND brand)
        {
            if (!ModelState.IsValid)
                return Content(System.Net.HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                int result = await new BrandDAO().CreateBrand(new BRAND()
                {
                    BrandName = brand.BrandName,
                    BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(brand.BrandName),
                    CreatedDate = DateTime.Now
                });
                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    var item = Content(System.Net.HttpStatusCode.BadRequest, "Create Fail");
                    return item;
                }
            }
            catch
            {
                return Content(System.Net.HttpStatusCode.BadRequest, "Create Fail");
            }
        }

        [HttpPut]
        [Route("api/brand/update")]
        public async Task<IHttpActionResult> UpdateBrand(int id, BRAND brand)
        {
            if (!ModelState.IsValid)
                return Content(System.Net.HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                brand.BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(brand.BrandName);
                int result = await new BrandDAO().EditBrand(brand, id);
                if (result == 0)
                {
                    return Content(System.Net.HttpStatusCode.Conflict, "Not updated");
                }
                return Ok();
            }
            catch
            {
                return Content(System.Net.HttpStatusCode.BadRequest, "Update Fail");
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
                return Content(System.Net.HttpStatusCode.Conflict, "Not deleted");
            }
            catch
            {
                return Content(System.Net.HttpStatusCode.BadRequest, "Delete Fail");
            }
        }
    }
}
