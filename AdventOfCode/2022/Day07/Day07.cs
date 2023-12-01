namespace AdventOfCode.Year2022
{
    public class Day07 : Day
    {

        public Day07(int today) : base(today)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ScannerFileSystem scannerFileSystem = new();
            scannerFileSystem.BuildFileSystem(data);
            scannerFileSystem.CalculateDirectorySizes();
            result = scannerFileSystem.Result.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ScannerFileSystem scannerFileSystem = new();
            scannerFileSystem.BuildFileSystem(data);
            scannerFileSystem.CalculateDirectorySizes();
            result = scannerFileSystem.FindSmallestSufficientDirectory().ToString();
            Console.WriteLine(result);
        }
        public class ScannerFileSystem
        {
            List<int> _dirSizes = new();
            public Node root = new Node { Name = "/", SubNodes = new List<Node>(), Files = new List<(string, int)>() };
            public int Result = 0;
            public void BuildFileSystem(string[] data)
            {
                Node cur = root;
                for(int i = 1; i < data.Length; i++)
                {
                    string[] command = data[i].Split(" ");
                    switch(command[1])
                    {
                        case "cd":
                            string dir = command[2];
                            if (dir == "..")
                                cur = cur.Parent;
                            else
                                cur = cur.SubNodes.Find(x => x.Name == dir);
                            break;
                        case "ls":
                            while (i + 1 < data.Length)
                            {
                                if (data[i + 1][0]!='$')
                                {
                                    string[] nextcommand = data[i + 1].Split(" ");
                                    if (nextcommand[0]=="dir")
                                    {
                                        cur.SubNodes.Add(new Node { Name = nextcommand[1], Parent = cur });
                                    }
                                    else
                                    {
                                        cur.Files.Add((nextcommand[1], int.Parse(nextcommand[0])));
                                    }
                                    i++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            public int FindSmallestSufficientDirectory()
            {
                int total = 70000000;
                int req = 30000000;
                int cur = 70000000 - root.NodeSize;
                int find = req - cur;
                _dirSizes.Sort();
                for(int i = 0; i < _dirSizes.Count; i++)
                {
                    if (_dirSizes[i] >= find)
                        return _dirSizes[i];

                }
                throw new InvalidDataException("No sufficient directory found");
            }
            public void CalculateDirectorySizes()
            {
                _calculateDirectorySizes(root);
            }
            public int _calculateDirectorySizes(Node cur)
            {
                int size = 0;
                foreach (Node node in cur.SubNodes)
                {
                    size += _calculateDirectorySizes(node);
                }
                foreach ((string, int) file in cur.Files)
                {
                    size += file.Item2;
                }
                cur.NodeSize = size;
                if (size <= 100000)
                    Result += size;
                _dirSizes.Add(size);
                return size;
            }
            public class Node
            {
                public string Name { get; set; }
                public Node Parent { get; set; }
                public int NodeSize = 0;
                public override string ToString()
                {
                    return $"Name {Name} - Files: {Files.Count} - Dirs: {SubNodes.Count()}";
                }
                public List<Node> SubNodes { get; set; } = new();
                public List<(string, int)> Files { get; set; } = new();
            }
        }
    }

}