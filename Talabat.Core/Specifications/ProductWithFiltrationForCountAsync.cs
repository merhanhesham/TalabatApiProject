using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiltrationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams Params) : base(p =>
            (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)//lw brandid feh value,exp=false, f hydkhol f b3d or, w ygeb elproduct, whole exp=true
            &&
            (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            )
        {
            
        }
    }
}
