namespace AdventOfCode.Year2022 {
    public class Day10 : Day {

        public Day10(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            CRTCPU cpu = new CRTCPU();
            result = cpu.GetSignal(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            result = "";
            Console.WriteLine(result);
        }
        public class CRTCPU {
            int reg1 = 1;
            int cycle = 1;
            public void RenderImage(string[] data) {
                for (int i = 0; i <= 220; i++) {

                }
            }
            public int GetSignal(string[] data) {
                int result = 0;
                foreach (string dat in data) {
                    if (cycle > 220)
                        return result;
                    string[] com = dat.Split(" ");
                    if (com[0] == "noop") {
                        cycle++;
                        if ((cycle + 20) % 40 == 0) {
                            result += (reg1 * cycle);
                        }
                    }
                    else {
                        if ((cycle + 21) % 40 == 0) {
                            result += (reg1 * (cycle + 1));
                        }
                        cycle += 2;
                        reg1 += int.Parse(com[1]);
                        if ((cycle + 20) % 40 == 0) {
                            result += (reg1 * cycle);
                        }
                    }
                }
                return result;
            }



        }
    }

}