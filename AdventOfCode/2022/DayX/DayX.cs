using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public class DayX : Day
    {

        public DayX(int today) : base(today)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
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