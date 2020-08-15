using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductDAO
    {
        ShopDienThoaiAPI db = null;
        public ProductDAO()
        {
            db = new ShopDienThoaiAPI();
        }
        public async Task<List<PRODUCT>> SelectCondition(string cond, int number)
        {
            if (cond.Equals("top"))
                return await db.PRODUCTs.OrderBy(x => x.Viewcount).Take(number).ToListAsync();
            else if (cond.Equals("newest"))
                return await db.PRODUCTs.OrderByDescending(x => x.CreatedDate).Take(number).ToListAsync();
            return null;

        }

        public async Task<int> CreateProduct(PRODUCT model)
        {
            try
            {
                db.PRODUCTs.Add(model);
                await db.SaveChangesAsync();
                return model.ProductID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> UpdateProduct(PRODUCT model)
        {
            var product = await db.PRODUCTs.FindAsync(model.ProductID);
            if (product == null)
                return 0;

            product.ProductName = model.ProductName;
            product.ProductDescription = model.ProductDescription;
            product.ProductPrice = model.ProductPrice;
            product.PromotionPrice = model.PromotionPrice;
            product.ProductImage = model.ProductImage;
            product.ProductStock = model.ProductStock;
            product.ProductURL = model.ProductURL;
            product.ProductStatus = model.ProductStatus;

            await db.SaveChangesAsync();
            return product.ProductID;
            //try
            //{
                
            //}
            //catch
            //{
            //    return 0;
            //}
        }

        public async Task<bool> DeleteProduct(int ID)
        {
            try
            {
                var prod = await db.PRODUCTs.FindAsync(ID);
                if (prod == null)
                {
                    return false;
                }
                db.PRODUCTs.Remove(prod);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<PRODUCT> LoadByID(int ID)
        {
            return await db.PRODUCTs.FindAsync(ID);
        }

        public async Task<int> UpdateViewCount(int ID)
        {
            try
            {
                var prod = await db.PRODUCTs.FindAsync(ID);
                if (prod == null)
                    return 0;

                prod.Viewcount += 1;
                await db.SaveChangesAsync();
                return prod.ProductID;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<List<PRODUCT>> LoadProduct()
        {
            return await db.PRODUCTs.AsNoTracking().ToListAsync();
        }

        public async Task<List<PRODUCT>> LoadName(string prefix)
        {;
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductName.Contains(prefix)).ToListAsync();
        }

        public async Task<PRODUCT> LoadNameByID(int id)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PRODUCT>> LoadProduct(int? brandid, string searchString, string sort, int pagesize, int pageindex)
        {
            // get list
            var list = (from s in db.PRODUCTs select s).AsNoTracking();

            if (!String.IsNullOrEmpty(brandid.ToString()))
            {
                list = list.Where(x => x.BrandID == brandid);
            }

            //filter
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.ProductName.Contains(searchString));
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.ProductName);
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.ProductName);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.ProductPrice);
                    break;
                case "price_asc":
                    list = list.OrderBy(s => s.ProductPrice);
                    break;
                default:
                    list = list.OrderBy(s => s.ProductID);
                    break;
            }
            //return
            return await list.Skip(pageindex * pagesize).Take(pagesize).ToListAsync();
        }

        public async Task<PRODUCT> LoadByURL(string url)
        {
            return await db.PRODUCTs.AsNoTracking().Where(x => x.ProductURL == url).FirstOrDefaultAsync();
        }
    }
}
