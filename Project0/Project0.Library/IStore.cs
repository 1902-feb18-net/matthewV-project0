using System.Collections.Generic;

namespace Project0.Library
{
    public interface IStore
    {
        //int NewStoreDoughAmount;     //was initially gonna just create int for each item's initial value, too specific

        /* //2nd idea was to create an overrideable method with a switch for retrieving all the initial store values, 
           //but realized didn't actually want a "const" dictionary, should be able to change items easier
        protected virtual int GetInitialStoreValuesByName(string item)
        {
            switch (item.ToLower())
            {
                case "dough": return 20; //For example, call with GetInitialStoreValuesByName("Dough") 
                //to get initial store's amount of dough, 20
                default: return -1; //not real item in stock
            }
        }
        */

        //Decided to go with Dictionary, as it is easier to expand upon and remove items later.
        //string is name of product, int is store's available inventory amount.
        Dictionary<string, int> Inventory { get; set; }
        Address Location { get; set; }

        void AddInventoryItem(string key, int initialAmount); //add a new item to the store's inventory

        void IncreaseInventoryQuantity(string itemName, int amount);  //increase inventory when new stock is availeble
        
        bool CheckOrderItemAvailability(string item, int amount); //to reject order that cannot be fulfilled with remaining inventory.

        bool OrderPlaced();        //inventory decreases when order accepted.



        //SortOrderHistory();
        //order history needs to be sortable by time and price, each both ways
    }

    //need a product with some complexity!

    /*original product struct idea
    enum Components //all components a pizza can be made of. 
    {
        Dough,
        Cheese,
        Pepperoni,
        Pineapple,
        Spinach,    
        Sausage, 
    }
    
    struct CheesePizza
    {
        internal Components Cheese => Components.Cheese; //does this make sense?
        internal Components Dough => Components.Dough;
        //the components of a cheese pizza don't change, but the price can
        public double Price { get; set; } 

        CheesePizza(double price)
        {
            Price = price; 
        }

    }

    struct SpinachPizza
    {
        readonly Components Dough;
        readonly Components Cheese;
        readonly Components Spinach;
    }

    struct PineapplePizza
    {
        readonly Components Dough;
        readonly Components Cheese;
        readonly Components Pineapple;

    }

    */

}
