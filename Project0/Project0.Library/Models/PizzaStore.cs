using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Project0.Library.Models
{
    [DataContract]
    public class PizzaStore : IStore
    {
        public int Id { get; set; }

        public static Dictionary<string, int> InitialInventory = new Dictionary<string, int>()
        {
            { "Cheese", 25 },
            { "Dough", 25 },
            { "Pepperoni", 15 },
            { "Pineapple", 5 },
            { "Sausage", 5 },
            { "Spinach", 5 }
        };

        //private static int nextID = 0; //for uniqueness of ids
        //[DataMember]
        //public int Id { get;} //for identification of this store object. Readonly. Cannot be null.

        private string _name = "Matthew's Pizzas"; //default name. Must not be null.

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Store's name must not be null.");
                }

                if (value.Length == 0)
                {
                    throw new ArgumentException("Store's name must not be empty.", nameof(value));
                }

                _name = value;
            }
        }


        [DataMember]
        public Dictionary<string, int> Inventory { get; set; } //string is name of product, int is store's available inventory. Assume unit is amount for one pizza.


        private readonly object inventoryLock = new object(); //lock to prevent inventory amount from changing between checking quantity and ordering
        private readonly object orderLock = new object(); //lock to prevent order history from changing while sorting it

        [DataMember]
        public List<Order> OrderHistory { get; } = new List<Order>();  //"location has order history" is a requirement. New store has no orders yet.
                                                                       //do not want rewriting history -> no set, just add to the List.

        //[DataMember]
        //public Address Location { get; set; } //must be unique?

        public PizzaStore() //every new pizza store comes with a default amount of some items and empty order history
        {
            //Id = Interlocked.Increment(ref nextID);
            Inventory = new Dictionary<string, int>(InitialInventory);
            // Location = new Address("701 W Nedderman Dr ", "Arlington", "TX", "76019", "US"); //unique default address
        }

        public PizzaStore(string name)
        {
            //Id = Interlocked.Increment(ref nextID);
            Name = name;
            Inventory = new Dictionary<string, int>(InitialInventory);
            //Location = new Address("701 W Nedderman Dr ", "Arlington", "TX", "76019", "US");
            //still has empty order history, need to add to after calling constructor
        }

        public PizzaStore(Dictionary<string, int> diffInitialInventory)    //a new store opening with a different inventory than the default
        {
            //Id = Interlocked.Increment(ref nextID);
            Inventory = new Dictionary<string, int>(diffInitialInventory);
            //Location = new Address("701 W Nedderman Dr ", "Arlington", "TX", "76019", "US");
        }

        //public PizzaStore(Address address)
        //{
        //    //Id = Interlocked.Increment(ref nextID);
        //    Inventory = new Dictionary<string, int>(InitialInventory);
        //    Location = address;
        //}

        //public PizzaStore(Dictionary<string, int> diffInitialInventory, Address address)
        //{
        //    //Id = Interlocked.Increment(ref nextID);
        //    Inventory = new Dictionary<string, int>(diffInitialInventory);
        //    Location = address;
        //}

        //public PizzaStore(string name, Address address)
        //{
        //    //Id = Interlocked.Increment(ref nextID);
        //    Name = name;
        //    Inventory = new Dictionary<string, int>(InitialInventory);
        //    Location = address;
        //}

        public PizzaStore(string name, Dictionary<string, int> diffInitialInventory)
        {
            //Id = Interlocked.Increment(ref nextID);

            if (name != null)
            {
                Name = name;
            }
            //else uses default
            if (diffInitialInventory != null)
            {
                Inventory = new Dictionary<string, int>(diffInitialInventory);
            }
            else
            {
                Inventory = new Dictionary<string, int>(InitialInventory);
            }

            //still has empty order history, need to add to after calling constructor
        }

        ////a new store opening with given values. Caller needs to verify address uniqueness?
        //public PizzaStore(string name, Dictionary<string, int> diffInitialInventory, Address address)
        //{
        //    //Id = Interlocked.Increment(ref nextID);

        //    if (name != null)
        //    {
        //        Name = name;
        //    }
        //    //else uses default
        //    if (diffInitialInventory != null)
        //    {
        //        Inventory = new Dictionary<string, int>(diffInitialInventory);
        //    }
        //    else
        //    {
        //        Inventory = new Dictionary<string, int>(InitialInventory);
        //    }
        //    if (address != null)
        //    {
        //        Location = address;
        //    }
        //    else
        //    {
        //        Location = new Address("701 W Nedderman Dr ", "Arlington", "TX", "76019", "US");
        //    }
        //    //still has empty order history, need to add to after calling constructor
        //}


        public void AddInventoryItem(string key, int initialAmount)   //add new item with the specified amount to the store's inventory
        {
            try
            {
                Inventory.Add(key, initialAmount);
            }
            catch (ArgumentNullException)
            {
                //key is null -> log it
                throw;
            }
            catch (ArgumentException)
            {
                //key matches existing item in inventory -> log it
                throw;
            }
        }

        public void IncreaseInventoryQuantity(string itemName, int amount)
        {
            Inventory[itemName] += amount;
        }

        public bool CheckOrderItemAvailability(Dictionary<Pizza, int> pizzas)
        {
            try
            {
                Dictionary<string, int> totalIngredientRequirements = new Dictionary<string, int>();
                foreach (var pizza in pizzas) //foreach pizza
                {
                    foreach (var ingredient in pizza.Key.Items) //foreach ingredient in that pizza
                    {
                        //add the amount of that ingredient for this pizza * number of this pizza to count
                        if (totalIngredientRequirements.ContainsKey(ingredient.Key))
                        {
                            totalIngredientRequirements[ingredient.Key] += ingredient.Value * pizza.Value;
                        }
                        else
                        {
                            totalIngredientRequirements.Add(ingredient.Key, ingredient.Value * pizza.Value);
                        }
                    }
                }

                foreach (var pizza in pizzas)
                {
                    foreach (var ingredient in pizza.Key.Items)
                    {
                        if (Inventory.TryGetValue(ingredient.Key, out int available))
                        {
                            if(available < totalIngredientRequirements[ingredient.Key])
                            { return false; }
                            //if the amount of an ingredient in inventory is less than the total amount of it being purchased 
                        }
                        else
                        {
                            //key not found -> log it 
                            throw new ArgumentOutOfRangeException($"Item ingredient {ingredient.Key} not in store's inventory.");
                        }
                    }
                }
                return true; //if inventory greater than or equal to amount required, for all ingredients 

            }
            catch (ArgumentNullException e)
            {
                //key is null -> log it
                throw e;
            }
        }

        public bool OrderItem(KeyValuePair<string, int> item, int amount)
        {
            Inventory[item.Key] -= item.Value * amount; //inventory decreases when order accepted
            return true;
        }


        public bool PlacedOrder(Customer cust, Dictionary<Pizza, int> pizzas)
        {
            foreach (var pizza in pizzas)
            {
                if (pizza.Value <= 0)
                    throw new ArgumentOutOfRangeException("Cannot order 0 or less of a pizza.");
            }

            DateTime ordertime = DateTime.Now;

            lock (orderLock) //prevent changes to orderhistory while checking it
            {
                foreach (var order in OrderHistory)
                {
                    //need to check 2hr time constraint for customer at this store
                    if (order.Customer.Equals(cust) && ordertime.Subtract(order.OrderTime) <= new TimeSpan(2, 0, 0))
                    {
                        throw new Exception("A customer cannot place another order at a pizza store within two hours.");
                    }
                }
            }

            lock (inventoryLock) //prevent changes to inventory while an order is being placed
            {
                foreach (var item in pizzas)
                {
                    if (!CheckOrderItemAvailability(pizzas))
                    {
                        throw new ArgumentException("Not enough inventory to place order, reject it");
                        //return false; //not enough inventory to place order, reject it
                    }

                }

                foreach (var item in pizzas)
                {
                    foreach (var ingredient in item.Key.Items)
                    {
                        if (!OrderItem(ingredient, item.Value))
                        {
                            throw new Exception("Failure ordering item, may need to revert transaction");
                            //Unknown error, need to undo partial order!
                        }

                    }
                }
            }

            Order currentOrder = new Order(this, cust, pizzas, ordertime);
            OrderHistory.Add(currentOrder);

            return true;
        }

        public List<Order> SortOrderHistoryDate()
        {
            List<Order> sortedHist = new List<Order>(OrderHistory); //copy history, then work on copy. Don't want to effect it while other methods use it.

            sortedHist.Sort((a, b) => a.OrderTime.CompareTo(b.OrderTime)); //sort on DateTime, earliest to latest

            return sortedHist;
        }

        public List<Order> SortOrderHistoryDateReverse()
        {
            List<Order> sortedHist = new List<Order>(OrderHistory); //copy history, then work on copy. Don't want to effect it while other methods use it.

            sortedHist.Sort((a, b) => a.OrderTime.CompareTo(b.OrderTime)); //sort on DateTime
            sortedHist.Reverse();   //then reverse to make latest to earliest

            return sortedHist;
        }

        public List<Order> SortOrderHistoryPrice()
        {
            List<Order> sortedHist = new List<Order>(OrderHistory); //copy history, then work on copy. Don't want to effect it while other methods use it.
            sortedHist.Sort((a, b) => a.TotalPrice.CompareTo(b.TotalPrice)); //TotalPrice = individualPizzaPrice*amountOfPizzas

            return sortedHist;
        }

        public List<Order> SortOrderHistoryPriceReverse()
        {
            List<Order> sortedHist = new List<Order>(OrderHistory); //copy history, then work on copy. Don't want to effect it while other methods use it.

            sortedHist.Sort((a, b) => a.TotalPrice.CompareTo(b.TotalPrice));
            sortedHist.Reverse(); //reverse to make Highest to Lowest

            return sortedHist;
        }
    }
}
