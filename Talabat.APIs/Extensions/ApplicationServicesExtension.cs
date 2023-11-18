using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    //when to create extension methods?? if i have some related services
    public static class ApplicationServicesExtension //z3el 34an i can't define ext method inside nongeneric non static class, mhtaga akhleh static
    {                                          //b this kda b2olo en elparam dah hykon caller of method, kda 3ref enha ext method 
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)//services>>container
        {
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            //Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));//34an lw 3ndy productbrand/producttype
                                                                                                  //Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
                                                                                                  //overload takes type of profile only
            Services.AddAutoMapper(typeof(MappingProfiles));
            //handle validation error
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(p => p.ErrorMessage)
                    .ToArray();

                    var ValidationErrorResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return Services;//b3d ma a add inside the container, return it after addition of services inside container
            //w 34an lma a nest haga y3rf ykml 3leh 
            //y3ni lw msln h3ml kda >> builder.Services.AddApplicationServices().AddScoped....
            //lkn msln lw 3mlt kda msh hykml >> builder.Services.AddControllers().Addscoped..., l2nha msh btreturn services container
        }
    }
}
