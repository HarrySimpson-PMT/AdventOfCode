
using System;
using System.Diagnostics;
using static AdventOfCode.Year2022.FallingRockSimulator;

namespace AdventOfCode.Year2023 {
    public class Day21 : Day {
        public Day21(int today, int year) : base(today, year) {
        }
        public int steps = 0;

        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            StepCounter stepCounter = new StepCounter(data);
            result = stepCounter.ReachablePlots(steps).ToString();
            //stepCounter.print();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            StepCounter stepCounter = new StepCounter(data);
            stepCounter.ExpandMap();
            stepCounter.ExpandMap();
            result = stepCounter.ReachablePlots(steps, true).ToString();
            //stepCounter.print();
            Console.WriteLine(result);
        }
        public enum Dir {
            N, S, E, W
        }
        public record Coord(int X, int Y) {
            public Coord Move(Dir dir, int dist = 1) {
                return dir switch {
                    Dir.N => new Coord(this.X - dist, this.Y),
                    Dir.S => new Coord(this.X + dist, this.Y),
                    Dir.E => new Coord(this.X, this.Y + dist),
                    Dir.W => new Coord(this.X, this.Y - dist),
                };
            }
        }        

        public class StepCounter {
            char[,] map;
            (int x, int y) start = (0, 0);
            int n;
            List<int> history = new List<int>();
            public StepCounter(string[] data) {
                n = data.Length;
                map = new char[data.Length, data[0].Length];
                for (int i = 0; i < data.Length; i++) {
                    for (int j = 0; j < data[0].Length; j++) {
                        map[i, j] = data[i][j];
                        if (map[i, j] == 'S') {
                            start = (i, j);
                            map[i, j] = '.';
                        }
                    }
                }
            }
            public void ExpandMap() {
                //we need to tile the map around the center map so height and width are 3 times the size
                char[,] newMap = new char[map.GetLength(0) * 3, map.GetLength(1) * 3];
                for (int i = 0; i < newMap.GetLength(0); i++) {
                    for (int j = 0; j < newMap.GetLength(1); j++) {
                        newMap[i, j] = map[i % map.GetLength(0), j % map.GetLength(1)];
                    }
                }
                start = (start.x + map.GetLength(0), start.y + map.GetLength(1));
                map = newMap;
                //update start
            }
            public long ReachablePlots(int steps, bool Mod = false) {
                Queue<(int x, int y)> process = new Queue<(int x, int y)>();
                process.Enqueue(start);
                HashSet<(int x, int y)> unique = new HashSet<(int x, int y)>();
                int level = 0;
                int scale = 0;
                while (process.Count > 0 && level++ < steps && scale < 3) {
                    int size = process.Count;
                    while (size-- > 0) {

                        (int x, int y) current = process.Dequeue();
                        if (map[current.x, current.y] != '.')
                            continue;
                        foreach (var direction in Directions.Direction) {
                            (int x, int y) next = (current.x + direction.x, current.y + direction.y);
                            if (next.x < 0 || next.x >= map.GetLength(0) || next.y < 0 || next.y >= map.GetLength(1))
                                continue;
                            process.Enqueue(next);
                        }
                    }
                    foreach (var item in process) {
                        if (map[item.x, item.y] == '.')
                            unique.Add(item);
                    }
                    process.Clear();
                    foreach (var item in unique) {
                        process.Enqueue(item);
                    }

                    if ((level*2+1) % 131 ==0) {
                        scale++;
                        history.Add(CountVisited(process));
                    }
                    if(level==steps) {
                        foreach (var item in process) {
                            map[item.x, item.y] = 'X';
                        }
                    }
                    unique.Clear();
                }
                if (!Mod) {

                    return process.Count();
                }
                //Solve for the quadratic coefficients

               int c = history[0];
                int aPlusB = history[1] - c;
                int fourAPlusTwoB = history[2] - c;
                int twoA = fourAPlusTwoB - (2 * aPlusB);
                int a = twoA / 2;
                int b = aPlusB - a;

                long F(long n) {
                    return a * (n * n) + b * n + c;
                }

                for (int i = 0; i < history.Count; i++) {
                    Console.WriteLine($"{history[i]} : {F(i)}");
                }
                long grids = steps / n;
                return F(grids);
            }
            public void print() {
                for (int i = 0; i < map.GetLength(0); i++) {
                    for (int j = 0; j < map.GetLength(1); j++) {
                        Console.Write(map[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            public int CountVisited(Queue<(int x, int y)> process) {
                int count = 0;
                HashSet<(int x, int y)> unique = new HashSet<(int x, int y)>();
                foreach (var item in process) {
                    if (map[item.x, item.y] == '.')
                        count++;
                }
                return count;
            }
            public static class Directions {
                public static (int x, int y) North = (-1, 0);
                public static (int x, int y) South = (1, 0);
                public static (int x, int y) East = (0, 1);
                public static (int x, int y) West = (0, -1);
                public static (int x, int y)[] Direction = new (int x, int y)[] { North, South, East, West };

                public static string DirectionName((int x, int y) direction) {
                    if (direction == North)
                        return "North";
                    if (direction == South)
                        return "South";
                    if (direction == East)
                        return "East";
                    if (direction == West)
                        return "West";
                    return "Unknown";
                }
            }
        }
    }

}