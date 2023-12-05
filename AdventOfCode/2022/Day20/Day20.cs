namespace AdventOfCode.Year2022
{
    public class Day20 : Day
    {

        public Day20(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Cypher cypher = new(data);
            cypher.ReorderNodes();
            List<long> res = cypher.GetAtCyclesFromZero();
            result = res.Sum().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Cypher cypher = new(data, 811589153);
            cypher.ReorderNodes(10);
            List<long> res = cypher.GetAtCyclesFromZero();
            result = res.Sum().ToString();
            Console.WriteLine(result);
        }
        public class Cypher
        {
            public List<Node> ReorderedNodes { get; set; }
            public List<Node> OriginalPositionNodes { get; set; }
            public Node Zero { get; set; }
            public Cypher(string[] data, long factor = 1)
            {
                OriginalPositionNodes = new List<Node>();
                ReorderedNodes = new List<Node>();
                foreach (string line in data)
                {
                    int value = int.Parse(line);
                    long newValue = value * factor;
                    while (newValue >= data.Length)
                    {
                        newValue -= ((newValue / (data.Length)) * (data.Length - 1));
                    }

                    while (newValue < -data.Length)
                    {
                        newValue += Math.Abs(((newValue / (data.Length)) * (data.Length - 1)));
                    }

                    Node node = new(Convert.ToInt32(newValue));
                    long sign = (value < 0 && factor % 2 == 0) ? -1 : 1;
                    node.OriginalValue = (value * factor) * sign;
                    OriginalPositionNodes.Add(node);
                    if (node.Value == 0)
                    {
                        Zero = node;
                    }
                }
                ReorderedNodes = OriginalPositionNodes.ToList();
            }
            public void ReorderNodes(int times = 1)
            {
                for (int i = 0; i < times; i++)
                {
                    foreach (Node node in OriginalPositionNodes)
                    {
                        int currentposition = ReorderedNodes.IndexOf(node);
                        int newposition = currentposition + node.Value;

                        while (newposition < 0)
                        {
                            newposition += OriginalPositionNodes.Count() - 1;
                        }
                        while (newposition >= OriginalPositionNodes.Count())
                        {
                            newposition -= OriginalPositionNodes.Count() - 1;
                        }

                        ReorderedNodes.Remove(node);
                        ReorderedNodes.Insert(newposition, node);
                    }
                }
            }
            public List<long> GetAtCyclesFromZero(bool original = false)
            {
                List<long> result = new();
                for (int i = 1000; i <= 3000; i += 1000)
                {

                    //get the position 1000 places to the right in the reorderdnode wrapping around if needed
                    int position = ReorderedNodes.IndexOf(Zero) + i;
                    while (position >= ReorderedNodes.Count())
                    {
                        position -= ReorderedNodes.Count();
                    }
                    result.Add(ReorderedNodes[position].OriginalValue);
                }
                return result;
            }

            public class Node
            {
                public override string ToString()
                {
                    return Value.ToString();
                }
                public Node(int value)
                {
                    Value = value;
                }
                public int Value { get; set; }
                public long OriginalValue { get; set; }
            }
        }
    }

}