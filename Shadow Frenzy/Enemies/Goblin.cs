using Shadow_Frenzy.WorldGeneration;

namespace Shadow_Frenzy.Enemies;

public class Goblin : Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Armor { get; set; }
    public int wpos { get; set; }
    public int hpos { get; set; }

    public Goblin(int health, string name, int damage, int armor, int width, int height)
    {
        Health = health;
        Name = name;
        Damage = damage;
        Armor = armor;
        (hpos, wpos) = SpawnHelper.GetEnemySpawnTile(height, width);
    }
}