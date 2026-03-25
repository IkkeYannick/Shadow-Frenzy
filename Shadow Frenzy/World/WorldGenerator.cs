namespace Shadow_Frenzy.World;

public static class WorldGenerator
{
    public static Tile GetTile(int x, int y, int seed)
    {
        float elevation = Noise(x, y, seed, scale: 0.08f);
        float moisture = Noise(x, y, seed + 1, scale: 0.10f);

        TileType type = (elevation, moisture) switch
        {
            _ when elevation < 0.3f => TileType.Water,
            _ when elevation > 0.75f => TileType.Mountain,
            _ when moisture > 0.6f => TileType.Tree,
            _ => TileType.Grass,
        };

        return new Tile { Type = type };
    }

    private static float Noise(int x, int y, int seed, float scale)
    {
        float smoothed = 0;
        for (int dx = -2; dx <= 2; dx++)
        for (int dy = -2; dy <= 2; dy++)
        {
            int nx = (int)((x + dx) * scale);
            int ny = (int)((y + dy) * scale);
            int n = nx + ny * 57 + seed * 131;
            n = (n << 13) ^ n;
            smoothed += 1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f;
        }

        smoothed /= 25f;
        return (smoothed + 1f) / 2f;
    }
}