using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebApiCore.Classes
{
    public class ProductQueryParameters:QueryParameters
    {
        public string sku { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public string OrderOrder { get; set; }
    }
}
