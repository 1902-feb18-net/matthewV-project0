
using Project0.DataAccess;

namespace Project0.Library.DAORepositories
{
    public interface IInventoryRepo : IRepository<Inventory>
    {
        //* rejects orders that cannot be fulfilled with remaining inventory
        

        //* inventory decreases when orders are accepted


    }
}
