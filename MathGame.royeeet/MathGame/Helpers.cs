using MathGame.Models;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Data.SqlTypes;
using System.Xml.Schema;

namespace MathGame
{
    internal class Helpers
    {
        public static string player = GetName();
        public static List<Game> games = new List<Game>();
        public static Stopwatch timer = new Stopwatch();

        public static void GameHistory()
        {
            Console.Clear();
            Console.WriteLine("game history");

            if (games == null || games.Count == 0)
            {
                Console.WriteLine("nothing to show");
            }
            else
            {
                foreach (var game in games)
                {
                    Console.WriteLine($"Player Name: {player} || Game mode: {game.Type} || Game difficulty: {game.Difficulty} || Score: {game.Score} || Time: {game.Time:mm\\:ss}");
                }
            }

            Console.WriteLine("press any key to exit");
            Console.ReadLine();
            Console.Clear();

            Menu menu = new Menu();
            menu.GameMenu(player);
        }

        internal static string GetName()
        {
            Console.WriteLine("enter your name: ");
            var player = Console.ReadLine();
            while (string.IsNullOrEmpty(player))
            {
                Console.WriteLine("i said state your name");
                player = Console.ReadLine();
            }
            return player;
        }

        internal static void AddToHistory(int score, GameType gameChoice, TimeSpan timeTaken, GameDifficulty gameDifficulty)
        {
            if (Game.GameChoice == "r")
            {
                gameChoice = GameType.Random;
            }

            var game = new Game
            {
                Type = gameChoice,
                Score = score,
                Time = timeTaken,
                Difficulty = gameDifficulty
            };
            games.Add(game);
        }

        internal static int Validation(string input)
        {
            int result;
            while (string.IsNullOrEmpty(input) || !Int32.TryParse(input, out result))
            {
                Console.WriteLine("answer needs to be an integer, try again");
                input = Console.ReadLine();
            }
            return result;
        }

        internal static string ValidationYesOrNo(string prompt)
        {
            Console.WriteLine(prompt);
            string yesOrNo = Console.ReadLine();
            while (yesOrNo != "y" && yesOrNo != "n")
            {
                Console.WriteLine("decide yes or no");
                yesOrNo = Console.ReadLine();
            }
            return yesOrNo;
        }

        public static (int, int, int) GetNumbersAndQuestion(string operation, GameDifficulty difficulty)
        {
            Random rand = new Random();
            int firstValue = 0;
            int secondValue = 0;

            switch (difficulty)
            {
                case GameDifficulty.Easy:
                    firstValue = rand.Next(0, 10);
                    secondValue = rand.Next(0, 10);
                    break;
                case GameDifficulty.Medium:
                    firstValue = rand.Next(10, 50);
                    secondValue = rand.Next(10, 50);
                    break;
                case GameDifficulty.Hard:
                    firstValue = rand.Next(10, 100);
                    secondValue = rand.Next(10, 100);
                    break;
            }

            Console.Clear();
            Console.WriteLine("solve the following: ");
            Console.WriteLine($"{firstValue} {operation} {secondValue}");

            Console.Write("your answer: ");
            timer.Start();
            string input = Console.ReadLine();

            int userAnswer = Validation(input);
            return (firstValue, secondValue, userAnswer);
        }

        internal static bool InitialiseGame()
        {
            Console.Clear();
            timer.Reset();
            bool loop = false;
            return loop;
        }

        internal static (int, int, int) CheckThatNumbersAreDivisable(GameDifficulty difficulty)
        {
            Random rand = new Random();
            int dividend = 0;
            int divisor = 0;

            do
            {
                switch (difficulty)
                {
                    case GameDifficulty.Easy:
                        dividend = rand.Next(1, 100);
                        divisor = rand.Next(1, 100);
                        break;
                    case GameDifficulty.Medium:
                        dividend = rand.Next(1, 100);
                        divisor = rand.Next(1, 200);
                        break;
                    case GameDifficulty.Hard:
                        dividend = rand.Next(1, 100);
                        divisor = rand.Next(1, 300);
                        break;
                }
            }
            while (dividend % divisor != 0);

            Console.WriteLine("solve the following: ");
            Console.WriteLine($"{dividend} / {divisor}");
            timer.Start();
            string input = Console.ReadLine();
            int userAnswer = Validation(input);

            return (dividend, divisor, userAnswer);
        }

        public static GameType Randomise()
        {
            Random rand = new Random();
            var randomGameMode = (GameType)rand.Next(4);
            return randomGameMode;
        }

        internal void GameLogic(int userAnswer, int expected, ref int score, GameType gameChoice, GameDifficulty gameDifficulty)
        {
            GameEngine engine = new GameEngine();
            Menu menu = new Menu();

            if (userAnswer == expected)
            {
                Console.Clear();
                Console.WriteLine("correct");
                score++;
            }
            
            else
            {
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                string elapsedTime = "time taken (mm:ss): " + timeTaken.ToString(@"mm\:ss");
                Console.WriteLine(elapsedTime);

                AddToHistory(score, gameChoice, timeTaken, gameDifficulty);
                Console.WriteLine($"incorrect. the correct answer was {expected}. your score is {score}.");
                string restartGame = ValidationYesOrNo("try again? y/n");

                if (restartGame == "y")
                {
                    Console.Clear();
                    timer.Reset();
                    switch (gameChoice)
                    {
                        case GameType.Addition:
                            gameDifficulty = ChooseDifficulty();
                            engine.PlayGame(GameType.Addition, "+", (a, b) => a + b, gameDifficulty);
                            break;
                        case GameType.Subtraction:
                            gameDifficulty = ChooseDifficulty();
                            engine.PlayGame(GameType.Subtraction, "-", (a, b) => a - b, gameDifficulty);
                            break;
                        case GameType.Multiplication:
                            gameDifficulty = ChooseDifficulty();
                            engine.PlayGame(GameType.Multiplication, "x", (a, b) => a * b, gameDifficulty);
                            break;
                        case GameType.Division:
                            gameDifficulty = ChooseDifficulty();
                            engine.PlayGame(GameType.Division, "/", (a, b) => a / b, gameDifficulty);
                            break;
                        case GameType.Random:
                            gameDifficulty = ChooseDifficulty();
                            engine.PlayGame(GameType.Random, "", null, gameDifficulty);
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    menu.GameMenu(player);
                }
            }
        }

        public static GameDifficulty ChooseDifficulty()
        {
            Console.Clear();
            Console.WriteLine("choose a difficulty: ");
            Console.WriteLine("1 - easy");
            Console.WriteLine("2 - medium");
            Console.WriteLine("3 - hard");
            string input = Console.ReadLine();
            int choice = Validation(input);
            return choice switch
            {
                1 => GameDifficulty.Easy,
                2 => GameDifficulty.Medium,
                3 => GameDifficulty.Hard,
                _ => throw new ArgumentOutOfRangeException("invalid choice")
            };
        }
    }
}
