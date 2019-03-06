using Project0.DataAccess;
using System.Collections.Generic;


namespace Project0.Library
{
    public class Mapper
    {
        //DAO to Model
        //public static Models.Address Map(Address address) =>
        //    new Models.Address
        //    { 
        //        Street = address.Street,
        //        City = address.City,
        //        State = address.State,
        //        Country = address.Country,
        //        Zipcode = address.Zipcode
        //    };

        public static Models.Customer Map(Customer customer) => new Models.Customer
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            //Address = Map(customer.Address),
            Store = Map(customer.Store),
        };

        public static Dictionary<string, int> Map(ICollection<PizzaIngredients> pizza)
        {
            var newItems = new Dictionary<string, int>();
            foreach (var ingredient in pizza)
            {
                newItems.Add(ingredient.Ingredients.Name, ingredient.Quantity);
            }
            return newItems;
        }

        public static Models.Pizza Map(Pizza pizza) => new Models.Pizza
        {
            Id = pizza.Id,
            Price = pizza.Price,
            Items = Map(pizza.PizzaIngredients)
        };

        public static Dictionary<string, int> Map(ICollection<Inventory> inventory)
        {
            Dictionary<string, int> newInv = new Dictionary<string, int>();
            foreach (var inv in inventory)
            {
                newInv.Add(inv.Ingredients.Name, inv.Quantity);
            }
            return newInv;
        }

        public static Dictionary<Models.Pizza, int> Map(ICollection<OrderItems> ordItems)
        {
            Dictionary<Models.Pizza, int> orderItems = new Dictionary<Models.Pizza, int>();
            foreach (var item in ordItems)
            {
                orderItems.Add(Map(item.Pizza), item.Quantity);
            }
            return orderItems;
        }

        public static Models.Order Map(Orders order)
        {
            Models.Order ord = new Models.Order(Map(order.Store), Map(order.Customer), Map(order.OrderItems), order.OrderTime);
            ord.Id = order.Id;
            return ord;
        }
        //new Models.Order(Map(order.Store), Map(order.Customer), Map(order.Address), Map(order.OrderItems), order.OrderTime);

        public static Models.PizzaStore Map(Location store)
        {
            Models.PizzaStore newStore = null;
            if (store != null)
            {
                newStore = new Models.PizzaStore(store.LocationName, Map(store.Inventory));
                newStore.Id = store.Id;
                foreach (var order in store.Orders)
                {
                    newStore.OrderHistory.Add(Map(order));
                }
            }

            return newStore;
        }


        //Model to DAO

        //public static Address Map(Models.Address address) =>
        //    new Address
        //    {
        //        Id = address.Id,
        //        Street = address.Street,
        //        City = address.City,
        //        State = address.State,
        //        Country = address.Country,
        //        Zipcode = address.Zipcode
        //    };

        public static Ingredients Map(string name) => new Ingredients { Name = name };

        public static ICollection<Inventory> MapInventory(Dictionary<string, int> inventory)
        {
            List<Inventory> inventories = new List<Inventory>();
            foreach (var item in inventory)
            {
                Inventory newInv = new Inventory()
                {
                    Quantity = item.Value,
                    Ingredients = Map(item.Key)
                };
                inventories.Add(newInv);
            }
            return inventories;
        }

        public static ICollection<PizzaIngredients> MapIngredients(Dictionary<string, int> ingredients)
        {
            List<PizzaIngredients> pizzaIngredients = new List<PizzaIngredients>();
            foreach (var ingred in ingredients)
            {
                PizzaIngredients newIng = new PizzaIngredients
                {
                    Quantity = ingred.Value,
                    Ingredients = Map(ingred.Key)
                };
                pizzaIngredients.Add(newIng);
            }
            return pizzaIngredients;
        }


        public static Pizza Map(Models.Pizza pizza) => new Pizza
        {
            PizzaIngredients = MapIngredients(pizza.Items),
            Price = pizza.Price
        };

        public static ICollection<OrderItems> Map(Dictionary<Models.Pizza, int> orderItems)
        {
            List<OrderItems> ordItems = new List<OrderItems>();
            foreach (var item in orderItems)
            {
                OrderItems newOrdItems = new OrderItems
                {
                    Pizza = Map(item.Key),
                    Quantity = item.Value,
                };
                ordItems.Add(newOrdItems);
            }
            return ordItems;
        }

        public static ICollection<Orders> Map(List<Models.Order> orderHistory)
        {
            List<Orders> orders = new List<Orders>();
            foreach (var item in orderHistory)
            {
                Orders newOrd = new Orders()
                {
                    //Address = Map(item.Address),
                    Customer = Map(item.Customer),
                    OrderItems = Map(item.OrderItems),
                    OrderTime = item.OrderTime,
                    Store = Map(item.Store)
                    //item.TotalPrice  //db obj doesn't save total price
                };
                orders.Add(newOrd);
            }
            return orders;
        }

        public static Location Map(Models.PizzaStore store) => new Location
        {
            Id = store.Id,
            LocationName = store.Name,
            //Address = Map(store.Location),
            Inventory = MapInventory(store.Inventory),
            Orders = Map(store.OrderHistory)
        };

        public static Customer Map(Models.Customer customer) => new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            //Address = Map(customer.Address),
            Store = Map(customer.Store)
        };


        //IEnumerables, DAOs to Models


        //public static IEnumerable<Models.Address> Map(IEnumerable<Address> address)
        //{
        //    foreach (var o in address)
        //    {
        //        yield return Map(o);
        //    }
        //}

        public static IEnumerable<Models.Customer> Map(IEnumerable<Customer> customer)
        {
            foreach (var o in customer)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Models.Pizza> Map(IEnumerable<Pizza> pizza)
        {
            foreach (var o in pizza)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Models.Order> Map(IEnumerable<Orders> order)
        {
            foreach (var o in order)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Models.PizzaStore> Map(IEnumerable<Location> stores)
        {
            foreach (var o in stores)
            {
                yield return Map(o);
            }
        }



        //IEnumerables, Models to DAOs

        //public static IEnumerable<Address> Map(IEnumerable<Models.Address> address)
        //{
        //    foreach (var o in address)
        //    {
        //        yield return Map(o);
        //    }
        //}

        public static IEnumerable<Ingredients> Map(IEnumerable<string> ingredients)
        {
            foreach (var o in ingredients)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Pizza> Map(IEnumerable<Models.Pizza> pizzas)
        {
            foreach (var o in pizzas)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Location> Map(IEnumerable<Models.PizzaStore> stores)
        {
            foreach (var o in stores)
            {
                yield return Map(o);
            }
        }

        public static IEnumerable<Customer> Map(IEnumerable<Models.Customer> customers)
        {
            foreach (var o in customers)
            {
                yield return Map(o);
            }
        }


    }
}
