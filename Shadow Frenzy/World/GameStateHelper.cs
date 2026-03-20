using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.Game;
using Shadow_Frenzy.Items;

namespace Shadow_Frenzy.World;

public static class GameStateHelper
{
    public static Random _random = new();

    public static GameState StartNewGame()
    {
        PlayingField playingField = new PlayingField(20, 20);

        string name = VisualHelper.GetPlayerName();
        Character player = new Character(name, playingField.Height, playingField.Width);
        player.Inventory.Add(new Item("Test Sword", "test", Rarity.Divine, ItemType.Sword));
        player.Inventory.Add(new Item("Test Pickaxe", "test", Rarity.Divine, ItemType.Pickaxe));
        player.Inventory.Add(new Item("Test Helmet", "test", Rarity.Divine, ItemType.Helmet));
        player.Inventory.Add(new Item("Super Helmet", "test", Rarity.Divine, ItemType.Helmet));
        player.Inventory.Add(new Item("Test Chestplate", "test", Rarity.Divine, ItemType.Chestplate));
        player.Inventory.Add(new Item("Super Chestplate", "test", Rarity.Divine, ItemType.Chestplate));

        Goblin een = new Goblin(20, "een", 5, 3, playingField.Width, playingField.Height);
        Goblin twee = new Goblin(20, "twee", 5, 2, playingField.Width, playingField.Height);
        Goblin drie = new Goblin(20, "drie", 5, 0, playingField.Width, playingField.Height);

        GameState game = new GameState(playingField, player);
        game.AddEnemy(een);
        game.AddEnemy(twee);
        game.AddEnemy(drie);

        return game;
    }

    public static void GetMovement(GameState gameState)
    {
        ConsoleKey key = Console.ReadKey(intercept: true).Key;

        switch (key)
        {
            case ConsoleKey.Z or ConsoleKey.UpArrow:
                if (gameState.Player.hpos > 0) gameState.Player.hpos--;
                break;
            case ConsoleKey.S or ConsoleKey.DownArrow:
                if (gameState.Player.hpos < gameState.PlayingField.Height - 1) gameState.Player.hpos++;
                break;
            case ConsoleKey.Q or ConsoleKey.LeftArrow:
                if (gameState.Player.wpos > 0) gameState.Player.wpos--;
                break;
            case ConsoleKey.D or ConsoleKey.RightArrow:
                if (gameState.Player.wpos < gameState.PlayingField.Width - 1) gameState.Player.wpos++;
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
            int newH = enemy.hpos;
            int newW = enemy.wpos;

            if (enemy.hpos < game.Player.hpos) newH++;
            else if (enemy.hpos > game.Player.hpos) newH--;

            if (enemy.wpos < game.Player.wpos) newW++;
            else if (enemy.wpos > game.Player.wpos) newW--;

            if (!game.Enemies.ContainsKey((newH, newW)))
                game.MoveEnemy(enemy, newH, newW);
        }
    }

    public static void checkCombat(GameState game)
    {
        foreach (var pair in game.Enemies)
        {
            var enemy = pair.Value;

            int hDiff = Math.Abs(enemy.hpos - game.Player.hpos);
            int wDiff = Math.Abs(enemy.wpos - game.Player.wpos);

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
}