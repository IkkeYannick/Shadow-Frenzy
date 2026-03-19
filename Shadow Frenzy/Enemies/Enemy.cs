namespace Shadow_Frenzy.Enemies;

public interface Enemy
{
    public int Health { get; set; }
    public string Name { get; set; }
    public int Damage  { get; set; }
    public int wpos { get; set; }
    public int hpos { get; set; }
    
}