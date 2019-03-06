using System;
using System.Collections.Generic;

namespace DataAccessObject
{
    public partial class PizzaIngredients
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public int IngredientsId { get; set; }
        public int Quantity { get; set; }

        public virtual Ingredients Ingredients { get; set; }
        public virtual Pizza Pizza { get; set; }
    }
}
