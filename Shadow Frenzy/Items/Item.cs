namespace Shadow_Frenzy.Characters;

public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public int MaxDamage { get; set; }
    public int Armor { get; set; }
    public int MaxArmor { get; set; }
    public Rarity Rarity {  get; set; } 
    public ItemType Type { get; set; }

    public Item(string name, string description, Rarity rarity,ItemType type)
    {
        Random random = new();
        Name = name;
        Description = description;
        Rarity = rarity;
        Type  = type;
        if (type.Equals(ItemType.Sword) ||  type.Equals(ItemType.Axe) ||   type.Equals(ItemType.Pickaxe) ||  type.Equals(ItemType.Shovel))
        {
            Damage = random.Next(0, 10);
            MaxDamage = random.Next(0, 50);
        }
        else
        {
            Health = random.Next(0, 10);
            MaxHealth = random.Next(100, 150);
            Armor = random.Next(0, 10);
            MaxArmor = random.Next(0, 50);
        }
    }
}