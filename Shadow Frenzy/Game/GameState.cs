using Shadow_Frenzy.Characters;
using Shadow_Frenzy.Enemies;
using Shadow_Frenzy.World;

namespace Shadow_Frenzy.Game;

public class GameState
{
    public PlayingField PlayingField { get; set; }
    public Character Player { get; set; }
    public Dictionary<(int h, int w), Enemy> Enemies { get; set; } = new();

    public void AddEnemy(Enemy enemy)
    {
        Enemies[(enemy.hpos, enemy.wpos)] = enemy;
    }

    public void MoveEnemy(Enemy enemy, int newH, int newW)
    {
        Enemies.Remove((enemy.hpos, enemy.wpos));
        enemy.hpos = newH;
        enemy.wpos = newW;
        Enemies[(newH, newW)] = enemy;
    }

    public GameState(World.PlayingField playingField, Character player)
    {
        PlayingField = playingField;
        Player = player;
    }
}