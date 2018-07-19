using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Http
{
    public interface IHttpClientProvider
    {
        Task<string> GetAsStringAsync(string uri);
        Task<string> PostAsStringAsync(string uri, object payload);
    }
}