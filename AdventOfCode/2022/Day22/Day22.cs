﻿namespace AdventOfCode.Year2022 {
    public class Day22 : Day {
        public Day22(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyMap MM = new(data);
            result = MM.RunDirections().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyMap MM = new(data, false, argumentType);
            result = MM.RunDirections().ToString();
            Console.WriteLine(result);

        }

        public class MonkeyMap {
            public char[,] Map { get; set; }
            public char[,] CorrectedMap { get; set; }
            public string Instructions { get; set; }
            public delegate (int x, int y) WrapAround(char[,] map, (int x, int y) newPosition, ref int facing);
            WrapAround wrapAround = null!;


            public MonkeyMap(string[] data, bool flatearth = true, ArgumentType argumentType = ArgumentType.Sample) {
                if (flatearth)
                    wrapAround = FlatWrap;
                else {
                    if (argumentType == ArgumentType.Sample)
                        wrapAround = GlobeWrap;
                    else
                        wrapAround = GlobeWrapV2;
                }


                int xlen = data.Length - 2;
                int ylen = 0;
                for (int i = 0; i < xlen; i++) {
                    if (data[i].Length > ylen)
                        ylen = data[i].Length;
                }
                Map = new char[xlen, ylen];
                for (int x = 0; x < xlen; x++) {
                    for (int y = 0; y < ylen; y++) {
                        Map[x, y] = ' ';
                        if (y < data[x].Length)
                            Map[x, y] = data[x][y];
                    }
                }
                Instructions = data[data.Length - 1];
            }
            public int RunDirections(bool FlatEarth = true) {
                int facing = 0;
                (int x, int y) position = (0, 0);
                while (Map[position.x, position.y] != '.')
                    position.y++;
                Map[position.x, position.y] = 'P';
                string steps = "";
                int stepsToMove;
                for (int i = 0; i < Instructions.Length; i++) {
                    if (Instructions[i] != 'R' && Instructions[i] != 'L')
                        steps = steps + Instructions[i];
                    else {
                        Map[position.x, position.y] = '.';
                        stepsToMove = int.Parse(steps);
                        steps = "";
                        position = Move(stepsToMove, position, ref facing);
                        facing = Turn(Instructions[i], facing);
                        Map[position.x, position.y] = 'P';
                    }
                }
                Map[position.x, position.y] = '.';
                stepsToMove = int.Parse(steps);
                position = Move(stepsToMove, position, ref facing);
                Map[position.x, position.y] = 'P';
                //Print();
                return ((position.x + 1) * 1000) + ((position.y + 1) * 4) + facing;
            }
            public void Print() {
                Console.Clear();
                Console.Clear();
                Console.Clear();

                for (int x = 0; x < Map.GetLength(0); x++) {
                    for (int y = 0; y < Map.GetLength(1); y++) {
                        Console.Write(Map[x, y]);
                    }
                    Console.WriteLine();
                    //add time dela of 1 ms

                }
            }
            public (int x, int y) Move(int steps, (int x, int y) current, ref int facing) {
                (int x, int y) newPosition = current;
                for (int i = 0; i < steps; i++) {
                    var previous = newPosition;
                    int previousfacing = facing;
                    switch (facing) {
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
                    if (newPosition.x < 0 || newPosition.x >= Map.GetLength(0) || newPosition.y < 0 || newPosition.y >= Map.GetLength(1) || Map[newPosition.x, newPosition.y] == ' ') {
                        newPosition = wrapAround(Map, newPosition, ref facing);
                    }
                    if (Map[newPosition.x, newPosition.y] == '#') {
                        facing = previousfacing;
                        return previous;
                    }
                }
                return newPosition;
            }


            public static (int x, int y) FlatWrap(char[,] map, (int x, int y) newPosition, ref int facing) {
                switch (facing) {
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
            public static (int x, int y) GlobeWrap(char[,] map, (int x, int y) newPosition, ref int facing) {
                int sectionsize = map.GetLength(0) / 3;
                int HeightSection = newPosition.x / sectionsize;
                int WidthSection = newPosition.y / sectionsize;

                switch (facing) {
                    case 0:
                        switch (HeightSection) {
                            case 0:
                                newPosition.y = map.GetLength(1) - 1;
                                newPosition.x = map.GetLength(0) - (newPosition.x + 1);
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 3) + (sectionsize * 2 - (newPosition.x + 1));
                                newPosition.x = sectionsize * 2;
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3 - 1);
                                newPosition.x = map.GetLength(0) - (newPosition.x + 1);
                                facing = Turn('R', Turn('R', facing));
                                break;
                        }

                        break;
                    case 1:
                        switch (WidthSection) {
                            case 0:
                                newPosition.y = (sectionsize * 3) - (newPosition.y + 1);
                                newPosition.x = (sectionsize * 3) - 1;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.x = (sectionsize * 2 - (newPosition.y + 1) + (sectionsize * 2));
                                newPosition.y = (sectionsize * 2);
                                facing = Turn('L', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3) - (newPosition.y + 1);
                                newPosition.x = (sectionsize * 2) - 1;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 3:
                                newPosition.x = sectionsize + (sectionsize * 4 - (newPosition.y + 1));
                                newPosition.y = 0;
                                facing = Turn('L', facing);
                                break;
                        }
                        break;
                    case 2:
                        switch (HeightSection) {
                            case 0:
                                newPosition.y = (sectionsize + newPosition.x);
                                newPosition.x = (sectionsize);
                                facing = Turn('L', facing);
                                break;
                            case 1:
                                newPosition.y = (sectionsize * 3 + (sectionsize * 2 - (newPosition.x + 1)));
                                newPosition.x = (sectionsize * 3 - 1);
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize + (sectionsize * 3 - (newPosition.x + 1)));
                                newPosition.x = (sectionsize * 2 - 1);
                                facing = Turn('R', facing);
                                break;
                        }

                        break;
                    case 3:
                        switch (WidthSection) {
                            case 0:
                                newPosition.y = (sectionsize * 3 - (newPosition.y + 1));
                                newPosition.x = 0;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1:
                                newPosition.x = (newPosition.y) - sectionsize;
                                newPosition.y = (sectionsize * 2);
                                facing = Turn('R', facing);
                                break;
                            case 2:
                                newPosition.y = (sectionsize * 3 - (newPosition.y + 1));
                                newPosition.x = sectionsize;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 3:
                                newPosition.x = sectionsize + (sectionsize * 4 - (newPosition.y + 1));
                                newPosition.y = (sectionsize * 3 - 1);
                                facing = Turn('L', facing);
                                break;
                        }

                        break;
                }
                return newPosition;
            }
            public static (int x, int y) GlobeWrapV2(char[,] map, (int x, int y) newPosition, ref int facing) {
                int sectionsize = map.GetLength(0) / 4;
                int HeightSection = newPosition.x / sectionsize;
                int WidthSection = newPosition.y / sectionsize;

                switch (facing) {
                    case 0: //going EAST 
                        switch (HeightSection) {
                            case 0: //y set by inner section size - sectionsize correction; x => set by inversion of x - sectionsize correction required
                                newPosition.x = sectionsize * 2 + (sectionsize - 1) - newPosition.x;
                                newPosition.y = sectionsize * 2 - 1;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1: //y => set by x - no correction required; x => set by inner section size - sectionsize correction required
                                newPosition.y = sectionsize + newPosition.x;
                                newPosition.x = sectionsize - 1;
                                facing = Turn('L', facing);
                                break;
                            case 2: //y => set by innser section size - sectionsize correction required; x => set by inversion of x - sectionsize correction required
                                newPosition.x = (sectionsize * 3 - 1) - newPosition.x;
                                newPosition.y = sectionsize * 3 - 1;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 3: //y => set by x - no correction required; x => set by inner section size - sectionsize correction required
                                newPosition.y = newPosition.x - (sectionsize * 2);
                                newPosition.x = sectionsize * 3 - 1;
                                facing = Turn('L', facing);
                                break;
                        }
                        break;
                    case 1: //down South
                        switch (WidthSection) {
                            case 0: //y => set by inversion of y - sectionsize correction required; x=> static - no correction required
                                newPosition.y = (sectionsize * 2) + newPosition.y;
                                newPosition.x = 0;
                                break;
                            case 1: // x=> set by y - no correction required; y=> inner sectionsize - sectionsize correction required
                                newPosition.x = sectionsize * 2 + newPosition.y;
                                newPosition.y = sectionsize - 1;
                                facing = Turn('R', facing);
                                break;
                            case 2: //x=> set by y - no correction required; y=> static inner - sectionsize correction required
                                newPosition.x = newPosition.y - sectionsize;
                                newPosition.y = sectionsize * 2 - 1;
                                facing = Turn('R', facing);
                                break;
                        }
                        break;
                    case 2: //left WEST
                        switch (HeightSection) {
                            case 0: //y => set to 0 - no correction required; x => set by inversion of x - sectionsize correction required
                                newPosition.x = (sectionsize * 2) + (sectionsize - 1) - (newPosition.x);
                                newPosition.y = 0;
                                facing = Turn('R', Turn('R', facing));
                                break;
                            case 1: //y=> set by x - no correction required; x => set by outer section size - no correction required
                                newPosition.y = newPosition.x - sectionsize;
                                newPosition.x = sectionsize * 2;
                                facing = Turn('L', facing);
                                break;
                            case 2: //y => set by outer section size - no correction required; x => set by inversion of x - sectionsize correction required
                                newPosition.x = (sectionsize * 3 - 1) - (newPosition.x);
                                newPosition.y = sectionsize;
                                facing = Turn('R', Turn('R', facing));

                                break;
                            case 3: // y => set by  x - no correction required; x => set to zero - no correction required
                                newPosition.y = sectionsize + newPosition.x - (sectionsize * 3);
                                newPosition.x = 0;
                                facing = Turn('L', facing);
                                break;
                        }

                        break;
                    case 3:
                        switch (WidthSection) {
                            case 0: //y => set to outer sectionsize - no correction required; x=> set by y - no correction required
                                newPosition.x = newPosition.y + (sectionsize);
                                newPosition.y = sectionsize;
                                facing = Turn('R', facing);
                                break;
                            case 1://y => set to 0 - no correction required; x=> set by y - no correction required
                                newPosition.x = (newPosition.y) + (sectionsize * 2);
                                newPosition.y = 0;
                                facing = Turn('R', facing);
                                break;
                            case 2://y => set by inversion of y - sectionsize correction required; x=> set by inner sectionsize - sectionsize correction required
                                newPosition.y = newPosition.y - sectionsize * 2;
                                newPosition.x = sectionsize * 4 - 1;
                                break;

                        }

                        break;
                }
                return newPosition;
            }


            public static int Turn(char dir, int curr) {
                if (dir == 'L')
                    curr = (curr + 3) % 4;
                else
                    curr = (curr + 1) % 4;
                return curr;
            }
        }
    }
}