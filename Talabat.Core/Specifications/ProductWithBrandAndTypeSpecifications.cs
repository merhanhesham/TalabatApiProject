using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications:BaseSpecifications<Product>
    {
        //scenario where brandid=1,typeid=2 for ex>>is valid
        //scenario where no brandid, typeid=2 is also valid
        //this is my buisness logic to be able to filter by both or onlt
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams @params)
            :base(p=>
            (!@params.BrandId.HasValue||p.ProductBrandId== @params.BrandId)//lw brandid feh value,exp=false, f hydkhol f b3d or, w ygeb elproduct, whole exp=true
            &&
            (!@params.TypeId.HasValue||p.ProductTypeId== @params.TypeId) 
            )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            if (!string.IsNullOrEmpty(@params.sort))
            {
                switch(@params.sort)
                {
                    case "PriceAsce":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default: 
                        AddOrderBy(p=> p.Name);
                        break;
                }
            }
            ApplyPagination(@params.PageSize*(@params.PageIndex)-1, @params.PageSize);
        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
