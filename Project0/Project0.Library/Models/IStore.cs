using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Project0.Library.Models
{

    public interface IStore
    {
        //string is name of ingredient, int is store's available inventory amount.
        [DataMember]
        Dictionary<string, int> Inventory { get; set; }

        [DataMember]
        string Name { get; set; }

        //[DataMember]
        //Address Location { get; set; }

        [DataMember]
        List<Order> OrderHistory { get; } //no rewriting history

        void AddInventoryItem(string key, int initialAmount); //add a new item to the store's inventory

        void IncreaseInventoryQuantity(string key, int amount);  //increase inventory when new stock is available
        
        bool CheckOrderItemAvailability(Dictionary<Pizza, int> pizzas); //to reject order that cannot be fulfilled with remaining inventory.

        bool OrderItem(KeyValuePair<string, int> item, int amount);  //inventory decreases when order accepted.

        //order history needs to be sortable by time and price, each both ways
        List<Order> SortOrderHistoryDate();
        List<Order> SortOrderHistoryDateReverse();
        List<Order> SortOrderHistoryPrice();
        List<Order> SortOrderHistoryPriceReverse();
    }

}
