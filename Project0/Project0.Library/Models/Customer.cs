
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [XmlElement]
        public Address Address { get; set; }
        //etc.

        public PizzaStore DefaultStore { get; set; } //need actual default store?


        //Cannot place more than one order from the same location within two hours!

        public Customer()
        {
            FirstName = "";
            LastName = "";
        }

        public Customer(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

        //create customer with given default store
        public Customer(string first, string last, PizzaStore newDefaultStore) 
        {
            FirstName = first;
            LastName = last;
            DefaultStore = newDefaultStore;
        }



    }
}
