using Shadow_Frenzy.World;

namespace Shadow_Frenzy.Enemies;

public class Goblin : Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Goblin(int health, string name, int damage, int armor, int x, int y)
    {
        Health = health;
        Name = name;
        Damage = damage;
        Armor = armor;
        X = x;
        Y = y;
    }
}