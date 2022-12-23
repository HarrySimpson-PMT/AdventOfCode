namespace AdventOfCode.Year2022
{
    public class Day19 : Day
    {

        public Day19(int today) : base(today)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ProductionSimulator PS = new(data);
            List<int> res = PS.FindBestBlueprint(24);
            int sum = 0;
            for (int i = 0; i < res.Count(); i++)
            {
                sum += (res[i] * (i + 1));
            }
            result = sum.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            string[] FristThree = new string[3];
            FristThree[0] = data[0];
            FristThree[1] = data[1];
            FristThree[2] = data.Length > 3 ? data[2] : "";
            ProductionSimulator PS = new(FristThree);
            List<int> res = PS.FindBestBlueprint(32);
            int sum = 1;
            for (int i = 0; i < res.Count(); i++)
            {
                sum *= res[i];
            }
            result = sum.ToString();
            Console.WriteLine(result);
        }
    }
    public class ProductionSimulator
    {
        public int CurrentBlueprint { get; set; }
        public List<Blueprint> Blueprints { get; set; } = new List<Blueprint>();
        public ProductionSimulator(string[] data)
        {
            foreach (string line in data)
            {
                if (line != "")
                    Blueprints.Add(new Blueprint(line));
            }
        }
        public List<int> FindBestBlueprint(int time)
        {
            List<int> result = new();
            foreach (Blueprint bp in Blueprints)
            {
                Reset();
                MaximumPotential(bp, time, null);
                result.Add(CurrentMax);
                CurrentMax = 0;
            }
            return result;
        }
        public void MaximumPotential(Blueprint bp, int MaxTime, NextRobot? nextRobot)
        {
            if (MaxTime > 1 && Cheat(bp, MaxTime))
            {
                int ore = 0;
                int clay = 0;
                int obsidian = 0;
                int TimeToBuild = 0;
                switch (nextRobot)
                {
                    case NextRobot.Ore:
                        ore = bp.OreRobotCost.ore;
                        break;
                    case NextRobot.Clay:
                        ore = bp.ClayRobotCost.ore;
                        break;
                    case NextRobot.Obsidian:
                        ore = bp.ObsidianRobotCost.ore;
                        clay = bp.ObsidianRobotCost.clay;
                        break;
                    case NextRobot.Geode:
                        ore = bp.GeodeRobotCost.ore;
                        obsidian = bp.GeodeRobotCost.obsidian;
                        break;
                    default:
                        break;
                }

                //Determine time required to aquire materials needed to build the robit.
                if (ore > 0 && CurrentOre < ore)
                {
                    int ProductionTime = (ore - CurrentOre) / CurrentOreProduction;
                    int reamain = (ore - CurrentOre) % CurrentOreProduction == 0 ? 0 : 1;
                    TimeToBuild = Math.Max(TimeToBuild, ProductionTime + reamain + 1);
                }
                else
                {
                    TimeToBuild = Math.Max(TimeToBuild, 1);
                }

                if (clay > 0 && CurrentClayProduction > 0 && CurrentClay < clay)
                {
                    int ProductionTime = (clay - CurrentClay) / CurrentClayProduction;
                    int reamain = (clay - CurrentClay) % CurrentClayProduction == 0 ? 0 : 1;
                    TimeToBuild = Math.Max(TimeToBuild, ProductionTime + reamain + 1);
                }
                else
                {
                    TimeToBuild = Math.Max(TimeToBuild, 1);
                }

                if (obsidian > 0 && CurrentObsidianProduction > 0 && CurrentObsidian < obsidian)
                {
                    int ProductionTime = (obsidian - CurrentObsidian) / CurrentObsidianProduction;
                    int reamain = (obsidian - CurrentObsidian) % CurrentObsidianProduction == 0 ? 0 : 1;
                    TimeToBuild = Math.Max(TimeToBuild, ProductionTime + reamain + 1);
                }
                else
                {
                    TimeToBuild = Math.Max(TimeToBuild, 1);
                }

                if (TimeToBuild < 1)
                    throw new Exception("Time to rebuild");

                if (MaxTime - TimeToBuild >= 0)
                {
                    int prevore = CurrentOre;
                    int prevclay = CurrentClay;
                    int prevobsidian = CurrentObsidian;
                    int preGeodes = CurrentGeodes;
                    CurrentOre = CurrentOre + (CurrentOreProduction * TimeToBuild) - ore;
                    CurrentClay = CurrentClay + (CurrentClayProduction * TimeToBuild) - clay;
                    CurrentObsidian = CurrentObsidian + (CurrentObsidianProduction * TimeToBuild) - obsidian;
                    CurrentGeodes = CurrentGeodes + (CurrentGeodesProduction * TimeToBuild);

                    //if any currents go negative throw error
                    if (CurrentOre < 0 || CurrentClay < 0 || CurrentObsidian < 0 || CurrentGeodes < 0)
                    {
                        throw new Exception("Currents went negative");
                    }

                    switch (nextRobot)
                    {
                        case NextRobot.Ore:
                            CurrentOreProduction += 1;
                            break;
                        case NextRobot.Clay:
                            CurrentClayProduction += 1;
                            break;
                        case NextRobot.Obsidian:
                            CurrentObsidianProduction += 1;
                            break;
                        case NextRobot.Geode:
                            CurrentGeodesProduction += 1;
                            break;
                        default:
                            break;
                    }

                    int Max = CurrentGeodes + CurrentGeodesProduction * (MaxTime - TimeToBuild);

                    foreach (NextRobot nr in Enum.GetValues(typeof(NextRobot)))
                    {
                        switch (nr)
                        {
                            case NextRobot.Ore:
                                if (CurrentOreProduction < bp.Maximums.ore)
                                    MaximumPotential(bp, MaxTime - TimeToBuild, nr);
                                break;
                            case NextRobot.Clay:
                                if (CurrentClayProduction < bp.Maximums.clay)
                                    MaximumPotential(bp, MaxTime - TimeToBuild, nr);
                                break;
                            case NextRobot.Obsidian:
                                if (CurrentClayProduction > 0 && CurrentObsidianProduction < bp.Maximums.obsidian)
                                    MaximumPotential(bp, MaxTime - TimeToBuild, nr);
                                break;
                            case NextRobot.Geode:
                                if (CurrentObsidianProduction > 0)
                                    MaximumPotential(bp, MaxTime - TimeToBuild, nr);

                                break;
                        }
                    }
                    //undeo the changes
                    CurrentOre = prevore;
                    CurrentClay = prevclay;
                    CurrentObsidian = prevobsidian;
                    CurrentGeodes = preGeodes;
                    switch (nextRobot)
                    {
                        case NextRobot.Ore:
                            CurrentOreProduction -= 1;
                            break;
                        case NextRobot.Clay:
                            CurrentClayProduction -= 1;
                            break;
                        case NextRobot.Obsidian:
                            CurrentObsidianProduction -= 1;
                            break;
                        case NextRobot.Geode:
                            CurrentGeodesProduction -= 1;
                            break;
                    }
                }
            }
            CurrentMax = Math.Max(CurrentMax, CurrentGeodes + CurrentGeodesProduction * (MaxTime));
        }
        public bool Cheat(Blueprint bp, int MaxTime)
        {
            int currentGeodes = CurrentGeodes;
            int currentGeodesProduction = CurrentGeodesProduction;
            int currentObsidian = CurrentObsidian;
            int currentObsidianProduction = CurrentObsidianProduction;
            int currentClay = CurrentClay;
            int currentClayProduction = CurrentClayProduction;
            for (int i = 0; i < MaxTime; i++)
            {
                currentGeodes += currentGeodesProduction;
                currentObsidian += currentObsidianProduction;
                currentClay += currentClayProduction;
                if (currentObsidian >= bp.GeodeRobotCost.obsidian)
                {
                    currentGeodesProduction += 1;
                    currentObsidian -= bp.GeodeRobotCost.obsidian;
                }
                if (currentClay >= bp.ObsidianRobotCost.clay)
                {
                    currentObsidianProduction += 1;
                    currentClay -= bp.ObsidianRobotCost.clay;
                }
                currentClayProduction += 1;
            }
            if (currentGeodes == 0 || currentGeodes > CurrentMax)
                return true;
            return false;
        }
        public void Reset()
        {
            CurrentTime = 0;
            CurrentOre = 0;
            CurrentOreProduction = 1;
            CurrentClay = 0;
            CurrentClayProduction = 0;
            CurrentObsidian = 0;
            CurrentObsidianProduction = 0;
            CurrentGeodes = 0;
            CurrentGeodesProduction = 0;
        }
        public int CurrentMax { get; set; } = 0;
        public int CurrentTime { get; set; } = 0;
        public int CurrentOre { get; set; } = 0;
        public int CurrentOreProduction { get; set; } = 1;
        public int CurrentClay { get; set; } = 0;
        public int CurrentClayProduction { get; set; } = 0;
        public int CurrentObsidian { get; set; } = 0;
        public int CurrentObsidianProduction { get; set; } = 0;
        public int CurrentGeodes { get; set; } = 0;
        public int CurrentGeodesProduction { get; set; } = 0;
        public enum NextRobot { Ore, Clay, Obsidian, Geode }
    }
    public class Blueprint
    {
        public int ID { get; set; }
        public (int ore, int clay, int obsidian) Maximums { get; set; }
        public (int ore, int clay, int obsidian) OreRobotCost { get; set; }
        public (int ore, int clay, int obsidian) ClayRobotCost { get; set; }
        public (int ore, int clay, int obsidian) ObsidianRobotCost { get; set; }
        public (int ore, int clay, int obsidian) GeodeRobotCost { get; set; }
        public Blueprint(string data)
        {
            string pattern = @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.";
            Regex regex = new(pattern);


            var read = regex.Match(data);
            ID = int.Parse(read.Groups[1].Value);
            OreRobotCost = (int.Parse(read.Groups[2].Value), 0, 0);
            Maximums = OreRobotCost;

            ClayRobotCost = (int.Parse(read.Groups[3].Value), 0, 0);
            Maximums = (Math.Max(ClayRobotCost.ore, Maximums.ore), 0, 0);

            ObsidianRobotCost = (int.Parse(read.Groups[4].Value), int.Parse(read.Groups[5].Value), 0);
            Maximums = (Math.Max(Maximums.ore, ObsidianRobotCost.ore), Math.Max(Maximums.clay, ObsidianRobotCost.clay), 0);

            GeodeRobotCost = (int.Parse(read.Groups[6].Value), 0, int.Parse(read.Groups[7].Value));
            Maximums = (Math.Max(Maximums.ore, GeodeRobotCost.ore), Math.Max(Maximums.clay, GeodeRobotCost.clay), Math.Max(Maximums.obsidian, GeodeRobotCost.obsidian));

        }
    }
}