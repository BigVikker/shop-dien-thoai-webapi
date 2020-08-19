using Models.DAO;
using Models.EF;
using ShopDienThoaiAPI.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShopDienThoaiAPI.Controllers.Api
{
    public class CustomerController : ApiController
    {
        [Authorize, HttpGet, Route("api/customer/loadbyusername")]
        public async Task<CUSTOMER> LoadCustomer(string username)
        {
            return await new CustomerDAO().LoadByUsername(username);
        }

    }
}
