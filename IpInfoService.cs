using System.Net.Http;
using Newtonsoft.Json;

namespace IpConsoleApp
{
    public static class IpInfoService
    {
        public static async Task<IpData?> GetIpDataAsync(HttpClient client, string ip)
        {
            try
            {
                var url = $"https://ipinfo.io/{ip}/json";
                var response = await client.GetStringAsync(url);

                dynamic json = JsonConvert.DeserializeObject(response);

                return new IpData
                {
                    Ip = json.ip,
                    Country = json.country,
                    City = json.city
                };
            }
            catch
            {
                return null;
            }
        }
    }
}