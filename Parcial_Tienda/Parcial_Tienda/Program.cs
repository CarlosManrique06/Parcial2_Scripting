namespace Parcial_Tienda
{


class Program
{
    static void Main()
    {
        Player player;

        Console.WriteLine("1. Ingresar oro manual");
        Console.WriteLine("2. Oro aleatorio");

        int opcion = int.Parse(Console.ReadLine());

        if (opcion == 1)
        {
            Console.Write("Ingrese oro: ");
            player = new Player(decimal.Parse(Console.ReadLine()));
        }
        else
        {
            var rand = new Random();
            player = new Player(rand.Next(50, 200));
        }

        var sword = new Item("Espada", 50, ItemCategoria.Weapon);
        var potion = new Item("Poción", 20, ItemCategoria.Supply);

        var store = new Store(new Dictionary<Item, int>
        {
            { sword, 3 },
            { potion, 5 }
        });

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nORO: {player.Gold}");
            Console.ResetColor();

            Console.WriteLine("\n1. Ir a la tienda");
            Console.WriteLine("2. Ver inventario");
            Console.WriteLine("3. Crear item en tienda");
            Console.WriteLine("0. Salir");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 0) break;

            if (choice == 2)
            {
                player.Inventory.ShowInventory();
            }
            else if (choice == 3)
            {
                Console.Write("Nombre: ");
                string name = Console.ReadLine();

                Console.Write("Precio: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Categoría: 0=Weapon,1=Armor,2=Accessory,3=Supply");
                ItemCategoria cat = (ItemCategoria)int.Parse(Console.ReadLine());

                Console.Write("Cantidad: ");
                int qty = int.Parse(Console.ReadLine());

                store.AddItem(new Item(name, price, cat), qty);
            }
            else if (choice == 1)
            {
                var items = store.GetAllItems();

                if (store.IsEmpty())
                {
                    Console.WriteLine("La tienda está vacía.");
                    continue;
                }

                int i = 1;
                var lista = new List<Item>();

                foreach (var pair in items)
                {
                    if (pair.Value > 0)
                    {
                        Console.WriteLine($"{i}. {pair.Key.Name} - {pair.Key.Price} (Stock: {pair.Value})");
                        lista.Add(pair.Key);
                        i++;
                    }
                }

                Console.Write("¿Cuántos productos distintos quieres comprar?: ");
                int n = int.Parse(Console.ReadLine());

                var carrito = new Dictionary<Item, int>();

                for (int j = 0; j < n; j++)
                {
                    Console.Write("Seleccione item #: ");
                    int idx = int.Parse(Console.ReadLine()) - 1;

                    Console.Write("Cantidad: ");
                    int qty = int.Parse(Console.ReadLine());

                    carrito[lista[idx]] = qty;
                }

                bool ok = store.BuyItems(player, carrito);

                Console.WriteLine(ok ? "Compra exitosa" : "Compra fallida");
            }
        }
    }

}
 }
