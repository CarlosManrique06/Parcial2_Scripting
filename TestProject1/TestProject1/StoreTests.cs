using System.Collections.Generic;
using Parcial_Tienda.Clases;

namespace RPGStore.Tests
{
    [TestFixture]
    public class StoreTests
    {
        
        // Item
       

        [TestCase("Iron Sword",    100.0, ItemCategoria.Weapon)]
        [TestCase("Leather Armor",  75.0, ItemCategoria.Armor)]
        [TestCase("Gold Ring",     200.0, ItemCategoria.Accessory)]
        [TestCase("Health Potion",  25.0, ItemCategoria.Supply)]
        [TestCase("Big Axe",       999.0, ItemCategoria.Weapon)]
        public void NewItem_Saves(string name, double price, ItemCategoria category)
        {
            Item item = new Item(name, (decimal)price, category);

            Assert.That(item.Name,     Is.EqualTo(name));
            Assert.That(item.Price,    Is.EqualTo((decimal)price));
            Assert.That(item.Category, Is.EqualTo(category));
        }

        [TestCase(  0.0)]
        [TestCase( -1.0)]
        [TestCase(-50.0)]
        public void NewItem_InvalidPrice(double price)
        {
            Assert.That(
                () => new Item("Sword", (decimal)price, ItemCategoria.Weapon),
                Throws.ArgumentException);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void NewItem_EmptyName(string name)
        {
            Assert.That(
                () => new Item(name, 10m, ItemCategoria.Armor),
                Throws.ArgumentException);
        }

        [TestCase(ItemCategoria.Weapon)]
        [TestCase(ItemCategoria.Armor)]
        [TestCase(ItemCategoria.Accessory)]
        [TestCase(ItemCategoria.Supply)]
        public void NewItemSavedCorrectly(ItemCategoria category)
        {
            Item item = new Item("Test", 1m, category);

            Assert.That(item.Category, Is.EqualTo(category));
        }

     
        // STORE CREATION
       

        [TestCase("Sword",  ItemCategoria.Weapon,     1)]
        [TestCase("Potion", ItemCategoria.Supply,    10)]
        [TestCase("Shield", ItemCategoria.Armor,      0)]
        [TestCase("Amulet", ItemCategoria.Accessory,  5)]
        public void NewStoreStock(string name, ItemCategoria category, int quantity)
        {
            Item item   = new Item(name, 50m, category);
            Store store = new Store(item, quantity);

            Assert.That(store.GetStock(name, category), Is.EqualTo(quantity));
        }

        [Test]
        public void NewStoreNullItem()
        {
            Assert.That(
                () => new Store(null, 5),
                Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(-100)]
        public void NewStoreNegativeQuantity(int quantity)
        {
            Item item = new Item("Sword", 10m, ItemCategoria.Weapon);

            Assert.That(
                () => new Store(item, quantity),
                Throws.ArgumentException);
        }

        [TestCase(3, 2, 5)]
        [TestCase(0, 5, 5)]
        [TestCase(1, 1, 2)]
        public void AddItemSameItem(int initial, int added, int expected)
        {
            Item item   = new Item("Potion", 20m, ItemCategoria.Supply);
            Store store = new Store(item, initial);

            store.AddItem(new Item("Potion", 20m, ItemCategoria.Supply), added);

            Assert.That(store.GetStock("Potion", ItemCategoria.Supply), Is.EqualTo(expected));
        }

        [Test]
        public void AddItemDifferentItems()
        {
            Item sword  = new Item("Sword",  100m, ItemCategoria.Weapon);
            Item potion = new Item("Potion",  20m, ItemCategoria.Supply);
            Store store = new Store(sword, 3);

            store.AddItem(potion, 10);

            Assert.That(store.GetStock("Sword",  ItemCategoria.Weapon), Is.EqualTo(3));
            Assert.That(store.GetStock("Potion", ItemCategoria.Supply), Is.EqualTo(10));
        }

        [TestCase(100.0, 200.0)]
        [TestCase( 50.0,  49.0)]
        public void AddItemDifferentPrice(double price1, double price2)
        {
            Item original = new Item("Sword", (decimal)price1, ItemCategoria.Weapon);
            Item conflict = new Item("Sword", (decimal)price2, ItemCategoria.Weapon);
            Store store   = new Store(original, 1);

            Assert.That(
                () => store.AddItem(conflict, 1),
                Throws.InvalidOperationException);
        }

        [Test]
        public void BuyAllStock()
        {
            Item potion   = new Item("Potion", 10m, ItemCategoria.Supply);
            Store store   = new Store(potion, 2);
            Player player = new Player(100m);

            store.Purchase(player, new List<(Item, int)> { (potion, 2) });

            Assert.That(store.GetStock("Potion", ItemCategoria.Supply), Is.EqualTo(0));
        }

        [Test]
        public void StoreWithNoStock()
        {
            Item potion   = new Item("Potion", 10m, ItemCategoria.Supply);
            Store store   = new Store(potion, 0);
            Player player = new Player(100m);

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)> { (potion, 1) }),
                Throws.InvalidOperationException);
        }

        //Player

        [TestCase(   0.0)]
        [TestCase(  50.0)]
        [TestCase(1000.0)]
        public void NewPlayer_HasCorrectGold(double gold)
        {
            Player player = new Player((decimal)gold);

            Assert.That(player.Gold, Is.EqualTo((decimal)gold));
        }

        [TestCase(  -1.0)]
        [TestCase(-500.0)]
        public void NewPlayer_NegativeGold_ThrowsException(double gold)
        {
            Assert.That(
                () => new Player((decimal)gold),
                Throws.ArgumentException);
        }

        [Test]
        public void NewPlayer_StartsWithEmptyInventory()
        {
            Player player = new Player(100m);

            Assert.That(player.Inventory.Equipment.Count, Is.EqualTo(0));
            Assert.That(player.Inventory.Supplies.Count,  Is.EqualTo(0));
        }

        [TestCase(100.0, 200.0)]
        [TestCase(  0.0, 500.0)]
        public void TwoPlayers_HaveIndependentGold(double gold1, double gold2)
        {
            Player p1 = new Player((decimal)gold1);
            Player p2 = new Player((decimal)gold2);

            Assert.That(p1.Gold, Is.EqualTo((decimal)gold1));
            Assert.That(p2.Gold, Is.EqualTo((decimal)gold2));
        }

        
        // Bbuying items
       

        [TestCase("Sword",  ItemCategoria.Weapon,    100.0, 500.0, 1, 400.0)]
        [TestCase("Potion", ItemCategoria.Supply,     20.0, 100.0, 3,  40.0)]
        [TestCase("Shield", ItemCategoria.Armor,      75.0,  75.0, 1,   0.0)]
        [TestCase("Amulet", ItemCategoria.Accessory,  50.0, 200.0, 2, 100.0)]
        public void BuyItem_GoldAndStockUpdated(
            string name, ItemCategoria category,
            double price, double startGold,
            int qty, double expectedGold)
        {
            Item item     = new Item(name, (decimal)price, category);
            Store store   = new Store(item, 10);
            Player player = new Player((decimal)startGold);

            store.Purchase(player, new List<(Item, int)> { (item, qty) });

            Assert.That(player.Gold, Is.EqualTo((decimal)expectedGold));
            Assert.That(store.GetStock(name, category), Is.EqualTo(10 - qty));
        }

        [TestCase(ItemCategoria.Weapon)]
        [TestCase(ItemCategoria.Armor)]
        [TestCase(ItemCategoria.Accessory)]
        public void BuyEquipment(ItemCategoria category)
        {
            Item item     = new Item("Gear", 10m, category);
            Store store   = new Store(item, 5);
            Player player = new Player(100m);

            store.Purchase(player, new List<(Item, int)> { (item, 2) });

            Assert.That(player.Inventory.Equipment.Count, Is.EqualTo(1));
            Assert.That(player.Inventory.Supplies.Count,  Is.EqualTo(0));
            Assert.That(player.Inventory.GetEquipmentQuantity("Gear", category), Is.EqualTo(2));
        }

        



        [Test]
        public void BuySameItemFromTwoStores_QuantityAddsUp()
        {
            Item p1       = new Item("Potion", 10m, ItemCategoria.Supply);
            Item p2       = new Item("Potion", 10m, ItemCategoria.Supply);
            Store store1  = new Store(p1, 5);
            Store store2  = new Store(p2, 5);
            Player player = new Player(100m);

            store1.Purchase(player, new List<(Item, int)> { (p1, 2) });
            store2.Purchase(player, new List<(Item, int)> { (p2, 3) });

            Assert.That(player.Inventory.GetSupplyQuantity("Potion"), Is.EqualTo(5));
        }

        [TestCase(100.0, 150.0)]
        [TestCase(  0.0,  10.0)]
        [TestCase( 99.9, 100.0)]
        public void BuyWithNotEnoughGold_NothingChanges(double startGold, double price)
        {
            Item item     = new Item("Sword", (decimal)price, ItemCategoria.Weapon);
            Store store   = new Store(item, 5);
            Player player = new Player((decimal)startGold);

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)> { (item, 1) }),
                Throws.InvalidOperationException);

            Assert.That(player.Gold, Is.EqualTo((decimal)startGold));
            Assert.That(store.GetStock("Sword", ItemCategoria.Weapon), Is.EqualTo(5));
            Assert.That(player.Inventory.Equipment.Count, Is.EqualTo(0));
        }

        [TestCase(1, 2)]
        [TestCase(0, 1)]
        [TestCase(4, 5)]
        public void BuyMoreThanInStock(int stock, int requested)
        {
            Item item      = new Item("Shield", 10m, ItemCategoria.Armor);
            Store store    = new Store(item, stock);
            Player player  = new Player(1000m);
            decimal goldBefore = player.Gold;

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)> { (item, requested) }),
                Throws.InvalidOperationException);

            Assert.That(player.Gold, Is.EqualTo(goldBefore));
            Assert.That(store.GetStock("Shield", ItemCategoria.Armor), Is.EqualTo(stock));
            Assert.That(player.Inventory.Equipment.Count, Is.EqualTo(0));
        }

       
        [Test]
        public void BuyItemNotInStore()
        {
            Item stock    = new Item("Sword",  100m, ItemCategoria.Weapon);
            Store store   = new Store(stock, 5);
            Player player = new Player(500m);
            Item foreign  = new Item("Dagger", 30m, ItemCategoria.Weapon);

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)> { (foreign, 1) }),
                Throws.InvalidOperationException);
        }

        [Test]
        public void BuyAfterStockRunsOut()
        {
            Item potion   = new Item("Potion", 10m, ItemCategoria.Supply);
            Store store   = new Store(potion, 1);
            Player player = new Player(100m);

            store.Purchase(player, new List<(Item, int)> { (potion, 1) });

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)> { (potion, 1) }),
                Throws.InvalidOperationException);
        }

        [Test]
        public void PlayerGoldBecomesZero()
        {
            Item item     = new Item("Sword", 100m, ItemCategoria.Weapon);
            Store store   = new Store(item, 1);
            Player player = new Player(100m);

            store.Purchase(player, new List<(Item, int)> { (item, 1) });

            Assert.That(player.Gold, Is.EqualTo(0m));
        }

        [Test]
        public void BuyWithEmptyCart()
        {
            Item item     = new Item("Sword", 100m, ItemCategoria.Weapon);
            Store store   = new Store(item, 5);
            Player player = new Player(500m);

            Assert.That(
                () => store.Purchase(player, new List<(Item, int)>()),
                Throws.ArgumentException);
        }
    }
}
