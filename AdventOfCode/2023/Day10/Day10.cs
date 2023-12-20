
using System.Collections;
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day10 : Day {
        public Day10(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            PipeAnalyzer analyzer = new PipeAnalyzer(data);
            result = analyzer.Result.ToString();
            //analyzer.PrintMap();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            PipeAnalyzer analyzer = new PipeAnalyzer(data);
            result = analyzer.CalulateSurroundedArea().ToString();
            //analyzer.PrintVisited();
            Console.WriteLine(result);
        }
        public class PipeAnalyzer {
            private Dictionary<char, Pipe> Pipes = null!;
            private Pipe[,] Map = null!;
            private bool[,] Visited = null!;
            public int Result { get; } = 0;
            public PipeAnalyzer(string[] data) {
                BuildPipes(data);
                Map = new Pipe[data.Length, data[0].Length];
                Visited = new bool[data.Length, data[0].Length];

                (int x, int y) Start = (-1, -1);
                for (int i = 0; i < data.Length; i++) {
                    for (int j = 0; j < data[i].Length; j++) {
                        Map[i, j] = Pipes[' '];
                        if (data[i][j] == 'S')
                            Start = (i, j);
                    }
                }
                //bfs with levels

                Queue<((int x, int y) cur, (int x, int y) prev)> queue = new Queue<((int x, int y), (int x, int y))>();
                queue.Enqueue((Start, (-1, -1)));
                int level = 0;
                while (queue.Count > 0) {
                    int currentLevel = queue.Count;
                    while (currentLevel > 0) {
                        currentLevel--;
                        var current = queue.Dequeue();
                        if (data[current.cur.x][current.cur.y] == 'S' && level > 0) {
                            Result = (level) / 2;
                            return;
                        }
                        Pipe pipe = Pipes[data[current.cur.x][current.cur.y]];
                        if (pipe.Contains(current.prev) || current.prev == (-1, -1)) {
                            foreach (var dir in pipe) {
                                if (dir != current.prev) {
                                    (int x, int y) next = (current.cur.x + dir.x, current.cur.y + dir.y);
                                    if (next.x < 0 || next.x >= data.Length || next.y < 0 || next.y >= data[0].Length)
                                        continue;
                                    Map[current.cur.x, current.cur.y] = pipe;
                                    queue.Enqueue((next, InvertDirection(dir)));
                                }
                            }
                        }
                    }
                    level++;
                }
            }
            public int CalulateSurroundedArea() {
                (int x, int y)[] dirs = new (int x, int y)[] { Directions.North, Directions.South, Directions.East, Directions.West };
                int result = 0;
                //start from 0,0 and bfs until we hit a pipe with a name != "0"
                Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
                queue.Enqueue((0, 0));
                bool PipeEdgesAdded = false;
                while (queue.Count > 0) {
                    var current = queue.Dequeue();
                    Visited[current.x, current.y] = true;
                    result++;
                    foreach (var dir in dirs) {
                        (int x, int y) next = (current.x + dir.x, current.y + dir.y);
                        if (next.x < 0 || next.x >= Map.GetLength(0) || next.y < 0 || next.y >= Map.GetLength(1) || Visited[next.x, next.y])
                            continue;
                        if (Map[next.x, next.y].Name != "0") {
                            if (!PipeEdgesAdded) {
                                PipeEdgesAdded = true;
                                //cur defines outside edge of the pipe, now we need to queue both 
                                //add outside edge
                                Queue<((int x, int y) pos, (int x, int y) outside)> ToPorcess = new();
                                ToPorcess.Enqueue((next, InvertDirection(dir)));
                                Visited[next.x, next.y] = true;
                                result++;
                                bool corrected = false;
                                while (ToPorcess.Count > 0) {
                                    var cur = ToPorcess.Dequeue();
                                    if (cur.pos == (2, 3)) {
                                        Debugger.Break();
                                    }
                                    Pipe pipe = Map[cur.pos.x, cur.pos.y];
                                    if (pipe.Count() > 2)
                                        continue;
                                    (int x, int y) outside = (cur.pos.x + cur.outside.x, cur.pos.y + cur.outside.y);
                                    if (outside.x > 0 && outside.x < Map.GetLength(0) && outside.y >= 0 && outside.y < Map.GetLength(1) && !Visited[outside.x, outside.y]) {
                                        if (Map[outside.x, outside.y].Name == "0") {
                                            if (!Visited[outside.x, outside.y]) {
                                                if (outside == (3, 14))
                                                    Debugger.Break();
                                                queue.Enqueue(outside);
                                                Visited[outside.x, outside.y] = true;
                                                result++;
                                            }
                                        }
                                    }
                                    switch (pipe.Name) {
                                        case "7":
                                            if (cur.outside == Directions.South)
                                                cur.outside = Directions.West;
                                            else if (cur.outside == Directions.West)
                                                cur.outside = Directions.South;
                                            else if (cur.outside == Directions.North)
                                                cur.outside = Directions.East;
                                            else if (cur.outside == Directions.East)
                                                cur.outside = Directions.North;
                                            break;
                                        case "J":
                                            if (cur.outside == Directions.North)
                                                cur.outside = Directions.West;
                                            else if (cur.outside == Directions.West)
                                                cur.outside = Directions.North;
                                            else if (cur.outside == Directions.South)
                                                cur.outside = Directions.East;
                                            else if (cur.outside == Directions.East)
                                                cur.outside = Directions.South;
                                            break;
                                        case "L":
                                            if (cur.outside == Directions.North)
                                                cur.outside = Directions.East;
                                            else if (cur.outside == Directions.East)
                                                cur.outside = Directions.North;
                                            else if (cur.outside == Directions.South)
                                                cur.outside = Directions.West;
                                            else if (cur.outside == Directions.West)
                                                cur.outside = Directions.South;
                                            break;
                                        case "F":
                                            if (cur.outside == Directions.South)
                                                cur.outside = Directions.East;
                                            else if (cur.outside == Directions.East)
                                                cur.outside = Directions.South;
                                            else if (cur.outside == Directions.North)
                                                cur.outside = Directions.West;
                                            else if (cur.outside == Directions.West)
                                                cur.outside = Directions.North;
                                            break;
                                    }
                                    if (new List<string>() { "F", "7", "J", "L" }.Contains(pipe.Name)) {
                                        outside = (cur.pos.x + cur.outside.x, cur.pos.y + cur.outside.y);
                                        if (outside.x >= 0 && outside.x < Map.GetLength(0) && outside.y >= 0 && outside.y < Map.GetLength(1) && !Visited[outside.x, outside.y]) {
                                            if (Map[outside.x, outside.y].Name == "0") {
                                                if (outside == (3, 14))
                                                    Debugger.Break();
                                                queue.Enqueue(outside);
                                                Visited[outside.x, outside.y] = true;
                                                result++;
                                            }
                                        }
                                        //if (cur.outside == Directions.South && pipe.Name == "7")
                                        //{
                                        //}
                                        //else if () { }
                                        //else if () { }
                                        //else if () { }
                                        //else
                                        //{

                                        //    foreach (var direction in pipe)
                                        //    {
                                        //        if (InvertDirection(cur.outside) != direction)
                                        //        {
                                        //            cur.outside = InvertDirection(direction);
                                        //            outside = (cur.pos.x + cur.outside.x, cur.pos.y + cur.outside.y);
                                        //            if (outside.x >= 0 && outside.x < Map.GetLength(0) && outside.y >= 0 && outside.y < Map.GetLength(1) && !Visited[outside.x, outside.y])
                                        //            {
                                        //                if (Map[outside.x, outside.y].Name == "0")
                                        //                {
                                        //                    if (outside == (3, 14))
                                        //                        Debugger.Break();
                                        //                    queue.Enqueue(outside);
                                        //                    Visited[outside.x, outside.y] = true;
                                        //                    result++;
                                        //                }
                                        //            }
                                        //            break;
                                        //        }
                                        //    }
                                        //}
                                    }
                                    foreach (var direction in pipe) {
                                        (int x, int y) nextPos = (cur.pos.x + direction.x, cur.pos.y + direction.y);
                                        if (nextPos.x < 0 || nextPos.x >= Map.GetLength(0) || nextPos.y < 0 || nextPos.y >= Map.GetLength(1) || Visited[nextPos.x, nextPos.y])
                                            continue;
                                        if (Map[nextPos.x, nextPos.y].Name != "0" && !Visited[nextPos.x, nextPos.y]) {
                                            Visited[nextPos.x, nextPos.y] = true;
                                            if ((nextPos.x, nextPos.y) == (3, 14))
                                                Debugger.Break();
                                            result++;
                                            if (InvertDirection(cur.outside) == direction && !corrected) {
                                                corrected = true;
                                                var dirchange = InvertDirection(Map[cur.pos.x, cur.pos.y].Where(x => x != direction).First());
                                                ToPorcess.Enqueue((nextPos, dirchange));
                                            }
                                            else {
                                                ToPorcess.Enqueue((nextPos, cur.outside));
                                            }
                                        }
                                    }
                                }
                            }
                            continue;
                        }
                        Visited[next.x, next.y] = true;
                        result++;
                        queue.Enqueue(next);
                    }
                }
                int count = 0;
                for (int i = 0; i < Visited.GetLength(0); i++) {
                    for (int j = 0; j < Visited.GetLength(1); j++) {
                        if (!Visited[i, j])
                            count++;
                    }
                }
                return count;
            }
            public void PrintMap() {
                for (int i = 0; i < Map.GetLength(0); i++) {
                    for (int j = 0; j < Map.GetLength(1); j++) {
                        Console.Write(Map[i, j].Name);
                    }
                    Console.WriteLine();
                }
            }
            public void PrintVisited() {
                for (int i = 0; i < Visited.GetLength(0); i++) {
                    for (int j = 0; j < Visited.GetLength(1); j++) {
                        Console.Write(Visited[i, j] ? "1" : "0");
                    }
                    Console.WriteLine();
                }
            }
            public static class Directions {
                public static (int x, int y) North = (-1, 0);
                public static (int x, int y) South = (1, 0);
                public static (int x, int y) East = (0, 1);
                public static (int x, int y) West = (0, -1);
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
            public void BuildPipes(string[] data) {
                Pipes = new Dictionary<char, Pipe>();
                Pipes.Add('|', new Pipe(new (int x, int y)[] { Directions.North, Directions.South }, "|"));
                Pipes.Add('-', new Pipe(new (int x, int y)[] { Directions.East, Directions.West }, "-"));
                Pipes.Add('L', new Pipe(new (int x, int y)[] { Directions.North, Directions.East }, "L"));
                Pipes.Add('J', new Pipe(new (int x, int y)[] { Directions.North, Directions.West }, "J"));
                Pipes.Add('7', new Pipe(new (int x, int y)[] { Directions.South, Directions.West }, "7"));
                Pipes.Add('F', new Pipe(new (int x, int y)[] { Directions.South, Directions.East }, "F"));
                Pipes.Add('.', new Pipe(new (int x, int y)[] { }, "."));
                Pipes.Add(' ', new Pipe(new (int x, int y)[] { }, "0"));
                Pipes.Add('S', new Pipe(new (int x, int y)[] { Directions.North, Directions.South, Directions.East, Directions.West }, "+"));
            }
            public static (int x, int y) InvertDirection((int x, int y) direction) {
                return (-direction.x, -direction.y);
            }
            public class Pipe : IEnumerable<(int x, int y)> {
                public string Name { get; private set; }
                public override string ToString() {
                    string result = "";
                    foreach (var direction in Directions)
                        result += $"({PipeAnalyzer.Directions.DirectionName(direction)})";
                    return result;
                }
                public (int x, int y)[] Directions { get; private set; }

                public Pipe((int x, int y)[] directions, string name) {
                    Directions = directions;
                    Name = name;
                }

                public (int x, int y) this[int index] {
                    get => Directions[index];
                    set => Directions[index] = value;
                }

                public IEnumerator<(int x, int y)> GetEnumerator() {
                    return ((IEnumerable<(int x, int y)>)Directions).GetEnumerator();
                }

                IEnumerator<(int x, int y)> IEnumerable<(int x, int y)>.GetEnumerator() {
                    return ((IEnumerable<(int x, int y)>)Directions).GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator() {
                    return ((IEnumerable<(int x, int y)>)Directions).GetEnumerator();
                }
            }
        }
    }

}