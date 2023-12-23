
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day23 : Day {
        public Day23(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfTrails elfTrails = new ElfTrails(data);
            elfTrails.BuildIntersectionMap();
            elfTrails.PrintIntersectionMap();
            result = elfTrails.LongestPath().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfTrails elfTrails = new ElfTrails(data);
            elfTrails.BuildIntersectionMap();
            elfTrails.UndirectIntersectionPaths();
            result = elfTrails.LongestPath2().ToString();
            Console.WriteLine(result);
        }
        public class ElfTrails {
            internal char[,] map;
            internal (int x, int y) start;
            internal (int x, int y) end;
            internal int n;
            internal int m;
            Dictionary<(int x, int y), Intersections> intersections = new Dictionary<(int x, int y), Intersections>();
            public ElfTrails(string[] data) {
                n = data.Length;
                m = data[0].Length;
                map = new char[n, m];
                for (int i = 0; i < data.Length; i++) {
                    for (int j = 0; j < data[0].Length; j++) {
                        map[i, j] = data[i][j];
                    }
                }
                start = (0, 1);
                end = (data.Length - 1, data[0].Length - 2);
            }
            public long LongestPath2() {
                long result = 0;
                Queue<(Intersections x, long dist, HashSet<Intersections> visited)> queue = new Queue<(Intersections x, long dist, HashSet<Intersections> visited)>();
                queue.Enqueue((intersections[start], 0, new HashSet<Intersections>()));
                while (queue.Count > 0) {
                    (Intersections x, long dist, HashSet<Intersections> visited) current = queue.Dequeue();
                    current.visited.Add(current.x);
                    foreach (var connection in current.x.connections) {
                        if (connection.x.position == end) {
                            result = Math.Max(result, current.dist + connection.dist);
                        }
                        else {
                            if (!current.visited.Contains(connection.x)) { 
                                queue.Enqueue((connection.x, current.dist + connection.dist, current.visited.ToHashSet()));
                            }
                        }
                    }
                }
                return result;
            }
            public void UndirectIntersectionPaths() {
                foreach (var intersection in intersections) {
                    foreach (var connection in intersection.Value.connections) {
                        if (!connection.x.connections.Any(x => x.x == intersection.Value)) {
                            connection.x.connections.Add((intersection.Value, connection.dist));
                        }
                    }
                }
            }
            public int LongestPath() {
                int result = 0;
                Queue<(Intersections x, int dist)> queue = new Queue<(Intersections x, int dist)>();
                queue.Enqueue((intersections[end], 0));
                while (queue.Count > 0) {
                    (Intersections x, int dist) current = queue.Dequeue();
                    foreach (var connection in current.x.connections) {
                        if (connection.x.position == start) {
                            result = Math.Max(result, current.dist + connection.dist);
                        }
                        else {
                            queue.Enqueue((connection.x, current.dist + connection.dist));
                        }
                    }
                }
                return result;
            }
            public void BuildIntersectionMap() {
                //BFS the map, we only need to track visited interesections to make sure we don't rerun them.
                HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
                Queue<(int x, int y, Intersections last, int dist)> queue = new Queue<(int x, int y, Intersections last, int dist)>();
                intersections.Add(start, new Intersections(start));
                queue.Enqueue((start.x, start.y, intersections[start], 0));
                while (queue.Count > 0) {
                    (int x, int y, Intersections last, int dist) current = queue.Dequeue();
                    foreach (var dir in Directions.All) {
                        (int x, int y) neighbour = (current.x + dir.x, current.y + dir.y);
                        if (neighbour == end) {
                            if (!intersections.ContainsKey(end)) {
                                intersections.Add(end, new Intersections(end));
                            }
                            intersections[end].connections.Add((current.last, current.dist + 1));
                            continue;
                        }
                        if (Directions.InBounds(neighbour, n, m) && map[neighbour.x, neighbour.y] != '#' && !visited.Contains(neighbour)) {
                            if (new List<char> { '<', '>', 'v' }.Contains(map[neighbour.x, neighbour.y]) && !intersections.ContainsKey((current.x, current.y))) { //intersection adj
                                if (dir == Directions.North) {
                                }
                                if (dir == Directions.South && map[neighbour.x, neighbour.y] == 'v') {
                                    neighbour = (neighbour.x + 1, neighbour.y);
                                    if (!intersections.ContainsKey(neighbour)) {
                                        intersections.Add((neighbour.x, neighbour.y), new Intersections((neighbour.x, neighbour.y)));
                                        queue.Enqueue((neighbour.x, neighbour.y, intersections[neighbour], 0));
                                    }
                                    intersections[neighbour].connections.Add((current.last, current.dist + 2));
                                }
                                if (dir == Directions.East && map[neighbour.x, neighbour.y] == '>') {
                                    neighbour = (neighbour.x, neighbour.y + 1);
                                    if (!intersections.ContainsKey(neighbour)) {
                                        intersections.Add((neighbour.x, neighbour.y), new Intersections((neighbour.x, neighbour.y)));
                                        queue.Enqueue((neighbour.x, neighbour.y, intersections[neighbour], 0));
                                    }
                                    intersections[neighbour].connections.Add((current.last, current.dist + 2));
                                }
                                if (dir == Directions.West && map[neighbour.x, neighbour.y] == '<') {
                                    neighbour = (neighbour.x, neighbour.y - 1);
                                    if (!intersections.ContainsKey(neighbour)) {
                                        intersections.Add((neighbour.x, neighbour.y), new Intersections((neighbour.x, neighbour.y)));
                                        queue.Enqueue((neighbour.x, neighbour.y, intersections[neighbour], 0));
                                    }
                                    intersections[neighbour].connections.Add((current.last, current.dist + 2));
                                }
                            }
                            else if (new List<char> { '<', '>', 'v' }.Contains(map[neighbour.x, neighbour.y])) {
                                if (dir == Directions.North) {
                                    continue;
                                }
                                if (dir == Directions.South && map[neighbour.x, neighbour.y] == 'v') {
                                    visited.Add(neighbour);
                                    neighbour = (neighbour.x + 1, neighbour.y);
                                    visited.Add(neighbour);
                                    queue.Enqueue((neighbour.x, neighbour.y, current.last, current.dist + 2));
                                }
                                if (dir == Directions.East && map[neighbour.x, neighbour.y] == '>') {
                                    visited.Add(neighbour);
                                    neighbour = (neighbour.x, neighbour.y + 1);
                                    visited.Add(neighbour);
                                    queue.Enqueue((neighbour.x, neighbour.y, current.last, current.dist + 2));
                                }
                                if (dir == Directions.West && map[neighbour.x, neighbour.y] == '<') {
                                    visited.Add(neighbour);
                                    neighbour = (neighbour.x, neighbour.y - 1);
                                    visited.Add(neighbour);
                                    queue.Enqueue((neighbour.x, neighbour.y, current.last, current.dist + 2));
                                }
                            }
                            else {
                                if (visited.Add(neighbour)) {
                                    queue.Enqueue((neighbour.x, neighbour.y, current.last, current.dist + 1));
                                }
                            }
                        }
                    }
                }
            }
            public void PrintIntersectionMap() {
                foreach (var intersection in intersections) {
                    Console.WriteLine($"{intersection.Key.x}, {intersection.Key.y}");
                    foreach (var connection in intersection.Value.connections) {
                        Console.WriteLine($"    {connection.x.position.x}, {connection.x.position.y} - {connection.dist}");
                    }
                }
            }
            internal class Intersections {
                internal (int x, int y) position;
                internal List<(Intersections x, int dist)> connections;
                public Intersections((int x, int y) position) {
                    this.position = position;
                    connections = new List<(Intersections x, int dist)>();
                }
                public void AddConnection(Intersections intersection, int dist) {
                    connections.Add((intersection, dist));
                }
                public override string ToString() {
                    return $"{position.x}, {position.y}";
                }
            }
        }
    }

}