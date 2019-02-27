using System;
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    public class Order
    {
        public DateTime OrderTime { get; set; }
        [XmlElement]
        public PizzaStore Store { get; set; }
        [XmlElement]
        public Customer Customer { get; set; }
        public Pizza Pizza { get; set; }
        public int Amount { get; set; }
        //total price? Should be calculable based on Pizza and Amount.

        public Order(PizzaStore store, Customer cust, Pizza pizza, int amount, DateTime now)
        {
            Store = store;
            Customer = cust;
            Pizza = pizza;
            Amount = amount;
            OrderTime = now;
        }

        //need additional business rules!

    }
}
