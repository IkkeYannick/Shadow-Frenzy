namespace Shadow_Frenzy.Game;

public enum Difficulty
{
    SuperEasy,
    Easy,
    Normal,
    Hard,
    SuperHard,
    Extreme,
    Nightmare,
    Impossible
}

public static class DifficultyExtensions
{
    private static readonly Dictionary<Difficulty, int> Thresholds = new()
    {
        { Difficulty.SuperEasy, 1 },
        { Difficulty.Easy, 3 },
        { Difficulty.Normal, 6 },
        { Difficulty.Hard, 10 },
        { Difficulty.SuperHard, 15 },
        { Difficulty.Extreme, 21 },
        { Difficulty.Nightmare, 28 },
        { Difficulty.Impossible, 36 }
    };

    public static int DayThreshold(this Difficulty difficulty)
    {
        return Thresholds[difficulty];
    }
}