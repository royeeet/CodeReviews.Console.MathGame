using System;
using MathGame.Models;

namespace MathGame
{
    internal class GameEngine
    {
        public void PlayGame(GameType gameType, string symbol, Func<int, int, int> operation, GameDifficulty difficulty)
        {
            int score = 0;
            Helpers helpers = new Helpers();
            var loop = Helpers.InitialiseGame();

            do
            {
                GameType currentGameType = gameType;
                string currentSymbol = symbol;
                Func<int, int, int> currentOperation = operation;

                if (gameType == GameType.Random)
                {
                    currentGameType = Helpers.Randomise();
                    currentSymbol = currentGameType switch
                    {
                        GameType.Addition => "+",
                        GameType.Subtraction => "-",
                        GameType.Multiplication => "x",
                        GameType.Division => "/",
                    };
                    currentOperation = currentSymbol switch
                    {
                        "+" => (a, b) => a + b,
                        "-" => (a, b) => a - b,
                        "x" => (a, b) => a * b,
                        "/" => (a, b) => a / b,
                        _ => (a, b) => a + b
                    };
                }
                var (firstValue, secondValue, userAnswer) = currentGameType == GameType.Division ? Helpers.CheckThatNumbersAreDivisable(difficulty) : Helpers.GetNumbersAndQuestion(currentSymbol, difficulty);

                int expected = currentOperation(firstValue, secondValue);

                helpers.GameLogic(userAnswer, expected, ref score, gameType, difficulty);

            } while (!loop);
        }
    }
}
