using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public int StoreId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Location Store { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
