
namespace Project0.DataAccess.Repositories
{
    public interface IInventoryRepo : IRepository<Inventory>
    {
        //* rejects orders that cannot be fulfilled with remaining inventory
        

        //* inventory decreases when orders are accepted


    }
}
