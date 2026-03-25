using MathGame_dotnet_upgrade;
using MathGame_dotnet_upgrade.Models;

namespace MathGame
{
    internal class Menu
    {
        GameEngine gameMenu = new();
        Game game = new();

        public void GameMenu(string player)
        {
            Console.WriteLine("choose an operation to play in: ");
            Console.WriteLine("a - addition");
            Console.WriteLine("s - subtraction");
            Console.WriteLine("m - multiplication");
            Console.WriteLine("d - division");
            Console.WriteLine("r - random");
            Console.WriteLine("v - view game history");
            Console.WriteLine("q - quit");
            Game.GameChoice = Console.ReadLine();
            var whichOperator = "";

            switch (Game.GameChoice.Trim().ToLower())
            {
                case "a":
                    whichOperator = "+";
                    var gameDifficulty = game.Difficulty = Helpers.ChooseDifficulty();
                    gameMenu.PlayGame(GameType.Addition, whichOperator, (a, b) => a + b, gameDifficulty);
                    break;

                case "s":
                    whichOperator = "-";
                    gameDifficulty = game.Difficulty = Helpers.ChooseDifficulty();
                    gameMenu.PlayGame(GameType.Subtraction, whichOperator, (a, b) => a - b, gameDifficulty);
                    break;

                case "m":
                    whichOperator = "x";
                    gameDifficulty = game.Difficulty = Helpers.ChooseDifficulty();
                    gameMenu.PlayGame(GameType.Multiplication, whichOperator, (a, b) => a * b, gameDifficulty);
                    break;

                case "d":
                    whichOperator = "/";
                    gameDifficulty = game.Difficulty = Helpers.ChooseDifficulty();
                    gameMenu.PlayGame(GameType.Division, whichOperator, (a, b) => a / b, gameDifficulty);
                    break;

                case "r":
                    gameDifficulty = game.Difficulty = Helpers.ChooseDifficulty();
                    gameMenu.PlayGame(GameType.Random, "", null, gameDifficulty);
                    break;

                case "v":
                    Helpers.GameHistory();
                    break;

                case "q":
                    Console.WriteLine("thanks for playing!");
                    Console.Read();
                    Environment.Exit(0);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("select one of the options");
                    GameMenu(player);
                    break;
            }
        }
    }
}
