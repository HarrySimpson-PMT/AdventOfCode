namespace AdventOfCode.Year2022
{
    public class Day21 : Day
    {

        public Day21(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyBusiness monkeyBusiness = new(data);
            monkeyBusiness.ComputeMonkies();
            result = monkeyBusiness.result;
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            MonkeyBusiness monkeyBusiness = new(data);
            monkeyBusiness.ComputeMonkiesV2();
            result = monkeyBusiness.result;
            Console.WriteLine(result);
        }
        public class MonkeyBusiness //LOL
        {
            public string result = "";
            public Dictionary<string, Monkey> monkeys = new();
            public Queue<Monkey> CompleteMonkes = new();

            public Dictionary<string, List<Monkey>> RequiredMonkies = new();


            public MonkeyBusiness(string[] data)
            {
                foreach (string line in data)
                {
                    var monke = new Monkey(line);
                    monkeys.Add(monke.ID, monke);
                    if (monke.Output != null)
                    {
                        CompleteMonkes.Enqueue(monke);
                    }
                    else
                    {
                        if (!RequiredMonkies.ContainsKey(monke.LeftInputID ?? throw new Exception("LeftInputID is null")))
                            RequiredMonkies.Add(monke.LeftInputID, new List<Monkey>());
                        RequiredMonkies[monke.LeftInputID].Add(monke);
                        if (!RequiredMonkies.ContainsKey(monke.RightInputID ?? throw new Exception("RightInputID is null")))
                            RequiredMonkies.Add(monke.RightInputID, new List<Monkey>());
                        RequiredMonkies[monke.RightInputID].Add(monke);
                    }
                }
            }
            public void ComputeMonkiesV2()
            {
                while (CompleteMonkes.Count > 0)
                {
                    var monke = CompleteMonkes.Dequeue();
                    if (monke.ID == "humn")
                    {
                        continue;
                    }
                    List<Monkey> MonkiesToUpdate = RequiredMonkies[monke.ID];
                    foreach (var monkeToUpdate in MonkiesToUpdate)
                    {
                        if (monkeToUpdate.LeftInputID == monke.ID)
                            monkeToUpdate.LeftInput = monke.Output;
                        else
                            monkeToUpdate.RightInput = monke.Output;
                        if (monkeToUpdate.LeftInput != null && monkeToUpdate.RightInput != null)
                        {
                            monkeToUpdate.Compute();
                            CompleteMonkes.Enqueue(monkeToUpdate);
                        }
                    }
                }
                Monkey Mooke = monkeys["root"];
                long check = 0;
                if (Mooke.LeftInput == null)
                {
                    check = (long)Mooke.RightInput;
                    Mooke = monkeys[Mooke.LeftInputID];
                    Mooke.Output = check;
                }
                else
                {
                    check = (long)Mooke.LeftInput;
                    Mooke = monkeys[Mooke.RightInputID];
                    Mooke.Output = check;
                }
                while (Mooke.ID != "humn")
                {
                    if (Mooke.ID == RequiredMonkies["humn"][0].ID)
                        Console.WriteLine("test");
                    switch (Mooke.Operator)
                    {
                        case '+':
                            if (Mooke.LeftInput == null)
                            {
                                Mooke.LeftInput = Mooke.Output - Mooke.RightInput;
                                check = (long)Mooke.LeftInput;
                                Mooke = monkeys[Mooke.LeftInputID];
                            }
                            else
                            {
                                Mooke.RightInput = Mooke.Output - Mooke.LeftInput;
                                check = (long)Mooke.RightInput;
                                Mooke = monkeys[Mooke.RightInputID];
                            }
                            break;
                        case '*':
                            if (Mooke.LeftInput == null)
                            {
                                Mooke.LeftInput = Mooke.Output / Mooke.RightInput;
                                check = (long)Mooke.LeftInput;
                                Mooke = monkeys[Mooke.LeftInputID];
                            }
                            else
                            {
                                Mooke.RightInput = Mooke.Output / Mooke.LeftInput;
                                check = (long)Mooke.RightInput;
                                Mooke = monkeys[Mooke.RightInputID];
                            }
                            break;
                        case '-':
                            if (Mooke.LeftInput == null)
                            {
                                Mooke.LeftInput = Mooke.Output + Mooke.RightInput;
                                check = (long)Mooke.LeftInput;
                                Mooke = monkeys[Mooke.LeftInputID];
                            }
                            else
                            {
                                Mooke.RightInput = Mooke.LeftInput - Mooke.Output;
                                check = (long)Mooke.RightInput;
                                Mooke = monkeys[Mooke.RightInputID];
                            }
                            break;
                        case '/':
                            if (Mooke.LeftInput == null)
                            {
                                Mooke.LeftInput = Mooke.Output * Mooke.RightInput;
                                check = (long)Mooke.LeftInput;
                                Mooke = monkeys[Mooke.LeftInputID];
                            }
                            else
                            {
                                Mooke.RightInput = Mooke.LeftInput * Mooke.Output;
                                check = (long)Mooke.RightInput;
                                Mooke = monkeys[Mooke.RightInputID];
                            }
                            break;
                    }
                    Mooke.Output = check;

                }

                result = Mooke.Output.ToString();


                return;


            }
            public void ComputeMonkies()
            {
                while (CompleteMonkes.Count > 0)
                {
                    var monke = CompleteMonkes.Dequeue();
                    if (monke.ID == "root")
                    {
                        result = monke.Output.ToString();
                        return;
                    }
                    List<Monkey> MonkiesToUpdate = RequiredMonkies[monke.ID];
                    foreach (var monkeToUpdate in MonkiesToUpdate)
                    {
                        if (monkeToUpdate.LeftInputID == monke.ID)
                            monkeToUpdate.LeftInput = monke.Output;
                        else
                            monkeToUpdate.RightInput = monke.Output;
                        if (monkeToUpdate.LeftInput != null && monkeToUpdate.RightInput != null)
                        {
                            monkeToUpdate.Compute();
                            CompleteMonkes.Enqueue(monkeToUpdate);
                        }
                    }

                }
            }
        }

        public class Monkey
        {
            public string ID { get; set; } = null!;
            public long? Output { get; set; }
            public long? LeftInput { get; set; }
            public long? RightInput { get; set; }
            public string? LeftInputID { get; set; }
            public char? Operator { get; set; }
            public string? RightInputID { get; set; }
            public Monkey(string data)
            {
                var match = Regex.Match(data, @"(?<ID>\w+):\s*(?:(?<LeftInputID>\w+)\s*(?<Operator>[+-/*])\s*(?<RightInputID>\w+)|(?<Output>\d+))");

                if (match.Success)
                {
                    ID = match.Groups["ID"].Value;
                    if (match.Groups["Output"].Success)
                    {
                        Output = int.Parse(match.Groups["Output"].Value);
                    }
                    else
                    {
                        LeftInputID = match.Groups["LeftInputID"].Value;
                        Operator = char.Parse(match.Groups["Operator"].Value);
                        RightInputID = match.Groups["RightInputID"].Value;
                    }
                }
                else throw new Exception("No match");
            }
            public void Compute()
            {
                if (LeftInput == null || RightInput == null)
                    throw new Exception("LeftInput or RightInput is null");
                switch (Operator)
                {
                    case '+':
                        Output = LeftInput + RightInput;
                        break;
                    case '-':
                        Output = LeftInput - RightInput;
                        break;
                    case '*':
                        Output = LeftInput * RightInput;
                        break;
                    case '/':
                        Output = LeftInput / RightInput;
                        break;
                    default:
                        throw new Exception("Operator is null");
                }
            }
        }
    }
}