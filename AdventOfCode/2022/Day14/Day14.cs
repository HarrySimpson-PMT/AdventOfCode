

namespace AdventOfCode.Year2022 {
    public class Day14 : Day {
        public Day14(int today) : base(today) { }

        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            FallingSandSimulator simulator = new(data);
            result = simulator.Simulate().ToString();
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            FallingSandSimulator simulator = new(data, true);
            result = simulator.Simulate(true).ToString();
        }

        public class FallingSandSimulator {
            private char[,] _map = null!;
            private int _minX;
            private int _maxX;
            private int _minY;
            private int _maxY;
            public FallingSandSimulator(string[] input, bool update = false) {
                LoadCoordinateInput(input, update);
            }

            public void LoadCoordinateInput(string[] lines, bool update = false) {
                //each line is formatted like "497,69 -> 497,73 -> 489,73 -> 489,78 -> 504,78 -> 504,73 -> 501,73 -> 501,69" which is a list of connected coordinates
                //each coordinate is formatted like "497,69" which is a list of x,y coordinates
                //get the max x and y coordinates
                int maxX = 0;
                int maxY = 0;
                foreach (string line in lines) {
                    string[] coordinates = line.Split(" -> ");
                    foreach (string coordinate in coordinates) {
                        string[] xy = coordinate.Split(",");
                        int x = int.Parse(xy[0]);
                        int y = int.Parse(xy[1]);
                        if (x > maxX) {
                            maxX = x;
                        }
                        if (y > maxY) {
                            maxY = y;
                        }
                    }
                }
                maxX++;
                if (update) {
                    maxY += 1;
                    maxX = 1000;
                    _minX = 0;
                    _maxX = 1000;
                    _minY = 0;
                    _maxY = maxY;
                }
                //create char map filled with '.'
                _map = new char[maxX + 1, maxY + 1];
                for (int x = 0; x < maxX + 1; x++) {
                    for (int y = 0; y < maxY + 1; y++) {
                        _map[x, y] = '.';
                    }
                }
                //write the coordinates to a map
                foreach (string line in lines) {
                    string[] coordinates = line.Split(" -> ");
                    for (int i = 0; i < coordinates.Length - 1; i++) {
                        string[] coordinate1 = coordinates[i].Split(",");
                        string[] coordinate2 = coordinates[i + 1].Split(",");
                        int x1 = int.Parse(coordinate1[0]);
                        int y1 = int.Parse(coordinate1[1]);
                        int x2 = int.Parse(coordinate2[0]);
                        int y2 = int.Parse(coordinate2[1]);
                        if (x1 == x2) {
                            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) {
                                _map[x1, y] = '#';
                            }
                        }
                        else {
                            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++) {
                                _map[x, y1] = '#';
                            }
                        }
                    }
                }
                if (!update) {
                    //find the edges
                    _minX = 1000;
                    _maxX = 0;
                    _minY = 1000;
                    _maxY = 0;
                    for (int x = 0; x < maxX + 1; x++) {
                        for (int y = 0; y < maxY + 1; y++) {
                            if (_map[x, y] == '#') {
                                if (x < _minX) {
                                    _minX = x;
                                }
                                if (x > _maxX) {
                                    _maxX = x;
                                }
                                if (y < _minY) {
                                    _minY = 0;
                                }
                                if (y > _maxY) {
                                    _maxY = y;
                                }
                            }
                        }
                    }
                    _minX--;
                    _maxX++;
                }
            }
            public int Simulate(bool floor = false) {
                //simulate the falling sand starting at point 500, 0, sand can fall down one space or down one and left or right one space, sand is marked as '0'
                //simulate sand falling one at a time and once it comes to a rest simulate the next one
                //once one sand reaches the bottom, stop simulating
                //return the number of spaces that are filled with sand
                int x = 500;
                int y = 0;
                _map[x, y] = '0';
                int sandCount = 0;
                while (y <= _maxY) {
                    //clear the console
                    //Console.Clear();
                    //Print();
                    //Console.ReadLine();
                    //check if there is an open space directly below x, y
                    if (y < _maxY && _map[x, y + 1] == '.') {
                        _map[x, y] = '.';
                        //move down one
                        y++;
                        _map[x, y] = '0';
                    }
                    //check if down and to the left is open 
                    else if (x > _minX && y < _maxY && _map[x - 1, y + 1] == '.') {
                        _map[x, y] = '.';
                        //move down and left one
                        x--;
                        y++;
                        _map[x, y] = '0';
                    }
                    //check if down and to the right is open
                    else if (x < _maxX && y < _maxY && _map[x + 1, y + 1] == '.') {
                        _map[x, y] = '.';
                        //move down and right one
                        x++;
                        y++;
                        _map[x, y] = '0';
                    }
                    else {
                        if (floor && x == 500 && y == 0) {
                            sandCount++;
                            break;
                        }
                        x = 500;
                        y = 0;
                        sandCount++;
                        _map[x, y] = '0';
                    }
                    if (y == _maxY && !floor) {
                        break;
                    }
                }
                return sandCount;
            }
            public void Print() {
                for (int y = _minY; y <= _maxY; y++) {
                    for (int x = _minX; x <= _maxX; x++) {
                        Console.Write(_map[x, y]);
                        if (x == _maxX && y == _minY)
                            Console.Write($"   => {_minX},{_minY} x {_maxX}, {_maxY}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
