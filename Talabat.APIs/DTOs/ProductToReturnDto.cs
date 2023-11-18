using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    //i did this bec, in the previous response i had a productBrandId prop and also nested object of productBrand
    //that has id inside, so i wanted to have only the productBrandId, so i mapped from product to ProductToReturnDto inside product controller
    //bec frontend doesn't need two ids or nested obj
    public class ProductToReturnDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }
        //public ProductBrand ProductBrand { get; set; } msh mhtaga productbrand k obj, la 3yza bs name of product
        public string ProductBrand { get; set; }

        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
    }
}
