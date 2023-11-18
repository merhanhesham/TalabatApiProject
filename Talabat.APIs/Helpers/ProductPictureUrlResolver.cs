using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{                                      //source>>hyygy string from db, db htgeble product, destination, return
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;//3bara 3n key w value 

        //i want to make the pictureurl clickable, to add to link, the base url, w tb3n mynf3sh a store it with baseurl inside database
        //l2no baseurl dah bybaa ellocal bta3y f h3ml function t resolve elink

        //get baseurl , ask clr to inject obj of class that implement iconfig, dah elmasek elappsettings 
        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl)) {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}"; //mfrod adef elbase url bs hwa byt8yr mn environment lltnya zy mn development l server of production f hroh a7oto f appsetting
            }
            return string.Empty;
        }
    }
}
