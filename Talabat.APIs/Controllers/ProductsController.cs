using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo, IMapper mapper, IGenericRepository<ProductType> TypeRepo, IGenericRepository<ProductBrand> BrandRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _typeRepo = TypeRepo;
            _brandRepo = BrandRepo;
        }
        //get all products


        //refactor to apply specification
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams @params)
        {
            // var products = await _productRepo.GetAllAsync();
            var spec= new ProductWithBrandAndTypeSpecifications(@params);//hycall first ctor create obj of criteria, includes
            var products = await _productRepo.GetAllWithSpecAsync(spec);
            var mappedProducts= _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);//to create flat response without nested obj
            var countSpec = new ProductWithFiltrationForCountAsync(@params);
            var count =await _productRepo.GetCountWithSpecAsync(spec);
            var ReturnedObj = new Pagination<ProductToReturnDto>()
            {
                PageIndex=@params.PageIndex,
                PageSize=@params.PageSize,
                Data=mappedProducts
            };
            //m3ndesh view arg3 feh f hrg3 f obj
            //OkObjectResult result = new OkObjectResult(products);
            return Ok(ReturnedObj);

            //i need to map products into dto
        }

        //get product by id
        [HttpGet("{id}")]
        //response can be of more than 1 shape
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product=await _productRepo.GetByIdWithSpecAsync(spec);
            if (product == null) { return NotFound(new ApiResponse(404)); }
            var mappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(mappedProduct);
        }

        [HttpGet("Types")] //BaseUrl/api/Products/Types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types=await _typeRepo.GetAllAsync();//msh mhtaga spec hena
            return Ok(types);
        }

        [HttpGet("Brands")] //BaseUrl/api/Products/Types
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();//msh mhtaga spec hena
            return Ok(brands);
        }

    }
}


/*pagination
 * dbcontext.products.orderBy(p=>p.name).skip(2).take(2).include(p=>p.productType).include(p=>p.productBrand)
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */