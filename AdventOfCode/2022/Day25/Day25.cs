namespace AdventOfCode.Year2022 {
    public class Day25 : Day {

        public Day25(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            FuelHeatingMachine FHM = new(data);
            result = FHM.FuelRequirementSnafu;
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
            Console.WriteLine(result);
        }
        /// <summary>
        /// this class is bullshit
        /// </summary>
        public class FuelHeatingMachine {
            public long FuelRequirement { get; set; }
            public string FuelRequirementSnafu => SNAFU.ConvertFromInt(FuelRequirement);
            public FuelHeatingMachine(string[] data) {
                foreach (string line in data) {
                    FuelRequirement += SNAFU.ConvertFromSnafu(line);
                }
            }
        }
        public static class SNAFU {
            public static long ConvertFromSnafu(string value) {
                long result = 0;
                for (int i = 0; i < value.Length; i++) {
                    result += ConvertToBase5(ConvertFromSymbol(value[i]), value.Length - i);
                }
                return result;
            }
            public static int ConvertFromSymbol(char value) {
                switch (value) {
                    case '-':
                        return -1;
                    case '=':
                        return -2;
                    case '0':
                        return 0;
                    case '1':
                        return 1;
                    case '2':
                        return 2;
                    default:
                        throw new Exception("Invalid symbol");
                }
            }
            public static long ConvertToBase5(int value, int place) {
                long factor = (long)Math.Pow(5, place - 1);
                return value * factor;
            }
            public static string ConvertFromInt(long value) {
                List<char> arr = new();
                while (value > 0) {
                    switch (value % 5) {
                        case 3:
                            arr.Add('=');
                            value += 5;
                            break;
                        case 4:
                            arr.Add('-');
                            value += 5;
                            break;
                        default:
                            arr.Add((char)((value % 5) + '0'));
                            break;
                    }
                    value /= 5;
                }
                arr.Reverse();
                return new string(arr.ToArray());
            }
        }
    }

}