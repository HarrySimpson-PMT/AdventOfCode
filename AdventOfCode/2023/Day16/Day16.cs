
namespace AdventOfCode.Year2023
{
    public class Day16 : Day
    {
        public Day16(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MirrorMaze mirrorMaze = new MirrorMaze(data);
            result = mirrorMaze.RunMaze().Result.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MirrorMaze mirrorMaze = new MirrorMaze(data);
            result = mirrorMaze.FindMaxPossible().ToString();
            Console.WriteLine(result);
        }
        public class MirrorMaze
        {
            public int Result { get; set; } = 0;
            int n;
            int m;
            private Node[,] Maze { get; set; }
            public MirrorMaze(string[] data)
            {
                n = data.Length;
                m = data[0].Length;
                Maze = new Node[n, m];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        Maze[i, j] = new Node(i, j, data[i][j]);
                    }
                }
            }
            public int FindMaxPossible()
            {
                int result = 0;
                //North and South
                for (int i = 0; i < m; i++)
                {
                    result = Math.Max(result, RunMaze(-1, i, 0, i).Result);
                    result = Math.Max(result, RunMaze(n, i, n - 1, i).Result);
                }
                //East and West
                for (int i = 0; i < n; i++)
                {
                    result = Math.Max(result, RunMaze(i, -1, i, 0).Result);
                    result = Math.Max(result, RunMaze(i, m, i, m - 1).Result);
                }
                return result;

            }
            public MirrorMaze RunMaze(int x = 0, int y = -1, int nextx = 0, int nexty = 0)
            {
                ClearVisited();
                Result = 0;
                HashSet<(Node, (int x, int y))> visited = new HashSet<(Node, (int x, int y))>();
                Queue<(Node, (int x, int y))> queue = new Queue<(Node, (int x, int y))>();
                queue.Enqueue((Maze[nextx, nexty], (x, y)));
                while (queue.Count > 0)
                {
                    (Node, (int x, int y)) cur = queue.Dequeue();
                    Node node = cur.Item1;
                    if (!node.visited)
                    {
                        node.visited = true;
                        Result++;
                    }
                    List<(int x, int y)> nextNodes = node.NextNode((cur.Item2.x, cur.Item2.y));
                    foreach ((int x, int y) nextNode in nextNodes)
                    {
                        if (nextNode.x < 0 || nextNode.x >= n || nextNode.y < 0 || nextNode.y >= m)
                            continue;
                        if(!visited.Add((Maze[nextNode.x, nextNode.y], (node.X, node.Y))))
                            continue;
                        queue.Enqueue((Maze[nextNode.x, nextNode.y], (node.X, node.Y)));
                    }
                }
                return this;
            }
            public void ClearVisited()
            {
                foreach (Node node in Maze)
                {
                    node.visited = false;
                }
            }
            public void PrintVisited()
            {
                for (int i = 0; i < n; i++)
                {
                    string line = "";
                    for (int j = 0; j < m; j++)
                    {
                        line += Maze[i, j].visited ? "1" : "0";
                    }
                    Console.WriteLine(line);
                }
            }

            class Node
            {
                public int X { get; set; }
                public int Y { get; set; }
                public char Shape { get; set; }
                public bool visited { get; set; } = false;
                public Node(int x, int y, char shape)
                {
                    X = x;
                    Y = y;
                    Shape = shape;
                }
                public List<(int x, int y)> NextNode((int x, int y) LastPosition)
                {
                    (int x, int y) Direction = (X - LastPosition.x, Y - LastPosition.y);
                    (int x, int y) NextDirection = (0, 0);
                    List<(int x, int y)> result = new List<(int x, int y)>();
                    switch (Shape)
                    {
                        case '.':
                            if (Direction == Directions.North)
                            {
                                NextDirection = Directions.North;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.South)
                            {

                                NextDirection = Directions.South;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.East)
                            {

                                NextDirection = Directions.East;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.West)
                            {
                                NextDirection = Directions.West;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            break;
                        case '/':
                            if (Direction == Directions.North)
                            {
                                NextDirection = Directions.East;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.South)
                            {
                                NextDirection = Directions.West;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.East)
                            {
                                NextDirection = Directions.North;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.West)
                            {
                                NextDirection = Directions.South;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            break;
                        case '\\':
                            if (Direction == Directions.North)
                            {
                                NextDirection = Directions.West;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.South)
                            {
                                NextDirection = Directions.East;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.East)
                            {
                                NextDirection = Directions.South;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.West)
                            {
                                NextDirection = Directions.North;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            break;
                        case '|':
                            if (Direction == Directions.North)
                            {
                                NextDirection = Directions.North;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.South)
                            {
                                NextDirection = Directions.South;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.East || Direction == Directions.West)
                            {
                                NextDirection = Directions.North;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                                NextDirection = Directions.South;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            break;
                        case '-':
                            if (Direction == Directions.North || Direction == Directions.South)
                            {
                                NextDirection = Directions.East;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                                NextDirection = Directions.West;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.East)
                            {
                                NextDirection = Directions.East;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            if (Direction == Directions.West)
                            {
                                NextDirection = Directions.West;
                                result.Add((X + NextDirection.x, Y + NextDirection.y));
                            }
                            break;

                    }
                    return result;
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