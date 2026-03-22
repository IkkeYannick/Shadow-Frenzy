using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.Characters;

public class Character
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int MaxHealth { get; set; }
    
    public int MaxMana { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public List<Item> Inventory { get; set; }
    public Dictionary<ItemType, Item> Equipped { get; set; } = new();
    public int X { get; set; }
    public int Y { get; set; }


    public Character(string name, int x, int y)
    {
        MaxHealth = 100;
        MaxMana = 100;
        Health = 100;
        Mana = 100;
        Armor = 0;
        Name = name;
        Damage = 10;
        Inventory = new List<Item>();
        (X, Y) = (x / 2, y / 2);
    }

    public void Equip(Item item)
    {
        if (Equipped.ContainsKey(item.Type) || (Equipped.Keys.Any(k => k.IsWeapon()) && item.Type.IsWeapon()))
            return;

        Equipped[item.Type] = item;
        if (item.Type.IsWeapon())
        {
            int damage = Math.Min(Damage + item.Damage, item.MaxDamage);
            Damage = damage;
            if (Damage < 10)
            {
                Damage = 10;
            }

            return;
        }

        Health = Math.Min(Health + item.Health, item.MaxHealth);
        Armor = Math.Min(Armor + item.Armor, item.MaxArmor);
    }

    public void SwitchEquipped(Item item)
    {
        if (item.Type.IsWeapon() && Equipped.Keys.Any(k => k.IsWeapon()))
        {
            // Remove whichever weapon is currently equipped
            var currentWeaponSlot = Equipped.Keys.First(k => k.IsWeapon());
            Equipped.Remove(currentWeaponSlot);
        }

        Equipped[item.Type] = item;
        if (item.Type.IsWeapon())
        {
            Damage = Math.Min(Damage + item.Damage, item.MaxDamage);
            return;
        }

        Health = Math.Min(Health + item.Health, item.MaxHealth);
        Armor = Math.Min(Armor + item.Armor, item.MaxArmor);
    }
}