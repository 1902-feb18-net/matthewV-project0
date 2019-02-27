
namespace Project0.Library.Models
{
    public class InventoryItem
    {
        public string Name;
        public int Quantity;

        public InventoryItem()
        {
            Name = "";
            Quantity = 0;
        }

        public InventoryItem(string name, int amount)
        {
            Name = name;
            Quantity = amount;
        }

    }
}
