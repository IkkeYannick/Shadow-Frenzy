using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.Characters;

public class Character
{
    private const int BaseHealth = 100;
    private const int BaseDamage = 10;
    private const int BaseArmor = 0;

    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public List<Item> Inventory { get; set; }
    public Dictionary<ItemType, Item> Equipped { get; set; } = new();
    public int X { get; set; }
    public int Y { get; set; }


    public Character(string name, int x, int y)
    {
        Health = BaseHealth;
        Armor = BaseArmor;
        Name = name;
        Damage = BaseDamage;
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
            if (Damage < BaseDamage)
            {
                Damage = BaseDamage;
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
            var currentWeaponSlot = Equipped.Keys.First(k => k.IsWeapon());
            RemoveStats(Equipped[currentWeaponSlot]);
            Equipped.Remove(currentWeaponSlot);
        }
        else if (Equipped.TryGetValue(item.Type, out var equippedItem))
        {
            RemoveStats(equippedItem);
            Equipped.Remove(item.Type);
        }

        Equipped[item.Type] = item;
        ApplyStats(item);
    }

    private void ApplyStats(Item item)
    {
        if (item.Type.IsWeapon())
        {
            Damage = Math.Min(Damage + item.Damage, item.MaxDamage);
            if (Damage < BaseDamage)
            {
                Damage = BaseDamage;
            }

            return;
        }

        Health = Math.Min(Health + item.Health, item.MaxHealth);
        Armor = Math.Min(Armor + item.Armor, item.MaxArmor);
    }

    private void RemoveStats(Item item)
    {
        if (item.Type.IsWeapon())
        {
            Damage = Math.Max(BaseDamage, Damage - item.Damage);
            return;
        }

        Health = Math.Max(BaseHealth, Health - item.Health);
        Armor = Math.Max(BaseArmor, Armor - item.Armor);
    }
}
