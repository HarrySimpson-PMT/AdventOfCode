
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day20 : Day {
        public Day20(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfRelaySystem relaySystem = new ElfRelaySystem(data);
            result = relaySystem.Process(1000).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfRelaySystem relaySystem = new ElfRelaySystem(data);
            result = relaySystem.CalculateCycles().ToString();
            Console.WriteLine(result);
        }
        public class ElfRelaySystem {
            internal Dictionary<string, RelayModule> relayMap { get; set; } = new Dictionary<string, RelayModule>();
            public ElfRelaySystem(string[] data) {
                foreach (string line in data) {
                    RelayModule relay = new RelayModule(line, relayMap);
                }
            }
            public long CalculateCycles() {
                List<string> SectionComs = new() { "rl", "rd", "nn", "qb" };
                long result = long.MaxValue;
                List<long> sections = new List<long>();
                foreach (string com in SectionComs) {
                    Dictionary<string, List<long>> cycles = new();
                    Dictionary<string, bool> lastState = new();
                    HashSet<string> visited = new();
                    RelayModule start = null!;
                    Queue<RelayModule> queue = new Queue<RelayModule>();
                    try {

                        queue.Enqueue(relayMap[com]);
                    }
                    catch (Exception) {
                        return 0;
                    }
                    while (queue.Count > 0) {
                        RelayModule module = queue.Dequeue();
                        if (visited.Contains(module.name)) {
                            continue;
                        }
                        visited.Add(module.name);
                        cycles.Add(module.name, new());
                        lastState.Add(module.name, module.state);
                        foreach (string source in module.Memory.Keys) {
                            if (source == "broadcaster") {
                                start = module;
                                continue;
                            }
                            RelayModule sourceModule = relayMap[source];
                            queue.Enqueue(sourceModule);
                        }
                    }

                    Queue<(RelayModule module, bool signal, string caller)> toProcess = new Queue<(RelayModule module, bool signal, string caller)>();
                    long i = 0;
                    bool complete = false;
                    while (!complete) {
                        toProcess.Enqueue((start, false, ""));
                        while (toProcess.Count > 0) {
                            (RelayModule module, bool signal, string caller) = toProcess.Dequeue();
                            List<(string module, bool signal)> next = module.Update(caller, signal);
                            if (module.name == com && !module.state) {
                                sections.Add(i + 1);
                                complete = true;
                                break;
                            }
                            foreach (var item in next) {
                                toProcess.Enqueue((relayMap[item.module], item.signal, module.name));
                            }
                        }
                        i++;

                        //foreach (var item in cycles) {
                        //    if (lastState[item.Key] != relayMap[item.Key].state) {
                        //        if (relayMap[item.Key].state) {
                        //            item.Value.Add(i + 1);
                        //        }
                        //        lastState[item.Key] = relayMap[item.Key].state;
                        //    }
                        //}
                    }
                    continue;
                    //Console.WriteLine(com);
                    //List<(long cycle, List<long> starts)> startCycles = new();
                    //foreach (var item in cycles) {
                    //    if (item.Value.Count == 0) {
                    //        continue;
                    //    }
                    //    Console.WriteLine($"{item.Key} => {item.Value[0]}");
                    //    List<long> ints = new();
                    //    for (int i = 0; i < 20; i++) {
                    //        ints.Add(item.Value[i + 1] - item.Value[i]);
                    //    }
                    //    Console.WriteLine(string.Join(", ", ints));

                    //    List<long> repeating = new();
                    //    int size = 0;
                    //    for (int i = 0; i < ints.Count; i++) {
                    //        if (ints[i] != ints[0]) {
                    //            size = i;
                    //            break;
                    //        }
                    //    }
                    //    if (size == 0) {
                    //        startCycles.Add((ints[0], new List<long>() { item.Value[0] }));
                    //    }
                    //    else {
                    //        repeating = ints.Take(size).ToList();
                    //        long cycle = repeating.Sum();
                    //        long getstart = item.Value[0];
                    //        List<long> starts = new();
                    //        foreach (long rep in repeating) {
                    //            getstart += rep;
                    //            starts.Add(getstart);
                    //        }
                    //        startCycles.Add((cycle, starts));
                    //    }

                    //}
                    //long test = LCM(startCycles.Select(x => x.cycle).ToArray());
                    //result = Math.Min(result, test);
                    //Console.WriteLine();

                }
                return LCM(sections.ToArray());
                return result;
            }
            //lcm with offsets
            public long LCM(long[] stuff) {
                return stuff.Aggregate((a, i) => (a / GCF(a, i)) * i);
            }
            public long GCF(long a, long b) {
                while (b != 0) {
                    long temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }
            public long Process(int iterations) {
                Queue<(RelayModule module, bool signal, string caller)> toProcess = new Queue<(RelayModule module, bool signal, string caller)>();
                int low = 0;
                int high = 0;
                for (int i = 0; i < iterations; i++) {
                    toProcess.Enqueue((relayMap["broadcaster"], false, ""));
                    low++;
                    while (toProcess.Count > 0) {
                        (RelayModule module, bool signal, string caller) = toProcess.Dequeue();
                        List<(string module, bool signal)> next = module.Update(caller, signal);
                        foreach (var item in next) {
                            toProcess.Enqueue((relayMap[item.module], item.signal, module.name));
                            if (item.signal) {
                                high++;
                            }
                            else {
                                low++;
                            }
                        }
                    }
                }
                return (long)low * high;
            }
            internal class RelayModule { // push changes?
                public long FFCycle = 0;
                internal string name { get; set; }
                internal bool state { get; set; } = false;
                internal Dictionary<string, bool> Memory = new Dictionary<string, bool>();
                internal List<RelayModule> outputs { get; set; } = new List<RelayModule>();
                internal relayType type { get; set; }
                internal enum relayType { FlipFlop, Conjunction, Broadcaster, Output }
                public RelayModule(string name) {
                    this.name = name;
                }
                public RelayModule(string data, Dictionary<string, RelayModule> relayMap) {
                    //"%ph -> dj, rd"
                    //"&rl -> tk, lq, zg, vm, jb, sx, cl"
                    //"broadcaster -> a, b, c"
                    string[] parts = data.Split(" -> ");
                    switch (parts[0].Substring(0, 1)) {
                        case "%":
                            type = relayType.FlipFlop;
                            name = parts[0].Substring(1);
                            break;
                        case "&":
                            type = relayType.Conjunction;
                            name = parts[0].Substring(1);
                            break;
                        default:
                            type = relayType.Broadcaster;
                            name = parts[0];
                            break;
                    }
                    if (name == "output") {
                        type = relayType.Output;
                    }
                    if (!relayMap.ContainsKey(name)) {
                        relayMap.Add(name, this);
                    }
                    else {
                        relayMap[name].type = this.type;
                        //Memory = relayMap[name].Memory;
                        //relayMap[name] = this;
                    }
                    foreach (string output in parts[1].Split(", ")) {
                        if (!relayMap.ContainsKey(output)) {
                            relayMap.Add(output, new RelayModule(output));
                            relayMap[output].type = relayType.Output;
                        }
                        relayMap[name].outputs.Add(relayMap[output]);
                        relayMap[output].Memory.Add(name, false);
                    }
                }
                public List<(string module, bool signal)> Update(string relay, bool signalIN) {
                    List<(string module, bool signal)> signals = new List<(string module, bool signal)>();
                    bool output = false;
                    switch (type) {
                        case relayType.FlipFlop:
                            if (!signalIN) {
                                state = !state;
                            }
                            else {
                                return signals;
                            }
                            if (state) {
                                output = true;
                            }
                            else {
                                output = false;
                            }
                            break;
                        case relayType.Conjunction:
                            Memory[relay] = signalIN;
                            foreach (bool value in Memory.Values) {
                                if (!value) {
                                    output = true;
                                    break;
                                }
                            }
                            state = output;
                            break;
                        case relayType.Broadcaster:
                            output = signalIN;
                            break;
                        case relayType.Output:
                            return signals;

                    }
                    foreach (RelayModule module in outputs) {
                        signals.Add((module.name, output));
                    }

                    return signals;
                }
                public override string ToString() {
                    return $"{name} {type} {state}";
                }
            }

        }
    }
}

