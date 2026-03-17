using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parcial_Tienda.Clases
{
    public class Item
    {
        
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public ItemCategoria Category { get; private set; }

        public Item(string name, decimal price, ItemCategoria category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del artículo no puede estar vacío.");

            if (price <= 0)
                throw new ArgumentException("El precio del artículo debe ser positivo.");

            Name = name;
            Price = price;
            Category = category;
        }
    }
}
