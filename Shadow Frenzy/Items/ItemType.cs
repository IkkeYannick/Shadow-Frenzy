namespace Shadow_Frenzy.Characters;

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
    Boots
}

public static class ItemTypeExtensions
{
    public static bool IsWeapon(this ItemType type)
    {
        return type is ItemType.Sword or ItemType.Axe or ItemType.Shovel or ItemType.Pickaxe;
    }
}