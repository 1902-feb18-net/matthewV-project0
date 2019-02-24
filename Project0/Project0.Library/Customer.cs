
namespace Project0.Library
{
    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        //etc.

        public IStore DefaultStore { get; set; } //need actual default store?


        //Cannot place more than one order from the same location within two hours!


        public Customer(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }

        //create customer with given default store
        public Customer(string first, string last, IStore newDefaultStore) 
        {
            FirstName = first;
            LastName = last;
            DefaultStore = newDefaultStore;
        }



    }
}
