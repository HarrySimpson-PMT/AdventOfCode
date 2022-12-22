// See https://aka.ms/new-console-template for more information
using AdventOfCode.Common;
using AdventOfCode.Year2022;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


var day = new Day21(21);

int x = 3;

switch (x)
{
    case 0:
        day.RunPart1(ArgumentType.Sample);
        break;
    case 1:
        day.RunPart1(ArgumentType.Full);
        break;
    case 2:
        day.RunPart2(ArgumentType.Sample);
        break;
    case 3:
        day.RunPart2(ArgumentType.Full);
        break;
}
