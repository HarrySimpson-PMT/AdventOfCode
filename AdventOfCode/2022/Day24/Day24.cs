namespace AdventOfCode.Year2022 {
    public class Day24 : Day {

        public Day24(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            BlizzardBasin BB = new(data);
            result = BB.SimulatePlayerGrowth();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            BlizzardBasin BB = new(data);
            result = BB.SimulatePlayerGrowthV2ThereAndBack();
            Console.WriteLine(result);
        }
        public class BlizzardBasin {
            public BlizzardSpace<Blizzard> BSpace { get; set; }
            public BlizzardBasin(string[] data) {
                BSpace = new(data);
            }
            public string SimulatePlayerGrowth() {
                int ElapsedTime = 0;
                BSpace.PlayerPositions.Add(BSpace.Entrance);
                while (!BSpace.PlayerPositions.Contains(BSpace.Exit)) {
                    SimulateNextMinute();
                    BSpace.PropagatePlayer();
                    ElapsedTime++;
                }
                return ElapsedTime.ToString();
            }
            public string SimulatePlayerGrowthV2ThereAndBack() {
                int ElapsedTime = 0;
                BSpace.PlayerPositions.Add(BSpace.Entrance);
                while (!BSpace.PlayerPositions.Contains(BSpace.Exit)) {
                    SimulateNextMinute();
                    BSpace.PropagatePlayer();
                    ElapsedTime++;
                }
                BSpace.PlayerPositions.Clear();
                BSpace.PlayerPositions.Add(BSpace.Exit);
                while (!BSpace.PlayerPositions.Contains(BSpace.Entrance)) {
                    SimulateNextMinute();
                    BSpace.PropagatePlayer();
                    ElapsedTime++;
                }
                BSpace.PlayerPositions.Clear();
                BSpace.PlayerPositions.Add(BSpace.Entrance);
                while (!BSpace.PlayerPositions.Contains(BSpace.Exit)) {
                    SimulateNextMinute();
                    BSpace.PropagatePlayer();
                    ElapsedTime++;
                }
                return ElapsedTime.ToString();
            }
            public void SimulateNextMinute() {
                foreach (var blizzard in BSpace.Entities) {
                    blizzard.Move(BSpace);
                }
            }
            public class BlizzardSpace<T> : EntitySpace<Blizzard> {
                public (int x, int y) Entrance { get; set; }
                public (int x, int y) Exit { get; set; }
                public HashSet<(int x, int y)> PlayerPositions { get; set; } = new();
                public int Height { get; set; }
                public int Width { get; set; }
                public BlizzardSpace(string[] data) : base() {
                    Height = data.Length;
                    Width = data[0].Length;
                    Entrance = (x: 0, y: data[0].IndexOf('.'));
                    Exit = (x: Height - 1, y: data[Height - 1].IndexOf('.'));
                    int count = 0;
                    for (int x = 1; x < Height - 1; x++) {
                        for (int y = 1; y < Width - 1; y++) {
                            if (data[x][y] != '.') {
                                AddStorm(data[x][y], x, y, count++);
                            }
                        }
                    }
                }
                public void PropagatePlayer() {
                    (int x, int y)[] directions = new (int x, int y)[] { (x: 0, y: 1), (x: 0, y: -1), (x: 1, y: 0), (x: -1, y: 0) };
                    foreach ((int x, int y) player in PlayerPositions.ToList()) {
                        if (X_Entities.ContainsKey(player.x) && X_Entities[player.x].Exists(point => point.Y == player.y)) {
                            PlayerPositions.Remove(player);
                        }
                        foreach ((int x, int y) direction in directions) {
                            (int x, int y) curr = (player.x + direction.x, player.y + direction.y);
                            if (curr == Exit)
                                PlayerPositions.Add(Exit);
                            if (curr == Entrance)
                                PlayerPositions.Add(Entrance);
                            if ((curr.x > 0 && curr.x < Height - 1 && curr.y > 0 && curr.y < Width - 1) && (!X_Entities.ContainsKey(curr.x) || !X_Entities[curr.x].Exists(point => point.Y == curr.y)))
                                PlayerPositions.Add(curr);
                        }
                    }
                }
                public void AddStorm(char key, int x, int y, int id) {
                    Blizzard storm;
                    switch (key) {
                        case '^':
                            storm = new NorthBlizzard(x, y, id);
                            break;
                        case '<':
                            storm = new WestBlizzard(x, y, id);
                            break;
                        case '>':
                            storm = new EastBlizzard(x, y, id);
                            break;
                        case 'V':
                        case 'v':
                            storm = new SouthBlizzard(x, y, id);
                            break;
                        default:
                            throw new ArgumentException("Invalid direction key");
                    }

                    Entities.Add(storm);
                    if (!X_Entities.ContainsKey(storm.X))
                        X_Entities.Add(storm.X, new());
                    X_Entities[storm.X].Add(storm);

                    if (!Y_Entities.ContainsKey(storm.Y))
                        Y_Entities.Add(storm.Y, new());
                    Y_Entities[storm.Y].Add(storm);
                }
                public void Print() {
                    Console.Clear();
                    StringBuilder[] lines = new StringBuilder[Height];
                    //line[0] needs should all be '#'
                    lines[0] = new StringBuilder();
                    for (int i = 0; i < Width; i++) {
                        lines[0].Append('#');
                    }
                    lines[0][Entrance.y] = '.';
                    for (int x = 1; x < Height - 1; x++) {
                        lines[x] = new StringBuilder();
                        lines[x].Append('#');
                        //fill with '.'
                        lines[x].Append(new string('.', Width - 2));
                        var XEntities = X_Entities[x];
                        for (int y = 1; y < Width - 1; y++) {
                            var curr = XEntities.Where(x => x.Y == y);
                            if (curr.Count() > 1)
                                lines[x][y] = (curr.Count() % 10).ToString()[0];
                            if (curr.Count() == 1)
                                lines[x][y] = curr.First().GetIcon();
                        }
                        lines[x].Append('#');
                    }
                    lines[Height - 1] = new StringBuilder();
                    for (int i = 0; i < Width; i++) {
                        lines[Height - 1].Append('#');
                    }
                    lines[Height - 1][Exit.y] = '.';
                    foreach (var player in PlayerPositions) {
                        if (lines[player.x][player.y] != '.')
                            throw new Exception("Invalid State");
                        lines[player.x][player.y] = 'P';
                    }
                    foreach (var line in lines)
                        Console.WriteLine(line.ToString());
                }
            }

            public abstract class Blizzard : Entity {
                public Direction Direction { get; set; }

                public Blizzard(int x, int y, int id) : base(x, y, id) {
                }
                public abstract void Move(BlizzardSpace<Blizzard> space);
                public override string ToString() {
                    switch (Direction) {
                        case Direction.North:
                            return $"Blizzard {ID} at ({X},{Y}) moving North";
                        case Direction.South:
                            return $"Blizzard {ID} at ({X},{Y}) moving South";
                        case Direction.East:
                            return $"Blizzard {ID} at ({X},{Y}) moving East";
                        case Direction.West:
                            return $"Blizzard {ID} at ({X},{Y}) moving West";
                        default:
                            throw new Exception("Invalid Direction State");
                    }
                }
                public override char GetIcon() {
                    switch (Direction) {
                        case Direction.North:
                            return '^';
                        case Direction.South:
                            return 'v';
                        case Direction.East:
                            return '>';
                        case Direction.West:
                            return '<';
                        default:
                            throw new Exception("Invalid Direction State");
                    }
                }
            }
            public class NorthBlizzard : Blizzard {
                public NorthBlizzard(int x, int y, int id) : base(x, y, id) {
                    this.Direction = Direction.North;
                }
                public override void Move(BlizzardSpace<Blizzard> space) {
                    space.X_Entities[X].Remove(this);
                    if (X - 1 <= 0)
                        X = space.Height - 2;
                    else
                        X--;
                    if (!space.X_Entities.ContainsKey(X))
                        space.X_Entities.Add(X, new());
                    space.X_Entities[X].Add(this);


                }
            }
            public class SouthBlizzard : Blizzard {
                public SouthBlizzard(int x, int y, int id) : base(x, y, id) {
                    this.Direction = Direction.South;
                }
                public override void Move(BlizzardSpace<Blizzard> space) {
                    space.X_Entities[X].Remove(this);
                    if (X + 1 >= space.Height - 1)
                        X = 1;
                    else
                        X++;
                    if (!space.X_Entities.ContainsKey(X))
                        space.X_Entities.Add(X, new());
                    space.X_Entities[X].Add(this);
                }
            }
            public class EastBlizzard : Blizzard {
                public EastBlizzard(int x, int y, int id) : base(x, y, id) {
                    this.Direction = Direction.East;
                }
                public override void Move(BlizzardSpace<Blizzard> space) {
                    space.Y_Entities[Y].Remove(this);

                    if (Y + 1 >= space.Width - 1)
                        Y = 1;
                    else
                        Y++;
                    if (!space.Y_Entities.ContainsKey(Y))
                        space.Y_Entities.Add(X, new());
                    space.Y_Entities[Y].Add(this);
                }
            }
            public class WestBlizzard : Blizzard {
                public WestBlizzard(int x, int y, int id) : base(x, y, id) {
                    this.Direction = Direction.West;
                }
                public override void Move(BlizzardSpace<Blizzard> space) {
                    space.Y_Entities[Y].Remove(this);
                    if (Y - 1 <= 0)
                        Y = space.Width - 2;
                    else
                        Y--;
                    if (!space.Y_Entities.ContainsKey(Y))
                        space.Y_Entities.Add(Y, new());
                    space.Y_Entities[Y].Add(this);
                }
            }

        }
    }

}