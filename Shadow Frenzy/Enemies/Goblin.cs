using Shadow_Frenzy.WorldGeneration;

namespace Shadow_Frenzy.Enemies;

public class Goblin : Enemy
{
    public int Health { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    public int wpos { get; set; }
    public int hpos { get; set; }

    public Goblin(int health, string name, int damage,int width, int height)
    {
        Health = health;
        Name = name;
        Damage = damage;
        (hpos, wpos) = SpawnHelper.GetEnemySpawnTile(height, width);
    }
}