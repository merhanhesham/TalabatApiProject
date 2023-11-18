using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        //register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) //recieves obj of registerDto, return obj of userDto
        {
            if(CheckEmailExists(model.Email).Result.Value) { return BadRequest(new ApiResponse(400, "this email already exists")); }

            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }
            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =await _tokenService.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }

        //login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login (LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) { return Unauthorized(new ApiResponse(401)); }
            //takes user and password, checks pass to sign in if true
            var result =await _signInManager.CheckPasswordSignInAsync(User, model.Password,false);//false>> i don't want to lock acc if pass is false
            if (!result.Succeeded) { return Unauthorized(new ApiResponse(401)); }
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });
            
            
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var Email = User.FindFirstValue(ClaimTypes.Email);//User is a property inside the controller base
           var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName, Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
            //var user = await _userManager.FindByEmailAsync(Email);
            //it won't get the address, bec address is a nav property so it's not loaded
            //so i will make an ext method to get user with address

        }

        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto OldAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if(user == null) { return Unauthorized(new ApiResponse(401)); }
            var address = _mapper.Map<AddressDto, Address>(OldAddress);
            address.Id = user.Address.Id;//in order to update on the old address, not create a new object
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) { return BadRequest(new ApiResponse(400)); }
            return Ok(OldAddress);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return _userManager.FindByEmailAsync(email) is not null ;
        }
    }
}

/*JWT
 * library i will use to encrypt and decrypt token
 * Jwt>>needs 3 kinds of info>> header(algorithm&type),payload,key(will encrypt through it)
 * 
 */