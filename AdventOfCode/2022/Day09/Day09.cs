using static AdventOfCode.Year2022.Day09;

namespace AdventOfCode.Year2022 {
    public class Day09 : Day {

        public Day09(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            PlanckBridge planckBridge = new();
            planckBridge.SimulateKnot(data, 2);
            result = planckBridge.UniqueTailLocations.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            PlanckBridge planckBridge = new();
            planckBridge.SimulateKnot(data, 10);
            //planckBridge.print();
            result = planckBridge.UniqueTailLocations.ToString();

            Console.WriteLine(result);
        }
        public class PlanckBridge {
            HashSet<(int, int)> TailLocations = new HashSet<(int, int)>();
            public void print() {
                for (int i = -20; i < 20; i++) {
                    for (int j = -20; j < 20; j++) {
                        Console.Write((TailLocations.Contains((i, j)) ? "X" : "."));
                    }
                    Console.WriteLine();
                }
            }
            public int UniqueTailLocations => TailLocations.Count;
            public static (int, int) update((int, int) start, (int, int) end) {
                //ZYYYZ
                //Y...y
                //Y.X.Y
                //Y...Y
                //ZYYYZ
                (int, int) diff = (start.Item1 - end.Item1, start.Item2 - end.Item2);
                if (diff.Item1 == 2 && diff.Item2 == 2) {
                    end = (end.Item1 + 1, end.Item2 + 1);
                }
                else if (diff.Item1 == 2 && diff.Item2 == -2) {
                    end = (end.Item1 + 1, end.Item2 - 1);
                }
                else if (diff.Item1 == -2 && diff.Item2 == -2) {
                    end = (end.Item1 - 1, end.Item2 - 1);
                }
                else if (diff.Item1 == -2 && diff.Item2 == 2) {
                    end = (end.Item1 - 1, end.Item2 + 1);
                }
                else if (diff.Item1 == 2) {
                    end = (end.Item1 + 1, end.Item2 + diff.Item2);
                }
                else if (diff.Item1 == -2) {
                    end = (end.Item1 - 1, end.Item2 + diff.Item2);

                }
                else if (diff.Item2 == 2) {
                    end = (end.Item1 + diff.Item1, end.Item2 + 1);

                }
                else if (diff.Item2 == -2) {
                    end = (end.Item1 + diff.Item1, end.Item2 - 1);
                }

                return end;
            }
            public void SimulateKnot(string[] data, int chainsize) {
                (int, int)[] dirs = { (0, -1), (0, 1), (-1, 0), (1, 0) };
                (int, int)[] chain = new (int, int)[chainsize];
                foreach (string action in data) {
                    string[] actions = action.Split(" ");
                    int dir = -1;
                    switch (actions[0]) {
                        case "L":
                            dir = 0;
                            break;
                        case "R":
                            dir = 1;
                            break;
                        case "U":
                            dir = 2;
                            break;
                        case "D":
                            dir = 3;
                            break;
                    }
                    int move = int.Parse(actions[1]);
                    for (int i = 0; i < move; i++) {
                        chain[0] = (chain[0].Item1 + dirs[dir].Item1, chain[0].Item2 + dirs[dir].Item2);
                        for (int j = 1; j < chainsize; j++) {
                            chain[j] = update(chain[j - 1], chain[j]);
                        }
                        TailLocations.Add(chain[chainsize - 1]);
                    }
                }
            }
        }
    }

}