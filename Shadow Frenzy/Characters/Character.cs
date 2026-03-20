namespace Shadow_Frenzy.Characters;

public class Character
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage  { get; set; }
    public List<Item> Inventory { get; set; }
    public Dictionary<ItemType, Item> Equipped { get; set; } = new();
    public int wpos { get; set; }
    public int hpos { get; set; }
    

    public Character(string name,int wpos, int hpos)
    {
        Health = 100;
        Name = name;
        Damage = 10;
        Inventory = new List<Item>();
        (this.wpos, this.hpos) =  (wpos/2, hpos/2);
    }
    
    public bool Equip(Item item)
    {
        if (item.Type.IsWeapon() && Equipped.ContainsKey(ItemType.Sword))
            return false;

        // Store all weapons under the same key so only 1 can be equipped
        ItemType slot = item.Type.IsWeapon() ? ItemType.Sword : item.Type;
        Equipped[slot] = item;
        return true;
    }
}