using Shadow_Frenzy.Game;

namespace Shadow_Frenzy.WorldGeneration;

public static class VisualHelper
{

    public static void ShowBoard(GameState game)
    {
        for (int h = 0; h < game.World.Height; h++)
        {
            for (int w = 0; w < game.World.Width; w++)
            {
                if (h == game.Player.hpos && w == game.Player.wpos)
                    Console.Write("P");
                else if (game.Enemies.ContainsKey((h, w)))
                    Console.Write("G");
                else
                    Console.Write("█");
            }
            Console.WriteLine();
        }
    }
}