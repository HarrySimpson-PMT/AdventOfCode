
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day25 : Day {
        public Day25(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            WeatherMachinePowerManager powerManager = new WeatherMachinePowerManager(data);
            result = powerManager.FindBridges().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
            Console.WriteLine(result);
        }
        public class WeatherMachinePowerManager {
            internal Dictionary<string, Component> Components = new Dictionary<string, Component>();
            public List<(string a, string b)> connections = new List<(string a, string b)>();
            public WeatherMachinePowerManager(string[] data) {
                foreach (string line in data) {
                    string[] strings = line.Split(':');
                    string[] strings1 = strings[1].Split(' ');
                    foreach (string s in strings1.Skip(1)) {
                        connections.Add((strings[0], s));
                    }
                    new Component(line, Components);
                }
            }
            public int FindBridges() {
                Dictionary<string, int> TimesVisited = new Dictionary<string, int>();
                //check 1000 random pairs of components and update the TimesVisited dictionary
                Random random = new Random();
                for (int i = 0; i < 1000; i++) {
                    string start = Components.ElementAt(random.Next(Components.Count)).Key;
                    string end = Components.ElementAt(random.Next(Components.Count)).Key;
                    FindPathBetweenTwoNodesAndMarkEachPointInPathVisited(start, end, TimesVisited);
                }
                //we need to find the top three that are not connected to each other
                List<string> top3 = new List<string>();
                foreach (string key in TimesVisited.Keys.OrderByDescending(x => TimesVisited[x])) {
                    if (top3.Count == 3) {
                        break;
                    }
                    bool connected = false;
                    foreach (string node in top3) {
                        if (Components[key].Connections.Select(x => x.ID).Contains(node)) {
                            connected = true;
                            break;
                        }
                    }
                    if (!connected) {
                        top3.Add(key);
                    }
                }
                //of these nodes there should be three connections that split the graph into two parts, find these by creating a dsu and unioning all the nodes minus three
                var first = Components[top3[0]];
                var second = Components[top3[1]];
                var third = Components[top3[2]];
                foreach (string key in first.Connections.Select(x => x.ID)) {
                    (string a, string b) cona1 = (key, top3[0]);
                    (string a, string b) cona2 = (top3[0], key);
                    foreach (string key2 in second.Connections.Select(x => x.ID)) {
                        (string a, string b) conb1 = (key2, top3[1]);
                        (string a, string b) conb2 = (top3[1], key2);
                        foreach (string key3 in third.Connections.Select(x => x.ID)) {
                            (string a, string b) conc1 = (key3, top3[2]);
                            (string a, string b) conc2 = (top3[2], key3);
                            if (key == key2 || key == key3 || key2 == key3) {
                                continue;
                            }
                            DSU dsu = new DSU(Components);
                            foreach ((string a, string b) connection in connections) {
                                if (connection == cona1 || connection == cona2 || connection == conb1 || connection == conb2 || connection == conc1 || connection == conc2) {
                                    continue;
                                }
                                dsu.Union(connection.a, connection.b);
                            }

                            List<int> groupSizes = dsu.GetGroupSizes();
                            if (groupSizes.Count == 2) {
                                return groupSizes[0] * groupSizes[1];
                            }
                        }
                    }
                }
                return -1;
            }
            public void FindPathBetweenTwoNodesAndMarkEachPointInPathVisited(string start, string end, Dictionary<string, int> TimesVisited) {
                Queue<(string, List<string>)> queue = new Queue<(string, List<string>)>();
                queue.Enqueue((start, new List<string>()));
                while (queue.Count > 0) {
                    (string current, List<string> path) = queue.Dequeue();
                    if (current == end) {
                        foreach (string node in path) {
                            if (!TimesVisited.ContainsKey(node)) {
                                TimesVisited.Add(node, 0);
                            }
                            TimesVisited[node]++;
                        }
                        return;
                    }
                    foreach (Component connection in Components[current].Connections) {
                        if (!path.Contains(connection.ID)) {
                            List<string> newPath = new List<string>(path);
                            newPath.Add(connection.ID);
                            queue.Enqueue((connection.ID, newPath));
                        }
                    }
                }

            }
            internal class Component {
                internal string ID;
                internal List<Component> Connections = new List<Component>();
                public Component(string name) { ID = name; }
                public Component(string data, Dictionary<string, Component> components) {
                    string[] split = data.Split(':');
                    ID = split[0];
                    if (!components.ContainsKey(ID)) {
                        components.Add(ID, this);
                    }
                    foreach (string connection in split[1].Split(' ').Skip(1)) {
                        if (!components.ContainsKey(connection)) {
                            Component newComponent = new Component(connection);
                            components.Add(connection, newComponent);
                        }
                        components[ID].Connections.Add(components[connection]);
                        components[connection].Connections.Add(components[ID]);
                    }
                }
                public override string ToString() {
                    return $"{ID}: {Connections.Count}";
                }
            }
            internal class DSU {
                internal Dictionary<string, string> Parent = new Dictionary<string, string>();
                internal Dictionary<string, int> Rank = new Dictionary<string, int>();
                public DSU(Dictionary<string, Component> data) {
                    foreach (string key in data.Keys) {
                        Parent.Add(key, key);
                        Rank.Add(key, 0);
                    }
                }
                public string Find(string x) {
                    if (Parent[x] != x) {
                        Parent[x] = Find(Parent[x]);
                    }
                    return Parent[x];
                }
                public void Union(string x, string y) {
                    string xRoot = Find(x);
                    string yRoot = Find(y);
                    if (xRoot == yRoot) {
                        return;
                    }
                    if (Rank[xRoot] < Rank[yRoot]) {
                        Parent[xRoot] = yRoot;
                    }
                    else if (Rank[xRoot] > Rank[yRoot]) {
                        Parent[yRoot] = xRoot;
                    }
                    else {
                        Parent[yRoot] = xRoot;
                        Rank[xRoot]++;
                    }
                }
                public List<int> GetGroupSizes() {
                    Dictionary<string, int> groupSizes = new Dictionary<string, int>();
                    foreach (string key in Parent.Keys) {
                        string root = Find(key);
                        if (!groupSizes.ContainsKey(root)) {
                            groupSizes.Add(root, 0);
                        }
                        groupSizes[root]++;
                    }
                    return groupSizes.Values.ToList();
                }
            }
        }

    }
}