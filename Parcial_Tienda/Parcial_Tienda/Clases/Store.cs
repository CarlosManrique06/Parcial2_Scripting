using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    internal class Store
    {

        public class Store
{
    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    public Store(Dictionary<Item, int> initialInventory)
    {
        if (initialInventory == null || initialInventory.Count == 0)
            throw new ArgumentException("La tienda debe iniciar con al menos un item.");

        foreach (var pair in initialInventory)
        {
            if (pair.Value < 0)
                throw new ArgumentException("Cantidad inválida.");

            inventory[pair.Key] = pair.Value;
        }
    }

    public void AddItem(Item item, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Cantidad inválida");

        foreach (var existing in inventory.Keys)
        {
            if (existing.Name == item.Name && existing.Category == item.Category)
            {
                if (existing.Price != item.Price)
                    throw new Exception("Mismo item con diferente precio");

                inventory[existing] += quantity;
                return;
            }
        }

        inventory[item] = quantity;
    }

    public int GetStock(Item item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    // 🧠 COMPRA SIMPLE
    public bool BuyItem(Player player, Item item, int quantity)
    {
        var cart = new Dictionary<Item, int> { { item, quantity } };
        return BuyItems(player, cart);
    }

    // 🔥 COMPRA MÚLTIPLE (CLAVE DEL TALLER)
    public bool BuyItems(Player player, Dictionary<Item, int> items)
    {
        decimal total = 0;

        // 1. VALIDAR TODO PRIMERO
        foreach (var pair in items)
        {
            var item = pair.Key;
            var qty = pair.Value;

            if (qty <= 0)
                return false;

            if (!inventory.ContainsKey(item) || inventory[item] < qty)
                return false;

            total += item.Price * qty;
        }

        if (player.Gold < total)
            return false;

        // 2. SI TODO OK → APLICAR CAMBIOS
        player.SpendGold(total);

        foreach (var pair in items)
        {
            inventory[pair.Key] -= pair.Value;
            player.Inventory.AddItem(pair.Key, pair.Value);
        }

        return true;
    }

    public bool IsEmpty()
    {
        foreach (var qty in inventory.Values)
            if (qty > 0) return false;

        return true;
    }
}
        
    }
}
