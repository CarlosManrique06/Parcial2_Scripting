using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    public class PlayerInventory
{
    private List<InventorySlot> equipment = new List<InventorySlot>();
    private List<InventorySlot> supplies = new List<InventorySlot>();

    public void AddItem(Item item, int quantity)
    {
        var list = item.Category == ItemCategoria.Supply ? supplies : equipment;

        var existing = list.Find(s => s.Item.Name == item.Name);

        if (existing != null)
            existing.Add(quantity);
        else
            list.Add(new InventorySlot(item, quantity));
    }

    public int GetTotalItems()
    {
        int total = 0;

        foreach (var e in equipment)
            total += e.Quantity;

        foreach (var s in supplies)
            total += s.Quantity;

        return total;
    }

    public void ShowInventory()
    {
        Console.WriteLine("\n--- EQUIPAMIENTO ---");
        foreach (var slot in equipment)
            Console.WriteLine($"{slot.Item.Name} x{slot.Quantity}");

        Console.WriteLine("\n--- CONSUMIBLES ---");
        foreach (var slot in supplies)
            Console.WriteLine($"{slot.Item.Name} x{slot.Quantity}");
    }
}
}
