using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendTraining.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<Product> Products()
        {
            var products = new Product[]
            {
                new Product { Description = "Bottle of water", Quantity = 10, Price = 1.5 },
                new Product { Description = "French fries", Quantity = 15, Price = 2.5 },
                new Product { Description = "Snack", Quantity = 7, Price = 5 }
            };

            return products;
        }

        public class Product
        {
            public string Description { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }
    }
}