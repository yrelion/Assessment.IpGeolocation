using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Assessment.IpGeolocation.Common.Interfaces;
using Assessment.IpGeolocation.Proxies.Models;
using RestSharp;

namespace Assessment.IpGeolocation.Proxies.Services
{
    public class ServiceProxyBase
    {
        protected IServiceProxy Proxy { get; set; }
        protected string UriPrefix { get; set; }

        protected IRestResponse<T> Request<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            try
            {
                return Proxy.Request<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
            }
            catch (Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }
        }

        protected T RequestData<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            try
            {
                return Proxy.RequestData<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
            }
            catch (Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }
        }

        protected async Task<T> RequestDataAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            try
            {
                return await Proxy.RequestDataAsync<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
            }
            catch (Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }
        }

        protected async Task<IRestResponse<T>> RequestAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            try
            {
                return await Proxy.RequestAsync<T>($"{UriPrefix}{uri}", httpMethod, bodyObject);
            }
            catch (Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }
        }
    }
}
