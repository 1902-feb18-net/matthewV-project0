using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Location
    {
        public Location()
        {
            Customer = new HashSet<Customer>();
            Inventory = new HashSet<Inventory>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string LocationName { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
