namespace AdventOfCode.Year2022
{
    public class Day01 : Day
    {

        public Day01(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfFoodSupply efs = new ElfFoodSupply(data);
            result = efs.Totals.Max().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfFoodSupply efs = new ElfFoodSupply(data);
            efs.Totals.Sort((a, b) => b.CompareTo(a));
            //take the sum of the top 3 
            result = efs.Totals.Take(3).Sum().ToString();
            Console.WriteLine(result);
        }
        public class ElfFoodSupply
        {
            public List<int> Totals { get; set; } = new();
            public ElfFoodSupply(string[] data)
            {
                int cur = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if(data[i]=="")
                    {
                        Totals.Add(cur);
                        cur = 0;
                        continue;
                    }
                    else
                    {
                        cur += int.Parse(data[i]);
                    }
                }
                Totals.Add(cur);
            }
        }
    }

}