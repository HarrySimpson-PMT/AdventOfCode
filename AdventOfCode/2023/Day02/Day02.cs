
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Year2023 {
    public class Day02 : Day {
        public Day02(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            CrappyElfGame game = new CrappyElfGame();
            result = game.PossibleGameCount(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            CrappyElfGame game = new CrappyElfGame();
            game.PossibleGameCount(data);
            result = game.GamePower.ToString();
            Console.WriteLine(result);
        }
        public class CrappyElfGame {
            public int GamePower { get; set; } = 0;
            public int PossibleGameCount(string[] data) {
                int red = 12, gree = 13, blue = 14;
                int result = 0;
                //each line is formatted like "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green" 
                for (int i = 0; i < data.Length; i++) {
                    string game = data[i];
                    int maxred = 0, maxgree = 0, maxblue = 0;
                    bool gamePossible = true;
                    string gamedata = game.Split(":")[1];
                    string[] rounds = gamedata.Split(";");
                    foreach (string round in rounds) {
                        string[] colors = round.Split(",");
                        foreach (string color in colors) {
                            string[] colorData = color.Trim().Split(" ");
                            int count = int.Parse(colorData[0]);
                            switch (colorData[1]) {
                                case "red":
                                    maxred = Math.Max(maxred, count);
                                    if (count > red) {
                                        gamePossible = false;
                                        Console.WriteLine($"{count} > {red}");
                                    }
                                    break;
                                case "green":
                                    maxgree = Math.Max(maxgree, count);
                                    if (count > gree) {
                                        gamePossible = false;
                                        Console.WriteLine($"{count} > {gree}");
                                    }
                                    break;
                                case "blue":
                                    maxblue = Math.Max(maxblue, count);
                                    if (count > blue) {
                                        gamePossible = false;
                                        Console.WriteLine($"{count} > {blue}");
                                    }
                                    break;
                            }

                        }
                    }
                    result += gamePossible ? i + 1 : 0;
                    GamePower += maxred * maxgree * maxblue;
                }
                return result;
            }
        }
    }
}