using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public static class Directions
    {
        public static (int x, int y) North = (-1, 0);
        public static (int x, int y) South = (1, 0);
        public static (int x, int y) East = (0, 1);
        public static (int x, int y) West = (0, -1);
        public static (int x, int y) Invert((int x, int y) direction)
        {
            return (direction.x * -1, direction.y * -1);
        }
    }
}
