using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.Game;
using Shadow_Frenzy.Items;
using Shadow_Frenzy.World;

return TestRunner.RunAll();

static class TestRunner
{
    private static readonly List<(string Name, Action Test)> Tests =
    [
        ("Character starts with base stats centered on the map", CharacterStartsWithBaseStatsCenteredOnMap),
        ("Equip adds armor gear stats and tracks the slot", EquipAddsArmorGearStatsAndTracksTheSlot),
        ("Equip ignores a second weapon when one is already equipped", EquipIgnoresSecondWeaponWhenOneIsAlreadyEquipped),
        ("SwitchEquipped replaces a weapon instead of stacking damage", SwitchEquippedReplacesAWeaponInsteadOfStackingDamage),
        ("AttackEnemy subtracts armor before damaging the enemy", AttackEnemySubtractsArmorBeforeDamagingTheEnemy),
        ("EnemyAttack subtracts armor before damaging the player", EnemyAttackSubtractsArmorBeforeDamagingThePlayer),
        ("Hesitate doubles the post-armor damage", HesitateDoublesThePostArmorDamage),
        ("UpdateDifficulty increases the tier and spawns a new wave when thresholds are met", UpdateDifficultyIncreasesTierAndSpawnsNewWaveWhenThresholdsAreMet),
        ("IsSpawnableTile respects blocked tiles", IsSpawnableTileRespectsBlockedTiles),
        ("GenerateItemName follows the rarity naming rules", GenerateItemNameFollowsTheRarityNamingRules),
        ("GetEnemySpawnTile stays inside bounds and away from the player on a large map", GetEnemySpawnTileStaysInsideBoundsAndAwayFromThePlayerOnALargeMap)
    ];

    public static int RunAll()
    {
        var failures = new List<string>();

        foreach (var (name, test) in Tests)
        {
            try
            {
                test();
                Console.WriteLine($"PASS {name}");
            }
            catch (Exception ex)
            {
                failures.Add($"{name}: {ex.Message}");
                Console.WriteLine($"FAIL {name}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"{Tests.Count - failures.Count}/{Tests.Count} tests passed.");

        if (failures.Count == 0)
        {
            return 0;
        }

        Console.WriteLine("Failures:");
        foreach (var failure in failures)
        {
            Console.WriteLine($" - {failure}");
        }

        return 1;
    }

    private static void CharacterStartsWithBaseStatsCenteredOnMap()
    {
        var character = new Character("Test", 20, 30);

        Assert.Equal(100, character.Health);
        Assert.Equal(10, character.Damage);
        Assert.Equal(0, character.Armor);
        Assert.Equal(10, character.X);
        Assert.Equal(15, character.Y);
    }

    private static void EquipAddsArmorGearStatsAndTracksTheSlot()
    {
        var character = new Character("Test", 20, 20);
        var helmet = CreateItem(ItemType.Helmet, health: 6, maxHealth: 150, armor: 4, maxArmor: 50);

        character.Equip(helmet);

        Assert.Equal(106, character.Health);
        Assert.Equal(4, character.Armor);
        Assert.True(character.Equipped.ContainsKey(ItemType.Helmet), "Helmet should be marked as equipped.");
    }

    private static void EquipIgnoresSecondWeaponWhenOneIsAlreadyEquipped()
    {
        var character = new Character("Test", 20, 20);
        var sword = CreateItem(ItemType.Sword, damage: 5, maxDamage: 50);
        var axe = CreateItem(ItemType.Axe, damage: 9, maxDamage: 50);

        character.Equip(sword);
        character.Equip(axe);

        Assert.Equal(15, character.Damage);
        Assert.True(character.Equipped.ContainsKey(ItemType.Sword), "Original weapon should remain equipped.");
        Assert.False(character.Equipped.ContainsKey(ItemType.Axe), "Second weapon should be ignored.");
    }

    private static void SwitchEquippedReplacesAWeaponInsteadOfStackingDamage()
    {
        var character = new Character("Test", 20, 20);
        var sword = CreateItem(ItemType.Sword, damage: 5, maxDamage: 50);
        var axe = CreateItem(ItemType.Axe, damage: 7, maxDamage: 50);

        character.Equip(sword);
        character.SwitchEquipped(axe);

        Assert.Equal(17, character.Damage);
        Assert.False(character.Equipped.ContainsKey(ItemType.Sword), "Old weapon should be removed after a switch.");
        Assert.True(character.Equipped.ContainsKey(ItemType.Axe), "New weapon should be equipped after a switch.");
    }

    private static void AttackEnemySubtractsArmorBeforeDamagingTheEnemy()
    {
        var player = new Character("Test", 20, 20) { Damage = 14 };
        Enemy goblin = new Goblin(health: 30, name: "Goblin", damage: 4, armor: 3, x: 0, y: 0);

        var damage = GameStateHelper.AttackEnemy(goblin, player);

        Assert.Equal(11, damage);
        Assert.Equal(19, goblin.Health);
    }

    private static void EnemyAttackSubtractsArmorBeforeDamagingThePlayer()
    {
        var player = new Character("Test", 20, 20) { Armor = 2 };
        Enemy goblin = new Goblin(health: 30, name: "Goblin", damage: 7, armor: 0, x: 0, y: 0);

        var damage = GameStateHelper.EnemyAttack(goblin, player);

        Assert.Equal(5, damage);
        Assert.Equal(95, player.Health);
    }

    private static void HesitateDoublesThePostArmorDamage()
    {
        var player = new Character("Test", 20, 20) { Armor = 2 };
        Enemy goblin = new Goblin(health: 30, name: "Goblin", damage: 7, armor: 0, x: 0, y: 0);

        var damage = GameStateHelper.Hesitate(goblin, player);

        Assert.Equal(10, damage);
        Assert.Equal(90, player.Health);
    }

    private static void UpdateDifficultyIncreasesTierAndSpawnsNewWaveWhenThresholdsAreMet()
    {
        var field = CreateWalkableField(80, 80);
        var player = new Character("Test", field.Height, field.Width);
        var game = new GameState(field, player) { Day = Difficulty.Easy.DayThreshold() };

        GameStateHelper.UpdateDifficulty(game);

        Assert.Equal(Difficulty.Easy, game.Difficulty);
        Assert.True(game.Enemies.Count > 0, "A new enemy wave should be spawned.");
    }

    private static void IsSpawnableTileRespectsBlockedTiles()
    {
        var field = CreateWalkableField(10, 10);
        field.Overrides[(2, 3)] = TileType.Water;
        var game = new GameState(field, new Character("Test", field.Height, field.Width));

        Assert.True(GameStateHelper.IsSpawnableTile(1, 1, game),"GameStateHelper.IsSpawnableTile(1, 1, game)");
        Assert.False(GameStateHelper.IsSpawnableTile(3, 2, game),"GameStateHelper.IsSpawnableTile(3, 2, game)");
    }

    private static void GenerateItemNameFollowsTheRarityNamingRules()
    {
        var commonName = GameStateHelper.GenerateItemName(Rarity.Common, ItemType.Sword);
        var rareName = GameStateHelper.GenerateItemName(Rarity.Rare, ItemType.Helmet);
        var divineName = GameStateHelper.GenerateItemName(Rarity.Divine, ItemType.Boots);

        Assert.Contains("Sword", commonName);
        Assert.DoesNotContain(" of ", commonName);
        Assert.Contains("Helmet", rareName);
        Assert.Contains(" of ", rareName);
        Assert.StartsWith("Divine ", divineName);
        Assert.Contains("Boots", divineName);
        Assert.Contains(" of ", divineName);
    }

    private static void GetEnemySpawnTileStaysInsideBoundsAndAwayFromThePlayerOnALargeMap()
    {
        for (int i = 0; i < 200; i++)
        {
            var (x, y) = SpawnHelper.GetEnemySpawnTile(100, 100, 50, 50);

            Assert.InRange(x, 0, 99);
            Assert.InRange(y, 0, 99);
            Assert.InRange(Math.Abs(x - 50), 5, 25);
            Assert.InRange(Math.Abs(y - 50), 5, 25);
        }
    }

    private static Item CreateItem(
        ItemType type,
        int health = 0,
        int maxHealth = 100,
        int damage = 0,
        int maxDamage = 10,
        int armor = 0,
        int maxArmor = 0,
        Rarity rarity = Rarity.Common)
    {
        return new Item("Test Item", "Test", rarity, type)
        {
            Health = health,
            MaxHealth = maxHealth,
            Damage = damage,
            MaxDamage = maxDamage,
            Armor = armor,
            MaxArmor = maxArmor
        };
    }

    private static PlayingField CreateWalkableField(int width, int height)
    {
        var field = new PlayingField(width, height, width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field.Overrides[(y, x)] = TileType.Grass;
            }
        }

        return field;
    }
}

static class Assert
{
    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException($"Expected {expected} but got {actual}.");
        }
    }

    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void False(bool condition, string message)
    {
        if (condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void Contains(string expectedSubstring, string actualValue)
    {
        if (!actualValue.Contains(expectedSubstring, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{actualValue}' to contain '{expectedSubstring}'.");
        }
    }

    public static void DoesNotContain(string unexpectedSubstring, string actualValue)
    {
        if (actualValue.Contains(unexpectedSubstring, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{actualValue}' not to contain '{unexpectedSubstring}'.");
        }
    }

    public static void StartsWith(string expectedPrefix, string actualValue)
    {
        if (!actualValue.StartsWith(expectedPrefix, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected '{actualValue}' to start with '{expectedPrefix}'.");
        }
    }

    public static void InRange(int actualValue, int minimum, int maximum)
    {
        if (actualValue < minimum || actualValue > maximum)
        {
            throw new InvalidOperationException($"Expected {actualValue} to be in range [{minimum}, {maximum}].");
        }
    }
}
