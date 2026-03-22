using Shadow_Frenzy.Game;
using Shadow_Frenzy.World;


GameState game = GameStateHelper.StartNewGame();
VisualHelper.ShowPlayerInfo(game.Player,start : true);
VisualHelper.WaitForStart();
while (true)
{
    while (game.Player.Health > 0)
    {
        Console.Clear();
        VisualHelper.ShowBoard(game);
        GameStateHelper.GetMovement(game);
        GameStateHelper.GoblinMovement(game);
        GameStateHelper.CheckCombat(game);
        GameStateHelper.UpdateDifficulty(game);
    }
    ConsoleKey key = Console.ReadKey(intercept: true).Key;
    if (key == ConsoleKey.R)
    {
        game = GameStateHelper.StartNewGame();
    }
    else
    {
        Environment.Exit(0);
    }
}