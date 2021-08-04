using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiUrl = "https://localhost:5001/Users/123501";
            var data = HttpHelper.GetDataFromApi<User>(apiUrl).Result;

            System.Console.WriteLine("id : " + data.UserId + " name : " + data.Name + " surname : " + data.SurName + " age : "+ data.Age + " birthday : " + data.Birthday
                                        +" state : " + data.address.State + " postal code : "+ data.address.PostalCode ) ;
            
        }
    }

    public class HttpHelper
    {
        public static async Task<T> GetDataFromApi<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                System.Console.WriteLine(resultContentString);
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;
            }
        }
    }

    
}
