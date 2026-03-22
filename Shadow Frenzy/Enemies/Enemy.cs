namespace Shadow_Frenzy.Enemies;

public interface Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage  { get; set; }
    public int Armor   { get; set; } 
    public int X { get; set; }
    public int Y { get; set; }
    
}