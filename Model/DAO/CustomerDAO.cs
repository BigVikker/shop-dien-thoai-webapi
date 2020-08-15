using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class CustomerDAO
    {
        ShopDienThoaiAPI db = null;
        public CustomerDAO()
        {
            db = new ShopDienThoaiAPI();
        }

        public async Task<List<CUSTOMER>> LoadCustomer()
        {
            return await db.CUSTOMERs.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteCustomer(int ID)
        {
            try
            {
                var cus = await db.CUSTOMERs.Where(x => x.CustomerID== ID).SingleOrDefaultAsync();
                db.CUSTOMERs.Remove(cus);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckUser(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().AnyAsync(x => x.CustomerUsername == username);
        }

        public async Task<CUSTOMER> LoadByID(int id)
        {
            return await db.CUSTOMERs.AsNoTracking().Where(x => x.CustomerID == id).SingleOrDefaultAsync();
        }

        public async Task<CUSTOMER> LoadByUsername(string username)
        {
            return await db.CUSTOMERs.AsNoTracking().Where(x => x.CustomerUsername.Equals(username)).SingleOrDefaultAsync();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            string encrypt = MD5Hash(password);
            var result = await db.CUSTOMERs.AsNoTracking().CountAsync(x => x.CustomerUsername.Equals(username)  && x.CustomerPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool Login(string username, string password)
        {
            string encrypt = MD5Hash(password);
            var result = db.CUSTOMERs.AsNoTracking().Count(x => x.CustomerUsername.Equals(username) && x.CustomerPassword.Equals(encrypt));
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<int> Register(CUSTOMER cus)
        {
            cus.CustomerPassword = MD5Hash(cus.CustomerPassword);
            db.CUSTOMERs.Add(cus);
            await db.SaveChangesAsync();
            return cus.CustomerID;
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
