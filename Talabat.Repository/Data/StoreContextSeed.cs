using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbcontext)
        {
            if (!dbcontext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await dbcontext.Set<ProductBrand>().AddAsync(brand);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            
            

            //seeding types

            if(!dbcontext.ProductTypes.Any())
            {
                var BrandTypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var BrandTypes = JsonSerializer.Deserialize<List<ProductType>>(BrandTypesData);
                if (BrandTypes?.Count > 0)
                {
                    foreach (var brandType in BrandTypes)
                    {
                        await dbcontext.Set<ProductType>().AddAsync(brandType);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }

            //seeding products
            if (!dbcontext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbcontext.Set<Product>().AddAsync(product);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }


        }
    }
}
