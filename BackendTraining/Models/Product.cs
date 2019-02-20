using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTraining.Models
{
    [Serializable]
    public class Product
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
