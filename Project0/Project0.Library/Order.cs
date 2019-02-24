using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library
{
    class Order
    {
        public IStore StoreLocation { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderTime { get; set; }
        //total price?
        //item quantities?

        //need additional business rules!

    }
}
