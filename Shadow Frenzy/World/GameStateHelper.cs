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
        PlayingField playingField = new PlayingField(20, 20);

        string name = VisualHelper.GetPlayerName();
        Character player = new Character(name, playingField.Height, playingField.Width);
        GameState game = new GameState(playingField, player);
        SpawnEnemies(game);
        return game;
    }

    public static void GetMovement(GameState gameState)
    {
        gameState.IncreaseDay();
        ConsoleKey key = Console.ReadKey(intercept: true).Key;

        switch (key)
        {
            case ConsoleKey.Z or ConsoleKey.UpArrow:
                if (gameState.Player.Y > 0) gameState.Player.Y--;
                break;
            case ConsoleKey.S or ConsoleKey.DownArrow:
                if (gameState.Player.Y < gameState.PlayingField.Height - 1) gameState.Player.Y++;
                break;
            case ConsoleKey.Q or ConsoleKey.LeftArrow:
                if (gameState.Player.X > 0) gameState.Player.X--;
                break;
            case ConsoleKey.D or ConsoleKey.RightArrow:
                if (gameState.Player.X < gameState.PlayingField.Width - 1) gameState.Player.X++;
                break;
            case ConsoleKey.Escape:
                return;
            case ConsoleKey.E or ConsoleKey.Tab:
                VisualHelper.ShowInventory(gameState);
                break;
        }
    }

    public static void GoblinMovement(GameState game)
    {
        //Get random amount of enemies to move. (TODO: needs tweaking for amount of enemeies)
        var enemiesToMove = game.Enemies.Values
            .OrderBy(_ => _random.Next())
            .Take(_random.Next(1, game.Enemies.Count + 1))
            .ToList();

        foreach (var enemy in enemiesToMove)
        {
            int newH = enemy.Y;
            int newW = enemy.X;

            if (enemy.Y < game.Player.Y) newH++;
            else if (enemy.Y > game.Player.Y) newH--;

            if (enemy.X < game.Player.X) newW++;
            else if (enemy.X > game.Player.X) newW--;

            if (!game.Enemies.ContainsKey((newH, newW)))
                game.MoveEnemy(enemy, newH, newW);
        }
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
        // Scale enemy count and stats with difficulty
        int difficultyMultiplier = (int)game.Difficulty + 1;
        int enemyCount = 1; //2 + difficultyMultiplier;
        int enemyHealth = (int)7.5 * difficultyMultiplier;
        int enemyDamage = (int)1.25 * difficultyMultiplier;
        int enemyArmor = (int)1.25 * difficultyMultiplier;

        for (int i = 0; i < enemyCount; i++)
        {
            var goblin = new Goblin(
                health: enemyHealth,
                name: $"Goblin_{game.Day}_{i}",
                armor: enemyArmor,
                damage: enemyDamage,
                x: game.PlayingField.Width,
                y: game.PlayingField.Height,
                playerY: game.Player.Y,
                playerX: game.Player.X
            );
            game.AddEnemy(goblin);
        }
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