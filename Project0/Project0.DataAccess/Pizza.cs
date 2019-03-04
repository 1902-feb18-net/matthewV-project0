using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Pizza
    {
        public Pizza()
        {
            OrderItems = new HashSet<OrderItems>();
            PizzaIngredients = new HashSet<PizzaIngredients>();
        }

        public int Id { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<PizzaIngredients> PizzaIngredients { get; set; }
    }
}
