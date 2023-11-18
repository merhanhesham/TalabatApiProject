using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    public class CustomerBasketDto
    {
        //we put validations in dto not model
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
