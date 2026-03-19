using Shadow_Frenzy.Game;

namespace Shadow_Frenzy.WorldGeneration;

public static class GameStateHelper
{
    public static void getMovement(GameState gameState)
    {
        ConsoleKey key = Console.ReadKey(intercept: true).Key;
    
        switch (key)
        {
            case ConsoleKey.Z or ConsoleKey.UpArrow:
                if (gameState.Player.hpos > 0) gameState.Player.hpos--;
                break;
            case ConsoleKey.S or ConsoleKey.DownArrow:
                if (gameState.Player.hpos < gameState.World.Height - 1) gameState.Player.hpos++;
                break;
            case ConsoleKey.Q or ConsoleKey.LeftArrow:
                if (gameState.Player.wpos > 0) gameState.Player.wpos--;
                break;
            case ConsoleKey.D or ConsoleKey.RightArrow:
                if (gameState.Player.wpos < gameState.World.Width - 1) gameState.Player.wpos++;
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}