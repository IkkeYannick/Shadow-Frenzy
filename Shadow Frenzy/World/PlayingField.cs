namespace Shadow_Frenzy.World;

public class PlayingField
{
    public int Width { get; set; }
    public int Height { get; set; }
    public char[,] Tiles { get; set; }


    public PlayingField(int width, int heigth)
    {
        Width = width;
        Height = heigth;
    }
}