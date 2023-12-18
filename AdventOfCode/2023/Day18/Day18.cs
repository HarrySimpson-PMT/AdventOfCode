
using System.Drawing;

namespace AdventOfCode.Year2023 {
    public class Day18 : Day {
        public Day18(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            LavalPool pool = new LavalPool(data, false);
            result = pool.CalculateArea().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            LavalPool pool = new LavalPool(data, true);
            result = pool.CalculateArea().ToString();
            //pool.PrintScaledMap();
            Console.WriteLine(result);
        }
        public class LavalPool {
            SortedDictionary<long, List<(bool, long)>> xTrenches = new SortedDictionary<long, List<(bool, long)>>();
            long north = 0;
            public LavalPool(string[] data, bool corrected) {
                long currentX = 0;
                long currentY = 0;
                foreach (string line in data) {
                    string[] parts = line.Split(' ');
                    long distance = long.Parse(parts[1]);
                    string color = parts[2].Substring(2, 7);
                    string direction = parts[0];
                    if (corrected) {
                        string hexdistance = color.Substring(0, 5);
                        distance = long.Parse(hexdistance, System.Globalization.NumberStyles.HexNumber);
                        switch (color[5]) {
                            case '0':
                                direction = "R";
                                break;
                            case '1':
                                direction = "D";
                                break;
                            case '2':
                                direction = "L";
                                break;
                            case '3':
                                direction = "U";
                                break;
                        }
                    }
                    switch (direction) {
                        case "R":
                            currentY += distance;
                            break;
                        case "L":
                            currentY -= distance;
                            break;
                        case "U":
                            //current point terminator
                            if (!xTrenches.ContainsKey(currentX)) {
                                xTrenches.Add(currentX, new List<(bool, long)>());
                            }
                            xTrenches[currentX].Add((false, currentY));
                            //next point initiator
                            currentX -= distance;
                            if (!xTrenches.ContainsKey(currentX)) {
                                xTrenches.Add(currentX, new List<(bool, long)>());
                            }
                            xTrenches[currentX].Add((true, currentY));
                            break;
                        case "D":
                            //current point initiator
                            if (!xTrenches.ContainsKey(currentX)) {
                                xTrenches.Add(currentX, new List<(bool, long)>());
                            }
                            xTrenches[currentX].Add((true, currentY));
                            //next point terminator
                            currentX += distance;
                            if (!xTrenches.ContainsKey(currentX)) {
                                xTrenches.Add(currentX, new List<(bool, long)>());
                            }
                            xTrenches[currentX].Add((false, currentY));
                            break;
                    }
                }
            }
            public long CalculateArea() {
                //start from the top of xtrenches and calulate area between each initiator
                long result = 0;
                long lastx = xTrenches.Keys.ElementAt(0);
                long area = 0;
                List<long> trenchpoint = new List<long>();
                List<(bool, long)> changes = new List<(bool, long)>();
                for (int i = 0; i < xTrenches.Count; i++) {
                    long x = xTrenches.Keys.ElementAt(i);
                    result += (x - lastx) * area;
                    area = 0;
                    lastx = x + 1;
                    changes.Clear();
                    changes.AddRange(xTrenches[x]);
                    changes.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                    List<long> tempTrench = trenchpoint.ToList();
                    int edge = 0;
                    for (int j = 0; j < changes.Count; j += 2) {
                        if (!changes[j].Item1) {
                            if (!changes[j + 1].Item1) { //remove edge
                                trenchpoint.Remove(changes[j].Item2);
                                trenchpoint.Remove(changes[j + 1].Item2);
                                //find position of changes[j] in tempTrench
                                tempTrench.Sort();
                                int pos = tempTrench.BinarySearch(changes[j].Item2);
                                if (pos < 0) {
                                    pos = ~pos;
                                }
                                if (pos % 2 != 0) { //take left edge
                                    tempTrench.Remove(changes[j].Item2);
                                    tempTrench.Remove(changes[j + 1].Item2);
                                }
                            }
                            else {//increment edge
                                trenchpoint.Remove(changes[j].Item2);
                                trenchpoint.Add(changes[j + 1].Item2);
                                tempTrench.Sort();
                                if (tempTrench.IndexOf(changes[j].Item2) % 2 != 0) { //take new edge
                                    tempTrench.Remove(changes[j].Item2);
                                    tempTrench.Add(changes[j + 1].Item2);
                                }
                                edge++;
                            }
                        }
                        if (changes[j].Item1) { //add edge
                            if (changes[j + 1].Item1) {
                                trenchpoint.Add(changes[j].Item2);
                                trenchpoint.Add(changes[j + 1].Item2);
                                tempTrench.Sort();
                                int pos = tempTrench.BinarySearch(changes[j].Item2);
                                if (pos < 0) {
                                    pos = ~pos;
                                }
                                if (pos % 2 == 0) { //take new edge
                                    tempTrench.Add(changes[j].Item2);
                                    tempTrench.Add(changes[j + 1].Item2);
                                }
                            }
                            else {//increment edge
                                trenchpoint.Add(changes[j].Item2);
                                trenchpoint.Remove(changes[j + 1].Item2);
                                tempTrench.Sort();
                                if (tempTrench.IndexOf(changes[j + 1].Item2) % 2 == 0) { //take right edge
                                    tempTrench.Add(changes[j].Item2);
                                    tempTrench.Remove(changes[j + 1].Item2);
                                }
                                edge++;
                            }
                        }
                    }
                    trenchpoint.Sort();
                    tempTrench.Sort();
                    for (int k = 0; k < tempTrench.Count; k += 2) {
                        result += tempTrench[k + 1] - tempTrench[k] + 1;
                    }
                    for (int k = 0; k < trenchpoint.Count; k += 2) {
                        area += trenchpoint[k + 1] - trenchpoint[k] + 1;
                    }

                }
                return result;
            }

        }
    }
}