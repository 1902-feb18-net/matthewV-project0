using System;
using System.Collections.Generic;

namespace DataAccessObject
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
