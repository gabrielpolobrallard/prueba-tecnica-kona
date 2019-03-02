using PruebaTecnica.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Service
{
    public static class UserService
    {

        public static async Task<Root> GetResults()
        {
            var result = await GetResults("https://randomuser.me/api?results=100");
            return Root.FromJson(result);
        }

        private static async Task<string> GetResults(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(new Uri(url));

                return response;
            }
        }
    }
}
