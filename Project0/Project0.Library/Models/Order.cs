using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Order //no rewriting history, fields are readonly
    {
        [DataMember]
        public DateTime OrderTime { get; } //DateTime cannot be null
        [DataMember]
        public PizzaStore Store { get; }
        [DataMember]
        public Customer Customer { get; }
        [DataMember]
        public Address Address { get; } = null; //Null address means carry-out

        [DataMember]
        public Dictionary<Pizza, int> OrderItems { get; }
        public decimal TotalPrice { get; } = 0; //Should be calculable based on Pizza and Amount.


        public Order(PizzaStore store, Customer cust, Dictionary<Pizza, int> orderItems, DateTime now) //carryout order
        {
            Store = store ?? throw new ArgumentNullException(nameof(store), "Order's store must not be null."); ;
            Customer = cust ?? throw new ArgumentNullException(nameof(cust), "Order's customer must not be null."); ;

            OrderItems = new Dictionary<Pizza, int>(orderItems); //don't want modifiable order history

            foreach(var pizza in orderItems) //total price is the sum of the number of each pizza times its price
            {
                TotalPrice += pizza.Value * pizza.Key.Price;
            }

            OrderTime = now;
        }

        public Order(PizzaStore store, Customer cust, Address deliveryAdd, Dictionary<Pizza, int> orderItems, DateTime now)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store), "Order's store must not be null."); ;
            Customer = cust ?? throw new ArgumentNullException(nameof(cust), "Order's customer must not be null.");

            Address = deliveryAdd; //null address means carry out

            OrderItems = new Dictionary<Pizza, int>(orderItems); //don't want modifiable order history
            foreach (var pizza in orderItems) //total price is the sum of the number of each pizza times its price
            {
                TotalPrice += pizza.Value * pizza.Key.Price;
            }

            OrderTime = now;
        }

    }
}
