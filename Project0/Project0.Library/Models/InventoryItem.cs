using System.Runtime.Serialization;

namespace Project0.Library.Models
{
    [DataContract]
    public class InventoryItem
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Quantity { get; set; }

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
