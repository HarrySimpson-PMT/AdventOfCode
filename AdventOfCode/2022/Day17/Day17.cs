using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public class Day17 : Day
    {
        //TODO - Finish my implementation

        public Day17(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;

            Solution2 solution = new Solution2();
            Console.WriteLine(solution.PartTwo(data[0]));

            int currentshape = 0;
            FallingRockSimulator FRS = new FallingRockSimulator(); 
            FallingRockSimulator.Shape shape = new FallingRockSimulator.Shape(currentshape++, 2);
            int ShapeHeight = 3;
            int moveselctor = 0;
            while(currentshape<2023)
            {
                //print FallingRockSimulator Tube
                FRS.Print();
                
                if (moveselctor >= data[0].Length)
                    moveselctor = 0;
                
                FRS.NextMove(data[0][moveselctor++], ref shape, ShapeHeight);
                
                if(!FRS.NextMove('D', ref shape, ShapeHeight))
                {
                    FRS.WriteShapeToTube(shape, ShapeHeight);
                    currentshape++;
                    if (currentshape == 2023)
                    {
                        result = FRS.currentHeight.ToString();
                        break;
                    }
                    ShapeHeight = FRS.currentHeight + 3;
                    shape = new FallingRockSimulator.Shape(currentshape % 5, ShapeHeight);
                }
                else
                {
                    ShapeHeight--;
                }
            }
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
            Console.WriteLine(result);
        }
    }
    public class FallingRockSimulator
    {
        public int currentHeight { get; set; } = 0;
        public int width { get; set; } = 7;
        public List<bool[]> Tube { get; set; } = new List<bool[]>();
        public Shape CurrentShape { get; set; }

        public void Print()
        {
            Console.Clear();
            for (int i = Tube.Count-1; i >= 0 ; i--)
            {
                for (int j = 0; j < Tube[i].Length; j++)
                {
                    Console.Write(Tube[i][j] ? "#" : ".");
                }
                Console.WriteLine();
            }
            System.Threading.Thread.Sleep(100);
        }
        public void WriteShapeToTube(Shape shape, int shapeHeight)
        {
            for (int level = shapeHeight; level < currentHeight; level++)
            {
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == currentHeight - level).ToList();
                foreach (Coord item in onlevel)
                {
                    Tube[level][item.x] = true;
                }
            }
            for (int level = currentHeight - shapeHeight; level <= shape.height; level++)
            {
                Tube.Add(new bool[width]);
                currentHeight++;
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == level).ToList();
                foreach (Coord item in onlevel)
                {
                    Tube[level][item.x] = true;
                }
            }
        }
        public bool NextMove(char direction, ref Shape current, int currentlevel)
        {
            Shape nextshape;
            switch (direction)
            {
                case '<':
                    if (current.left == 0)
                        return false;
                    nextshape = new Shape(current.ShapeTypeID, current.left - 1);
                    if (nextshape.left < 0 || nextshape.right >= width)
                        return false;
                    if (IsBlocked(nextshape, currentlevel))
                        return false;
                    current = nextshape;
                    return true;
                case '>':
                    if (current.right == width - 2)
                        return false;
                    nextshape = new Shape(current.ShapeTypeID, current.left + 1);
                    if (nextshape.left < 0 || nextshape.right >= width)
                        return false;
                    if (IsBlocked(nextshape, currentlevel))
                        return false;
                    current = nextshape;
                    return true;
                case 'D':
                    if (IsBlocked(current, currentlevel - 1))
                        return false;
                    return true;
                default:
                    throw new Exception("Invalid direction");
            }
        }
        public bool IsBlocked(Shape shape, int shapeHeight)
        {
            if (shapeHeight==-1)
                return true;
            for (int level = shapeHeight; level < currentHeight; level++)
            {
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == currentHeight - level).ToList();
                foreach (Coord item in onlevel)
                {
                    if (Tube[level][item.x])
                        return true;
                }
            }
            return false;
        }
        public class Shape
        {
            public List<Coord> ShapePoints { get; set; } = new List<Coord>();
            public int ShapeTypeID { get; private set; }
            public int height { get; set; }
            public int left { get; set; }
            public int right { get; set; }
            public Shape(int type, int left)
            {
                ShapeTypeID = type;
                switch (type)
                {
                    //5 types of shapes
                    //first shape is a flat rock 4 wide and two from the left
                    case 0:
                        ShapePoints.Add(new Coord(0 + left, 0));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 0));
                        ShapePoints.Add(new Coord(3 + left, 0));
                        right = 4 + left;
                        height = 0;
                        break;
                    //second shape is a cross 3 wide 3 high and two from the left
                    case 1:
                        ShapePoints.Add(new Coord(0 + left, 1));
                        ShapePoints.Add(new Coord(1 + left, 1));
                        ShapePoints.Add(new Coord(2 + left, 1));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(1 + left, 3));
                        right = 3 + left;
                        height = 2;
                        break;
                    //third shape is an L 3 wide 3 high and two from the left
                    case 2:
                        ShapePoints.Add(new Coord(0 + left, 0));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 1));
                        ShapePoints.Add(new Coord(2 + left, 2));
                        right = 3 + left;
                        height = 2;
                        break;
                    //fourth shape is a line 4 high and two from the left
                    case 3:
                        ShapePoints.Add(new Coord(left, 0));
                        ShapePoints.Add(new Coord(left, 1));
                        ShapePoints.Add(new Coord(left, 2));
                        ShapePoints.Add(new Coord(left, 3));
                        right = left;
                        height = 3;
                        break;
                    //fourth shape is a square 2x2 and two from the left
                    case 4:
                        ShapePoints.Add(new Coord(0 + left, 0));
                        ShapePoints.Add(new Coord(0 + left, 1));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 1));
                        right = 2 + left;
                        height = 1;
                        break;
                    default:
                        throw new Exception("Invalid shape type");
                }
            }
        }
        public record Coord(int x, int y);
    }
    class Solution2
    {

        public object PartOne(string input)
        {
            return new Tunnel(input).AddRocks(2022).height;
        }

        public object PartTwo(string input)
        {
            return new Tunnel(input).AddRocks(1000000000000).height;
        }
        
        class Tunnel
        {
            // preserve just the top of the whole cave this is a practical 
            // constant, there is NO THEORY BEHIND it.
            const int linesToStore = 100;
            List<string> lines;
            long linesNotStored;

            public long height => lines.Count + linesNotStored - 1;

            IEnumerator<string[]> rocks;
            IEnumerator<char> jets;

            public Tunnel(string jets)
            {
                var rocks = new[]{
                "####".Split("\n"),
                " # \n###\n # ".Split("\n"),
                "  #\n  #\n###".Split("\n"),
                "#\n#\n#\n#".Split("\n"),
                "##\n##".Split("\n")
            };

                this.rocks = Loop(rocks).GetEnumerator();
                this.jets = Loop(jets.Trim()).GetEnumerator();
                this.lines = new List<string>() { "+-------+" };
            }

            public Tunnel AddRocks(long rocks)
            {
                // We are adding rocks one by one until we find a recurring pattern.

                // Then we can jump forward full periods with just increasing the height 
                // of the cave: the top of the cave should look the same after a full period
                // so no need to simulate he rocks anymore. 

                // Then we just add the remaining rocks. 

                var seen = new Dictionary<string, (long rocks, long height)>();
                while (rocks > 0)
                {
                    var hash = string.Join("\n", lines);
                    if (seen.TryGetValue(hash, out var cache))
                    {
                        // we have seen this pattern.
                        // compute length of the period, and how much does it
                        // add to the height of the cave:
                        var heightOfPeriod = this.height - cache.height;
                        var periodLength = cache.rocks - rocks;

                        // advance forwad as much as possible
                        linesNotStored += (rocks / periodLength) * heightOfPeriod;
                        rocks = rocks % periodLength;
                        break;
                    }
                    else
                    {
                        seen[hash] = (rocks, this.height);
                        this.AddRock();
                        rocks--;
                    }
                }

                while (rocks > 0)
                {
                    this.AddRock();
                    rocks--;
                }
                return this;
            }

            public Tunnel AddRock()
            {
                // Adds one rock to the cave
                rocks.MoveNext();
                var rock = rocks.Current;

                // make room: 3 lines + the height of the rock
                for (var i = 0; i < rock.Length + 3; i++)
                {
                    lines.Insert(0, "|       |");
                }

                // simulate falling
                var (rockX, rockY) = (3, 0);
                while (true)
                {
                    jets.MoveNext();
                    if (jets.Current == '>' && !Hit(rock, rockX + 1, rockY))
                    {
                        rockX++;
                    }
                    else if (jets.Current == '<' && !Hit(rock, rockX - 1, rockY))
                    {
                        rockX--;
                    }
                    if (Hit(rock, rockX, rockY + 1))
                    {
                        break;
                    }
                    rockY++;
                }

                Draw(rock, rockX, rockY);
                return this;
            }

            bool Hit(string[] rock, int x, int y)
            {
                // tells if a rock hits the walls of the cave or some other rock

                var (crow, ccol) = (rock.Length, rock[0].Length);
                for (var irow = 0; irow < crow; irow++)
                {
                    for (var icol = 0; icol < ccol; icol++)
                    {
                        if (rock[irow][icol] == '#' && lines[irow + y][icol + x] != ' ')
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            void Draw(string[] rock, int rockX, int rockY)
            {
                // draws a rock pattern into the cave at the given x,y coordinates,

                var (crow, ccol) = (rock.Length, rock[0].Length);
                for (var irow = 0; irow < crow; irow++)
                {
                    var chars = lines[irow + rockY].ToArray();
                    for (var icol = 0; icol < ccol; icol++)
                    {

                        if (rock[irow][icol] == '#')
                        {
                            if (chars[icol + rockX] != ' ')
                            {
                                throw new Exception();
                            }
                            chars[icol + rockX] = '#';
                        }
                    }
                    lines[rockY + irow] = string.Join("", chars);
                }

                // remove empty lines from the top
                while (!lines[0].Contains('#'))
                {
                    lines.RemoveAt(0);
                }

                // keep the tail
                if (lines.Count > linesToStore)
                {
                    var r = lines.Count - linesToStore - 1;
                    lines.RemoveRange(linesToStore, r);
                    linesNotStored += r;
                }
            }

            IEnumerable<T> Loop<T>(IEnumerable<T> items)
            {
                while (true)
                {
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }
            }

        }
    }
}