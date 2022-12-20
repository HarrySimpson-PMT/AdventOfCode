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
        public Day17(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;

            //Solution2 solution = new Solution2();
            //Console.WriteLine(solution.PartTwo(data[0]));

            int currentshape = 0;
            FallingRockSimulator FRS = new FallingRockSimulator();
            FallingRockSimulator.Shape shape = new FallingRockSimulator.Shape(currentshape, 2);
            long ShapeLevel = 3;
            int moveselctor = 0;
            while (currentshape < 2023)
            {
                //print FallingRockSimulator Tube
                //FRS.Print();

                if (moveselctor >= data[0].Length)
                    moveselctor = 0;

                FRS.NextMove(data[0][moveselctor++], ref shape, ShapeLevel);

                if (!FRS.NextMove('D', ref shape, ShapeLevel))
                {
                    FRS.WriteShapeToTube(shape, ShapeLevel);
                    currentshape++;
                    if (currentshape == 2022)
                    {
                        result = FRS.currentHeight.ToString();
                        break;
                    }
                    ShapeLevel = FRS.currentHeight + 3;
                    shape = new FallingRockSimulator.Shape(currentshape % 5, 2);
                    //print shape
                    //shape.Print();
                }
                else
                {
                    ShapeLevel--;
                }
            }
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            long MaxRocks = 1000000000000;
            FallingRockSimulator FRS = new FallingRockSimulator();
            FallingRockSimulator.Shape shape = new FallingRockSimulator.Shape(0, 2);
            long ShapeLevel = 3;
            int moveselctor = 0;
            int shapeid = 1;
            var seen = new Dictionary<string, (long rocks, long height)>();
            while (MaxRocks > 0)
            {
                //print FallingRockSimulator Tube
                //FRS.Print();

                var lines = FRS.Tube.Take(100).Select(x => new string(string.Join(",", x))).ToArray();
                var hash = FRS.Tube.Count() > 0 ? FRS.TubeHash() : "";
                if (seen.TryGetValue(hash, out var cache))
                {
                    var heightOfPeriod = FRS.currentHeight - cache.height;
                    var periodLength = cache.rocks - MaxRocks;
                    
                    long reduction = MaxRocks / periodLength;
                    MaxRocks = MaxRocks % periodLength;
                    FRS.currentHeight += heightOfPeriod * reduction;
                    ShapeLevel = FRS.currentHeight + 3;
                    break;

                }
                else
                {

                    //print FallingRockSimulator Tube
                    //FRS.Print();

                    if (moveselctor >= data[0].Length)
                        moveselctor = 0;

                    FRS.NextMove(data[0][moveselctor++], ref shape, ShapeLevel);

                    if (!FRS.NextMove('D', ref shape, ShapeLevel))
                    {
                        if (FRS.currentHeight > 1)
                            seen.Add(hash, (MaxRocks, FRS.currentHeight));
                        FRS.WriteShapeToTube(shape, ShapeLevel);
                        MaxRocks--;
                        if (MaxRocks == 0)
                            break;
                        ShapeLevel = FRS.currentHeight + 3;
                        shape = new FallingRockSimulator.Shape((shapeid++ % 5), 2);
                    }
                    else
                    {
                        ShapeLevel--;
                    }
                }

            }
            while (MaxRocks > 0)
            {
                if (moveselctor >= data[0].Length)
                    moveselctor = 0;

                FRS.NextMove(data[0][moveselctor++], ref shape, ShapeLevel);

                if (!FRS.NextMove('D', ref shape, ShapeLevel))
                {
                    FRS.WriteShapeToTube(shape, ShapeLevel);
                    MaxRocks--;
                    if (MaxRocks == 0)
                        break;
                    ShapeLevel = FRS.currentHeight + 3;
                    shape = new FallingRockSimulator.Shape((shapeid++ % 5), 2);
                }
                else
                {
                    ShapeLevel--;
                }
            }
            result = FRS.currentHeight.ToString();


            Console.WriteLine(result);
        }
    }
    public class FallingRockSimulator
    {
        public long currentHeight { get; set; } = 0;
        public int workingtube { get; set; } = 100;
        public int width { get; set; } = 7;
        public List<bool[]> Tube { get; set; } = new List<bool[]>();
        //aggregate all bool[] in tube into string seperated with newlines
        //public string TubeHash => Tube.Aggregate((x, next) => x.Concat(next).ToArray()).Aggregate("", (x, next) => x + (next ? "#" : "."));
        public string TubeHash()
        {
            var sb = new StringBuilder();
            foreach (var line in Tube)
            {
                foreach (var c in line)
                {
                    sb.Append(c ? "#" : ".");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public Shape CurrentShape { get; set; }

        public void Print()
        {
            Console.Clear();
            for (int i = Tube.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Tube[i].Length; j++)
                {
                    Console.Write(Tube[i][j] ? "#" : ".");
                }
                Console.WriteLine();
            }
            System.Threading.Thread.Sleep(100);
        }
        public void WriteShapeToTube(Shape shape, long ShapeLevel)
        {
            int Overlap = (int)(currentHeight - ShapeLevel);
            for (int level = 0; level < Overlap; level++)
            {
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == level).ToList();
                foreach (Coord item in onlevel)
                {
                    Tube[Tube.Count() - Overlap + level][item.x] = true;
                }
            }
            for (int level = Overlap; level <= shape.height; level++)
            {
                if (Tube.Count() > 100)
                    Tube.RemoveAt(0);
                Tube.Add(new bool[width]);
                currentHeight++;
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == level).ToList();
                foreach (Coord item in onlevel)
                {
                    Tube[Tube.Count - 1][item.x] = true;
                }
            }
        }
        public bool NextMove(char direction, ref Shape current, long ShapeLevel)
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
                    if (IsBlocked(nextshape, ShapeLevel))
                        return false;
                    current = nextshape;
                    return true;
                case '>':
                    if (current.right == width)
                        return false;
                    nextshape = new Shape(current.ShapeTypeID, current.left + 1);
                    if (nextshape.left < 0 || nextshape.right >= width)
                        return false;
                    if (IsBlocked(nextshape, ShapeLevel))
                        return false;
                    current = nextshape;
                    return true;
                case 'D':
                    if (IsBlocked(current, ShapeLevel - 1))
                        return false;
                    return true;
                default:
                    throw new Exception("Invalid direction");
            }
        }
        public bool IsBlocked(Shape shape, long currentlevel)
        {
            if (currentlevel == -1)
                return true;
            int overlap = (int)(currentHeight - currentlevel);
            for (int level = 0; level < overlap; level++)
            {
                List<Coord> onlevel = shape.ShapePoints.Where(x => x.y == level).ToList();
                foreach (Coord item in onlevel)
                {
                    if (Tube[Tube.Count() - overlap + level][item.x])
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
            public Shape(int type, int Left)
            {
                left = Left;
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
                        right = 3 + left;
                        height = 0;
                        break;
                    //second shape is a cross 3 wide 3 high and two from the left
                    case 1:
                        ShapePoints.Add(new Coord(0 + left, 1));
                        ShapePoints.Add(new Coord(1 + left, 1));
                        ShapePoints.Add(new Coord(2 + left, 1));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(1 + left, 2));
                        right = 2 + left;
                        height = 2;
                        break;
                    //third shape is an L 3 wide 3 high and two from the left
                    case 2:
                        ShapePoints.Add(new Coord(0 + left, 0));
                        ShapePoints.Add(new Coord(1 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 0));
                        ShapePoints.Add(new Coord(2 + left, 1));
                        ShapePoints.Add(new Coord(2 + left, 2));
                        right = 2 + left;
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
                        ShapePoints.Add(new Coord(1 + left, 1));
                        right = 1 + left;
                        height = 1;
                        break;
                    default:
                        throw new Exception("Invalid shape type");
                }
            }
            public void Print()
            {
                for (int i = height; i >= 0; i--)
                {
                    for (int j = left; j <= right; j++)
                    {
                        Console.Write(ShapePoints.Any(x => x.x == j && x.y == i) ? "#" : ".");
                    }
                    Console.WriteLine();
                }
            }
        }
        public record Coord(int x, int y);
    }
}