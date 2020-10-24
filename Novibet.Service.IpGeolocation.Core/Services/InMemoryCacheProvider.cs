using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Novibet.Service.IpGeolocation.Common.Interfaces;

namespace Novibet.Service.IpGeolocation.Core.Services
{
    /// <summary>
    /// A wrapper around <see cref="IMemoryCache"/> for generic entity retrieval
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;

        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets or creates an <see cref="ICacheEntry"/> with the given resolver
        /// </summary>
        /// <typeparam name="TResponse">The response type</typeparam>
        /// <param name="key">The key to associate the <see cref="ICacheEntry"/> with</param>
        /// <param name="resolver">The resolver to invoke to produce the <see cref="ICacheEntry"/></param>
        /// <returns>The <see cref="ICacheEntry"/></returns>
        public TResponse GetOrCreate<TResponse>(object key, Func<TResponse> resolver)
        {
            return _cache.GetOrCreate(key, factory =>
            {
                //TODO: Move elsewhere
                factory.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(1))
                });

                return resolver.Invoke();
            });
        }
    }
}
