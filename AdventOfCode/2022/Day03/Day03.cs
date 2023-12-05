using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Year2022
{
    public class Day03 : Day
    {

        public Day03(int today) : base(today)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Rucksacks rucksacks = new();
            rucksacks.FindMisplacedItems(data);
            result = rucksacks.shared.ConvertAll(x=>Rucksacks.ItemPriority(x)).Sum().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Rucksacks rucksacks = new();
            rucksacks.FindGroupBadges(data);
            result = rucksacks.shared.ConvertAll(x => Rucksacks.ItemPriority(x)).Sum().ToString();
            Console.WriteLine(result);
        }
        public class Rucksacks
        {
            public List<char> shared = new();
            public void FindMisplacedItems(string[] data)
            {
                foreach (string line in data)
                {
                    int split = line.Length/2;
                    HashSet<char> chars = new();
                    for (int i = 0; i < split; i++)
                        chars.Add(line[i]);
                    for (int i = split; i < line.Length; i++)
                    {
                        if (chars.Contains(line[i]))
                        {
                            shared.Add(line[i]);
                            break;
                        }
                    }
                }
            }
            public void FindGroupBadges(string[] data)
            {
                for (int i = 0; i < data.Length; i+=3)
                {
                    HashSet<char> first = new();
                    HashSet<char> second = new();
                    foreach (char item in data[i])
                        first.Add(item);
                    foreach(char item in data[i+1])
                    {
                        if (first.Contains(item))
                            second.Add(item);
                    }
                    foreach (char item in data[i + 2])
                        if (second.Contains(item))
                        {
                            shared.Add(item);
                            break;
                        }
                }
            }
            public static int ItemPriority(char item)
            {
                //a-z = 1-26
                //A-Z = 27-52
                int a = 'a';
                int A = 'A';
                int result = item - 'a' + 1;
                if (result < 1)
                    return item - 'A' + 27;
                return result;
            }
        }
    }

}