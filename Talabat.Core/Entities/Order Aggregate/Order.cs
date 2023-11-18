using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order:BaseEntity
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }

        public int DeliveryMethodId { get; set; }//pk of 1 as fk in many
        public DeliveryMethod DeliveryMethod { get; set; }//nav property, each order has one delivery method,
        //f kda el 3mlt represent l side elone gwa elorder, mhtaga arepresent elmany gwa delivery method, bs msh h3ml kda,leh?
        //l2n elbuisness bta3y msh byttlab ene mn eldelivery method a3rf kam order 3ndy, tb h3ml eh? h7ot fk

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();//nav property, order has many order items,lazm a initialize el many
        public decimal SubTotal { get; set; } //price*quantity
        //not needed in db
        /*[NotMapped]
        public decimal Total { get => SubTotal * DeliveryMethod.Cost; }//paid*/
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } = string.Empty;

    }
}
