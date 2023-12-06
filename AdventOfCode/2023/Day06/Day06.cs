
namespace AdventOfCode.Year2023
{
    public class Day06 : Day
    {
        public Day06(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RaceCalculator raceCalculator = new RaceCalculator();
            result = raceCalculator.WaysToWin(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RaceCalculator raceCalculator = new RaceCalculator();
            result = raceCalculator.WaysToWin(data, true).ToString();
            Console.WriteLine(result);
        }
        public class RaceCalculator
        {
            public long WaysToWin(string[] data, bool correction = false)
            {
                List<long> Times = data[0].Split(":")[1].Trim().Split(" ").Where(x=>x!="").Select(x => long.Parse(x)).ToList();
                List<long> Distances = data[1].Split(":")[1].Trim().Split(" ").Where(x => x != "").Select(x => long.Parse(x)).ToList();
                if(correction)
                {
                    Times = new();
                    Times.Add(long.Parse(data[0].Split(":")[1].Trim().Replace(" ", "")));
                    Distances = new();
                    Distances.Add(long.Parse(data[1].Split(":")[1].Trim().Replace(" ", "")));
                }
                long n = Times.Count;
                long result = 1;
                long ways = 0;
                for (int i = 0; i < n; i++)
                {
                    bool win = false;
                    for (long speed = Times[i]; speed >=  0; speed--)
                    {
                        long distance = speed * (Times[i] - speed);
                        if (distance > Distances[i])
                        {
                            win = true;
                            ways++;
                        }
                        else if (win)
                        {
                            break;
                        }
                    }
                    result *= ways;
                    ways = 0;
                }
                return result;
            }
        }
    }

}