
using System.Diagnostics;

namespace AdventOfCode.Year2023
{
    public class Day09 : Day
    {
        public Day09(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            OasisSimulator simulator = new OasisSimulator(data);
            result = simulator.DataSequences.Sum(x => x.NextValue).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            OasisSimulator simulator = new OasisSimulator(data);
            result = simulator.DataSequences.Sum(x => x.PreviousValue).ToString();
            Console.WriteLine(result);
        }
        public class OasisSimulator
        {
            public List<DataSequence> DataSequences { get; set; } = new();
            public OasisSimulator(string[] data)
            {
                foreach(string line in data)
                {
                    DataSequences.Add(new DataSequence(line));
                }
            }
            public class DataSequence
            {
                protected List<List<int>> _Data = new();
                public long NextValue { get; set; }

                public long PreviousValue { get; set; }
                public DataSequence(string data)
                {
                    var test = data.Split(" ").Select(x => int.Parse(x)).ToList();
                    _Data.Add(data.Split(" ").Select(x => int.Parse(x)).ToList());
                    int idx = 0;
                    while (true)
                    {
                        List<int> change = new();
                        for (int i = 0; i < _Data[idx].Count-1; i++)
                        {
                            change.Add(_Data[idx][i+1] - _Data[idx][i]);
                        }
                        _Data.Add(change);
                        idx++;
                        bool AllZeros = true;
                        foreach(long item in _Data[idx])
                        {
                            if(item != 0)
                            {
                                AllZeros = false;
                                break;
                            }
                        }
                        if(AllZeros)
                        {
                            break;
                        }
                    }
                    _Data[idx].Add(0);
                    int findNext= 0;
                    int findPrevious = 0;
                    for(int i = _Data.Count-2; i >= 0; i--)
                    {
                        findNext = findNext + _Data[i][_Data[i].Count - 1];
                        findPrevious = _Data[i][0] - findPrevious;
                    }
                    NextValue = findNext;
                    PreviousValue = findPrevious;
                }
            }
        }
    }

}