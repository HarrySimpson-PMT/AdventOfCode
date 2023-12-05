
using System.Data.SqlTypes;

namespace AdventOfCode.Year2023
{
    public class Day04 : Day
    {
        public Day04(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            EflGamblingFoolishness eflGamblingFoolishness = new();
            result = eflGamblingFoolishness.ScratchCardScore(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            EflGamblingFoolishness eflGamblingFoolishness = new();
            result = eflGamblingFoolishness.CorrectedScratchCardCount(data).ToString();
            Console.WriteLine(result);
        }
        public class EflGamblingFoolishness
        {
            public int CorrectedScratchCardCount(string[] data)
            {
                List<int> counts = new(data.Length);
                for (int x = 0; x < data.Length; x++)
                    counts.Add(1);
                int i = 0;
                while (i<counts.Count&&counts[i]>0)
                {
                    List<int> matchesets = new();
                    string line = data[i]; HashSet<int> winners = new();
                    string[] scores = line.Split(":")[1].Trim().Split("|");
                    foreach (string score in scores[0].Trim().Split(" "))
                    {
                        if (score == "")
                        {
                            continue;
                        }
                        winners.Add(int.Parse(score));
                    }
                    int matches = 0;
                    string[] test = scores[1].Trim().Split(" ");
                    foreach (string score in scores[1].Trim().Split(" "))
                    {
                        if (score == "")
                        {
                            continue;
                        }
                        if (winners.Contains(int.Parse(score)))
                        {
                            matches++;
                        }
                    }
                    for (int j = 1; j <= matches && j + i < counts.Count; j++)
                    {                        
                        counts[i + j]+=counts[i];
                    }
                    i++;
                }
                return counts.Sum();
            }
            public int ScratchCardScore(string[] data)
            {
                List<int> matchesets = new();
                matchesets.Add(1);
                for (int i = 0; i < 30; i++)
                {
                    matchesets.Add(matchesets[i] * 2);
                }
                int result = 0;
                foreach (string line in data)
                {
                    HashSet<int> winners = new();
                    string[] scores = line.Split(":")[1].Trim().Split("|");
                    foreach (string score in scores[0].Trim().Split(" "))
                    {
                        if (score == "")
                        {
                            continue;
                        }
                        winners.Add(int.Parse(score));
                    }
                    int matches = 0;
                    string[] test = scores[1].Trim().Split(" ");
                    foreach (string score in scores[1].Trim().Split(" "))
                    {
                        if (score == "")
                        {
                            continue;
                        }
                        if (winners.Contains(int.Parse(score)))
                        {
                            matches++;
                        }
                    }
                    if (matches > 0)
                        result += matchesets[matches-1];
                }
                return result;
            }
        }
    }

}