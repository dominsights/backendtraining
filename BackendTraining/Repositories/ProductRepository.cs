using BackendTraining.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackendTraining.Repositories
{
    public class ProductRepository
    {
        public Product[] GetAll()
        {
            //simulate delay to retrieve the products list
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < 1000)
            {
                Thread.SpinWait(1000);
            }

            var products = new Product[]
            {
                new Product { Description = "Bottle of water", Quantity = 10, Price = 1.5 },
                new Product { Description = "French fries", Quantity = 15, Price = 2.5 },
                new Product { Description = "Snack", Quantity = 7, Price = 5 }
            };

            return products;
        }
    }
}