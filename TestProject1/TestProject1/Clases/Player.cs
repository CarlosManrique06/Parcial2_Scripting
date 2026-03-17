using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial_Tienda.Clases
{
    public class Player
    {
        public decimal Gold { get; private set; }
        public PlayerInventory Inventory { get; private set; }

        public Player(decimal gold)
        {
            if (gold < 0)
                throw new ArgumentException("El oro no puede ser negativo.");

            Gold = gold;
            Inventory = new PlayerInventory();
        }

        public void DeductGold(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("La cantidad a deducir no puede ser negativa.");

            if (amount > Gold)
                throw new InvalidOperationException("El jugador no tiene suficiente oro.");

            Gold -= amount;
        }
    }
}

