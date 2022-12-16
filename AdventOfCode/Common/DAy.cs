using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public abstract class Day
    {
        public string[] Sample;
        public string[] Full;
        public string result = "";
        public string day = "";

        public Day(int today)
        {
            day = today.ToString();
            Sample = System.IO.File.ReadAllLines($@"2022/Day{day}/Sample.txt");
            Full = System.IO.File.ReadAllLines($@"2022/Day{day}/Full.txt");
        }
        public abstract void RunPart1(ArgumentType argumentType);
        public abstract void RunPart2(ArgumentType argumentType);


    }
}
