using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)//allow DI
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        //make 3 endpoints
        //get or recreate
        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var Basket=await _basketRepository.GetBasketAsync(basketId);
            return Basket is null ? new CustomerBasket(basketId) : Basket;
        }
        //update or create new basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var Basket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if(Basket is null) { return BadRequest(new ApiResponse(400)); }
            return Ok(Basket);
        }
        //delete basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomerBasket(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
            
        }
    }
}
