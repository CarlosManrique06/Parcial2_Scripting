using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    internal class InventorySlot
    {
        public Item Item { get; private set; }
        public int Quantity { get; set; }

        public InventorySlot(Item item, int quantity)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (quantity < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");

            Item = item;
            Quantity = quantity;
        }
    }
}
