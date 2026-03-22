namespace Shadow_Frenzy.WorldGeneration;

public static class SpawnHelper
{
    private static Random _random = new();

    public static (int height, int width) GetEnemySpawnTile(int height, int width, int playerHeight, int playerWidth)
    {
        int rndHeight = _random.Next(1, height);
        int rndWidth = _random.Next(1, width);

        while (IsTooCloseToCenter(rndHeight, rndWidth, height, width) ||
               IsTooCloseToPlayer(rndHeight, rndWidth, playerHeight, playerWidth))
        {
            rndHeight = _random.Next(1, height);
            rndWidth = _random.Next(1, width);
        }

        return (rndHeight, rndWidth);
    }

    private static bool IsTooCloseToCenter(int h, int w, int height, int width)
    {
        int middleHeight = height / 2;
        int middleWidth = width / 2;
        return h > middleHeight - 5 && h < middleHeight + 5
               || w > middleWidth - 5 && w < middleWidth + 5;
    }

    private static bool IsTooCloseToPlayer(int h, int w, int playerHeight, int playerWidth)
    {
        int hDiff = Math.Abs(h - playerHeight);
        int wDiff = Math.Abs(w - playerWidth);
        return hDiff <= 5 && wDiff <= 5;
    }
}