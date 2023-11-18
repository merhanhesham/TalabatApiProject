using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    //this is the class which will be used in the cutomerbasket, which will be stored in the memory not DB,so they don't need to inherit base entity 
    public class BasketItem
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }//+2
    }
}
