using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    public class PlayerInventory
{

        private List<InventorySlot> _equipment = new List<InventorySlot>();
        private List<InventorySlot> _supplies = new List<InventorySlot>();

        public List<InventorySlot> Equipment { get { return _equipment; } }
        public List<InventorySlot> Supplies { get { return _supplies; } }

        public void AddItem(Item item, int quantity)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser positiva.");

            if (item.Category == ItemCategoria.Supply)
                AddToList(_supplies, item, quantity);
            else
                AddToList(_equipment, item, quantity);
        }

        private void AddToList(List<InventorySlot> list, Item item, int quantity)
        {
            InventorySlot existing = FindSlot(list, item.Name, item.Category);

            if (existing != null)
                existing.Quantity += quantity;
            else
                list.Add(new InventorySlot(item, quantity));
        }

        private InventorySlot FindSlot(List<InventorySlot> list, string name, ItemCategoria category)
        {
            foreach (InventorySlot slot in list)
            {
                if (slot.Item.Name == name && slot.Item.Category == category)
                    return slot;
            }
            return null;
        }

        public int GetEquipmentQuantity(string name, ItemCategoria category)
        {
            InventorySlot slot = FindSlot(_equipment, name, category);
            return slot != null ? slot.Quantity : 0;
        }

        public int GetSupplyQuantity(string name)
        {
            InventorySlot slot = FindSlot(_supplies, name, ItemCategoria.Supply);
            return slot != null ? slot.Quantity : 0;
        }
    }
}
