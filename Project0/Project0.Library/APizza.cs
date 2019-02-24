using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{ 
    abstract class APizza //could make set of product pizza classes, each inheriting from APizza, with their own ingredients in items hashset
    {
        //Set of items a pizza can be made out of. Assumed one of each
        public HashSet<string> items = new HashSet<string> { "Dough", "Cheese" }; //Every pizza needs dough and cheese.

    }
}
