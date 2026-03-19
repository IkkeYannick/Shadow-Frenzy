namespace Shadow_Frenzy.WorldGeneration;

public class World
{
    public int Width { get; set; }
    public int Height { get; set; }
    public char[,] Tiles { get; set; }
    
    
    public World(int width, int heigth)
    {
        this.Width = width;
        this.Height = heigth;
    }
    
    
}