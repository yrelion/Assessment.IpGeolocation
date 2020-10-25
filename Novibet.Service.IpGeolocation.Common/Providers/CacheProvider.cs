using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Novibet.Service.IpGeolocation.Common.Interfaces;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Common.Providers
{
    /// <summary>
    /// A wrapper around <see cref="IMemoryCache"/> for generic entity retrieval
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IInMemoryCacheSettings _settings;

        public CacheProvider(IMemoryCache cache, InMemoryCacheSettings settings)
        {
            _cache = cache;
            _settings = settings;
        }

        /// <summary>
        /// Gets or creates an <see cref="ICacheEntry"/> with the given resolver
        /// </summary>
        /// <typeparam name="TResponse">The response type</typeparam>
        /// <param name="key">The key to associate the <see cref="ICacheEntry"/> with</param>
        /// <param name="resolver">The resolver to invoke to produce the <see cref="ICacheEntry"/></param>
        /// <returns>The <see cref="ICacheEntry"/></returns>
        public async Task<TResponse> GetOrCreateAsync<TResponse>(object key, Func<Task<TResponse>> resolver)
        {
            return await _cache.GetOrCreateAsync(key, async factory =>
            {
                factory.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow
                        .AddMinutes(_settings.AbsoluteExpirationMinutes))
                });

                return await resolver.Invoke();
            });
        }

        /// <summary>
        /// Removes the <see cref="ICacheEntry"/> matching the provided key
        /// </summary>
        /// <param name="key">The key to remove</param>
        public void Remove(object key)
            => _cache.Remove(key);
    }
}
