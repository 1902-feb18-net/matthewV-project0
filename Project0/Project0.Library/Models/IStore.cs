using System.Collections.Generic;
using System.Xml.Serialization;

namespace Project0.Library.Models
{
    public interface IStore
    {
        //Decided to go with Dictionary, as it is easier to expand upon and remove items later.
        //string is name of product, int is store's available inventory amount.
        //Dictionary<string, int> Inventory { get; set; }
        //List<(string, int)> Inventory { get; set; }
        [XmlArrayItem("ListOfInventoryItems")]
        HashSet<InventoryItem> Inventory { get; set; }

        Address Location { get; set; }

        [XmlArrayItem("ListOfOrders")]
        List<Order> OrderHistory { get; set; }

        void AddInventoryItem(string key, int initialAmount); //add a new item to the store's inventory

        void IncreaseInventoryQuantity(string item, int amount);  //increase inventory when new stock is available
        
        bool CheckOrderItemAvailability(string item, int amount); //to reject order that cannot be fulfilled with remaining inventory.

        bool OrderItem(string item, int amount);        //inventory decreases when order accepted.

        //need a product with some complexity!

        //order history needs to be sortable by time and price, each both ways
        List<Order> SortOrderHistoryDate();
        List<Order> SortOrderHistoryDateReverse();
        List<Order> SortOrderHistoryPrice();
        List<Order> SortOrderHistoryPriceReverse();
    }


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

}
