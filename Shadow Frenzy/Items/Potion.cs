namespace Shadow_Frenzy.Items;

public class Potion : Item
{
    private int Healing { get; set; }

    public Potion(string name, string description, Rarity rarity, ItemType type, int healing) : base(name, description, rarity, type)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        Type = type;
        Healing = healing;
    }
}