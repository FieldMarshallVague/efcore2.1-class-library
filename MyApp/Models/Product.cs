using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Models
{
    public class Product : Entity<long>
    {
        public float BuyPrice { get; set; }
        public float SellPrice { get; set; }
    }
}
