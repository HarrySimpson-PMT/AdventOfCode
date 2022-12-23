namespace AdventOfCode.Year2022
{
    public class Day22 : Day
    {

        public Day22(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyMap MM = new(data);
            result = MM.RunDirections().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyMap MM = new(data, false);
            result = MM.RunDirections().ToString();
            Console.WriteLine(result);

        }
        public class MonkeyMap
        {
            public char[,] map { get; set; }
            public string Instructions { get; set; }
            public delegate (int x, int y) WrapAround(char[,] map, (int x, int y) newPosition, ref int facing);
            WrapAround wrapAround = null!;
            public MonkeyMap(string[] data, bool flatearth = true)
            {
                if (flatearth)
                    wrapAround = FlatWrap;
                else
                    wrapAround = GlobeWrap;

                int xlen = data.Length - 2;
                int ylen = 0;
                for (int i = 0; i < xlen; i++)
                {
                    if (data[i].Length > ylen)
                        ylen = data[i].Length;
                }
                map = new char[xlen, ylen];
                for (int x = 0; x < xlen; x++)
                {
                    for (int y = 0; y < ylen; y++)
                    {
                        map[x, y] = ' ';
                        if (y < data[x].Length)
                            map[x, y] = data[x][y];
                    }
                }
                Instructions = data[data.Length - 1];
            }
            public int RunDirections(bool FlatEarth = true)
            {
                int facing = 0;
                (int x, int y) position = (0, 0);
                while (map[position.x, position.y] != '.')
                    position.y++;
                map[position.x, position.y] = 'P';
                string steps = "";
                int stepsToMove;
                for (int i = 0; i < Instructions.Length; i++)
                {
                    if (Instructions[i] != 'R' && Instructions[i] != 'L')
                        steps = steps + Instructions[i];
                    else
                    {
                        Print();
                        map[position.x, position.y] = '.';
                        stepsToMove = int.Parse(steps);
                        steps = "";
                        position = Move(stepsToMove, position, facing);
                        facing = Turn(Instructions[i], facing);
                        map[position.x, position.y] = 'P';
                        Print();
                    }
                }
                map[position.x, position.y] = '.';
                stepsToMove = int.Parse(steps);
                position = Move(stepsToMove, position, facing);
                return ((position.x + 1) * 1000) + ((position.y + 1) * 4) + facing;
            }
            public void Print()
            {
                Console.Clear();
                Console.Clear();
                Console.Clear();

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        Console.Write(map[x, y]);
                    }
                    Console.WriteLine();
                    //add time dela of 1 ms

                }
            }
            public (int x, int y) Move(int steps, (int x, int y) current, int facing)
            {
                (int x, int y) newPosition = current;
                for (int i = 0; i < steps; i++)
                {
                    var previous = newPosition;
                    switch (facing)
                    {
                        case 0:
                            newPosition.y++;
                            break;
                        case 1:
                            newPosition.x++;
                            break;
                        case 2:
                            newPosition.y--;
                            break;
                        case 3:
                            newPosition.x--;
                            break;
                    }
                    if (newPosition.x < 0 || newPosition.x >= map.GetLength(0) || newPosition.y < 0 || newPosition.y >= map.GetLength(1) || map[newPosition.x, newPosition.y] == ' ')
                    {
                        newPosition = wrapAround(map, newPosition, ref facing);
                    }
                    if (map[newPosition.x, newPosition.y] == '#')
                    {
                        return previous;
                    }
                }
                return newPosition;
            }

            public static (int x, int y) FlatWrap(char[,] map, (int x, int y) newPosition, ref int facing)
            {
                switch (facing)
                {
                    case 0:
                        newPosition.y = 0;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.y++;
                        break;
                    case 1:
                        newPosition.x = 0;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.x++;
                        break;
                    case 2:
                        newPosition.y = map.GetLength(1) - 1;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.y--;
                        break;
                    case 3:
                        newPosition.x = map.GetLength(0) - 1;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.x--;
                        break;
                }
                return newPosition;
            }
            public static (int x, int y) GlobeWrap(char[,] map, (int x, int y) newPosition, ref int facing)
            {
                int sectionsize = map.GetLength(0) / 3;
                int HeightSection = newPosition.x / sectionsize;
                int WidthSection = newPosition.y / sectionsize;

                switch (facing)
                {
                    case 0:
                        switch (HeightSection)
                        {
                            case 0:
                                newPosition.y = map.GetLength(1) - 1 - newPosition.y;
                                newPosition.x = map.GetLength(0) - 1;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 3) + (sectionsize * 2 - (newPosition.x + 1));
                                newPosition.x = sectionsize * 2;
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3 - 1);
                                newPosition.x = map.GetLength(1) - (newPosition.x + 1);
                                facing = Turn('R', Turn('R', facing));

                                break;
                        }


                        break;
                    case 1:
                        switch (WidthSection)
                        {
                            case 0:
                                newPosition.y = (sectionsize * 3) - (newPosition.y + 1);
                                newPosition.x = (sectionsize * 3);
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 3);
                                newPosition.x = (sectionsize * 3 - (newPosition.y + 1)) + (sectionsize * 2);
                                facing = Turn('L', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3) - (newPosition.y + 1);
                                newPosition.x = (sectionsize * 2);
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 3:
                                newPosition.y = 0;
                                newPosition.x = (sectionsize * 2) - (sectionsize * 4 - (newPosition.y + 1));
                                facing = Turn('L', facing);
                                break;
                        }
                        newPosition.x = 0;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.x++;
                        break;
                    case 2:
                        switch (HeightSection)
                        {
                            case 0:
                                newPosition.y = (sectionsize + newPosition.x);
                                newPosition.x = (sectionsize);
                                facing = Turn('L', facing);
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 4 - (sectionsize * 2 - (newPosition.x + 1)));
                                newPosition.x = (sectionsize * 3 - 1);
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 2 - (sectionsize * 3 - (newPosition.x + 1)));
                                newPosition.x = (sectionsize * 2 - 1);
                                facing = Turn('R', facing);
                                break;
                        }
                        newPosition.y = map.GetLength(1) - 1;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.y--;
                        break;
                    case 3:
                        switch (WidthSection)
                        {
                            case 0:
                                newPosition.y = (sectionsize * 3 - (newPosition.y + 1));
                                newPosition.x = 0;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 2);
                                newPosition.x = sectionsize = (sectionsize * 2 - (newPosition.y + 1));
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3 - (newPosition.y + 1));
                                newPosition.x = sectionsize;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 3:
                                newPosition.y = (sectionsize * 3 - 1);
                                newPosition.x = sectionsize + (sectionsize * 3 - (newPosition.y + 1));
                                facing = Turn('L', facing);
                                break;
                        }
                        newPosition.x = map.GetLength(0) - 1;
                        while (map[newPosition.x, newPosition.y] == ' ')
                            newPosition.x--;
                        break;
                }
                return newPosition;
            }
            public static int Turn(char dir, int curr)
            {
                if (dir == 'L')
                    curr = (curr + 3) % 4;
                else
                    curr = (curr + 1) % 4;
                return curr;
            }
        }
    }
}