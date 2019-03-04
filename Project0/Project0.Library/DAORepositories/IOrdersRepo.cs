using Project0.DataAccess;
using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    public interface IOrdersRepo : IRepository<Orders>
    {
        //* get a suggested order for a customer based on his order history
        Orders SuggestOrder(int custId);

        //* display all order history of a customer
        IEnumerable<Orders> GetOrdersOfCustomer(int custId);

        //* display all order history of a store location
        IEnumerable<Orders> GetOrdersOfLocation(int storeId);

        //* display order history sorted by earliest, latest, cheapest, most expensive
        IEnumerable<Orders> GetOrdersSortedEarliest();
        IEnumerable<Orders> GetOrdersSortedLatest();
        IEnumerable<Orders> GetOrdersSortedCheapest();
        IEnumerable<Orders> GetOrdersSortedExpensive();

        //* display some statistics based on order history
        IEnumerable<Orders> GetOrdersStatistics();

        

        //inventory decreases when orders are accepted


        //rejects orders that cannot be fulfilled with remaining inventory


    }
}
