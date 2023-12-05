
using System.Diagnostics;

namespace AdventOfCode.Year2023
{
    public class Day05 : Day
    {
        public Day05(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            AlmanacFromHell almanac = new();
            result = almanac.SmallestLocation(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            AlmanacFromHell almanac = new();
            result = almanac.SmallestLocation(data, true).ToString();
            Console.WriteLine(result);
        }
        public class AlmanacFromHell
        {
            public long SmallestLocation(string[] data, bool TLE = false)
            {//dest, source, range;
                List<List<long[]>> Maps = new();
                //we need to split data by blank lines
                List<string[]> splitData = new();
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == "")
                    {
                        splitData.Add(data.Take(i).ToArray());
                        data = data.Skip(i + 1).ToArray();
                        i = 0;
                    }
                }
                splitData.Add(data.ToArray());
                List<long> Seeds = splitData[0][0].Split(':')[1].Trim().Split(' ').Select(x => long.Parse(x)).ToList();
                for (int i = 1; i < splitData.Count; i++)
                {
                    List<long[]> map = new();
                    for (long j = 1; j < splitData[i].Length; j++)
                    {
                        map.Add(splitData[i][j].Split(' ').Select(x => long.Parse(x)).ToArray());
                    }
                    Maps.Add(map);
                }

                long result = long.MaxValue;
                List<long> locations = new();
                //foreach seed we need to translate through each map to detemrine the location and then see if it is the lowest possible
                List<List<long>> MapRanges = new();
                for (int i = 0; i < Maps.Count; i++)
                {
                    MapRanges.Add(new());
                    foreach (long[] mapset in Maps[i])
                    {
                        MapRanges[i].Add(mapset[1]);
                        MapRanges[i].Add(mapset[1] + mapset[2]);                    
                    }
                    MapRanges[i].Sort();
                }
                
                for(int i = 0; i < Seeds.Count; i++)
                {
                    if(TLE)
                    {
                        for (long j = Seeds[i]; j <= Seeds[i]+Seeds[i+1]; j++)
                        {                                                       
                            long skipwindow = long.MaxValue;
                            long cur = j;
                            for (int k = 0; k < Maps.Count; k++)                            
                            {
                                List<long[]> map = Maps[k];
                                //find the next greatest value in MapRanges[k] after cur and update skip window to min
                                int rangechange = MapRanges[k].BinarySearch(cur);
                                if(rangechange < 0)
                                    rangechange = ~rangechange;
                                if (rangechange < MapRanges[k].Count)
                                    skipwindow = Math.Min(skipwindow, MapRanges[k][rangechange] - cur);

       
                                foreach (long[] mapset in map)
                                {
                                    if (cur >= mapset[1] && cur < mapset[1] + mapset[2])
                                    {
                                        cur = mapset[0] + (cur - mapset[1]);
                                        break;
                                    }
                                }
                            }                           
                            result = Math.Min(result, cur);
                            locations.Add(cur);

                            if (skipwindow == long.MaxValue)
                                break;
                            if(skipwindow - 1>0)
                                j += skipwindow-1;
                        }
                       i++;
                    }
                    else
                    {
                        long cur = Seeds[i];
                        foreach (List<long[]> map in Maps)
                        {
                            foreach (long[] mapset in map)
                            {
                                if (cur >= mapset[1] && cur < mapset[1] + mapset[2])
                                {
                                    cur = mapset[0] + (cur - mapset[1]);
                                    break;
                                }
                            }
                        }
                        result = Math.Min(result, cur);
                        locations.Add(cur);
                    }
                }
                locations.Sort();
                return result;
            }           
        }
    }

}