namespace AdventOfCode.Year2022 {
    public class Day18 : Day {

        public Day18(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            LavaScanner scanner = new(data);
            result = scanner.sides.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            LavaScanner scanner = new(data, true);
            scanner.FillInternalVoids();
            result = scanner.sides.ToString();
            Console.WriteLine(result);
        }
    }
    public class LavaScanner {
        public int sides => lavaCubes.Aggregate(0, (acc, x) => acc + x.Value.OpenSides);
        public (int x, int y, int z) LowerRange { get; set; } = (int.MaxValue, int.MaxValue, int.MaxValue);
        public (int x, int y, int z) UpperRange { get; set; } = (int.MinValue, int.MinValue, int.MinValue);
        public bool UpgradeActive { get; set; } = false;

        public Dictionary<int, LavaCube> lavaCubes = new();
        public Dictionary<int, List<LavaCube>> x_cubes = new();
        public Dictionary<int, List<LavaCube>> y_cubes = new();
        public Dictionary<int, List<LavaCube>> z_cubes = new();
        public Dictionary<int, List<int>> Adjacencies = new();
        public LavaScanner(string[] data, bool Upgrade = false) {
            for (int y = 0; y < data.Length; y++) {
                LavaCube cube = new(data[y], y);

                if (Upgrade)
                    UpdateScanFieldRanges(cube);

                lavaCubes.Add(cube.ID, cube);
                if (!x_cubes.ContainsKey(cube.x)) {
                    x_cubes.Add(cube.x, new List<LavaCube>());
                }
                x_cubes[cube.x].Add(cube);
                if (!y_cubes.ContainsKey(cube.y)) {
                    y_cubes.Add(cube.y, new List<LavaCube>());
                }
                y_cubes[cube.y].Add(cube);
                if (!z_cubes.ContainsKey(cube.z)) {
                    z_cubes.Add(cube.z, new List<LavaCube>());
                }
                z_cubes[cube.z].Add(cube);
            }
            if (!Upgrade)
                CalculateAdjacentCubes();
        }
        public override string ToString() {
            return $"Contains {lavaCubes.Count} cubes with {sides} sides";
        }
        public void UpdateScanFieldRanges(LavaCube cube) {
            if (cube.x < LowerRange.x) {
                LowerRange = (cube.x, LowerRange.y, LowerRange.z);
            }
            if (cube.y < LowerRange.y) {
                LowerRange = (LowerRange.x, cube.y, LowerRange.z);
            }
            if (cube.z < LowerRange.z) {
                LowerRange = (LowerRange.x, LowerRange.y, cube.z);
            }
            if (cube.x > UpperRange.x) {
                UpperRange = (cube.x, UpperRange.y, UpperRange.z);
            }
            if (cube.y > UpperRange.y) {
                UpperRange = (UpperRange.x, cube.y, UpperRange.z);
            }
            if (cube.z > UpperRange.z) {
                UpperRange = (UpperRange.x, UpperRange.y, cube.z);
            }
        }
        public void FillInternalVoids() {
            HashSet<(int x, int y, int z)> ScanTarget = new();
            //Search for line gaps in straight lines bewteen cubes in the x plane
            foreach (var x in x_cubes.Keys) {
                var cubes = x_cubes[x].OrderBy(c => c.y).ToList();
                foreach (int z in cubes.Select(c => c.z).Distinct()) {
                    var zcubes = cubes.Where(c => c.z == z).ToList();
                    for (int i = 0; i < zcubes.Count - 1; i++) {
                        if (zcubes[i].y + 1 != zcubes[i + 1].y) {
                            ScanTarget.Add((x, zcubes[i].y + 1, z));
                        }
                    }
                }
                cubes = x_cubes[x].OrderBy(c => c.z).ToList();
                foreach (int y in cubes.Select(c => c.y).Distinct()) {
                    var ycubes = cubes.Where(c => c.y == y).ToList();
                    for (int i = 0; i < ycubes.Count - 1; i++) {
                        if (ycubes[i].z + 1 != ycubes[i + 1].z) {
                            ScanTarget.Add((x, y, ycubes[i].z + 1));
                        }
                    }
                }
            }


            int[][] Directions = new int[][] { new int[] { 1, 0, 0 }, new int[] { -1, 0, 0 }, new int[] { 0, 1, 0 }, new int[] { 0, -1, 0 }, new int[] { 0, 0, 1 }, new int[] { 0, 0, -1 } };
            HashSet<(int x, int y, int z)> Visited = new();

            foreach (var target in ScanTarget) {
                if (Visited.Contains(target))
                    continue;

                HashSet<(int x, int y, int z)> NewArea = new();
                //bfs to see if we can reach the edge of the scanfield
                Queue<(int x, int y, int z)> ToProcess = new();
                ToProcess.Enqueue(target);
                while (ToProcess.Count > 0) {
                    var current = ToProcess.Dequeue();
                    //check if any point has reached either the upper or lower limit of the space
                    if (NewArea.Contains(current))
                        continue;
                    if (current.x == LowerRange.x || current.x == UpperRange.x || current.y == LowerRange.y || current.y == UpperRange.y || current.z == LowerRange.z || current.z == UpperRange.z) {
                        //if so, the area is not internal
                        NewArea.Clear();
                        ToProcess.Clear();
                        break;
                    }
                    //check if current already exists in LavaCubes by searching for it in the x_cubes dictionary

                    Visited.Add(current);
                    NewArea.Add(current);
                    foreach (var direction in Directions) {
                        (int x, int y, int z) NextDirection = (current.x + direction[0], current.y + direction[1], current.z + direction[2]);
                        if (x_cubes[NextDirection.x].Exists(x => x.y == NextDirection.y && x.z == NextDirection.z))
                            continue;
                        ToProcess.Enqueue(NextDirection);
                    }
                }
                if (NewArea.Count > 0) {
                    foreach ((int x, int y, int z) item in NewArea) {
                        LavaCube cube = new(item, lavaCubes.Count());
                        lavaCubes.Add(cube.ID, cube);
                        if (!x_cubes.ContainsKey(cube.x)) {
                            x_cubes.Add(cube.x, new List<LavaCube>());
                        }
                        x_cubes[cube.x].Add(cube);
                        if (!y_cubes.ContainsKey(cube.y)) {
                            y_cubes.Add(cube.y, new List<LavaCube>());
                        }
                        y_cubes[cube.y].Add(cube);
                        if (!z_cubes.ContainsKey(cube.z)) {
                            z_cubes.Add(cube.z, new List<LavaCube>());
                        }
                        z_cubes[cube.z].Add(cube);
                    }
                }
            }
            CalculateAdjacentCubes();
        }


        public void CalculateAdjacentCubes() {
            foreach (LavaCube cube in lavaCubes.Values) {
                foreach (LavaCube other in x_cubes[cube.x]) {
                    if (other.ID != cube.ID && !other.AdjacentCubes.Contains(cube.ID)
                        && (((other.y == cube.y - 1 || other.y == cube.y + 1) && other.z == cube.z)
                        || ((other.z == cube.z - 1 || other.z == cube.z + 1) && other.y == cube.y))) {
                        cube.AdjacentCubes.Add(other.ID);
                        other.AdjacentCubes.Add(cube.ID);
                    }
                }
                foreach (LavaCube other in y_cubes[cube.y]) {
                    if (other.ID != cube.ID && !other.AdjacentCubes.Contains(cube.ID)
                        && (((other.x == cube.x - 1 || other.x == cube.x + 1) && other.z == cube.z)
                        || ((other.z == cube.z - 1 || other.z == cube.z + 1) && other.x == cube.x))) {
                        cube.AdjacentCubes.Add(other.ID);
                        other.AdjacentCubes.Add(cube.ID);
                    }
                }
                foreach (LavaCube other in z_cubes[cube.z]) {
                    if (other.ID != cube.ID && !other.AdjacentCubes.Contains(cube.ID)
                        && (((other.x == cube.x - 1 || other.x == cube.x + 1) && other.y == cube.y)
                        || ((other.y == cube.y - 1 || other.y == cube.y + 1) && other.x == cube.x))) {
                        cube.AdjacentCubes.Add(other.ID);
                        other.AdjacentCubes.Add(cube.ID);
                    }
                }
                if (cube.AdjacentCubes.Count() > 6)
                    throw new Exception("Too many adjacent cubes");
            }
        }

        public class LavaCube {
            //add unique id
            public int ID { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            public List<int> AdjacentCubes { get; set; } = new List<int>();
            public int OpenSides => 6 - AdjacentCubes.Count();
            public override string ToString() {
                return $"(ID: {ID} => {x},{y},{z} with {OpenSides} sides open";
            }
            public LavaCube((int x, int y, int z) point, int id) {
                x = point.x;
                y = point.y;
                z = point.z;
                ID = id;
            }
            public LavaCube(string data, int id) {
                //data input format is like "2,2,2" where x,y,z are integers
                //parse "2 ,2,2" into x,y, z using regex
                string pattern = @"(\d+),(\d+),(\d+)";
                var match = Regex.Match(data, pattern);
                x = int.Parse(match.Groups[1].Value);
                y = int.Parse(match.Groups[2].Value);
                z = int.Parse(match.Groups[3].Value);
                ID = id;
            }
        }
    }
}