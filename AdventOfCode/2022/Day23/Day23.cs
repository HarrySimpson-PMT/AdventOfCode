﻿namespace AdventOfCode.Year2022 {
    public class Day23 : Day {

        public Day23(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            UnstableDiffusion unstableDiffusion = new(data);
            unstableDiffusion.Dance(10);
            result = unstableDiffusion.DanceResult();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            UnstableDiffusion unstableDiffusion = new(data);
            result = unstableDiffusion.DanceV2();
            Console.WriteLine(result);
        }
        public class UnstableDiffusion {
            public List<((int x, int y) Left, (int x, int y) Mid, (int x, int y) Right, Direction direction)> Moves = new() {
                ((-1, -1), (-1, 0), (-1, 1), Direction.North),
                ((1, -1), (1, 0), (1, 1), Direction.South),
                ((-1, -1), (0, -1), (1, -1), Direction.West),
                ((-1, 1), (0, 1), (1, 1), Direction.East)
            };


            public Dictionary<string, int> AttentionSpace;
            public ElfSpace ESpace = new();
            public UnstableDiffusion(string[] data) {
                int count = 0;
                for (int x = 0; x < data.Length; x++) {
                    for (int y = 0; y < data[x].Length; y++) {
                        if (data[x][y] == '#') {
                            ESpace.AddElf(x, y, count++);
                        }
                    }
                }
            }
            public void Dance(int n) {
                //ESpace.Print();
                for (int i = 0; i < n; i++) {
                    AttentionSpace = new();
                    ConsiderMovement(i);
                    MakeMovement();
                    var temp = Moves[0];
                    Moves.Remove(temp);
                    Moves.Add(temp);
                    //ESpace.Print();
                }
            }
            public string DanceV2() {
                int i = 0;
                AttentionSpace = new();
                AttentionSpace.Add("breal", -1);
                while (AttentionSpace.Count > 0) {
                    AttentionSpace = new();
                    ConsiderMovement(i);
                    MakeMovement();
                    var temp = Moves[0];
                    Moves.Remove(temp);
                    Moves.Add(temp);
                    i++;
                }
                return i.ToString();
            }
            public string DanceResult() {
                return (ESpace.Volume - ESpace.Entities.Count()).ToString();
            }
            public void ConsiderMovement(int perm) {

                foreach (Elfo YouKnow in ESpace.Entities) {
                    YouKnow.GetAttentionV1(this);
                }
            }
            public void MakeMovement() {
                foreach (KeyValuePair<string, int> attention in AttentionSpace) {
                    ESpace.MoveElf(attention.Key, attention.Value);
                }
            }


            public class Elfo : IEntity {
                public int ID { get; set; }
                public int X { get; set; }
                public int Y { get; set; }
                public void GetAttentionV1(UnstableDiffusion e) {
                    bool notAlone = false;
                    string attention = null;
                    foreach (var move in e.Moves) {
                        if (
                            (!e.ESpace.X_Entities.ContainsKey(X + move.Left.x) || !e.ESpace.X_Entities[X + move.Left.x].Exists(x => x.Y == Y + move.Left.y)) &&
                            (!e.ESpace.X_Entities.ContainsKey(X + move.Mid.x) || !e.ESpace.X_Entities[X + move.Mid.x].Exists(x => x.Y == Y + move.Mid.y)) &&
                            (!e.ESpace.X_Entities.ContainsKey(X + move.Right.x) || !e.ESpace.X_Entities[X + move.Right.x].Exists(x => x.Y == Y + move.Right.y))
                            ) {
                            if (attention == null) {
                                switch (move.direction) {
                                    case Direction.East:
                                        attention = $"{X}X{Y + 1}";
                                        break;
                                    case Direction.South:
                                        attention = $"{X + 1}X{Y}";
                                        break;
                                    case Direction.West:
                                        attention = $"{X}X{Y - 1}";
                                        break;
                                    case Direction.North:
                                        attention = $"{X - 1}X{Y}";
                                        break;
                                    default:
                                        throw new Exception("Now you fucked up");
                                }
                                if (notAlone)
                                    break;
                            }
                        }
                        else {
                            notAlone = true; //secondary check to ensure aloneness
                            if (attention != null)
                                break;
                        }
                    }
                    if (attention != null && notAlone)
                        if (!e.AttentionSpace.ContainsKey(attention))
                            e.AttentionSpace.Add(attention, ID);
                        else
                            e.AttentionSpace[attention] = -1; //now nobody gets to move here.
                }
            }
            public class ElfSpace : EntitySpace<Elfo> {
                public int Top => X_Entities.Where(x => x.Value.Count > 0).Select(x => x.Key).Min();
                public int Bottom => X_Entities.Where(x => x.Value.Count > 0).Select(x => x.Key).Max();
                public int Left => Y_Entities.Where(x => x.Value.Count > 0).Select(x => x.Key).Min();
                public int Right => Y_Entities.Where(x => x.Value.Count > 0).Select(x => x.Key).Max();
                public int Volume => (Bottom - Top + 1) * (Right - Left + 1);
                public void Print() {
                    Console.Clear();
                    for (int x = Top; x <= Bottom; x++) {
                        for (int y = Left; y <= Right; y++) {
                            if (X_Entities.ContainsKey(x) && X_Entities[x].Exists(x => x.Y == y))
                                Console.Write("#");
                            else
                                Console.Write(".");
                        }
                        Console.WriteLine();
                    }
                }


                public void AddElf(int x, int y, int id) {
                    Elfo elf = new() { X = x, Y = y, ID = id };
                    Entities.Add(elf);
                    if (!X_Entities.ContainsKey(x))
                        X_Entities.Add(x, new());
                    X_Entities[x].Add(elf);
                    if (!Y_Entities.ContainsKey(y))
                        Y_Entities.Add(y, new());
                    Y_Entities[y].Add(elf);
                }
                public void MoveElf(string attention, int id) {
                    if (id == -1)
                        return;
                    var elf = Entities[id];
                    X_Entities[elf.X].Remove(elf);
                    Y_Entities[elf.Y].Remove(elf);
                    string[] move = attention.Split("X");
                    elf.X = int.Parse(move[0]);
                    elf.Y = int.Parse(move[1]);
                    if (!X_Entities.ContainsKey(elf.X))
                        X_Entities.Add(elf.X, new());
                    X_Entities[elf.X].Add(elf);
                    if (!Y_Entities.ContainsKey(elf.Y))
                        Y_Entities.Add(elf.Y, new());
                    Y_Entities[elf.Y].Add(elf);
                }

            }
        }
    }
}