using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles() {
            CreateMap <Product,ProductToReturnDto> ()
                //              destination                 source
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))//map from producttype.name inside product into producttype in ProductToReturnDto
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom (s=>s.ProductBrand.Name))
                .ForMember(p=>p.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>())
                ;

            CreateMap<AppUser, UserDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }

    }
}
