﻿using Models.DAO;
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
    //[Authorize]
    public class OrderController : ApiController
    {
        [Route("api/order/getorderlist")]
        public async Task<IEnumerable<ORDER>> GetOrderList(int customerid)
        {
            var list = await new OrderDAO().LoadOrder(customerid);
            return list;
        }

        [Route("api/order/getproductlist")]
        public async Task<IEnumerable<ORDERDETAIL>> GetProductList(int orderid)
        {
            return await new OrderDAO().LoadProductOrder(orderid);
        }

        [Authorize, Route("api/order/getordername")]
        public async Task<ORDERSTATU> GetOrderName(int orderid)
        {
            var item = await new OrderDAO().LoadByID(orderid);
            var statusname = await new OrderStatusDAO().LoadByID(item.OrderStatusID.Value);
            return statusname;
        }
    }
}