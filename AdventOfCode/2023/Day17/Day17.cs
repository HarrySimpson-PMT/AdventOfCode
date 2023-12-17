
using System.ComponentModel;
using System.Diagnostics;

namespace AdventOfCode.Year2023
{
    public class Day17 : Day
    {
        public Day17(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MagmaMadness magmaMadness = new MagmaMadness(data);
            result = magmaMadness.CalculatePath().ToString();
            //magmaMadness.Print();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MagmaMadness magmaMadness = new MagmaMadness(data);
            result = magmaMadness.CalculatePath2().ToString();
            Console.WriteLine(result);
        }
        public class MagmaMadness
        {
            CityBlock[,] cityBlocks;
            int xlen;
            int ylen;
            public MagmaMadness(string[] data)
            {
                xlen = data.Length;
                ylen = data[0].Length;
                cityBlocks = new CityBlock[xlen, ylen];
                for (int i = 0; i < xlen; i++)
                {
                    for (int j = 0; j < ylen; j++)
                    {
                        cityBlocks[i, j] = new CityBlock(int.Parse(data[i][j].ToString()));
                    }
                }
            }
            public int CalculatePath2()
            {
                Queue<((int x, int y) position, (int x, int y) Direction, int heat)> queue = new Queue<((int x, int y) position, (int x, int y) Direction, int heat)>();
                int cumulatedHeat = 0;
                for (int i = 1; i < 11; i++)
                {
                    cumulatedHeat += cityBlocks[0, i].heatLoss;
                    if(i>4)
                        queue.Enqueue(((0, i), Directions.East, cumulatedHeat));
                }
                cumulatedHeat = 0;
                for (int i = 1; i < 11; i++)
                {
                    cumulatedHeat += cityBlocks[i, 0].heatLoss;
                    if(i > 4)
                        queue.Enqueue(((i, 0), Directions.South, cumulatedHeat));
                }
                while (queue.Count > 0)
                {
                    ((int x, int y) position, (int x, int y) Direction, int heat) = queue.Dequeue();
                    if (position.x == 0 && position.y == 8 && heat == 21)
                        Debugger.Break();
                    if (position.x < 0 || position.x >= xlen || position.y < 0 || position.y >= ylen)
                        continue;
                    List<((int x, int y) direction, int heat)> outs = cityBlocks[position.x, position.y].CalculateOut(Direction, heat);
                    foreach (((int x, int y) direction, int heat) gout in outs)
                    {
                        if (gout.direction == Directions.North)
                        {
                            cumulatedHeat = gout.heat;
                            for (int i = position.x - 1; i >= 0 && position.x - 1 - i < 10; i--)
                            {
                                cumulatedHeat += cityBlocks[i, position.y].heatLoss;
                                if(position.x - i >= 4)
                                    queue.Enqueue(((i, position.y), Directions.North, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.South)
                        {
                            cumulatedHeat = gout.heat;
                            for (int i = position.x + 1; i < xlen && i - position.x - 1 < 10; i++)
                            {
                                cumulatedHeat += cityBlocks[i, position.y].heatLoss;
                                if (i - position.x >= 4)
                                    queue.Enqueue(((i, position.y), Directions.South, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.East)
                        {
                            cumulatedHeat = gout.heat;
                            for (int i = position.y + 1; i < ylen && i - position.y - 1 < 10; i++)
                            {
                                cumulatedHeat += cityBlocks[position.x, i].heatLoss;
                                if (i - position.y >= 4)
                                    queue.Enqueue(((position.x, i), Directions.East, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.West)
                        {
                            cumulatedHeat = gout.heat;
                            for (int i = position.y - 1; i >= 0 && position.y - 1 - i < 10; i--)
                            {
                                cumulatedHeat += cityBlocks[position.x, i].heatLoss;
                                if (position.y - i >= 4)
                                    queue.Enqueue(((position.x, i), Directions.West, cumulatedHeat));
                            }
                        }
                    }
                }

                return cityBlocks[xlen - 1, ylen - 1].MinimumHeatOut();
            }
            public int CalculatePath()
            {
                Queue<((int x, int y) position, (int x, int y) Direction, int heat)> queue = new Queue<((int x, int y) position, (int x, int y) Direction, int heat)>();
                int cumulatedHeat = 0;
                for (int i = 1; i < 4; i++)
                {
                    cumulatedHeat += cityBlocks[0, i].heatLoss;
                    queue.Enqueue(((0, i), Directions.East, cumulatedHeat));
                }
                cumulatedHeat = 0;
                for (int i = 1; i < 4; i++)
                {
                    cumulatedHeat += cityBlocks[i, 0].heatLoss;
                    queue.Enqueue(((i, 0), Directions.South, cumulatedHeat));
                }
                while (queue.Count > 0)
                {
                    ((int x, int y) position, (int x, int y) Direction, int heat) = queue.Dequeue();
                    if(position.x == 2 && position.y == 8)
                        Debugger.Break();
                    if (position.x < 0 || position.x >= xlen || position.y < 0 || position.y >= ylen)
                        continue;
                    List<((int x, int y) direction, int heat)> outs = cityBlocks[position.x, position.y].CalculateOut(Direction, heat);
                    foreach (((int x, int y) direction, int heat) gout in outs)
                    {
                        if (gout.direction == Directions.North)
                        {
                            cumulatedHeat = gout.heat;
                            //next 3 positions
                            for (int i = position.x - 1; i >= 0 && position.x - 1 - i < 3; i--)
                            {
                                cumulatedHeat += cityBlocks[i, position.y].heatLoss;
                                queue.Enqueue(((i, position.y), Directions.North, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.South)
                        {
                            cumulatedHeat = gout.heat;
                            //next 3 positions
                            for (int i = position.x + 1; i < xlen && i - position.x - 1 < 3; i++)
                            {
                                cumulatedHeat += cityBlocks[i, position.y].heatLoss;
                                queue.Enqueue(((i, position.y), Directions.South, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.East)
                        {
                            cumulatedHeat = gout.heat;
                            //next 3 positions
                            for (int i = position.y + 1; i < ylen && i - position.y - 1 < 3; i++)
                            {
                                cumulatedHeat += cityBlocks[position.x, i].heatLoss;
                                queue.Enqueue(((position.x, i), Directions.East, cumulatedHeat));
                            }
                        }
                        if (gout.direction == Directions.West)
                        {
                            cumulatedHeat = gout.heat;
                            //next 3 positions
                            for (int i = position.y - 1; i >= 0 && position.y - 1 - i < 3; i--)
                            {
                                cumulatedHeat += cityBlocks[position.x, i].heatLoss;
                                queue.Enqueue(((position.x, i), Directions.West, cumulatedHeat));
                            }
                        }
                    }
                }

                return cityBlocks[xlen - 1, ylen - 1].MinimumHeatOut();
            }
            public class CityBlock
            {
                public int heatLoss { get; }
                public int NorthOutMinimumHeatLoss { get; internal set; } = int.MaxValue;
                public int SouthOutMinimumHeatLoss { get; internal set; } = int.MaxValue;
                public int EastOutMinimumHeatLoss { get; internal set; } = int.MaxValue;
                public int WestOutMinimumHeatLoss { get; internal set; } = int.MaxValue;
                public CityBlock(int heatLoss)
                {
                    this.heatLoss = heatLoss;
                }
                public List<((int x, int y) direction, int head)> CalculateOut((int x, int y) inDirection, int heat)
                {
                    List<((int x, int y) direction, int head)> result = new List<((int x, int y) direction, int head)>();
                    if (inDirection == Directions.North)
                    {
                        if (EastOutMinimumHeatLoss > heat)
                        {
                            EastOutMinimumHeatLoss = heat;
                            result.Add((Directions.East, heat));
                        }
                        if (WestOutMinimumHeatLoss > heat)
                        {
                            WestOutMinimumHeatLoss = heat;
                            result.Add((Directions.West, heat));
                        }
                    }
                    if (inDirection == Directions.South)
                    {
                        if (EastOutMinimumHeatLoss > heat)
                        {
                            EastOutMinimumHeatLoss = heat;
                            result.Add((Directions.East, heat));
                        }
                        if (WestOutMinimumHeatLoss > heat)
                        {
                            WestOutMinimumHeatLoss = heat;
                            result.Add((Directions.West, heat));
                        }
                    }
                    if (inDirection == Directions.East)
                    {
                        if (NorthOutMinimumHeatLoss > heat)
                        {
                            NorthOutMinimumHeatLoss = heat;
                            result.Add((Directions.North, heat));
                        }
                        if (SouthOutMinimumHeatLoss > heat)
                        {
                            SouthOutMinimumHeatLoss = heat;
                            result.Add((Directions.South, heat));
                        }
                    }
                    if (inDirection == Directions.West)
                    {
                        if (NorthOutMinimumHeatLoss > heat)
                        {
                            NorthOutMinimumHeatLoss = heat;
                            result.Add((Directions.North, heat));
                        }
                        if (SouthOutMinimumHeatLoss > heat)
                        {
                            SouthOutMinimumHeatLoss = heat;
                            result.Add((Directions.South, heat));
                        }
                    }
                    return result;
                }
                public int MinimumHeatOut()
                {
                    return Math.Min(Math.Min(NorthOutMinimumHeatLoss, SouthOutMinimumHeatLoss), Math.Min(EastOutMinimumHeatLoss, WestOutMinimumHeatLoss));
                }
            }
            public void Print()
            {
                for (int i = 0; i < xlen; i++)
                {
                    for (int j = 0; j < ylen; j++)
                    {
                        Console.Write(cityBlocks[i, j].heatLoss);
                    }
                    Console.WriteLine();
                }
            }
            public static class Directions
            {
                public static (int x, int y) North = (-1, 0);
                public static (int x, int y) South = (1, 0);
                public static (int x, int y) East = (0, 1);
                public static (int x, int y) West = (0, -1);
                public static (int x, int y) Invert((int x, int y) direction)
                {
                    return (direction.x * -1, direction.y * -1);
                }
            }
        }
    }

}