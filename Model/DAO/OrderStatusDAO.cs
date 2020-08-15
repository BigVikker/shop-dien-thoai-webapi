using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderStatusDAO
    {
        ShopDienThoaiAPI db = null;
        public OrderStatusDAO()
        {
            db = new ShopDienThoaiAPI();
        }

        public async Task<List<ORDERSTATU>> LoadStatus()
        {
            return await db.ORDERSTATUS.AsNoTracking().ToListAsync();
        }

        public async Task<ORDERSTATU> LoadByID(int id)
        {
            var item = await db.ORDERSTATUS.AsNoTracking().Where(x => x.StatusID == id).FirstAsync();
            return item;
        }
    }
}
