using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;

namespace Novibet.Service.IpGeolocation.Common.Interfaces
{
    public interface IServiceProxy
    {
        IRestResponse<T> Request<T>(string uri, HttpMethod httpMethod, object bodyObject);
        T RequestData<T>(string uri, HttpMethod httpMethod, object bodyObject = null);
        Task<IRestResponse<T>> RequestAsync<T>(string uri, HttpMethod httpMethod, object bodyObject);
        Task<T> RequestDataAsync<T>(string uri, HttpMethod httpMethod, object bodyObject);
    }
}
