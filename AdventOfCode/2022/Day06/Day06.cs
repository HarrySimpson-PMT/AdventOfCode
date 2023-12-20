namespace AdventOfCode.Year2022 {
    public class Day06 : Day {

        public Day06(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MalfunctioningScanner scanner = new MalfunctioningScanner();
            scanner.LocateStartSignal(data, 4);
            result = scanner.result.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MalfunctioningScanner scanner = new MalfunctioningScanner();
            scanner.LocateStartSignal(data, 14);
            result = scanner.result.ToString();
            Console.WriteLine(result);
        }
        public class MalfunctioningScanner {
            public int result { get; set; }
            public void LocateStartSignal(string[] data, int signalsize) {
                int[] cursignal = new int[26];
                int uniques = 0;
                string signal = data[0];
                for (int i = 0; i < signal.Length; i++) {
                    if (i >= signalsize) {
                        cursignal[signal[i - signalsize] - 'a']--;
                        if (cursignal[signal[i - signalsize] - 'a'] == 0) {
                            uniques--;
                        }
                    }
                    cursignal[signal[i] - 'a']++;
                    if (cursignal[signal[i] - 'a'] == 1) {
                        uniques++;
                    }
                    if (uniques == signalsize) {
                        result = i + 1;
                        return;
                    }
                }
            }

        }
    }

}