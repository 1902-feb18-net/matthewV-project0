using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class PizzaStore : IStore
    {
        //default initial inventory for every new Pizza store
        static HashSet<InventoryItem> InitialInventory = new HashSet<InventoryItem>() 
        {
            new InventoryItem("Cheese", 25),
            new InventoryItem("Dough", 25),
            new InventoryItem("Pepperoni", 15),
            new InventoryItem("Pineapple", 5),
            new InventoryItem("Sausage", 5),
            new InventoryItem("Spinach", 5)
        };

        private static int nextID = 0; //for uniqueness of ids
        public int Id; //for identification of this store object

        //public Dictionary<string, int> Inventory { get; set; } //string is name of product, int is store's available inventory. Assume unit is amount for one pizza.
        //public List<(string, int)> Inventory { get; set; } 

        [XmlArray("ListOfInventoryItems")]
        public HashSet<InventoryItem> Inventory { get; set; }

        private readonly object inventoryLock = new object(); //lock to prevent inventory amount from changing between checking quantity and ordering
        //private readonly object orderLock = new object(); //lock to prevent order history from changing while sorting it

        [XmlArrayItem("ListOfOrders")]
        public List<Order> OrderHistory { get; set; } = new List<Order>();  //"location has order history" is a requirement. New store has no orders yet.

        [XmlElement]
        public Address Location { get; set; }


        public PizzaStore() //every new pizza store comes with a default amount of some items
        {
            Id = Interlocked.Increment(ref nextID);
            Inventory = new HashSet<InventoryItem>(InitialInventory);
        }

        public PizzaStore(List<InventoryItem> diffInitialInventory)    //a new store opening with a different inventory than the default
        {
            Id = Interlocked.Increment(ref nextID);
            Inventory = new HashSet<InventoryItem>(diffInitialInventory);
        }

        public void AddInventoryItem(string key, int initialAmount)   //add new item with the specified amount to the store's inventory
        {
            if (key is null)
            {
                //key is null -> log it
                throw new ArgumentNullException("Inventory Key cannot be null");
            }
            else
            {
                foreach (InventoryItem item in Inventory)
                {
                    if (item.Name == key) //forced to add equality check through loop.
                    {
                        //key matches existing item in inventory -> log it
                        throw new ArgumentException("Inventory Key matches existing item");
                    }
                }

                Inventory.Add(new InventoryItem(key, initialAmount));
            }
        }

        public void IncreaseInventoryQuantity(string itemName, int amount)
        {
            foreach (InventoryItem item in Inventory)
            {
                if (item.Name == itemName)
                {
                    item.Quantity += amount;
                    return; //only one item can have the name, save time by returning.
                }
            }
        }

        public bool CheckOrderItemAvailability(string itemName, int amount)
        {
            foreach (InventoryItem item in Inventory)
            {
                if (item.Name == itemName)
                {
                    return (amount >= item.Quantity);
                }
            }

            //key not found -> log it 
            throw new ArgumentOutOfRangeException("Item not in inventory");
        }

        public bool OrderItem(string itemName, int amount)
        {
            foreach (InventoryItem item in Inventory)
            {
                if (item.Name == itemName)
                {
                    item.Quantity -= amount; //inventory decreases when order accepted
                    return true;
                }
            }

            return false; //shouldn't be reachable
        }

        public bool PlacedOrder(Customer cust, Pizza pizza, int amount)
        {
            lock (inventoryLock)
            {
                foreach (var item in pizza.Items)
                {
                    if (!CheckOrderItemAvailability(item, amount))
                    {
                        return false; //not enough inventory to place order, reject it
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
