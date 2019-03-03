using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int IngredientsId { get; set; }
        public int Quantity { get; set; }

        public virtual Ingredients Ingredients { get; set; }
        public virtual Location Store { get; set; }
    }
}
