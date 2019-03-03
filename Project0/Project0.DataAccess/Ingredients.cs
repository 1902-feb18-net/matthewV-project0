using System;
using System.Collections.Generic;

namespace Project0.DataAccess
{
    public partial class Ingredients
    {
        public Ingredients()
        {
            Inventory = new HashSet<Inventory>();
            PizzaIngredients = new HashSet<PizzaIngredients>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<PizzaIngredients> PizzaIngredients { get; set; }
    }
}
