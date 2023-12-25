namespace AdventOfCode.Year2022 {
    public class Day04 : Day {

        public Day04(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Camp camp = new Camp();
            camp.FindCompletelyOverlappingSets(data);
            result = camp.Overlap.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            Camp camp = new Camp();
            camp.FindOverLappingPairs(data);
            result = camp.Overlap.ToString();
            Console.WriteLine(result);
        }
        public class Camp {
            public int Overlap { get; set; } = 0;
            public void FindOverLappingPairs(string[] data) {
                for (int i = 0; i < data.Length; i++) {
                    string[] ranges = data[i].Split(',');
                    int FirstStart = int.Parse(ranges[0].Split('-')[0]);
                    int FirstEnd = int.Parse(ranges[0].Split('-')[1]);
                    int SecondStart = int.Parse(ranges[1].Split('-')[0]);
                    int SecondEnd = int.Parse(ranges[1].Split('-')[1]);
                    if (FirstStart <= SecondStart && FirstEnd >= SecondStart) {
                        Overlap++;
                    }
                    else if (SecondStart <= FirstStart && SecondEnd >= FirstStart) {
                        Overlap++;
                    }
                }
            }
            public void FindCompletelyOverlappingSets(string[] data) {
                for (int i = 0; i < data.Length; i++) {
                    string[] ranges = data[i].Split(',');
                    int FirstStart = int.Parse(ranges[0].Split('-')[0]);
                    int FirstEnd = int.Parse(ranges[0].Split('-')[1]);
                    int SecondStart = int.Parse(ranges[1].Split('-')[0]);
                    int SecondEnd = int.Parse(ranges[1].Split('-')[1]);
                    if (FirstStart <= SecondStart && FirstEnd >= SecondStart) {
                        Overlap++;
                    }
                    else if (FirstStart <= SecondEnd && FirstEnd >= SecondEnd) {
                        Overlap++;
                    }
                }
            }
        }
    }
}