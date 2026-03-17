using System;

public class Item
{
    public string Name { get; }
    public decimal Price { get; }
    public ItemCategoria Category { get; }

    public Item(string name, decimal price, ItemCategoria category)
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
