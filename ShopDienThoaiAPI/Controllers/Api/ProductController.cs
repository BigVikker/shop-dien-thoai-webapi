using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Models.DAO;
using Models.EF;

namespace ShopDienThoaiAPI.Controllers.Api
{
    public class ProductController : ApiController
    {
        // GET: api/Product
        [HttpGet]
        [Route("api/product/getall")]
        public async Task<IEnumerable<PRODUCT>> GetProduct()
        {
            return await new ProductDAO().LoadProduct();
        }

        [HttpGet]
        [Route("api/product/detail")]
        public async Task<PRODUCT> GetProductDetail(int id)
        {
            return await new ProductDAO().LoadByID(id);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/product/create")]
        public async Task<IHttpActionResult> CreateProduct(PRODUCT product)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                int result = await new ProductDAO().CreateProduct(product);
                if (result == 0)
                    return Content(HttpStatusCode.Conflict, "Create Fail");
                else
                    return Ok();
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Create Fail");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("api/product/update")]
        public async Task<IHttpActionResult> UpdateProduct(int id, PRODUCT product)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, "Not invalid model");
            try
            {
                int result = await new ProductDAO().UpdateProduct(id, product);
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

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("api/product/delete")]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            try
            {
                bool result = await new ProductDAO().DeleteProduct(id);
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