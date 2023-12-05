namespace AdventOfCode.Year2022
{
    public class Day15 : Day
    {
        public Day15(int today) : base(today) { }

        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            SensorNet net = new(data);
            result = net.ScanXRayForBeaconFreeSpaces(argumentType == ArgumentType.Sample ? 10 : 2000000, argumentType == ArgumentType.Sample ? true : false).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            SensorNet net = new(data);
            var coords = net.ScanGridRangesForMissingBeacon(argumentType == ArgumentType.Sample ? 20 : 4000000);
            //result is the x coord  * 4000000 plus the y coord and should be very large
            result = (coords.Item1 * (long)4000000 + coords.Item2).ToString();

            Console.WriteLine(result);
        }
    }
    public class SensorNet
    {
        public List<Sensor> sensors = new();
        public SensorNet(string[] data)
        {
            foreach (var line in data)
            {
                sensors.Add(new Sensor(line));
            }
        }
        public void DarwGrid()
        {
            //find the limits of all the points in sensors
            int minX = sensors.Min(s => s.X);
            minX = Math.Min(minX, sensors.Min(s => s.BeaconX));
            int maxX = sensors.Max(s => s.X);
            maxX = Math.Max(maxX, sensors.Max(s => s.BeaconX));
            int minY = sensors.Min(s => s.Y);
            minY = Math.Min(minY, sensors.Min(s => s.BeaconY));
            int maxY = sensors.Max(s => s.Y);
            maxY = Math.Max(maxY, sensors.Max(s => s.BeaconY));
            //draw the grid
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var sensor = sensors.FirstOrDefault(s => (s.X == x && s.Y == y) || (s.BeaconX == x && s.BeaconY == y));
                    if (sensor != null)
                    {
                        if (sensor.X == x && sensor.Y == y)
                        {
                            Console.Write('S');
                        }
                        else
                        {
                            Console.Write('B');
                        }
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }
        public (int x, int y) ScanGridRangesForMissingBeacon(int len)
        {
            //find the limits of all the points in sensors
            for (int y = 0; y <= len; y++)
            {
                List<(int x1, int x2)> ranges = new();

                //find sensors whos distance to beacon overlaps the current y level
                var sensorsInRange = sensors.Where(s => (s.Y >= y && s.Y - s.DistanceToBeacon <= y) || (s.Y <= y && s.Y + s.DistanceToBeacon >= y)).ToList();
                foreach (Sensor sensor in sensorsInRange)
                {
                    //find the x range of the current sensor
                    ranges.Add(sensor.GetXRange(y));
                    //combine overlapping ranges
                    for (int i = 0; i < ranges.Count; i++)
                    {
                        for (int j = 0; j < ranges.Count; j++)
                        {
                            if (i != j)
                            {
                                if (ranges[i].x1 <= ranges[j].x1 && ranges[i].x2 >= ranges[j].x1)
                                {
                                    ranges[i] = (ranges[i].x1, Math.Max(ranges[i].x2, ranges[j].x2));
                                    ranges.RemoveAt(j);
                                    i = 0;
                                    j = 0;
                                }
                                else if (ranges[i].x1 >= ranges[j].x1 && ranges[i].x1 <= ranges[j].x2)
                                {
                                    ranges[i] = (ranges[j].x1, Math.Max(ranges[i].x2, ranges[j].x2));
                                    ranges.RemoveAt(j);
                                    i = 0;
                                    j = 0;
                                }
                            }
                        }
                    }
                    ranges = ranges.OrderBy(r => r.x1).ToList();

                }
                if (ranges[0].x2 < len)
                {
                    return (ranges[0].x2 + 1, y);
                }
            }
            return (-1, -1);
        }

        //flag as obsolet and error
        [Obsolete("This method is obsolete. Use ScanGridRangesForMissingBeacon instead.", true)]
        public (int x, int y) ScanGridForMissingBeacon(int len, bool draw)
        {
            bool[,] grid = new bool[len + 1, len + 1];
            foreach (var sensor in sensors)
            {
                for (int y = sensor.Y - sensor.DistanceToBeacon; y <= sensor.Y + sensor.DistanceToBeacon; y++)
                {
                    for (int x = sensor.X - sensor.DistanceToBeacon; x <= sensor.X + sensor.DistanceToBeacon; x++)
                    {
                        if (Math.Abs(x - sensor.X) + Math.Abs(y - sensor.Y) <= sensor.DistanceToBeacon)
                        {
                            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                            {
                                if (x == 14 && y == 11)
                                    Console.WriteLine("here");
                                grid[x, y] = true;
                            }
                        }
                    }
                }
            }
            if (draw)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        if (grid[x, y])
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
            //find the first instance of false
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (!grid[x, y])
                    {
                        return (x, y);
                    }
                }
            }
            return (-1, -1);
        }
        public int ScanXRayForBeaconFreeSpaces(int y, bool draw)
        {
            //DarwGrid();
            //Find all sensors where x is between its locations and its beacon
            List<Sensor> sensorsInRange = sensors.Where(s => (s.Y >= y && s.Y - s.DistanceToBeacon <= y) || (s.Y <= y && s.Y + s.DistanceToBeacon >= y)).ToList();
            //find the range of x that covers the distance between the sensors
            int minX = int.MaxValue;
            int MaxX = 0;
            foreach (var sensor in sensorsInRange)
            {
                if (sensor.X - sensor.DistanceToBeacon < minX)
                    minX = sensor.X - sensor.DistanceToBeacon;
                if (sensor.X + sensor.DistanceToBeacon > MaxX)
                    MaxX = sensor.X + sensor.DistanceToBeacon;
            }
            bool[] ray = new bool[MaxX - minX + 1];
            foreach (var sensor in sensorsInRange)
            {
                var center = sensor.X - minX;
                var DistanceFromSensorToRay = sensor.DistanceToBeacon - Math.Abs(sensor.Y - y);
                for (int i = 0; i <= DistanceFromSensorToRay; i++)
                {
                    ray[center + i] = true;
                    ray[center - i] = true;
                }
                if (draw)
                    Console.WriteLine(string.Join("", ray.Select(b => b ? "#" : ".")));
            }
            //remove any beacons from ray
            List<Sensor> SensorsInline = sensors.Where(x => x.BeaconY == y).ToList();
            foreach (Sensor sensor1 in SensorsInline)
            {
                ray[sensor1.BeaconX - minX] = false;
            }
            //remove any sensors from ray
            SensorsInline = sensors.Where(x => x.Y == y).ToList();
            foreach (Sensor sensor1 in SensorsInline)
            {
                ray[sensor1.X - minX] = false;
            }
            if (draw)
                Console.WriteLine(string.Join("", ray.Select(b => b ? "#" : ".")));
            return ray.Count(b => b == true);

        }
    }
    public class Sensor
    {
        //add x y location
        public int X { get; set; }
        public int Y { get; set; }
        //add x y beacon found location
        public int BeaconX { get; set; }
        public int BeaconY { get; set; }
        //Manhatten distance
        public int DistanceToBeacon => Math.Abs(X - BeaconX) + Math.Abs(Y - BeaconY);
        public (int x1, int x2) GetXRange(int y)
        {
            int x = X;
            int distance = DistanceToBeacon - Math.Abs(Y - y);
            return (x - distance, x + distance);
        }
        public Sensor(string data)
        {
            //data like Sensor at x=2, y=18: closest beacon is at x=-2, y=15, parse data
            var split = data.Split(':');
            var location = split[0].Split(' ');
            X = int.Parse(location[2].Split('=')[1].TrimEnd(','));
            Y = int.Parse(location[3].Split('=')[1]);
            var beacon = split[1].Split(' ');
            BeaconX = int.Parse(beacon[5].Split('=')[1].TrimEnd(','));
            BeaconY = int.Parse(beacon[6].Split('=')[1]);
        }
    }
}