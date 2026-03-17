namespace Parcial_Tienda
{
    internal class Program
    {
      static void Main()
    {
        Random rnd = new Random();
        decimal gold;

        Console.WriteLine("1. Ingresar oro manualmente");
        Console.WriteLine("2. Oro aleatorio");

        int option = int.Parse(Console.ReadLine());

        if (option == 1)
        {
            Console.Write("Ingrese su oro: ");
            gold = decimal.Parse(Console.ReadLine());
        }
        else
        {
            gold = rnd.Next(50, 201);
            Console.WriteLine($"Oro asignado: {gold}");
        }

        Player player = new Player(gold);
        Store store = new Store();

        var sword = new Item("Espada Excalibour", 500, ItemCategoria.Weapon);
        var armor = new Item("Armadura de oro", 70, ItemCategoria.Armor);
        var potion = new Item("Poción curacion 1", 20, ItemCategoria.Supply);

        store.AddItem(sword, 2);
        store.AddItem(armor, 1);
        store.AddItem(potion, 5);

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nORO ACTUAL: {player.Gold}");
            Console.ResetColor();

            Console.WriteLine("\n1. Ir a la tienda");
            Console.WriteLine("2. Ver inventario");
            Console.WriteLine("3. Salir");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                var items = store.GetItems();

                if (items.Count == 0)
                {
                    Console.WriteLine("La tienda está vacía.");
                    continue;
                }

                store.ShowItems();

                Console.WriteLine("Seleccione item:");
                for (int i = 0; i < items.Count; i++)
                    Console.WriteLine($"{i + 1}. {items[i].Name}");

                int itemChoice = int.Parse(Console.ReadLine()) - 1;

                Console.Write("Cantidad: ");
                int qty = int.Parse(Console.ReadLine());

                store.BuyItem(player, items[itemChoice], qty);
            }
            else if (choice == 2)
            {
                player.Inventory.ShowInventory();
            }
            else
            {
                break;
            }
        }
    }
    }
}
