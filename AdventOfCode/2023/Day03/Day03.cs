
namespace AdventOfCode.Year2023
{
    public class Day03 : Day
    {
        public Day03(int today, int year) : base(today, year)
        {

        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            CrappyElfEngineDiagnosticEquipment crappyElfEngineDiagnosticEquipment = new();
            result = crappyElfEngineDiagnosticEquipment.DetermineBrokenPart(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            CrappyElfEngineDiagnosticEquipment crappyElfEngineDiagnosticEquipment = new();
            crappyElfEngineDiagnosticEquipment.DetermineBrokenPart(data).ToString();
            result = crappyElfEngineDiagnosticEquipment.GearRatio.ToString();
            Console.WriteLine(result);
        }
        public class CrappyElfEngineDiagnosticEquipment
        {
            List<(int x, int y)> SymbolLocations = new();
            (int x, int y)[] dirs = { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) };
            int m = 0, n = 0;
            List<char[]> data = new();
            public int GearRatio { get; set; } = 0;
            public int DetermineBrokenPart(string[] baddata)
            {
                n = baddata.Length;
                m = baddata[0].Length;
                for (int x = 0; x < n; x++)
                    data.Add(baddata[x].ToCharArray());
                int result = 0;
                for (int x = 0; x < n; x++)
                {
                    for (int y = 0; y < data[x].Length; y++)
                    {
                        if (!".1234567890".Contains(data[x][y].ToString()))
                        {
                            SymbolLocations.Add((x, y));
                        }
                    }
                }
                foreach (var location in SymbolLocations)
                {
                    bool GearSymbol = false;
                    if (data[location.x][location.y] == '*')
                        GearSymbol = true;
                    int neighbors = 0;
                    List<int> neighborvalues = new();
                    foreach (var dir in dirs)
                    {
                        int x = location.x + dir.x;
                        int y = location.y + dir.y;
                        if (x >= 0 && x < n && y >= 0 && y < m && "1234567890".Contains(data[x][y]))
                        {
                            neighbors++;
                            string number = "";
                            while (y - 1 >= 0 && "1234567890".Contains(data[x][y - 1]))
                                y--;
                            while (y < m && "1234567890".Contains(data[x][y]))
                            {
                                number += data[x][y];
                                data[x][y++] = '.';
                            }

                            neighborvalues.Add(int.Parse(number));
                        }
                    }
                    result += neighborvalues.Sum();
                    if (GearSymbol&&neighborvalues.Count==2)
                    {
                        GearRatio += neighborvalues[0] * neighborvalues[1];
                    }
                }
                return result;
            }
        }
    }
}