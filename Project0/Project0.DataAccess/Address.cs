using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Address
    {
        public Address()
        {
            Customer = new HashSet<Customer>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
