using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.Game;
using Shadow_Frenzy.WorldGeneration;

World world = new World(20,20);
Console.WriteLine("The game is starting");
Console.WriteLine("Enter your characters name:");
string? name = Console.ReadLine();
while (name.Length == 0)
{
    Console.WriteLine("Enter your characters name:");
    name = Console.ReadLine();
}
Character player =  new Character(name, world.Height/2, world.Width/2);

Console.WriteLine($"Your character: {player.Name}/n" +
                  $"HP: {player.Health}/n" +
                  $"Damage: {player.Damage}");
Goblin een = new Goblin(20, "een", 5,world.Width,world.Height);
Goblin twee = new Goblin(20, "twee", 5,world.Width,world.Height);
Goblin drie = new Goblin(20, "drie", 5,world.Width,world.Height);

GameState game = new GameState(world, player);
game.AddEnemy(een);
game.AddEnemy(twee);
game.AddEnemy(drie);

Console.WriteLine("Press enter to start.");
Console.ReadLine();
while (true)
{
    VisualHelper.ShowBoard(game);
    //position check
    ConsoleKey key = Console.ReadKey(intercept: true).Key;
    
    switch (key)
    {
        case ConsoleKey.Z or ConsoleKey.UpArrow:
            if (player.hpos > 0) player.hpos--;
            break;
        case ConsoleKey.S or ConsoleKey.DownArrow:
            if (player.hpos < world.Height - 1) player.hpos++;
            break;
        case ConsoleKey.Q or ConsoleKey.LeftArrow:
            if (player.wpos > 0) player.wpos--;
            break;
        case ConsoleKey.D or ConsoleKey.RightArrow:
            if (player.wpos < world.Width - 1) player.wpos++;
            break;
        case ConsoleKey.Escape:
            return;
    }
}