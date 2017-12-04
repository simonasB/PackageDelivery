using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageDelivery.UI.Api
{
    public interface IApiService
    {
        Task<T> Delete<T>(string uri) where T : class;
        Task<T> Get<T>(string uri) where T : class;
        Task<List<T>> GetMany<T>(string uri) where T : class;
        Task<T> Post<T>(string uri, T value) where T : class;
        Task<T> Put<T>(string uri, T value) where T : class;
    }
}