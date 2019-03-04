using Project0.DataAccess;
using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    interface ICustomerRepo : IRepository<Customer>
    {
        //must be able to search customers by name
        IEnumerable<Customer> GetCustomersByFirstName(string name);
        IEnumerable<Customer> GetCustomersByFullName(string name);

        //Has a default store location to order from

        //cannot place more than one order from the same location within two hours

    }
}
