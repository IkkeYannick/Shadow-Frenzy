# Shadow Frenzy

A console-based roguelike survival game built in C# where you battle goblins, loot items, and survive escalating difficulty waves.

---

## Gameplay

You control a character on a 20×20 grid. Each turn, you move across the field while goblins hunt you down. Defeat all enemies to advance to the next wave — but the difficulty ramps up every round.

### Controls

| Key | Action |
|-----|--------|
| `Z` / `↑` | Move up |
| `S` / `↓` | Move down |
| `Q` / `←` | Move left |
| `D` / `→` | Move right |
| `E` / `Tab` | Open inventory |
| `Escape` | Cancel / close menu |

### Combat

When adjacent to a goblin, combat begins automatically. You choose your action each turn:

- **1 — Attack:** Deal damage to the enemy. They strike back if still alive.
- **2 — Block:** 50% chance to fully block; otherwise take half damage.
- **Any other key:** You hesitate and take double damage.

### Items & Inventory

Defeating enemies has a 20% chance to drop a randomly generated item. Items have randomized stats, rarities, and names.

**Weapon types:** Sword, Axe, Shovel, Pickaxe  
**Armor types:** Helmet, Chestplate, Gauntlets, Pants, Boots

**Rarities (drop rate):**

| Rarity | Chance |
|--------|--------|
| Common | 40% |
| Uncommon | 25% |
| Rare | 15% |
| Epic | 10% |
| Legendary | 5% |
| Mythic | 3% |
| Divine | 2% |

Navigate your inventory with arrow keys, press `→` to inspect an item, and `E` to equip it. If a slot is already filled, you'll be prompted to swap.

---

## Difficulty System

Difficulty increases as you survive more days and clear enemy waves. There are 8 tiers:

`SuperEasy → Easy → Normal → Hard → SuperHard → Extreme → Nightmare → Impossible`

Each tier raises enemy health, damage, and armor. A new wave spawns once all enemies are defeated.

---

## Project Structure

```
Shadow Frenzy/
├── Characters/
│   └── Character.cs        # Player stats, inventory, and equipment logic
├── Enemies/
│   ├── Enemy.cs            # Enemy interface
│   └── Goblin.cs           # Goblin implementation
├── Game/
│   ├── Difficulty.cs       # Difficulty enum and day thresholds
│   └── GameState.cs        # Central game state (field, player, enemies, day)
├── Items/
│   ├── Item.cs             # Item stats and generation
│   ├── ItemType.cs         # Weapon/armor type enum
│   └── Rarity.cs           # Rarity enum
├── World/
│   ├── GameStateHelper.cs  # Game logic (movement, combat, spawning, loot)
│   ├── PlayingField.cs     # Board dimensions
│   ├── SpawnHelper.cs      # Enemy spawn positioning
│   └── VisualHelper.cs     # All console rendering and UI
└── Program.cs              # Entry point and game loop
```

---

## Requirements

- [.NET 6+](https://dotnet.microsoft.com/download)

## Running the Game

```bash
dotnet run
```

Or build and run the executable:

```bash
dotnet build
dotnet run --project "Shadow Frenzy"
```

---

## Tips

- Enemies move toward you each turn — use the open field to create distance.
- Blocking is safe when your health is low; attacking ends fights faster.
- Check your inventory often (`E`) — equipped gear significantly changes your stats.
- Higher rarity items have better max stat rolls.


## TODOS:
- Healing (Health Bar)
- Bosses & Different monsters/enemies
- Bigger map (hardcoded 20x20 now) (possible infinite map where you can run through)
- Scaling item stats & rarities with Difficulty
- Biomes with stat changes etc. (to be interpreted)
- Mana bar & Spells (Scrolls)
- (frontent to make it visualy pleasing)
- Api calls to connect frontend (when ready) and backend (not needed if i can find a different way to display the "gameplay")
