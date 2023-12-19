
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day19 : Day {
        public Day19(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RatingSystem ratingSystem = new RatingSystem(data);
            ratingSystem.ProcessJunk();
            result = ratingSystem.SumOfJunk().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RatingSystem ratingSystem = new RatingSystem(data);
            result = ratingSystem.ProcessTheoreticalJunk().ToString();
            Console.WriteLine(result);
        }
        public class RatingSystem {
            Dictionary<string, ruleSet> rules;
            List<XmasJunk> toProcess = new List<XmasJunk>();
            List<XmasJunk> acceptedJunk = new List<XmasJunk>();
            List<XmasJunk> rejectedJunk = new List<XmasJunk>();
            public RatingSystem(string[] data) {
                rules = new Dictionary<string, ruleSet>();
                //parse rules until blank line
                int i = 0;
                while (data[i] != "") {
                    ruleSet r = new ruleSet(data[i]);
                    rules.Add(r.id, r);
                    i++;
                }
                //parse junk
                i++;
                while (i < data.Length) {
                    toProcess.Add(new XmasJunk(data[i]));
                    i++;
                }

            }
            public long ProcessTheoreticalJunk() {
                long result = 0;
                TheoreticalXmasJunk current = new TheoreticalXmasJunk();
                Queue<TheoreticalXmasJunk> ToProcess = new Queue<TheoreticalXmasJunk>();
                ToProcess.Enqueue(current);
                while (ToProcess.Count > 0) {
                    current = ToProcess.Dequeue();
                    List<TheoreticalXmasJunk>? newJunk = rules[current.NextRule].CalculateNewJunk(current);
                    if(newJunk == null) { Debugger.Break(); }
                    if (newJunk != null) {
                        foreach (TheoreticalXmasJunk t in newJunk) {
                            if (t.NextRule == "A") {
                                result += t.PossibleJunk();
                            }
                            else if (t.NextRule == "R") {
                                Debugger.Break();
                            }
                            else {
                                ToProcess.Enqueue(t);
                            }
                        }
                    }
                }
                return result;
            }
            public void ProcessJunk() {
                //process junk
                while (toProcess.Count > 0) {
                    XmasJunk xmasJunk = toProcess[0];
                    toProcess.RemoveAt(0);
                    string result = rules["in"].ProcessJunk(xmasJunk);
                    while (result != "A" && result != "R") {
                        result = rules[result].ProcessJunk(xmasJunk);
                    }
                    if (result == "A") {
                        acceptedJunk.Add(xmasJunk);
                    }
                    if (result == "R") {
                        rejectedJunk.Add(xmasJunk);
                    }
                }
            }
            public int SumOfJunk() {
                int result = 0;
                foreach (XmasJunk xmasJunk in acceptedJunk) {
                    result += xmasJunk.Sum();
                }
                return result;
            }
            class ruleSet {
                public string id;
                public List<rule> rules;
                public ruleSet(string line) {//ex = "px{a<2006:qkq,m>2090:A,rfg}"
                    string[] strings = line.Replace("}", "").Split('{');
                    id = strings[0];
                    rules = new List<rule>();
                    foreach (string s in strings[1].Split(',')) {
                        rules.Add(new rule(s));
                    }
                }
                public string ProcessJunk(XmasJunk xmasJunk) {
                    string result = "";
                    foreach (rule r in rules) {
                        result = r.Evaluate(xmasJunk);
                        if (result != "") {
                            break;
                        }
                    }
                    if (result == "") { Debugger.Break(); }
                    return result;
                }
                public List<TheoreticalXmasJunk>? CalculateNewJunk(TheoreticalXmasJunk junk) {
                    List<TheoreticalXmasJunk> result = new List<TheoreticalXmasJunk>();
                    TheoreticalXmasJunk ToProcess = junk;
                    if (!ToProcess.visited.Add(id)) {
                        return null;
                    }
                    for (int i = 0; i < rules.Count; i++) {
                        var cur = rules[i].NextJunk(ToProcess);
                        foreach (TheoreticalXmasJunk t in cur) {
                            if (t.NextRule != "") {
                                result.Add(t);
                            }
                            else {
                                ToProcess = t;
                            }
                        }
                    }
                    return result;
                }
            }
            class rule {
                string destination;
                bool noCheck;
                bool greaterThan;
                int value;
                enum type { x, m, a, s }
                type checkType;
                public rule(string rule) {// ex = "a<2006:qkq" , "m>2090:A" "rfg"
                    string[] split = rule.Split(':');
                    if (split.Length == 1) {
                        noCheck = true;
                        destination = split[0];
                    }
                    else {
                        string[] split2 = split[0].Split('<', '>');
                        if (split2[0] == "x") {
                            checkType = type.x;
                        }
                        else if (split2[0] == "m") {
                            checkType = type.m;
                        }
                        else if (split2[0] == "a") {
                            checkType = type.a;
                        }
                        else if (split2[0] == "s") {
                            checkType = type.s;
                        }
                        if (split[0][1] == '<') {
                            greaterThan = false;
                        }
                        else {
                            greaterThan = true;
                        }
                        value = int.Parse(split2[1]);
                        destination = split[1];
                    }
                }
                public List<TheoreticalXmasJunk> NextJunk(TheoreticalXmasJunk junk) {
                    List<TheoreticalXmasJunk> result = new List<TheoreticalXmasJunk>();
                    result.Add(junk.Clone());
                    if (noCheck) {
                        result[0].NextRule = destination;
                        return result;
                    }
                    result.Add(junk.Clone());
                    switch (checkType) {
                        case type.x:
                            if (greaterThan) {
                                result[0].xmin = value+1; //matches
                                result[0].NextRule = destination;
                                result[1].xmax = value; //fails
                                result[1].NextRule = "";
                            }
                            else {
                                result[0].xmax = value-1;
                                result[0].NextRule = destination;
                                result[1].xmin = value;
                                result[1].NextRule = "";
                            }
                            break;
                        case type.m:
                            if (greaterThan) {
                                result[0].mmin = value+1;
                                result[0].NextRule = destination;
                                result[1].mmax = value;
                                result[1].NextRule = "";
                            }
                            else {
                                result[0].mmax = value-1;
                                result[0].NextRule = destination;
                                result[1].mmin = value;
                                result[1].NextRule = "";
                            }
                            break;
                        case type.a:
                            if (greaterThan) {
                                result[0].amin = value+1;
                                result[0].NextRule = destination;
                                result[1].amax = value;
                                result[1].NextRule = "";
                            }
                            else {
                                result[0].amax = value-1;
                                result[0].NextRule = destination;
                                result[1].amin = value;
                                result[1].NextRule = "";
                            }
                            break;
                        case type.s:
                            if (greaterThan) {
                                result[0].smin = value+1;
                                result[0].NextRule = destination;
                                result[1].smax = value;
                                result[1].NextRule = "";
                            }
                            else {
                                result[0].smax = value-1;
                                result[0].NextRule = destination;
                                result[1].smin = value;
                                result[1].NextRule = "";
                            }
                            break;
                    }
                    return result;
                }
                public string Evaluate(XmasJunk xmasJunk) {
                    string result = "";
                    if (noCheck) {
                        result = destination;
                    }
                    else {
                        if (checkType == type.x) {
                            if (greaterThan) {
                                if (xmasJunk.x > value) {
                                    result = destination;
                                }
                            }
                            else {
                                if (xmasJunk.x < value) {
                                    result = destination;
                                }
                            }
                        }
                        else if (checkType == type.m) {
                            if (greaterThan) {
                                if (xmasJunk.m > value) {
                                    result = destination;
                                }
                            }
                            else {
                                if (xmasJunk.m < value) {
                                    result = destination;
                                }
                            }
                        }
                        else if (checkType == type.a) {
                            if (greaterThan) {
                                if (xmasJunk.a > value) {
                                    result = destination;
                                }
                            }
                            else {
                                if (xmasJunk.a < value) {
                                    result = destination;
                                }
                            }
                        }
                        else if (checkType == type.s) {
                            if (greaterThan) {
                                if (xmasJunk.s > value) {
                                    result = destination;
                                }
                            }
                            else {
                                if (xmasJunk.s < value) {
                                    result = destination;
                                }
                            }
                        }
                    }
                    return result;
                }

            }
            class TheoreticalXmasJunk {
                public int xmin = 1, xmax = 4000, mmin = 1, mmax = 4000, amin = 1, amax = 4000, smin = 1, smax = 4000;
                public string NextRule = "in";
                public HashSet<string> visited = new HashSet<string>();
                public long PossibleJunk() {
                    //check for invalid ranges
                    if (xmin > xmax || mmin > mmax || amin > amax || smin > smax) { 
                        Debugger.Break();
                    }
                    long result = (long)(xmax - xmin+1) * (long)(mmax - mmin+1) * (long)(amax - amin + 1) * (long)(smax - smin + 1);
                    return result;
                }
                public override string ToString() {
                    return $"x:{xmin}-{xmax}, m:{mmin}-{mmax}, a:{amin}-{amax}, s:{smin}-{smax}";
                }
                public TheoreticalXmasJunk Clone() {
                    TheoreticalXmasJunk result = new TheoreticalXmasJunk();
                    result.xmin = xmin;
                    result.xmax = xmax;
                    result.mmin = mmin;
                    result.mmax = mmax;
                    result.amin = amin;
                    result.amax = amax;
                    result.smin = smin;
                    result.smax = smax;
                    result.NextRule = NextRule;
                    result.visited = new HashSet<string>(visited);
                    return result;
                }
            }
            class XmasJunk {
                public int x, m, a, s;
                public XmasJunk(string line) {
                    string[] split = line.Split(',');
                    x = int.Parse(split[0].Split('=')[1]);
                    m = int.Parse(split[1].Split('=')[1]);
                    a = int.Parse(split[2].Split('=')[1]);
                    s = int.Parse(split[3].Split('=')[1].Replace("}", ""));
                }
                public int Sum() {
                    return x + m + a + s;
                }
                public override string ToString() {
                    return $"x:{x}, m:{m}, a:{a}, s:{s}";
                }
            }

        }
    }

}