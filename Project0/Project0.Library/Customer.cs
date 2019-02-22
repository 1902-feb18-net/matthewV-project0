using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address address { get; set; }
        public Store defaultStore { get; set; } = new Store();

        //Cannot place more than one order from the same location within two hours?

        //Constructors
        public Customer(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

        public Customer(string first, string last, Store newDefaultStore)
        {
            FirstName = first;
            LastName = last;
            defaultStore = newDefaultStore;
        }



    }
}
