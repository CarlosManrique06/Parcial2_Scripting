using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases

public class Item
{
    public string Name { get; }
    public decimal Price { get; }
    public Itemcategory Category { get; }

    public Item(string name, decimal price, ItemCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nombre inválido");

        if (price <= 0)
            throw new ArgumentException("El precio debe ser positivo");

        Name = name;
        Price = price;
        Category = category;
    }
}
