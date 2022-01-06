using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteAccessManager.Adapters;
using SiteAccessManager.Models;

namespace SiteAccessManager.Services
{
    public class AccessService
    {
        static readonly HttpClient client = new HttpClient();

        private string Url = "https://api.countapi.xyz";

        private async Task<AccessAdapter> SiteHits(string endpint)
        {
            HttpResponseMessage response = await client.GetAsync(endpint);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Access access = JsonConvert.DeserializeObject<Access>(responseBody);

            return new AccessAdapter(access);
        }

        public async Task<AccessAdapter> Hit(Site site) {
            try
            {
                string endpint = $"{this.Url}/hit/{site.Name}/{site.Name}{site.Id}";
                return await SiteHits(endpint);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }

        public async Task<AccessAdapter> Hits(Site site) {
            try	
            {
                string endpint = $"{this.Url}/get/{site.Name}/{site.Name}{site.Id}";
                Console.WriteLine(endpint);
                return await SiteHits(endpint);
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
    }
}