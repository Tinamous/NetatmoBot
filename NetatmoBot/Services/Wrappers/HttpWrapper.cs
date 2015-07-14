using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using NetatmoBot.Exceptions;

namespace NetatmoBot.Services.Wrappers
{
    public class HttpWrapper : IHttpWrapper
    {
        public async Task<T> ReadGet<T>(string url)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Read failed. Url: " + url);
                throw new NetatmoReadException("Failed to read from service. Status code: " + response.StatusCode + ". Url: " + url);
            }

            string responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Get: " + responseText);

            return await response.Content.ReadAsAsync<T>();
        }
    }
}