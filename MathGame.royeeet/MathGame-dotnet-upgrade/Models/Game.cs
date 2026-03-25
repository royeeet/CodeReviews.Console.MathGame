namespace MathGame_dotnet_upgrade.Models;

public class Game
{
    public int Score { get; set; }
    public GameType Type { get; set; }
    public TimeSpan Time { get; set; }
    public static string GameChoice { get; set; }
    public GameDifficulty Difficulty { get; set; }
}

public enum GameType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Random
}

public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}