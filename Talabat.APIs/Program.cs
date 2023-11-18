using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    #region configure services (Add services to the container)
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<StoreContext>(options=>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));//allow DI for dbcontext
    });
    builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
    {
        var Connection = builder.Configuration.GetConnectionString("RedisConnection");
        return ConnectionMultiplexer.Connect(Connection);
    });
    /*
//builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped(typeof( IGenericRepository<>),typeof( GenericRepository<>));//34an lw 3ndy productbrand/producttype
//builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
//overload takes type of profile only
builder.Services.AddAutoMapper(typeof(MappingProfiles));
//handle validation error
builder.Services.Configure<ApiBehaviorOptions>(Options =>
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
*/
    builder.Services.AddApplicationServices();//call extension method that has services
    builder.Services.AddIdentityServices(builder.Configuration);
    builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
        });
#endregion

var app = builder.Build();

        #region Update-Database (instead of npm)
         //StoreContext dbContext = new StoreContext();//create obj of dbcontext, need param,msh h3ml kda  
         //await dbContext.Database.MigrateAsync();
         //do process of asking clr for obj of dbcontext explicitly
         //1-hmsek elservices bt3ty (amsek elscope)
         using var scope=app.Services.CreateScope();//container for services
         var services=scope.ServiceProvider;//actual services
         var LoggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbcontext = services.GetRequiredService<StoreContext>();
                await dbcontext.Database.MigrateAsync();
                //scope.Dispose(); use using instead
                //this cycle will happen once i run program, will check of there are new migrations to be updated to database and update it
                var Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>();
                await Identitydbcontext.Database.MigrateAsync();
                //data seeding
                var UserManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                //add interfaces, identity stores for 
                //put them in extension method
                /*builder.Services.AddIdentity<AppUser, IdentityRole>()
                                .AddEntityFrameworkStores<AppIdentityDbContext>();
                builder.Services.AddAuthentication();*/
                
                await StoreContextSeed.SeedAsync(dbcontext);
            }
            catch(Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during applying the migration");
            }

        #endregion

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerMiddlewares();
    }
    app.UseStatusCodePagesWithReExecute("/errors/{0}");
    app.UseHttpsRedirection();
    app.UseStaticFiles();//load images
    app.UseAuthentication(); 
    app.UseAuthorization();
    
    app.MapControllers();

    app.Run();
   

//cleaning up program class
/*
 * 
 * 
 * 
 * 
 */