using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace Project0.Library.Models
{
    [DataContract]
    public class PizzaStore : IStore
    {
        //default initial inventory for every new Pizza store
        //public static HashSet<InventoryItem> InitialInventory = new HashSet<InventoryItem>() 
        //{
        //    new InventoryItem("Cheese", 25),
        //    new InventoryItem("Dough", 25),
        //    new InventoryItem("Pepperoni", 15),
        //    new InventoryItem("Pineapple", 5),
        //    new InventoryItem("Sausage", 5),
        //    new InventoryItem("Spinach", 5)
        //};

        public static Dictionary<string, int> InitialInventory = new Dictionary<string, int>()
        {
            { "Cheese", 25 },
            { "Dough", 25},
            { "Pepperoni", 15 },
            { "Pineapple", 5},
            { "Sausage", 5 },
            { "Spinach", 5 }
        };


        private static int nextID = 0; //for uniqueness of ids
        [DataMember]
        public int Id; //for identification of this store object

        [DataMember]
        public string Name { get; set; } = "Matthew's Pizzas";

        [DataMember]
        public Dictionary<string, int> Inventory { get; set; } //string is name of product, int is store's available inventory. Assume unit is amount for one pizza.
        //public List<(string, int)> Inventory { get; set; } 
        //[XmlArray("ListOfInventoryItems")]
        //public HashSet<InventoryItem> Inventory { get; set; }

        private readonly object inventoryLock = new object(); //lock to prevent inventory amount from changing between checking quantity and ordering
        //private readonly object orderLock = new object(); //lock to prevent order history from changing while sorting it

        [DataMember]
        public List<Order> OrderHistory { get; set; } = new List<Order>();  //"location has order history" is a requirement. New store has no orders yet.

        [DataMember]
        public Address Location { get; set; } //must be unique

        //public PizzaStore() //every new pizza store comes with a default amount of some items
        //{
        //    Id = Interlocked.Increment(ref nextID);
        //    //Inventory = new HashSet<InventoryItem>(InitialInventory);
        //}

        public PizzaStore() //every new pizza store comes with a default amount of some items
        {
            Id = Interlocked.Increment(ref nextID);
            Inventory = new Dictionary<string, int>(InitialInventory);
            Location = new Address("701 W Nedderman Dr " + Id, "Arlington", "TX", "76019", "US");
        }

        //public PizzaStore(List<InventoryItem> diffInitialInventory)    //a new store opening with a different inventory than the default
        //{
        //    Id = Interlocked.Increment(ref nextID);
        //    //Inventory = new HashSet<InventoryItem>(diffInitialInventory);
        //}

        public PizzaStore(Dictionary<string, int> diffInitialInventory)    //a new store opening with a different inventory than the default
        {
            Id = Interlocked.Increment(ref nextID);
            Inventory = new Dictionary<string, int>(diffInitialInventory);
            Location = new Address("701 W Nedderman Dr " + Id, "Arlington", "TX", "76019", "US");
        }

        //public void AddInventoryItem(string key, int initialAmount)   //add new item with the specified amount to the store's inventory
        //{
        //    if (key is null)
        //    {
        //        //key is null -> log it
        //        throw new ArgumentNullException("Inventory Key cannot be null");
        //    }
        //    else
        //    {
        //        foreach (InventoryItem item in Inventory)
        //        {
        //            if (item.Name == key) //forced to add equality check through loop.
        //            {
        //                //key matches existing item in inventory -> log it
        //                throw new ArgumentException("Inventory Key matches existing item");
        //            }
        //        }
        //        Inventory.Add(new InventoryItem(key, initialAmount));
        //    }
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

        //public void IncreaseInventoryQuantity(string itemName, int amount)
        //{
        //    foreach (InventoryItem item in Inventory)
        //    {
        //        if (item.Name == itemName)
        //        {
        //            item.Quantity += amount;
        //            return; //only one item can have the name, save time by returning.
        //        }
        //    }
        //}

        public void IncreaseInventoryQuantity(string itemName, int amount)
        {
            Inventory[itemName] += amount;
        }

        //public bool CheckOrderItemAvailability(string itemName, int amount)
        //{
        //    foreach (InventoryItem item in Inventory)
        //    {
        //        if (item.Name == itemName)
        //        {
        //            return (item.Quantity >= amount ); 
        //        }
        //    }
        //    //key not found -> log it 
        //    throw new ArgumentOutOfRangeException("Item not in inventory");
        //}

        public bool CheckOrderItemAvailability(string item, int amount)  
        {
            try
            {
                if (Inventory.TryGetValue(item, out int available))
                {
                    return (available >= amount); //if amount in inventory is more than amount being purchased
                }
                else
                {
                    //key not found -> log it 
                    throw new ArgumentOutOfRangeException("Item not in inventory");
                }
            }
            catch (ArgumentNullException e)
            {
                //key is null -> log it
                throw e;
            }
        }

        //public bool OrderItem(string itemName, int amount)
        //{
        //    foreach (InventoryItem item in Inventory)
        //    {
        //        if (item.Name == itemName)
        //        {
        //            //inventory decreases when order accepted
        //            Inventory.Remove(new InventoryItem(item.Name, item.Quantity) );
        //            Inventory.Add(new InventoryItem(item.Name, item.Quantity - amount));
        //                //item.Quantity -= amount; 
        //            return true;
        //        }
        //    }
        //    return false; //shouldn't be reachable if checked order item availability beforehand
        //}

        public bool OrderItem(string itemName, int amount)
        {
            Inventory[itemName] -= amount; //inventory decreases when order accepted
            return true;
        }


        public bool PlacedOrder(Customer cust, Pizza pizza, int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Cannot order 0 or less pizzas, reject it");

            lock (inventoryLock) //prevent changes to inventory while an order is being placed
            {
                foreach (var item in pizza.Items)
                {
                    if (! CheckOrderItemAvailability(item, amount))
                    {
                        throw new ArgumentException("Not enough inventory to place order, reject it");
                        //return false; //not enough inventory to place order, reject it
                    }
                }

                foreach (var item in pizza.Items)
                {
                    if(!OrderItem(item, amount))
                    {
                        throw new Exception("Failure ordering item, may need to revert transaction"); 
                        //Unknown error, need to undo partial order!
                    }
                }
            }

            Order currentOrder = new Order(this, cust, pizza, amount, DateTime.Now);
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
            sortedHist.Sort((a, b) => (a.Amount * a.Pizza.Price).CompareTo(b.Amount * b.Pizza.Price)); //TotalPrice = indivdual pizzaPrice*amountOfPizzas

            return sortedHist; 
        }

        public List<Order> SortOrderHistoryPriceReverse()
        {
            List<Order> sortedHist = new List<Order>(OrderHistory); //copy history, then work on copy. Don't want to effect it while other methods use it.

            sortedHist.Sort((a, b) => (a.Amount * a.Pizza.Price).CompareTo(b.Amount * b.Pizza.Price));
            sortedHist.Reverse(); //reverse to make Highest to Lowest

            return sortedHist;
        }
    }
}
