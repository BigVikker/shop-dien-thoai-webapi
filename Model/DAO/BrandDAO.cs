using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class BrandDAO
    {
        ShopDienThoaiAPI db = null;
        public BrandDAO()
        {
            db = new ShopDienThoaiAPI();
        }

        public async Task<List<BRAND>> LoadData()
        {
            return await db.BRANDs.AsNoTracking().ToListAsync();
        }

        public async Task<BRAND> LoadByID(int id)
        {
            return await db.BRANDs.FindAsync(id);
        }

        public async Task<BRAND> LoadByURL(string url)
        {
            return await db.BRANDs
                .AsNoTracking()
                .Where(x => x.BrandURL.Equals(url))
                .SingleOrDefaultAsync();
        }

        public async Task<int> CreateBrand(BRAND brand)
        {
            try
            {
                BRAND item = new BRAND()
                {
                    BrandName = brand.BrandName,
                    BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(brand.BrandName),
                    CreatedDate = DateTime.Now
                };
                db.BRANDs.Add(item);
                await db.SaveChangesAsync();
                return item.BrandID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteBrand(int ID)
        {
            try
            {
                var prod = await db.BRANDs.Where(x => x.BrandID == ID).SingleOrDefaultAsync();
                db.BRANDs.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> UpdateBrand(int ID, BRAND brand)
        {
            try
            {
                var item = db.BRANDs.Find(ID);
                if (item == null)
                {
                    return 0;
                }
                item.BrandName = brand.BrandName;
                item.BrandURL = SlugGenerator.SlugGenerator.GenerateSlug(brand.BrandName);
                await db.SaveChangesAsync();
                return item.BrandID;
            }
            catch
            {
                return 0;
            }
        }
    }
}
