
namespace AdventOfCode.Year2023
{
    public class DayXX : Day
    {
        public DayXX(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "d";
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
            Console.WriteLine(result);
        }
    }

}