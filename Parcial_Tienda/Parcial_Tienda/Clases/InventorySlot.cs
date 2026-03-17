using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    public class InventorySlot
    {
        public Item Item { get; private set; }
        public int Quantity { get; private set; }

        public InventorySlot(Item item, int quantity)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (quantity < 0)
                throw new ArgumentException("Cantidad inválida");

            Item = item;
            Quantity = quantity;
        }

        public void Add(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Cantidad inválida");

            Quantity += amount;
        }
    }
    
    
}
