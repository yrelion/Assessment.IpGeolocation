﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Assessment.IpGeolocation.Common.Interfaces
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets or creates an <see cref="ICacheEntry"/> with the given resolver
        /// </summary>
        /// <typeparam name="TResponse">The response type</typeparam>
        /// <param name="key">The key to associate the <see cref="ICacheEntry"/> with</param>
        /// <param name="resolver">The resolver to invoke to produce the <see cref="ICacheEntry"/></param>
        /// <returns>The <see cref="ICacheEntry"/></returns>
        Task<TResponse> GetOrCreateAsync<TResponse>(object key, Func<Task<TResponse>> resolver);

        /// <summary>
        /// Removes the <see cref="ICacheEntry"/> matching the provided key
        /// </summary>
        /// <param name="key">The key to remove</param>
        void Remove(object key);
    }
}
