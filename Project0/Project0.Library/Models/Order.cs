using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public DateTime OrderTime { get; set; }
        [DataMember]
        public PizzaStore Store { get; set; }
        [DataMember]
        public Customer Customer { get; set; }
        [DataMember]
        public Pizza Pizza { get; set; }
        [DataMember]
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
