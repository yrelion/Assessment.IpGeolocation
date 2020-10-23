using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Novibet.Service.IpGeolocation.Common.Abstractions;
using RestSharp;

namespace Novibet.Service.IpGeolocation.Common.Models
{
    public class ServiceProxy : IServiceProxy
    {
        public ServiceConfiguration Configuration;
        private string _accessToken { get; set; }
        
        private IRestClient _serviceClient { get; }

        public ServiceProxy(Action<ServiceConfiguration> configuration)
        {
            Configuration = Configuration ?? new ServiceConfiguration();

            configuration?.Invoke(Configuration);
            _serviceClient = new RestClient(Configuration.BaseUrl);
        }

        /// <summary>
        /// Initiates an <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="uri">The URI of the target resource</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        /// <param name="bodyObject">The object to pass to the request body</param>
        public T Request<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            var request = CreateRequestBase(uri, bodyObject);
            var response = ExecuteRequest<T>(request, httpMethod);

            var deserializedResponse = JsonConvert.DeserializeObject<T>(response.Content);

            return deserializedResponse;
        }

        /// <summary>
        /// Initiates an asynchronous <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="uri">The URI of the target resource</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        /// <param name="bodyObject">The object to pass to the request body</param>
        public async Task<IRestResponse<T>> RequestAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            var request = CreateRequestBase(uri, bodyObject);
            var response = await ExecuteRequestAsync<T>(request, httpMethod);

            return response;
        }

        /// <summary>
        /// Initiates an <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="uri">The URI of the target resource</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        /// <param name="bodyObject">The object to pass to the request body</param>
        public T RequestData<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            var request = CreateRequestBase(uri, bodyObject);
            var response = ExecuteRequest<T>(request, httpMethod);

            var deserializedResponse = JsonConvert.DeserializeObject<T>(response.Content);

            return deserializedResponse;
        }

        /// <summary>
        /// Initiates an asynchronous <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="uri">The URI of the target resource</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        /// <param name="bodyObject">The object to pass to the request body</param>
        public async Task<T> RequestDataAsync<T>(string uri, HttpMethod httpMethod, object bodyObject = null)
        {
            var request = CreateRequestBase(uri, bodyObject);
            var response = await ExecuteRequestAsync<T>(request, httpMethod);

            var deserializedResponse = JsonConvert.DeserializeObject<T>(response.Content);

            return deserializedResponse;
        }

        /// <summary>
        /// Creates and decorates an <see cref="IRestRequest"/>
        /// </summary>
        /// <param name="uri">The URI of the target resource</param>
        /// <param name="bodyObject">The object to pass to the request body</param>
        /// <returns>An <see cref="IRestRequest"/></returns>
        private IRestRequest CreateRequestBase(string uri, object bodyObject)
        {
            var request = new RestRequest(uri);
            AddRequestHeaders(request);

            if (Configuration.AccessKey != string.Empty)
                request.AddQueryParameter("access_key", Configuration.AccessKey);

            if (bodyObject != null)
            {
                var jsonBody = JsonConvert.SerializeObject(bodyObject, Formatting.None);
                request.AddJsonBody(jsonBody);
            }

            return request;
        }

        /// <summary>
        /// Adds headers to a <see cref="IRestRequest"/>
        /// </summary>
        /// <param name="request">The <see cref="IRestRequest"/> to add headers to</param>
        private void AddRequestHeaders(IRestRequest request)
        {
            request.AddHeader("Content-Type", Configuration.ContentType);
        }

        /// <summary>
        /// Executes an <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="request">The <see cref="IRestRequest"/> object</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        private IRestResponse<T> ExecuteRequest<T>(IRestRequest request, HttpMethod httpMethod)
        {
            IRestResponse<T> response;

            switch (httpMethod.Method)
            {
                case "GET":
                    response = _serviceClient.Get<T>(request);
                    break;
                case "POST":
                    response = _serviceClient.Post<T>(request);
                    break;
                case "PUT":
                    response = _serviceClient.Put<T>(request);
                    break;
                case "PATCH":
                    response = _serviceClient.Patch<T>(request);
                    break;
                case "DELETE":
                    response = _serviceClient.Delete<T>(request);
                    break;
                default:
                    response = null;
                    break;
            }

            return response;
        }

        /// <summary>
        /// Executes an asynchronous <see cref="IRestRequest"/> with a specific <see cref="HttpMethod"/>
        /// </summary>
        /// <typeparam name="T">The model to map the request result to</typeparam>
        /// <param name="request">The <see cref="IRestRequest"/> object</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/> to use</param>
        private async Task<IRestResponse<T>> ExecuteRequestAsync<T>(IRestRequest request, HttpMethod httpMethod)
        {
            IRestResponse<T> response;

            switch (httpMethod.Method)
            {
                case "GET":
                    response = await _serviceClient.ExecuteAsync<T>(request, Method.GET);
                    break;
                case "POST":
                    response = await _serviceClient.ExecuteAsync<T>(request, Method.POST);
                    break;
                case "PUT":
                    response = await _serviceClient.ExecuteAsync<T>(request, Method.PUT);
                    break;
                case "PATCH":
                    response = await _serviceClient.ExecuteAsync<T>(request, Method.PATCH);
                    break;
                case "DELETE":
                    response = await _serviceClient.ExecuteAsync<T>(request, Method.DELETE);
                    break;
                default:
                    response = null;
                    break;
            }

            return response;
        }
    }
}
