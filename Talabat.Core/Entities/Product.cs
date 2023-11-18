using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }//the migration problem with decimal is that he doesn't know 
        //what to convert it into in the db so he decides it to be decimal(18,2), gives warning, i will go and configure it in productconfig

        //represent relationship product-productbrand M-1 >> pk of 1 as fk in many, same for productType
        //it's supposed to add collection of product to represent many in productbrand, but i won't do it bec i don't need it
        //in my buisness, i will configure it elsewhere
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
