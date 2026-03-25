using System.Runtime.CompilerServices;
using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.Game;
using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.World;

public static class GameStateHelper
{
    private static Random _random = new();

    public static GameState StartNewGame()
    {
        PlayingField playingField = new PlayingField(2000, 2000, 60, 20);

        string name = VisualHelper.GetPlayerName();
        Character player = new Character(name, playingField.Height, playingField.Width);
        player.Inventory.Add(new Item("Helmet of helms","test",Rarity.Common,ItemType.Helmet));
        player.Inventory.Add(new Item("HELMMMMM","test",Rarity.Common,ItemType.Helmet));
        GameState game = new GameState(playingField, player);

        (player.X, player.Y) = FindNearestWalkableTile(player.X, player.Y, playingField);

        SpawnEnemies(game);
        return game;
    }

    private static (int x, int y) FindNearestWalkableTile(int startX, int startY, PlayingField field)
    {
        if (field.GetTile(startX, startY).Walkable)
            return (startX, startY);

        // Spiral outward until we find a walkable tile
        for (int radius = 1; radius < field.Width; radius++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                // Only check the outer ring of this radius
                if (Math.Abs(dx) != radius && Math.Abs(dy) != radius) continue;

                int x = Math.Clamp(startX + dx, 0, field.Width - 1);
                int y = Math.Clamp(startY + dy, 0, field.Height - 1);

                if (field.GetTile(x, y).Walkable)
                    return (x, y);
            }
        }

        return (startX, startY); // fallback, should never hit this
    }

    public static void GetMovement(GameState game)
    {
        game.IncreaseDay();
        ConsoleKey key = Console.ReadKey(intercept: true).Key;
        var player = game.Player;

        switch (key)
        {
            case ConsoleKey.Z or ConsoleKey.W or ConsoleKey.UpArrow:
                if (player.Y > 0 && game.PlayingField.GetTile(player.Y - 1, player.X).Walkable)
                    player.Y--;
                break;
            case ConsoleKey.S or ConsoleKey.DownArrow:
                if (player.Y < game.PlayingField.Height - 1 &&
                    game.PlayingField.GetTile(player.Y + 1, player.X).Walkable)
                    player.Y++;
                break;
            case ConsoleKey.Q or ConsoleKey.A or ConsoleKey.LeftArrow:
                if (player.X > 0 && game.PlayingField.GetTile(player.Y, player.X - 1).Walkable)
                    player.X--;
                break;
            case ConsoleKey.D or ConsoleKey.RightArrow:
                if (player.X < game.PlayingField.Width - 1 &&
                    game.PlayingField.GetTile(player.Y, player.X + 1).Walkable)
                    player.X++;
                break;
            case ConsoleKey.Escape:
                return;
            case ConsoleKey.E or ConsoleKey.Tab:
                VisualHelper.ShowInventory(game);
                break;
        }
    }

    public static void EnemyMovement(GameState game)
    {
        var enemiesToMove = game.Enemies.Values
            .OrderBy(_ => _random.Next())
            .Take(_random.Next(1, game.Enemies.Count + 1))
            .ToList();

        foreach (var enemy in enemiesToMove)
        {
            int dirH = enemy.Y < game.Player.Y ? 1 : enemy.Y > game.Player.Y ? -1 : 0;
            int dirW = enemy.X < game.Player.X ? 1 : enemy.X > game.Player.X ? -1 : 0;

            // Try candidates in priority order: direct, slide horizontal, slide vertical, diagonal back
            (int h, int w)[] candidates =
            [
                (enemy.Y + dirH, enemy.X + dirW), // Direct toward player
                (enemy.Y, enemy.X + dirW), // Slide horizontally
                (enemy.Y + dirH, enemy.X), // Slide vertically
                (enemy.Y + dirH, enemy.X - dirW), // Diagonal away on one axis
                (enemy.Y - dirH, enemy.X + dirW), // Diagonal away on other axis
            ];

            foreach (var (h, w) in candidates)
            {
                if (CheckEnemyMovementTile(h, w, game) && !game.Enemies.ContainsKey((h, w)))
                {
                    game.MoveEnemy(enemy, h, w);
                    break;
                }
            }
        }
    }

    private static bool CheckEnemyMovementTile(int newH, int newW, GameState game)
    {
        if (game.PlayingField.GetTile(newH, newW).Walkable)
        {
            return true;
        }

        return false;
    }

    public static void CheckCombat(GameState game)
    {
        foreach (var pair in game.Enemies)
        {
            var enemy = pair.Value;

            int hDiff = Math.Abs(enemy.Y - game.Player.Y);
            int wDiff = Math.Abs(enemy.X - game.Player.X);

            if (hDiff <= 1 && wDiff <= 1)
                VisualHelper.ShowCombat(enemy, game.Player, game);
        }
    }

    public static void GenerateItem(GameState game)
    {
        Rarity rarity = GetRandomRarity();
        ItemType itemType = GetRandomItemType();
        string itemName = GenerateItemName(rarity, itemType);
        Item generatedItem = new Item(itemName, "NOT IMPLEMENTED YET", rarity, itemType);
        //Show new item:
        VisualHelper.ShowNewItem(generatedItem);
        //Add item to player inventory
        game.Player.Inventory.Add(generatedItem);
    }

    public static Rarity GetRandomRarity()
    {
        int roll = _random.Next(0, 100);

        return roll switch
        {
            < 40 => Rarity.Common, // 40%
            < 65 => Rarity.Uncommon, // 25%
            < 80 => Rarity.Rare, // 15%
            < 90 => Rarity.Epic, // 10%
            < 96 => Rarity.Legendary, //  5%
            < 99 => Rarity.Mythic, //  3%
            _ => Rarity.Divine //  2%
        };
    }

    public static ItemType GetRandomItemType()
    {
        int roll = _random.Next(0, 100);
        bool isMelee = _random.Next(0, 2) == 0;

        if (isMelee)
        {
            return roll switch
            {
                < 25 => ItemType.Axe,
                < 50 => ItemType.Pickaxe,
                < 75 => ItemType.Sword,
                _ => ItemType.Shovel
            };
        }

        return roll switch
        {
            < 20 => ItemType.Gauntlets,
            < 40 => ItemType.Helmet,
            < 60 => ItemType.Chestplate,
            < 80 => ItemType.Pants,
            _ => ItemType.Boots
        };
    }

    public static string GenerateItemName(Rarity rarity, ItemType type)
    {
        string[] prefixes =
        {
            "Shadow", "Iron", "Cursed", "Ancient", "Frozen",
            "Blazing", "Void", "Storm", "Silent", "Runed"
        };

        string[] suffixes =
        {
            "of Doom", "of the Fallen", "of Eternity", "of the Abyss",
            "of Shadows", "of the Ancients", "of Ruin", "of Glory"
        };

        string prefix = prefixes[_random.Next(prefixes.Length)];
        string suffix = suffixes[_random.Next(suffixes.Length)];

        return rarity switch
        {
            Rarity.Common => $"{prefix} {type}", // "Shadow Sword"
            Rarity.Uncommon => $"{prefix} {type}", // "Frozen Helmet"
            Rarity.Rare => $"{prefix} {type} {suffix}", // "Blazing Sword of Doom"
            Rarity.Epic => $"{prefix} {type} {suffix}", // "Void Helmet of Ruin"
            Rarity.Legendary => $"Legendary {prefix} {type} {suffix}", // "Legendary Storm Sword of Glory"
            Rarity.Mythic => $"Mythic {prefix} {type} {suffix}", // "Mythic Cursed Boots of Eternity"
            Rarity.Divine => $"Divine {prefix} {type} {suffix}", // "Divine Ancient Chestplate of the Ancients"
            _ => $"{prefix} {type}"
        };
    }

    public static int BlockAttempt(Enemy enemy, Character player)
    {
        bool blocked = _random.Next(0, 2) == 0;
        if (blocked)
        {
            return 0;
        }
        else
        {
            int reduced = Math.Max(0, enemy.Damage - player.Armor) / 2;
            player.Health -= reduced;
            if (reduced == 0)
            {
                return 0;
            }

            return reduced;
        }
    }

    public static int AttackEnemy(Enemy enemy, Character player)
    {
        int damageDealt = Math.Max(0, player.Damage - enemy.Armor);
        enemy.Health -= damageDealt;
        return damageDealt;
    }

    public static int EnemyAttack(Enemy enemy, Character player)
    {
        int damageTaken = Math.Max(0, enemy.Damage - player.Armor);
        player.Health -= damageTaken;
        return damageTaken;
    }

    public static int Hesitate(Enemy enemy, Character player)
    {
        int hesitationDamageTaken = Math.Max(0, enemy.Damage - player.Armor) * 2;
        player.Health -= hesitationDamageTaken;
        return hesitationDamageTaken;
    }

    public static void DefeatEnemy(GameState game, Enemy enemy)
    {
        game.Enemies.Remove((enemy.Y, enemy.X));
        //Chance to drop loot (20%)
        bool loot = _random.Next(0, 5) == 0;
        if (loot)
        {
            GenerateItem(game);
        }
    }

    public static void UpdateDifficulty(GameState game)
    {
        if (game.Enemies.Count != 0) return;

        int max = Enum.GetValues(typeof(Difficulty)).Length - 1;
        int nextDifficultyIndex = (int)game.Difficulty + 1;

        if (nextDifficultyIndex <= max)
        {
            Difficulty nextDifficulty = (Difficulty)nextDifficultyIndex;
            if (game.Day >= nextDifficulty.DayThreshold())
                game.IncreaseDifficulty();
        }

        SpawnEnemies(game);
    }

    public static void SpawnEnemies(GameState game)
    {
        int difficultyMultiplier = (int)game.Difficulty + 1;
        int enemyCount = 2 + difficultyMultiplier;
        int enemyHealth = (int)7.5 * difficultyMultiplier;
        int enemyDamage = (int)1.25 * difficultyMultiplier;
        int enemyArmor = (int)1.25 * difficultyMultiplier;

        for (int i = 0; i < enemyCount; i++)
        {
            (int x, int y) = SpawnHelper.GetEnemySpawnTile(game.PlayingField.Height, game.PlayingField.Width,
                game.Player.Y, game.Player.X);

            while (!IsSpawnableTile(x, y, game))
                (x, y) = SpawnHelper.GetEnemySpawnTile(game.PlayingField.Height, game.PlayingField.Width, game.Player.Y,
                    game.Player.X);

            var goblin = new Goblin(
                health: enemyHealth,
                name: $"Goblin_{game.Day}_{i}",
                armor: enemyArmor,
                damage: enemyDamage,
                x: x,
                y: y
            );
            game.AddEnemy(goblin);
        }
    }

    public static bool IsSpawnableTile(int x, int y, GameState game)
    {
        if (game.PlayingField.GetTile(y, x).Walkable) return true;
        return false;
    }

    public static void SwitchEquipped(GameState game, Item item)
    {
        game.Player.SwitchEquipped(item);
    }

    public static void EquipItem(GameState game, Item item)
    {
        game.Player.Equip(item);
    }
}