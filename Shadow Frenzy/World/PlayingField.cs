namespace Shadow_Frenzy.World;

public class PlayingField
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int ViewportWidth { get; set; }
    public int ViewportHeight { get; set; }
    public int Seed { get; set; }
    public Dictionary<(int y, int x), TileType> Overrides { get; set; } = new();

    public PlayingField(int width, int height, int viewportWidth, int viewportHeight)
    {
        Width = width;
        Height = height;
        ViewportWidth = viewportWidth;
        ViewportHeight = viewportHeight;
        Seed = new Random().Next();
    }

    public Tile GetTile(int y, int x)
    {
        if (Overrides.TryGetValue((y, x), out TileType overrideType))
            return new Tile { Type = overrideType };

        return WorldGenerator.GetTile(x, y, Seed);
    }
}