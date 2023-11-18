using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "price can't be zero")]
        public decimal Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
       
        public string Type { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "quantity can't be zero")]
        public int Quantity { get; set; }//+2
    }
}