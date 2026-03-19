namespace Shadow_Frenzy.WorldGeneration;

public static class SpawnHelper
{
    public static (int height, int width) GetEnemySpawnTile(int height, int width)
    {
        Random rnd = new Random();
        int middleHeight = height / 2;
        int middleWidth = width / 2;
        int rndHeight = rnd.Next(1, height);
        int rndWidth = rnd.Next(1, width);

        while (rndHeight > middleHeight - 5 && rndHeight < middleHeight + 5
               || rndWidth > middleWidth - 5 && rndWidth < middleWidth + 5)
        {
            rndHeight = rnd.Next(1, height);
            rndWidth = rnd.Next(1, width);
        }

        return (rndHeight, rndWidth);
    }

    public static (int height, int width) GetPlayerSpawnTile(int height, int width)
    {
        return (height/2, width/2);
    }
}