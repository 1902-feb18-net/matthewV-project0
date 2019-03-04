
using System;
using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class Customer
    {
        private PizzaStore _store = new PizzaStore(); //customer has default pizza store  

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
        public PizzaStore Store
        {
            get => _store;
            set
            {
                _store = value ?? throw new ArgumentNullException(nameof(value), "Customer's store must not be null.");
            }
        }   


        public Customer()  //constructor uses default store
        {
            FirstName = null;
            LastName = null;
        }

        public Customer(string first, string last) //constructor uses default store
        {
            FirstName = first;
            LastName = last;
        }

        //create customer with given default store
        public Customer(string first, string last, PizzaStore newStore)   //constructor uses given store
        {
            FirstName = first;
            LastName = last;
            Store = newStore;
        }



    }
}
