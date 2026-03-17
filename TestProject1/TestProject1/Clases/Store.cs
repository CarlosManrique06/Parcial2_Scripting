using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{


public class Store
{
        private List<InventorySlot> _inventory = new List<InventorySlot>();

        public List<InventorySlot> Inventory { get { return _inventory; } }

        // Una tienda debe crearse con al menos un artículo.
        public Store(Item item, int quantity)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (quantity < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");

            _inventory.Add(new InventorySlot(item, quantity));
        }

        public void AddItem(Item item, int quantity)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (quantity < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");

            InventorySlot existing = FindSlot(item.Name, item.Category);

            if (existing != null)
            {
                // El artículo ya existe: el precio debe coincidir.
                if (existing.Item.Price != item.Price)
                    throw new InvalidOperationException(
                        "Ya existe un artículo con ese nombre y categoría pero con un precio diferente.");

                existing.Quantity += quantity;
            }
            else
            {
                _inventory.Add(new InventorySlot(item, quantity));
            }
        }

        public int GetStock(string name, ItemCategoria category)
        {
            InventorySlot slot = FindSlot(name, category);
            return slot != null ? slot.Quantity : 0;
        }

        // Realiza la compra de forma atómica:
        // valida todo antes de cambiar cualquier estado.
        public void Purchase(Player player, List<(Item Item, int Quantity)> cart)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            if (cart == null || cart.Count == 0)
                throw new ArgumentException("El carrito no puede estar vacío.");

            // 1. Verificar que cada artículo existe y tiene stock suficiente.
            foreach ((Item item, int qty) in cart)
            {
                if (item == null)
                    throw new ArgumentNullException("El carrito contiene un artículo nulo.");

                if (qty <= 0)
                    throw new ArgumentException("La cantidad de cada artículo debe ser positiva.");

                InventorySlot slot = FindSlot(item.Name, item.Category);

                if (slot == null)
                    throw new InvalidOperationException(
                        "El artículo '" + item.Name + "' no existe en esta tienda.");

                if (slot.Quantity < qty)
                    throw new InvalidOperationException(
                        "No hay suficiente stock de '" + item.Name + "'.");
            }

            // 2. Calcular el total y verificar que el jugador tiene oro suficiente.
            decimal total = 0;
            foreach ((Item item, int qty) in cart)
            {
                InventorySlot slot = FindSlot(item.Name, item.Category);
                total += slot.Item.Price * qty;
            }

            if (player.Gold < total)
                throw new InvalidOperationException(
                    "El jugador no tiene suficiente oro para completar la compra.");

            // 3. Todo está bien: aplicar los cambios.
            player.DeductGold(total);

            foreach ((Item item, int qty) in cart)
            {
                InventorySlot slot = FindSlot(item.Name, item.Category);
                slot.Quantity -= qty;
                player.Inventory.AddItem(slot.Item, qty);
            }
        }

        private InventorySlot FindSlot(string name, ItemCategoria category)
        {
            foreach (InventorySlot slot in _inventory)
            {
                if (slot.Item.Name == name && slot.Item.Category == category)
                    return slot;
            }
            return null;
        }
    }
}
