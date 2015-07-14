using System.Threading.Tasks;

namespace NetatmoBot.Services.Wrappers
{
    public interface IHttpWrapper
    {
        Task<T> ReadGet<T>(string url);
    }
}