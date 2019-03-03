
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Address Address { get; set; }
        //etc.
        [DataMember]
        public PizzaStore DefaultStore { get; set; } = new PizzaStore(); 
        //need specifics of actual default store?


        //Cannot place more than one order from the same location within two hours!

        public Customer() //needed for xml serialization
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
