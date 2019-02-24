using System;
using System.Collections.Generic;

namespace Project0.Library
{
    public class PizzaStore : IStore
    {
        static Dictionary<string, int> InitialInventory = new Dictionary<string, int>() //default initial inventory for every new Pizza store
        {
            {"Bacon", 5},
            {"Cheese", 25},
            {"Dough", 25},
            {"Pepperoni", 15},
            {"Pineapple", 5},
            {"Sausage", 5},
            {"Spinach", 5}
        }; //should this be in its own file?

        //string is name of product, int is store's available inventory amount. Assume unit is amount for one pizza.
        public Dictionary<string, int> Inventory { get; set; }   //should inventory be accessible (have get/set)?
        public Address Location { get; set; }


        public PizzaStore() //every new pizza store comes with a default amount of some items
        {
            Inventory = new Dictionary<string, int> (InitialInventory);  
        }

        public PizzaStore(Dictionary<string, int> diffInitialInventory)    //a new store opening with a different inventory than the default
        {
            Inventory = new Dictionary<string, int>(diffInitialInventory);
        }

        public void AddInventoryItem(string key, int initialAmount)   //add new item with the specified amount to the store's inventory
        {
            try
            {
                Inventory.Add(key, initialAmount);
            }
            catch(ArgumentNullException)
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

        public bool CheckOrderItemAvailability(string item, int amount)  //
        {
            try
            {
                if (Inventory.TryGetValue(item, out int available))
                {
                    return (amount >= available);
                }
                else
                {
                    //key not found -> log it 
                    throw new ArgumentOutOfRangeException("Item not in inventory");
                }
            }
            catch(ArgumentNullException e)
            {
                //key is null -> log it
                throw e;
            }
        }

        public void IncreaseInventoryQuantity(string itemName, int amount)
        {
            Inventory[itemName] += amount;
        }


        public bool OrderPlaced()
        {


            throw new NotImplementedException();
        }

    }
}
