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
Character player =  new Character(name, world.Height, world.Width);

Console.WriteLine($"Your character: {player.Name}\nHP: {player.Health}\nDamage: {player.Damage}");
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
    Console.Clear();
    VisualHelper.ShowBoard(game);
    GameStateHelper.getMovement(game);
}