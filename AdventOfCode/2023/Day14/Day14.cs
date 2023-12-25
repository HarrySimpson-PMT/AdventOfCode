
namespace AdventOfCode.Year2023 {
    public class Day14 : Day {
        public Day14(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RockyMirror mirror = new RockyMirror(data);
            mirror.RunPart1();
            result = mirror.Result.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RockyMirror mirror = new RockyMirror(data);
            mirror.Run(1000000000);
            result = mirror.Result.ToString();
            Console.WriteLine(result);
        }
        public class RockyMirror {
            protected char[,] _Data;
            protected Dictionary<string, int> hashes = new Dictionary<string, int>();
            protected int n;
            protected int m;
            private int _cycleStart = 0;
            private int _cycleLength = 0;
            public int Result { get; set; } = 0;
            public RockyMirror(string[] data) {
                _Data = new char[data.Length, data[0].Length];
                n = data.Length;
                m = data[0].Length;
                for (int i = 0; i < data.Length; i++) {
                    for (int j = 0; j < data[0].Length; j++) {
                        _Data[i, j] = data[i][j];
                    }
                }
            }
            public void RunPart1() {
                RollNorth();
                CalculateNorthLoad();
            }
            public void Run(int TotalTimesToRun) {
                while (!IsHashed()) {
                    RollNorth();
                    RollWest();
                    RollSouth();
                    RollEast();
                }
                Console.WriteLine($"Cycle start: {_cycleStart}, Cycle length: {_cycleLength}");
                int timesToRun = ((TotalTimesToRun - _cycleStart) % _cycleLength);
                for (int i = 0; i < timesToRun; i++) {
                    RollNorth();
                    RollWest();
                    RollSouth();
                    RollEast();
                }
                CalculateNorthLoad();
            }
            public void Print() {
                for (int i = 0; i < _Data.GetLength(0); i++) {
                    for (int j = 0; j < _Data.GetLength(1); j++) {
                        Console.Write(_Data[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            public bool IsHashed() {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < _Data.GetLength(0); i++) {
                    for (int j = 0; j < _Data.GetLength(1); j++) {
                        sb.Append(_Data[i, j]);
                    }
                }
                string hash = sb.ToString().GetHashCode().ToString();
                if (hashes.ContainsKey(hash)) {
                    _cycleStart = hashes[hash];
                    _cycleLength = hashes.Count - _cycleStart;
                    return true;
                }
                else {
                    hashes.Add(hash, hashes.Count);
                    return false;
                }
            }
            public void RollNorth() {
                for (int j = 0; j < m; j++) {
                    Stack<(int pos, int val)> Column = new Stack<(int pos, int val)>();
                    for (int i = 0; i < n; i++) {
                        if (_Data[i, j] == '#') {
                            _Data[i, j] = '.';
                            Column.Push((i, 0));
                        }
                        else if (_Data[i, j] == 'O') {
                            _Data[i, j] = '.';
                            if (Column.Count > 0) {
                                Column.Push((Column.Peek().pos + 1, 1));
                            }
                            else {
                                Column.Push((0, 1));
                            }
                        }
                    }
                    while (Column.Count > 0) {
                        var current = Column.Pop();
                        _Data[current.pos, j] = current.val == 0 ? '#' : 'O';
                    }
                }

            }
            public void RollWest() {
                for (int i = 0; i < n; i++) {
                    Stack<(int pos, int val)> Row = new Stack<(int pos, int val)>();
                    for (int j = 0; j < m; j++) {
                        if (_Data[i, j] == '#') {
                            _Data[i, j] = '.';
                            Row.Push((j, 0));
                        }
                        else if (_Data[i, j] == 'O') {
                            _Data[i, j] = '.';
                            if (Row.Count > 0) {
                                Row.Push((Row.Peek().pos + 1, 1));
                            }
                            else {
                                Row.Push((0, 1));
                            }
                        }
                    }
                    while (Row.Count > 0) {
                        var current = Row.Pop();
                        _Data[i, current.pos] = current.val == 0 ? '#' : 'O';
                    }
                }
            }
            public void RollSouth() {
                for (int j = 0; j < m; j++) {
                    Stack<(int pos, int val)> Column = new Stack<(int pos, int val)>();
                    for (int i = n - 1; i >= 0; i--) {
                        if (_Data[i, j] == '#') {
                            _Data[i, j] = '.';
                            Column.Push((i, 0));
                        }
                        else if (_Data[i, j] == 'O') {
                            _Data[i, j] = '.';
                            if (Column.Count > 0) {
                                Column.Push((Column.Peek().pos - 1, 1));
                            }
                            else {
                                Column.Push((n - 1, 1));
                            }
                        }
                    }
                    while (Column.Count > 0) {
                        var current = Column.Pop();
                        _Data[current.pos, j] = current.val == 0 ? '#' : 'O';
                    }
                }
            }
            public void RollEast() {
                for (int i = 0; i < n; i++) {
                    Stack<(int pos, int val)> Row = new Stack<(int pos, int val)>();
                    for (int j = m - 1; j >= 0; j--) {
                        if (_Data[i, j] == '#') {
                            _Data[i, j] = '.';
                            Row.Push((j, 0));
                        }
                        else if (_Data[i, j] == 'O') {
                            _Data[i, j] = '.';
                            if (Row.Count > 0) {
                                Row.Push((Row.Peek().pos - 1, 1));
                            }
                            else {
                                Row.Push((m - 1, 1));
                            }
                        }
                    }
                    while (Row.Count > 0) {
                        var current = Row.Pop();
                        _Data[i, current.pos] = current.val == 0 ? '#' : 'O';
                    }
                }

            }

            public void CalculateNorthLoad() {
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < m; j++) {
                        if (_Data[i, j] == 'O') {
                            Result += (n - i);
                        }
                    }
                }
            }
            public static int CalculateNorthLoad(string[] data) {
                int n = data.Length;
                int result = 0;
                for (int j = 0; j < data[0].Length; j++) {
                    Stack<(int pos, int val)> Column = new Stack<(int pos, int val)>();
                    for (int i = 0; i < data.Length; i++) {
                        if (data[i][j] == '#') {
                            Column.Push((i, 0));
                        }
                        else if (data[i][j] == 'O') {
                            if (Column.Count > 0) {
                                Column.Push((Column.Peek().pos + 1, 1));
                            }
                            else {
                                Column.Push((0, 1));
                            }
                        }
                    }
                    while (Column.Count > 0) {
                        var current = Column.Pop();
                        result += current.val * (n - current.pos);
                    }
                }
                return result;
            }
        }
    }

}