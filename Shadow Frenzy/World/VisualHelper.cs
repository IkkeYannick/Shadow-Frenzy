using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.Game;
using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.World;

public static class VisualHelper
{
    private static Random _random = new();


    public static string GetPlayerName()
    {
        Console.WriteLine("The game is starting");
        Console.WriteLine("Enter your characters name:");
        string? name = Console.ReadLine();

        while (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Enter your characters name:");
            name = Console.ReadLine();
        }

        return name;
    }

    public static void ShowPlayerInfo(Character player)
    {
        Console.WriteLine($"""
                           Your character: {player.Name}
                           HP: {player.Health}
                           Damage: {player.Damage}
                           """);
    }

    public static void WaitForStart()
    {
        Console.WriteLine("Press enter to start.");
        Console.ReadLine();
    }

    public static void ShowBoard(GameState game)
    {
        for (int h = 0; h < game.PlayingField.Height; h++)
        {
            for (int w = 0; w < game.PlayingField.Width; w++)
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

    public static void ShowCombat(Enemy enemy, Character player, GameState game)
    {
        Console.WriteLine($"You ran into a: {enemy.Name}!");

        while (enemy.Health > 0 && player.Health > 0)
        {
            Console.WriteLine("============================");
            Console.WriteLine($"{enemy.Name} HP: {enemy.Health}");
            Console.WriteLine($"Your HP: {player.Health}");
            Console.WriteLine("============================");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Block");

            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    // Player attacks enemy
                    int damageDealt = Math.Max(0, player.Damage - enemy.Armor);
                    enemy.Health -= damageDealt;
                    Console.WriteLine($"You dealt {damageDealt} damage to {enemy.Name}!");

                    // Enemy attacks back
                    if (enemy.Health > 0)
                    {
                        int damageTaken = Math.Max(0, enemy.Damage - player.Armor);
                        player.Health -= damageTaken;
                        Console.WriteLine($"{enemy.Name} dealt {damageTaken} damage to you!");
                    }

                    break;

                case ConsoleKey.D2:
                    // Block: 50% chance to fully block, otherwise take half damage
                    bool blocked = _random.Next(0, 2) == 0;
                    if (blocked)
                    {
                        Console.WriteLine("You fully blocked the attack!");
                    }
                    else
                    {
                        int reduced = Math.Max(0, enemy.Damage - player.Armor) / 2;
                        player.Health -= reduced;
                        if (reduced == 0)
                        {
                            Console.WriteLine("You fully blocked the attack!");
                        }

                        Console.WriteLine($"You partially blocked, taking {reduced} damage!");
                    }

                    break;

                default:
                    int hesitationDamageTaken = Math.Max(0, enemy.Damage - player.Armor) * 2;
                    Console.WriteLine(
                        $"Invalid input, you hesitated and got hit for:{hesitationDamageTaken} by: {enemy.Name}!");
                    player.Health -= hesitationDamageTaken;
                    break;
            }
        }

        if (enemy.Health <= 0)
        {
            Console.WriteLine($"You defeated {enemy.Name}!");
            game.Enemies.Remove((enemy.hpos, enemy.wpos));
            //Chance to drop loot (20%)
            bool loot = _random.Next(0, 5) == 0;
            if (loot)
            {
                GameStateHelper.GenerateItem(game);
            }
        }
        else
            Console.WriteLine("You died!");
    }

    public static void ShowInventory(GameState game)
    {
        int selected = 0;
        bool detailView = false;

        while (true)
        {
            Console.Clear();
            const int width = 40;
            string line = new string('═', width);
            var inventory = game.Player.Inventory;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"╔{line}╗");
            PrintRow("  INVENTORY");
            Console.WriteLine($"╠{line}╣");
            PrintRow($"  {game.Player.Name}");
            PrintRow($"  HP: {game.Player.Health}");
            PrintRow($"  DAMAGE: {game.Player.Damage}");
            PrintRow($"  ARMOR: {game.Player.Armor}");
            Console.WriteLine($"╠{line}╣");

            if (inventory.Count == 0)
            {
                PrintRow("  No items in inventory");
            }
            else if (detailView)
            {
                // Detail view for selected item
                var item = inventory[selected];
                ConsoleColor color = GetRarityColor(item.Rarity);
                bool isEquipped = game.Player.Equipped.TryGetValue(item.Type, out Item? eq) && eq == item;

                Console.WriteLine($"╠{line}╣");
                PrintRow($"  {item.Name}", color: color);
                PrintRow($"  Type:   {item.Type}", color: color);
                PrintRow($"  Rarity: {item.Rarity}", color: color);
                if (item.Damage > 0) PrintRow($"  Damage: {item.Damage}", color: color);
                if (item.Health > 0) PrintRow($"  Health: {item.Health}", color: color);
                if (item.Armor > 0) PrintRow($"  Armor:  {item.Armor}", color: color);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"╠{line}╣");
                PrintRow(isEquipped ? "  [EQUIPPED]" : "  Press E to equip",
                    color: isEquipped ? ConsoleColor.Green : ConsoleColor.White);
                PrintRow("  Press LEFT to go back");
            }
            else
            {
                // List view
                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    bool isEquipped = game.Player.Equipped.TryGetValue(item.Type, out Item? eq) && eq == item;
                    ConsoleColor color = GetRarityColor(item.Rarity);
                    string prefix = i == selected ? "> " : "  ";
                    string equippedTag = isEquipped ? " [E]" : "";
                    PrintRow($"{prefix}{item.Name}{equippedTag}", color: color);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"╠{line}╣");
                }

                PrintRow("  UP/DOWN to navigate, RIGHT for details");
            }

            Console.WriteLine($"╚{line}╝");
            Console.ResetColor();

            // Input
            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (!detailView && selected > 0) selected--;
                    break;
                case ConsoleKey.DownArrow:
                    if (!detailView && selected < inventory.Count - 1) selected++;
                    break;
                case ConsoleKey.RightArrow:
                    if (inventory.Count > 0) detailView = true;
                    break;
                case ConsoleKey.LeftArrow:
                    if (!detailView)
                    {
                        return;
                    }

                    detailView = false;
                    break;
                case ConsoleKey.E:
                    if (detailView)
                    {
                        var item = inventory[selected];
                        bool alreadyEquipped = game.Player.Equipped.TryGetValue(item.Type, out Item? currentItem) &&
                                               currentItem == item;

                        if (alreadyEquipped)
                        {
                            PrintRow("  Already equipped!", color: ConsoleColor.Yellow);
                            Thread.Sleep(800);
                        }
                        else if (game.Player.Equipped.ContainsKey(item.Type) ||
                                 (game.Player.Equipped.Keys.Any(k => k.IsWeapon()) && item.Type.IsWeapon()))
                        {
                            // Something else is in this slot, ask to switch
                            Console.Clear();

                            Console.WriteLine($"╔{line}╗");
                            PrintRow("  SWITCH ITEM?");
                            Console.WriteLine($"╠{line}╣");
                            PrintRow(
                                $"  Current: {game.Player.Equipped[game.Player.Equipped.Keys.First(k => k.IsWeapon())].Name}",
                                color: GetRarityColor(game.Player
                                    .Equipped[game.Player.Equipped.Keys.First(k => k.IsWeapon())].Rarity));
                            PrintRow($"  New:     {item.Name}",
                                color: GetRarityColor(item.Rarity));
                            Console.WriteLine($"╠{line}╣");
                            PrintRow("  Y to confirm, any key to cancel");
                            Console.WriteLine($"╚{line}╝");

                            ConsoleKey confirm = Console.ReadKey(intercept: true).Key;
                            if (confirm == ConsoleKey.Y)
                            {
                                game.Player.SwitchEquipped(item);
                                PrintRow("  Item switched!", color: ConsoleColor.Green);
                                Thread.Sleep(800);
                            }
                        }
                        else
                        {
                            game.Player.Equip(item);
                            PrintRow("  Equipped!", color: ConsoleColor.Green);
                            Thread.Sleep(800);
                        }
                    }

                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    public static void ShowNewItem(Item item)
    {
        Console.Clear();
        const int width = 40;
        string line = new string('═', width);

        Console.WriteLine("You found an item!");
        Console.WriteLine("Press any key to uncover it:");
        Console.ReadKey(intercept: true);

        string[] frames =
        {
            "[ ? ? ? ]",
            "[ !  ?  ]",
            "[ ! ! ? ]",
            "[ ! ! ! ]",
        };

        foreach (var frame in frames)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(frame);
            Thread.Sleep(300);
        }

        // Spin the box
        string[] spin = { "|", "/", "-", "\\", "|", "/", "-", "\\" };
        foreach (var s in spin)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"[ {s} ]   ");
            Thread.Sleep(100);
        }

        // Flash the reveal
        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[ *** ]");
            Thread.Sleep(150);
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.ResetColor();
            Console.Write("[     ]");
            Thread.Sleep(150);
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"╔{line}╗");
        PrintRow($"{item.Name} Aquired!", color: ConsoleColor.Green);
        Console.WriteLine($"╚{line}╝");
        Console.ResetColor();
        Thread.Sleep(1500);
    }

    private static ConsoleColor GetRarityColor(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => ConsoleColor.White,
            Rarity.Uncommon => ConsoleColor.Green,
            Rarity.Rare => ConsoleColor.Blue,
            Rarity.Epic => ConsoleColor.Magenta,
            Rarity.Legendary => ConsoleColor.Yellow,
            Rarity.Mythic => ConsoleColor.Red,
            Rarity.Divine => ConsoleColor.Cyan,
            _ => ConsoleColor.White
        };
    }

    private static void PrintRow(string text, int width = 40, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        string padded = text.Length > width ? text[..width] : text.PadRight(width);
        Console.WriteLine($"║{padded}║");
    }
}