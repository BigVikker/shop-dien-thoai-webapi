using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ConfigurationDAO
    {
        ShopDienThoaiAPI db = null;
        public ConfigurationDAO()
        {
            db = new ShopDienThoaiAPI();
        }

        public async Task<CONFIGURATION> LoadConfig(int id)
        {
            return await db.CONFIGURATIONs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<int> CreateConfiguration(CONFIGURATION model)
        {
            try
            {
                if (db.PRODUCTs.FindAsync(model.ProductID) == null)
                    return 0;

                db.CONFIGURATIONs.Add(model);
                await db.SaveChangesAsync();
                return model.ConfigID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
