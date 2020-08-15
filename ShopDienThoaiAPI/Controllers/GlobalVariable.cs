using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ShopDienThoaiAPI.Controllers
{
    public class GlobalVariable
    {
        public async Task<string> GetApiAsync(string url)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage getResponse = await httpClient.GetAsync(url);
            var responseJsonString = await getResponse.Content.ReadAsStringAsync();

            return responseJsonString;
        }

        //public async Task<string> PostApiAsync(string url)
        //{
        //    var httpClient = new HttpClient();
        //    HttpResponseMessage getResponse = await httpClient.PostAsync();
        //    var responseJsonString = await getResponse.Content.ReadAsStringAsync();

        //    return responseJsonString;
        //}
    }
}