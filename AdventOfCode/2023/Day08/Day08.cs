
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Year2023
{
    public class Day08 : Day
    {
        public Day08(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = new DesertMap(data).Navigate().steps.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = new DesertMap(data).GhostNavigation().steps.ToString();
            Console.WriteLine(result);
        }
        public class DesertMap
        {
            private Dictionary<string, Node> _Map { get; set; } = new Dictionary<string, Node>();
            public List<string> Starts = new();
            private readonly string _Instructions;
            public long steps { get; set; } = 0;
            public DesertMap(string[] data)
            {
                _Instructions = data[0].Trim();
                foreach (string dataline in data.Skip(2))
                {
                    string ID = dataline.Split("=")[0].Trim();
                    if (ID[2] == 'A')
                        Starts.Add(ID);
                    if (!_Map.ContainsKey(ID))
                        _Map.Add(ID, new Node(ID));
                    string left = dataline.Split("=")[1].Trim().Split(",")[0].Trim().Replace("(", "");
                    string right = dataline.Split("=")[1].Trim().Split(",")[1].Trim().Replace(")", "");
                    if (!_Map.ContainsKey(left))
                        _Map.Add(left, new Node(left));
                    _Map[ID].Left = _Map[left];
                    if (!_Map.ContainsKey(right))
                        _Map.Add(right, new Node(right));
                    _Map[ID].Right = _Map[right];
                }
            }
            public DesertMap GhostNavigation()
            {
                List<long> cycles = new List<long>();
                List<long> cyclestart = new List<long>();
                for (int i = 0; i < Starts.Count; i++)
                {
                    long localsteps = 0;
                    Dictionary<string, long> map = new Dictionary<string, long>();
                    Node cur = _Map[Starts[i]];
                    string key = $"0:{cur.ID}";
                    while (!map.ContainsKey(key))
                    {
                        map.Add(key, localsteps);
                        switch (_Instructions[(int)(steps % _Instructions.Length)])
                        {
                            case 'R':
                                cur = cur.Right;
                                break;
                            case 'L':
                                cur = cur.Left;
                                break;
                        }
                        localsteps++;
                        string instruction = ((int)(localsteps % _Instructions.Length)).ToString();
                        key = $"{instruction}:{cur.ID}";
                    }
                    cycles.Add(localsteps - map[key]);
                    cyclestart.Add(map[key]);
                }
                int factor = 1;
                long max = cycles.Max();
                while (true)
                {
                    long check = max*factor;
                    bool result = true;
                    foreach(long x in cycles)
                    {
                        if(check%x!=0)
                            result = false;
                    }
                    if (result)
                        break;
                    factor++;
                }
                steps = max * factor;
                return this;
            }

            public DesertMap Navigate()
            {
                //navigate "AAA" to "ZZZ" using Instructions;
                Node cur = _Map["AAA"];
                while (cur.ID != "ZZZ")
                {
                    switch (_Instructions[(int)(steps % _Instructions.Length)])
                    {
                        case 'R':
                            cur = cur.Right;
                            steps++;
                            break;
                        case 'L':
                            cur = cur.Left;
                            steps++;
                            break;
                    }
                }

                return this;
            }
            private class Node
            {
                public string ID { get; }
                public Node Left { get; set; } = null!;
                public Node Right { get; set; } = null!;
                public Node(string ID) { this.ID = ID; }
                public override string ToString()
                {
                    return $"{ID} L: {Left?.ID ?? "NULL"} R: {Right?.ID ?? "NULL"}";
                }
            }
        }

    }
}


