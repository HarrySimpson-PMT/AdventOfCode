
using System.Xml.Serialization;

namespace AdventOfCode.Year2023
{
    public class Day11 : Day
    {
        public Day11(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfAstronomy elfAstronomy = new ElfAstronomy(data);
            elfAstronomy.ExpandXSpace(1).RemapSpace();
            result = elfAstronomy.SumOfAllPairs().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfAstronomy elfAstronomy = new ElfAstronomy(data);
            elfAstronomy.ExpandXSpace(999999).RemapSpace();
            result = elfAstronomy.SumOfAllPairs().ToString();
            Console.WriteLine(result);
        }
        public class ElfAstronomy
        { 
            List<Star> Stars = new List<Star>();
            Dictionary<long, List<Star>> XStars = new Dictionary<long, List<Star>>();
            Dictionary<long, List<Star>> YStars = new Dictionary<long, List<Star>>();
            long _XSize = 0;
            long _YSize = 0;
            public ElfAstronomy(string[] data)
            {
                _XSize = data[0].Length;
                _YSize = data.Length;
                for(int i = 0; i < data.Length; i++)
                {
                    for(int j = 0; j< data[i].Length; j++)
                    {
                        if (data[i][j] == '#')
                        {
                            if (!XStars.ContainsKey(i))
                            {
                                XStars.Add(i, new List<Star>());
                            }
                            if (!YStars.ContainsKey(j))
                            {
                                YStars.Add(j, new List<Star>());
                            }
                            Star star = new Star() { X = i, Y = j };
                            XStars[i].Add(star);
                            YStars[j].Add(star);
                            Stars.Add(star);
                        }
                    }
                }
            }
            public ElfAstronomy ExpandXSpace(int expansion)
            {
                long currentExpansion = 0;
                for(int i = 0; i<_XSize; i++)
                {
                    if(!XStars.ContainsKey(i))
                    {
                        currentExpansion += expansion;
                    }
                    else
                    {
                        foreach(Star star in XStars[i])
                        {
                            star.X += currentExpansion;
                        }
                    }
                }
                currentExpansion = 0;
                for (int i = 0; i < _YSize; i++)
                {
                    if(!YStars.ContainsKey(i))
                    {
                        currentExpansion += expansion;
                    }
                    else
                    {
                        foreach (Star star in YStars[i])
                        {
                            star.Y += currentExpansion;
                        }
                    }
                }
                return this;
            }
            public void print()
            {
                for(int i = 0; i < _XSize; i++)
                {
                    for(int j = 0; j < _YSize; j++)
                    {
                        if (XStars.ContainsKey(i) && XStars[i].Any(x => x.Y == j))
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
            }
            public ElfAstronomy RemapSpace()
            {
                XStars = new Dictionary<long, List<Star>>();
                YStars = new Dictionary<long, List<Star>>();
                foreach(Star star in Stars)
                {
                    if (!XStars.ContainsKey(star.X))
                    {
                        XStars.Add(star.X, new List<Star>());
                    }
                    if (!YStars.ContainsKey(star.Y))
                    {
                        YStars.Add(star.Y, new List<Star>());
                    }
                    XStars[star.X].Add(star);
                    YStars[star.Y].Add(star);
                    if(star.X> _XSize)
                    {
                        _XSize = star.X+1;
                    }
                    if(star.Y > _YSize)
                    {
                        _YSize = star.Y+1;
                    }
                }
                return this;
            }
            public long SumOfAllPairs()
            {
                long sum = 0;
                for(int i = 0; i < Stars.Count; i++)
                {
                    for(int j = i+1; j < Stars.Count; j++)
                    {
                        sum += ManhatteanDistance(Stars[i], Stars[j]);
                    }
                }
                return sum;
            }
            long ManhatteanDistance(Star star1, Star star2)
            {
                return Math.Abs(star1.X - star2.X) + Math.Abs(star1.Y - star2.Y);
            }
            class Star
            {
                public long X { get; set; }
                public long Y { get; set; }
            }
        }
    }
}