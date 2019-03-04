using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Orders
    {
        public Orders()
        {
            OrderItems = new HashSet<OrderItems>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public int? AddressId { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Store { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
