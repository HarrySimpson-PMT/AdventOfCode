
using System.Diagnostics;

namespace AdventOfCode.Year2023 {
    public class Day22 : Day {
        public Day22(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            FallingSlabSim fallingSlabs = new FallingSlabSim(data);
            //fallingSlabs.drawSlabSpace();
            fallingSlabs.RunGravity();
            //fallingSlabs.drawSlabSpace();
            result = fallingSlabs.CountDeletableSections().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            FallingSlabSim fallingSlabs = new FallingSlabSim(data);
            fallingSlabs.RunGravity();
            result = fallingSlabs.CountChainReactions().ToString();
            Console.WriteLine(result);
        }
        public class FallingSlabSim {
            Slab[,,] SlabSpace;
            List<Slab> Slabs;
            List<Slab> FallingSlabs;
            List<Slab> GroundedSlabs;
            public FallingSlabSim(string[] data) {
                Slabs = new List<Slab>();
                int slabCount = 0;
                foreach (string line in data) {
                    Slabs.Add(new Slab(line, slabCount++));
                }
                int maxX = Slabs.Max(x => x.position.x + x.XSize);
                int maxY = Slabs.Max(x => x.position.y + x.YSize);
                int maxZ = Slabs.Max(x => x.position.z + x.ZSize);
                SlabSpace = new Slab[maxX, maxY, maxZ];
                //write slabs to the slab space
                foreach (Slab slab in Slabs) {
                    for (int x = slab.position.x; x < slab.position.x + slab.XSize; x++) {
                        for (int y = slab.position.y; y < slab.position.y + slab.YSize; y++) {
                            for (int z = slab.position.z; z < slab.position.z + slab.ZSize; z++) {
                                SlabSpace[x, y, z] = slab;
                            }
                        }
                    }
                }
            }
            public void RunGravity() {
                //check each slab to see if it is grounded => a z coord of 1 is grounded
                foreach (Slab slab in Slabs) {
                    if (slab.position.z == 1) {
                        slab.Grounded = true;
                    }
                }
                UpdateFallingAndGroundedSlabs();

                while (FallingSlabs.Count > 0) {
                    SimulateStep();
                    UpdateFallingAndGroundedSlabs();
                }
            }
            public void SimulateStep() {
                //from falling slabs check if they can fall a single step in the z direction without colliding with another slab
                //if they can fall then move them down one step
                //if they can't fall mark them grounded and determine the number of supporting slabs
                FallingSlabs.Sort((x, y) => x.position.z.CompareTo(y.position.z));
                foreach (Slab slab in FallingSlabs) {
                    if (slab.id == 5) { Debugger.Break(); }
                    bool canFall = true;
                    (int x, int y, int z) newPosition = (slab.position.x, slab.position.y, slab.position.z - 1);
                    if (SlabSpace[newPosition.x, newPosition.y, newPosition.z] == null) {
                        for (int x = slab.position.x; x < slab.position.x + slab.XSize; x++) {
                            for (int y = slab.position.y; y < slab.position.y + slab.YSize; y++) {
                                if (SlabSpace[x, y, slab.position.z - 1] != null) {
                                    canFall = false;
                                }
                            }
                        }
                    }
                    else {
                        canFall = false;
                    }
                    if (canFall) {
                        //remove the slab from slab space then readd it at the new position
                        for (int x = slab.position.x; x < slab.position.x + slab.XSize; x++) {
                            for (int y = slab.position.y; y < slab.position.y + slab.YSize; y++) {
                                for (int z = slab.position.z; z < slab.position.z + slab.ZSize; z++) {
                                    SlabSpace[x, y, z] = null;
                                }
                            }
                        }
                        slab.position = newPosition;
                        for (int x = slab.position.x; x < slab.position.x + slab.XSize; x++) {
                            for (int y = slab.position.y; y < slab.position.y + slab.YSize; y++) {
                                for (int z = slab.position.z; z < slab.position.z + slab.ZSize; z++) {
                                    SlabSpace[x, y, z] = slab;
                                }
                            }
                        }
                        if (slab.position.z == 1) {
                            slab.Grounded = true;
                        }
                    }
                    else {
                        //mark the slab grounded and determine the number of supporting slabs
                        for (int x = slab.position.x; x < slab.position.x + slab.XSize; x++) {
                            for (int y = slab.position.y; y < slab.position.y + slab.YSize; y++) {
                                if (SlabSpace[x, y, slab.position.z - 1] != null) {
                                    slab.GroundConnections.Add(SlabSpace[x, y, slab.position.z - 1]);
                                    SlabSpace[x, y, slab.position.z - 1].SupportingConnections.Add(slab);
                                    slab.Grounded |= SlabSpace[x, y, slab.position.z - 1].Grounded;
                                }
                            }
                        }
                    }
                }
            }
            public int CountChainReactions() {
                int result = 0;
                foreach (Slab slab in Slabs) {
                    int supportingSlabs = 0;
                    HashSet<Slab> Chain = new HashSet<Slab>();
                    Chain.Add(slab);
                    Queue<Slab> Queue = new Queue<Slab>();
                    foreach (Slab supportingSlab in slab.SupportingConnections) {
                        Queue.Enqueue(supportingSlab);
                    }
                    while (Queue.Count > 0) {
                        Slab currentSlab = Queue.Dequeue();
                        //if there are grounded connections then we can only add this slab to the chain if all the grounded connections are in the chain
                        bool CanAdd = true;
                        foreach (Slab groundedSlab in currentSlab.GroundConnections) {
                            if (!Chain.Contains(groundedSlab)) {
                                CanAdd = false;
                            }
                        }
                        if (CanAdd) {
                            Chain.Add(currentSlab);
                            foreach (Slab supportingSlab in currentSlab.SupportingConnections) {
                                Queue.Enqueue(supportingSlab);
                            }
                        }
                    }
                    result += Chain.Count-1;
                }
                return result;
            }
            public int CountDeletableSections() {
                HashSet<Slab> deletableSections = new HashSet<Slab>();
                foreach (Slab slab in Slabs) {
                    if (slab.SupportingConnections.Count == 0) {
                        deletableSections.Add(slab);
                    }
                    bool CanDelete = true;
                    foreach (Slab supportingSlab in slab.SupportingConnections) {
                        if (supportingSlab.GroundConnections.Count == 1) {
                            CanDelete = false;
                        }
                    }
                    if (CanDelete) {
                        deletableSections.Add(slab);
                    }
                }
                return deletableSections.Count;
            }
            public void UpdateFallingAndGroundedSlabs() {
                FallingSlabs = new List<Slab>();
                GroundedSlabs = new List<Slab>();
                foreach (Slab slab in Slabs) {
                    if (slab.Grounded) {
                        GroundedSlabs.Add(slab);
                    }
                    else {
                        FallingSlabs.Add(slab);
                    }
                }
            }
            public void drawSlabSpace() {
                for (int z = 0; z < SlabSpace.GetLength(2); z++) {
                    Console.WriteLine($"z={z}");
                    for (int x = 0; x < SlabSpace.GetLength(0); x++) {
                        for (int y = 0; y < SlabSpace.GetLength(1); y++) {
                            if (SlabSpace[x, y, z] != null) {
                                Console.Write(SlabSpace[x, y, z].id);
                            }
                            else {
                                Console.Write(".");
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
            internal class Slab {
                public int id { get; set; }
                public int YSize { get; set; }
                public int XSize { get; set; }
                public int ZSize { get; set; }
                public bool Grounded { get; set; }
                public override string ToString() {
                    return $"id: {id} position: {position} height: {YSize} width: {XSize} depth: {ZSize} Grounded: {Grounded}";
                }
                public HashSet<Slab> SupportingConnections { get; set; } = new HashSet<Slab>();
                public HashSet<Slab> GroundConnections { get; set; } = new HashSet<Slab>();
                public (int x, int y, int z) position { get; set; }
                public Slab(string data, int id) { //7,1,204~7,4,204 x, y, z
                    this.id = id;
                    string[] coords = data.Split('~');
                    int[] pointA = coords[0].Split(',').Select(x => int.Parse(x)).ToArray();
                    int[] pointB = coords[1].Split(',').Select(x => int.Parse(x)).ToArray();
                    //determin the point at the top left of the slab, only one plane will be different
                    if (pointA[0] != pointB[0]) {
                        //the object spans the x asix so the top left is the smaller x
                        if (pointA[0] < pointB[0]) {
                            position = (pointA[0], pointA[1], pointA[2]);
                        }
                        else {
                            position = (pointB[0], pointB[1], pointB[2]);
                        }

                    }
                    else if (pointA[1] != pointB[1]) {
                        //the object spans the y asix so the top left is the smaller y
                        if (pointA[1] < pointB[1]) {
                            position = (pointA[0], pointA[1], pointA[2]);
                        }
                        else {
                            position = (pointB[0], pointB[1], pointB[2]);
                        }
                    }
                    else {
                        //the object spans the z asix so the top left is the smaller z
                        if (pointA[2] < pointB[2]) {
                            position = (pointA[0], pointA[1], pointA[2]);
                        }
                        else {
                            position = (pointB[0], pointB[1], pointB[2]);
                        }
                    }
                    //determine the height, depth and width of the slab
                    XSize = Math.Abs(pointA[0] - pointB[0]) + 1;
                    YSize = Math.Abs(pointA[1] - pointB[1]) + 1;
                    ZSize = Math.Abs(pointA[2] - pointB[2]) + 1;

                }
            }
        }
    }

}