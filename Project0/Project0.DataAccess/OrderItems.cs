using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PizzaId { get; set; }
        public int Quantity { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Pizza Pizza { get; set; }
    }
}
