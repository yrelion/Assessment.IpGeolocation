using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Novibet.Service.IpGeolocation.Common.Abstractions;
using RestSharp;

namespace Novibet.Service.IpGeolocation.Proxies.Services
{
    public class ServiceProxyBase
    {
        protected IServiceProxy Proxy { get; set; }
        protected string UriPrefix { get; set; }

        protected T Request<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            return Proxy.Request<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
        }

        protected T RequestData<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            return Proxy.RequestData<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
        }

        protected async Task<T> RequestDataAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            return await Proxy.RequestDataAsync<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
        }

        protected async Task<IRestResponse<T>> RequestAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            return await Proxy.RequestAsync<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
        }
    }
}
