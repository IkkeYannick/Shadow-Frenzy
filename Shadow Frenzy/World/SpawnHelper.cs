namespace Shadow_Frenzy.World;

public static class SpawnHelper
{
    private static Random _random = new();
    private const int MinSpawnDistance = 5;
    private const int MaxSpawnDistance = 25;

    public static (int x, int y) GetEnemySpawnTile(int mapWidth, int mapHeight, int playerX, int playerY)
    {
        // Pick a random offset within the valid spawn ring
        int offsetX = _random.Next(MinSpawnDistance, MaxSpawnDistance + 1) * (_random.Next(2) == 0 ? 1 : -1);
        int offsetY = _random.Next(MinSpawnDistance, MaxSpawnDistance + 1) * (_random.Next(2) == 0 ? 1 : -1);

        // Clamp to map bounds
        int x = Math.Clamp(playerX + offsetX, 0, mapWidth - 1);
        int y = Math.Clamp(playerY + offsetY, 0, mapHeight - 1);

        return (x, y);
    }
}