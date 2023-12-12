
using System.Diagnostics;

namespace AdventOfCode.Year2023
{
    public class Day12 : Day
    {
        public Day12(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            SpringChart springChart = new SpringChart(data);
            result = springChart.CountValidPossibilities().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            SpringChart springChart = new SpringChart(data, 5);
            result = springChart.CountValidPossibilities().ToString();
            Console.WriteLine(result);
        }
        public class SpringChart
        {
            List<LineItem> lineItems = new List<LineItem>();
            long possibleValidSprings = 0;
            public SpringChart(string[] data, int mul = 1)
            {
                lineItems = data.Select(x => new LineItem(x, mul)).ToList();
            }
            public long CountValidPossibilities()
            {
                for(int idx = 0; idx < lineItems.Count; idx++)
                {
                    LineItem lineItem = lineItems[idx];
                    long[,] dp = new long[lineItem.data.Length, lineItem.verifiedSprings.Count];
                    for (int i = 0; i < lineItem.data.Length; i++)
                        for (int j = 0; j < lineItem.verifiedSprings.Count; j++)
                            dp[i, j] = -1;
                    long count = CountValidPossibilitesDP(dp, 0, 0, lineItem);
                    if(count < 0)
                    {
                        Debugger.Break();
                    }
                    possibleValidSprings += count;
                }
                return possibleValidSprings;
            }
            long CountValidPossibilitesDP(long[,] dp, int idx, int idy, LineItem item)
            {
                long result = 0;
                if (idx >= item.data.Length && idy == item.verifiedSprings.Count)
                {
                    return 1;
                }
                if (idx >= item.data.Length)
                    return 0;
                if (idy >= item.verifiedSprings.Count)
                {
                    bool check = true;
                    for (int i = idx; i < item.data.Length; i++)
                    {
                        if (item.data[i] == '#')
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        return 1;
                    }
                    return 0;
                }
                if (idx + item.verifiedSprings[idy] > item.data.Length)
                    return 0;
                if (dp[idx, idy] != -1)
                {
                    return dp[idx, idy];
                }
                bool valid = true;
                for (int i = idx; i < idx + item.verifiedSprings[idy]; i++)
                {
                    if (item.data[i] == '.')
                    {
                        valid = false;
                        break;
                    }
                }
                if (idx + item.verifiedSprings[idy] < item.data.Length && item.data[idx + item.verifiedSprings[idy]] == '#')
                    valid = false;

                if (valid)
                    result += CountValidPossibilitesDP(dp, idx + item.verifiedSprings[idy] + 1, idy + 1, item);
                if (item.data[idx] != '#')
                    result += CountValidPossibilitesDP(dp, idx + 1, idy, item);

                return dp[idx, idy] = result;
            }
            protected class LineItem
            {
                public string data { get; } = "";
                public List<int> verifiedSprings { get; } = new List<int>();
                public LineItem(string line, int multiplies = 1)
                {
                    data = line.Split(" ")[0];
                    verifiedSprings = line.Split(" ")[1].Split(",").Select(x => int.Parse(x)).ToList();
                    if(multiplies > 1)
                    {
                        string temp = "?"+data;
                        for(int i = 1; i < multiplies; i++)
                        {
                            data += temp;
                        }
                        List<int> temp2 = verifiedSprings.ToList();
                        for(int i = 1; i < multiplies; i++)
                        {
                            verifiedSprings.AddRange(temp2);
                        }
                    }
                }
            }
        }
    }

}