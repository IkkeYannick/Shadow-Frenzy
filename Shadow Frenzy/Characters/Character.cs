using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.Characters;

public class Character
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public List<Item> Inventory { get; set; }
    public Dictionary<ItemType, Item> Equipped { get; set; } = new();
    public int wpos { get; set; }
    public int hpos { get; set; }


    public Character(string name, int wpos, int hpos)
    {
        Health = 100;
        Armor = 0;
        Name = name;
        Damage = 10;
        Inventory = new List<Item>();
        (this.wpos, this.hpos) = (wpos / 2, hpos / 2);
    }

    public void Equip(Item item)
    {
        if (Equipped.ContainsKey(item.Type) || (Equipped.Keys.Any(k => k.IsWeapon()) && item.Type.IsWeapon()))
            return;

        Equipped[item.Type] = item;
        if (item.Type.IsWeapon())
        {
            Damage = Math.Min(Damage + item.Damage, item.MaxDamage);
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