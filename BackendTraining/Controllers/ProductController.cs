using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BackendTraining.Models;
using BackendTraining.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace BackendTraining.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IDistributedCache _cache;

        [HttpGet("[action]")]
        public async Task<IEnumerable<Product>> Products()
        {
            var cachedProducts = await _cache.GetAsync("products");

            if (cachedProducts != null)
            {
                using (var stream = new MemoryStream())
                {
                    await stream.WriteAsync(cachedProducts, 0, cachedProducts.Length);
                    stream.Position = 0;

                    var formatter = new BinaryFormatter();
                    return (Product[])formatter.Deserialize(stream);
                }
            }

            var productRepository = new ProductRepository();
            var products = productRepository.GetAll();

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, products);

                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                await _cache.SetAsync("products", stream.ToArray());
            }

            return products;
        }

        public ProductController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public class ProductCachedTest
        {
            public string CachedTimeUTC { get; set; }
            public Product[] Products { get; set; }
        }
    }
}