﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;

namespace ShopDienThoaiAPI.Controllers
{
    public class GlobalVariable
    {
        public static string url = "https://localhost:44319/";
        public async Task<string> GetApiAsync(string posturl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage Response = await httpClient.GetAsync(posturl);
                if (Response.IsSuccessStatusCode)
                {
                    var responseJsonString = await Response.Content.ReadAsStringAsync();
                    return responseJsonString;
                } else
                {
                    return Response.ReasonPhrase;
                }
            }
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