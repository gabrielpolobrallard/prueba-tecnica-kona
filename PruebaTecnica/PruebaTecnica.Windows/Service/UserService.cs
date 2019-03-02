using PruebaTecnica.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaTecnica.Service
{
    public static class UserService
    {
        public static async Task<Root> GetResults(int page = 1)
        {
            var result = await GetResults(string.Format("https://randomuser.me/api?results=100&page={0}", page));
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