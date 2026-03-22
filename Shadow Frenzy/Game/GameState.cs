using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.World;

namespace Shadow_Frenzy.Game;

public class GameState
{
    public PlayingField PlayingField { get; set; }
    public Character Player { get; set; }
    public Dictionary<(int h, int w), Enemy> Enemies { get; set; } = new();
    public int Day { get; set; }
    public Difficulty Difficulty { get; set; } = Difficulty.SuperEasy;

    public void AddEnemy(Enemy enemy)
    {
        Enemies[(enemy.Y, enemy.X)] = enemy;
    }

    public void MoveEnemy(Enemy enemy, int newH, int newW)
    {
        Enemies.Remove((enemy.Y, enemy.X));
        enemy.Y = newH;
        enemy.X = newW;
        Enemies[(newH, newW)] = enemy;
    }

    public void IncreaseDay()
    {
        Day++;
    }

    public void IncreaseDifficulty()
    {
        int max = Enum.GetValues(typeof(Difficulty)).Length - 1;

        if ((int)Difficulty < max)
            Difficulty = (Difficulty)((int)Difficulty + 1);
    }

    public void DecreaseDifficulty()
    {
        if ((int)Difficulty > 0)
            Difficulty = (Difficulty)((int)Difficulty - 1);
    }

    public GameState(PlayingField playingField, Character player)
    {
        PlayingField = playingField;
        Player = player;
        Day = 0;
    }
}