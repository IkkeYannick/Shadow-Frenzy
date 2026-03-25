namespace Shadow_Frenzy.World;

public enum TileType { Grass, Tree, Mountain, Water }

public struct Tile
{
    public TileType Type;
    public bool Walkable => Type != TileType.Water && Type != TileType.Mountain;
    public char Symbol => Type switch
    {
        TileType.Tree     => '^',
        TileType.Mountain => '$',
        TileType.Water    => '~',
        _                 => '.',
    };
}