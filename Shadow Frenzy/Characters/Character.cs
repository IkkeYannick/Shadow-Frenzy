namespace Shadow_Frenzy.Characters;

public class Character
{
    public int Health { get; set; }
    public string Name { get; set; }
    public int Damage  { get; set; }
    public int wpos { get; set; }
    public int hpos { get; set; }
    

    public Character(string name,int wpos, int hpos)
    {
        Health = 100;
        Name = name;
        Damage = 10;
        (this.wpos, this.hpos) =  (wpos/2, hpos/2);
    }
}