using Models.DAO;
using Models.EF;
using ShopDienThoaiAPI.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ShopDienThoaiAPI.Controllers.Api
{
    public class CustomerController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("api/customer/loadbyusername")]
        public async Task<CUSTOMER> LoadCustomer(string username)
        {
            return await new CustomerDAO().LoadByUsername(username);
        }

        [HttpPost]
        [Route("api/customer/register")]
        public async Task<IHttpActionResult> RegisterCustomer(RegisterCustomer customer)
        {
            if (!ModelState.IsValid)
                return Content(System.Net.HttpStatusCode.BadRequest, "Not invalid model");

            var dao = new CustomerDAO();
            if (!await dao.CheckUser(customer.CustomerUsername))
            {
                try
                {
                    int result = await new CustomerDAO().Register(new CUSTOMER()
                    {
                        CustomerUsername = customer.CustomerUsername,
                        CustomerPassword = customer.CustomerPassword,
                        CustomerName = customer.CustomerName,
                        CustomerPhone = customer.CustomerPhone,
                        CustomerEmail = customer.CustomerEmail,
                        CreatedDate = DateTime.Now
                    });
                    if (result > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return Content(System.Net.HttpStatusCode.BadRequest, "Create Fail");
                    }
                }
                catch
                {
                    return Content(System.Net.HttpStatusCode.BadRequest, "Create Fail");
                }
            }
            else
            {
                return Content(System.Net.HttpStatusCode.Conflict, "Username has already exist");
            }
        }

    }
}
