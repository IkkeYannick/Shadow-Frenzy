namespace Shadow_Frenzy.Items;

public enum ItemType
{
    Sword,
    Axe,
    Shovel,
    Pickaxe,
    Helmet,
    Chestplate,
    Gauntlets,
    Pants,
    Boots,
    Potion
}

public static class ItemTypeExtensions
{
    public static bool IsWeapon(this ItemType type)
    {
        return type is ItemType.Sword or ItemType.Axe or ItemType.Shovel or ItemType.Pickaxe;
    }

    public static bool IsPotion(this ItemType type)
    {
        return type is ItemType.Potion;
    }
}