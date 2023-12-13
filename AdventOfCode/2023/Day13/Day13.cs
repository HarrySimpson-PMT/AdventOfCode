
namespace AdventOfCode.Year2023
{
    public class Day13 : Day
    {
        public Day13(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MirrorMadness mm = new MirrorMadness(data);
            result = mm.result.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MirrorMadness mm = new MirrorMadness(data, true);
            result = mm.result.ToString();
            Console.WriteLine(result);
        }
        public class MirrorMadness
        {
            public long result { get; set; } = 0;
            public MirrorMadness(string[] pattern, bool fix = false)
            {
                //split pattern by empty lines
                List<string[]> patterns = new List<string[]>();
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (pattern[i] == "")
                    {
                        patterns.Add(pattern.Take(i).ToArray());
                        pattern = pattern.Skip(i + 1).ToArray();
                        i = 0;
                    }
                }
                patterns.Add(pattern);
                foreach (string[] p in patterns)
                {
                    if (fix)
                        result += FixedPatternDivisionsPoint(p);
                    else
                        result += PatternDivisionsPoint(p);
                }
            }
            public int FixedPatternDivisionsPoint(string[] dpattern)
            {
                char[][] pattern = dpattern.Select(x => x.ToCharArray()).ToArray();
                for (int x = 0; x < pattern.Length; x++)
                {
                    for (int y = 0; y < pattern[x].Length; y++)
                    {
                        pattern[x][y] = pattern[x][y] == '#' ? '.' : '#';
                        string xLine = new string(pattern[0]);
                        for (int i = 1; i < pattern.Length; i++)
                        {
                            if (new string(pattern[i]) == xLine)
                            {
                                //check that the folding rangeinclude the line that was flipped at x
                                int dist = Math.Min(i, pattern.Length - i);
                                if (x <= i - 1- dist || x >= i + dist)
                                    continue;

                                int top = i - 1, bottom = i;
                                while (top >= 0 && bottom < pattern.Length && new string(pattern[top]) == new string(pattern[bottom]))
                                {
                                    top--;
                                    bottom++;
                                }
                                if (top == -1 || bottom == pattern.Length)
                                    return i * 100;
                            }
                            xLine = new string(pattern[i]);
                        }
                        string yLine = new string(pattern.Select(x => x[0]).ToArray());
                        for (int i = 1; i < pattern[0].Length; i++)
                        {
                            if (new string(pattern.Select(x => x[i]).ToArray()) == yLine)
                            {
                                //check that the folding rangeinclude the line that was flipped at y
                                int dist = Math.Min(i, pattern[0].Length - i);
                                if (y <= i - 1 - dist || y >= i + dist)
                                    continue;

                                int left = i - 1, right = i;
                                while (left >= 0 && right < pattern[0].Length && new string(pattern.Select(x => x[left]).ToArray()) == new string(pattern.Select(x => x[right]).ToArray()))
                                {
                                    left--;
                                    right++;
                                }
                                if (left == -1 || right == pattern[0].Length)
                                    return i;

                            }
                            yLine = new string(pattern.Select(x => x[i]).ToArray());
                        }
                        pattern[x][y] = pattern[x][y] == '#' ? '.' : '#';
                    }
                }
                return 0;

            }
            public int PatternDivisionsPoint(string[] dpattern)
            {
                char[][] pattern = dpattern.Select(x => x.ToCharArray()).ToArray();
                string xLine = new string(pattern[0]);
                for (int i = 1; i < pattern.Length; i++)
                {
                    if (new string(pattern[i]) == xLine)
                    {
                        int top = i - 1, bottom = i;
                        while (top >= 0 && bottom < pattern.Length && new string(pattern[top]) == new string(pattern[bottom]))
                        {
                            top--;
                            bottom++;
                        }
                        if (top == -1 || bottom == pattern.Length)
                            return i * 100;
                    }
                    xLine = new string(pattern[i]);
                }
                string yLine = new string(pattern.Select(x => x[0]).ToArray());
                for (int i = 1; i < pattern[0].Length; i++)
                {
                    if (new string(pattern.Select(x => x[i]).ToArray()) == yLine)
                    {
                        int left = i - 1, right = i;
                        while (left >= 0 && right < pattern[0].Length && new string(pattern.Select(x => x[left]).ToArray()) == new string(pattern.Select(x => x[right]).ToArray()))
                        {
                            left--;
                            right++;
                        }
                        if (left == -1 || right == pattern[0].Length)
                            return i;

                    }
                    yLine = new string(pattern.Select(x => x[i]).ToArray());
                }
                return 0;

            }
        }
    }
}