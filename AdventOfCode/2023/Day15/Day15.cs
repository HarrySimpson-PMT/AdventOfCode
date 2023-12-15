
using System.Runtime.CompilerServices;

namespace AdventOfCode.Year2023
{
    public class Day15 : Day
    {
        public Day15(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = LavaInit.HashFunction(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            LavaInit lavaInit = new(data);
            result = lavaInit.RunInit().CalculateValue().ToString();
            Console.WriteLine(result);
        }
        public class LavaInit
        {
            protected List<string> _Data = new();
            protected List<LinkedList<Lens>> _Boxes = new();

            public LavaInit(string[] data)
            {
                _Data = data[0].Split(",").ToList();
                for(int i = 0; i < 256; i++)
                    _Boxes.Add(new LinkedList<Lens>());
            }
            public LavaInit RunInit()
            {
                foreach(string item in _Data)
                {
                    string[] strings = item.Split("=");
                    if (strings.Length == 1)
                        Remove(strings[0].Split('-')[0]);
                    else
                        Add(strings[0], int.Parse(strings[1]));
                }
                return this;
            }
            public long CalculateValue()
            {
                long result = 0;
                for(int i = 0; i < 256; i++)
                {
                    if (_Boxes[i].Count == 0)
                        continue;
                    for(int j = 0; j < _Boxes[i].Count; j++)
                    {
                        result += ((i+1) * (j+1) * _Boxes[i].ElementAt(j).Value);
                    }
                }
                return result;
            }
            public void Remove(string label)
            {
                int box = Hash(label);
                if (_Boxes[box].Any(x => x.Label == label))
                    _Boxes[box].Remove(_Boxes[box].First(x => x.Label == label));
            }
            public void Add(string label, int focus)
            {
                int box = Hash(label);
                if (_Boxes[box].Any(x => x.Label == label))
                    _Boxes[box].First(x => x.Label == label).Value = focus;
                else
                    _Boxes[box].AddLast(new Lens(label, focus));
            }
            public static int Hash(string input)
            {
                int result = 0;
                foreach (char c in input)
                {
                    result += c;
                    result = result * 17;
                    result = result % 256;
                }
                return result;
            }
            public class Lens
            {
                public string Label { get; set; }
                public int Value { get; set; }
                public override string ToString()
                {
                    return $"{Label}={Value}";
                }
                public Lens(string label, int value)
                {
                    Label = label;
                    Value = value;
                }
            }
            public static int HashFunction(string[] data)
            {
                int result = 0;
                foreach (string item in data[0].Split(',').ToList())
                    result += Hash(item);
                return result;
            }
        }
    }

}