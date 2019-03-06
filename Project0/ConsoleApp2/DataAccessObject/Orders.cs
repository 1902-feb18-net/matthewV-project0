using System;
using System.Collections.Generic;

namespace DataAccessObject
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
        public DateTime OrderTime { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Store { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
