
using System.Numerics;
using Microsoft.Z3;

namespace AdventOfCode.Year2023 {
    public class Day24 : Day {
        public decimal start = 0;
        public decimal end = 1000000;
        public Day24(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            HailSpace space = new HailSpace(data, start, end);
            result = space.CrossingHailXY().ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            HailSpace space = new HailSpace(data, start, end);
            space.LimitSpace();
            result = space.FindIntersectionBetweenAllLines().ToString();
            Console.WriteLine(result);
        }
        public class HailSpace {
            internal List<Hail> Hails;
            internal decimal limitlow = 0;
            internal decimal limithigh = 0;
            internal decimal xlow = decimal.MaxValue;
            internal decimal xhigh = decimal.MinValue;
            internal decimal ylow = decimal.MaxValue;
            internal decimal yhigh = decimal.MinValue;
            internal decimal zlow = decimal.MaxValue;
            internal decimal zhigh = decimal.MinValue;
            internal HailSpace(string[] data, decimal start, decimal end) {
                Hails = new List<Hail>();
                limitlow = start;
                limithigh = end;
                foreach (string line in data) {
                    Hails.Add(new Hail(line));
                }
            }
            public void LimitSpace() {
                foreach (Hail hail in Hails) {
                    if (hail.x1 < xlow) { xlow = hail.x1; }
                    if (hail.x1 > xhigh) { xhigh = hail.x1; }
                    if (hail.y1 < ylow) { ylow = hail.y1; }
                    if (hail.y1 > yhigh) { yhigh = hail.y1; }
                    if (hail.z1 < zlow) { zlow = hail.z1; }
                    if (hail.z1 > zhigh) { zhigh = hail.z1; }
                }
                long width = (long)(xhigh - xlow);
                long height = (long)(yhigh - ylow);
                long depth = (long)(zhigh - zlow);
                Console.WriteLine($"width: {width} height: {height} depth: {depth}");
            }
            public long FindIntersectionBetweenAllLines() {
                var ctx = new Context();
                var solver = ctx.MkSolver();
                var x = ctx.MkRealConst("x");
                var y = ctx.MkRealConst("y");
                var z = ctx.MkRealConst("z");
                var vx = ctx.MkRealConst("vx");
                var vy = ctx.MkRealConst("vy");
                var vz = ctx.MkRealConst("vz");
                for(int i = 0; i < 3; i++) {
                    var t = ctx.MkRealConst($"t{i}");
                    Hail hail1 = Hails[i];
                    var px = ctx.MkInt(hail1.x1);
                    var py = ctx.MkInt(hail1.y1);
                    var pz = ctx.MkInt(hail1.z1);

                    var pvx = ctx.MkInt(hail1.vx);
                    var pvy = ctx.MkInt(hail1.vy);
                    var pvz = ctx.MkInt(hail1.vz);

                    var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
                    var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
                    var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz

                    var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
                    var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
                    var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz

                    solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
                    solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
                    solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
                    solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
                }
                solver.Check();
                var model = solver.Model;

                var rx = model.Eval(x);
                var ry = model.Eval(y);
                var rz = model.Eval(z);

                return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
            }
            public int CrossingHailXY() {
                HashSet<Hail> checkedHails = new HashSet<Hail>();
                int count = 0;
                foreach (Hail hail1 in Hails) {
                    checkedHails.Add(hail1);
                    foreach (Hail hail2 in Hails) {
                        if (checkedHails.Contains(hail2)) { continue; }
                        if (intersect(hail1, hail2)) { count++; }
                    }
                }
                return count;
            }
            internal bool intersect(Hail haila, Hail hailb) {
                decimal test = 1m * 20 / -40;
                decimal HailASlope = 1m * (haila.SecondPosition.y - haila.StartPosition.y) / (haila.SecondPosition.x - haila.StartPosition.x);
                decimal yInterceptA = 1m * haila.SecondPosition.y - (HailASlope * haila.SecondPosition.x);
                decimal HailBSlope = 1m * (hailb.SecondPosition.y - hailb.StartPosition.y) / (hailb.SecondPosition.x - hailb.StartPosition.x);
                decimal yInterceptB = 1m * hailb.SecondPosition.y - (HailBSlope * hailb.SecondPosition.x);
                //check if they are parallel
                if (Math.Abs(HailASlope - HailBSlope) < 0.0001m) {
                    if (Math.Abs(yInterceptA - yInterceptB) < 0.0001m) { return true; }
                    else { return false; }
                }
                decimal x = (yInterceptB - yInterceptA) / (HailASlope - HailBSlope);
                decimal y = (HailASlope * x) + yInterceptA;
                //we need to check if the intersection occurs after the start of each hail


                if (x >= limitlow && x <= limithigh && y >= limitlow && y <= limithigh) {
                    //check if the distance has the same sign as the velocity
                    if (Math.Sign(haila.vx) == Math.Sign(x - haila.StartPosition.x) && Math.Sign(haila.vy) == Math.Sign(y - haila.StartPosition.y) &&
                                               Math.Sign(hailb.vx) == Math.Sign(x - hailb.StartPosition.x) && Math.Sign(hailb.vy) == Math.Sign(y - hailb.StartPosition.y)) {
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            internal class Hail {
                //input 19, 13, 30 @ -2,  1, -2
                internal long x1;
                internal long y1;
                internal long z1;
                internal (long x, long y, long z) StartPosition => (x1, y1, z1);
                internal long vx;
                internal long vy;
                internal long vz;
                internal (long x, long y, long z) velocity => (vx, vy, vz);
                internal long x2;
                internal long y2;
                internal long z2;
                internal (long x, long y, long z) SecondPosition => (x2, y2, z2);

                public override string ToString() {
                    return $"{x1},{y1},{z1} @ {vx},{vy},{vz}";
                }
                internal Hail(string data) { //19, 13, 30 @ -2,  1, -2
                    string[] parts = data.Split('@');
                    string[] pos = parts[0].Split(',');
                    string[] vel = parts[1].Split(',');
                    x1 = long.Parse(pos[0]);
                    y1 = long.Parse(pos[1]);
                    z1 = long.Parse(pos[2]);
                    vx = long.Parse(vel[0]);
                    vy = long.Parse(vel[1]);
                    vz = long.Parse(vel[2]);
                    x2 = x1 + (vx * 2);
                    y2 = y1 + (vy * 2);
                    z2 = z1 + (vz * 2);
                }
            }
        }

    }
}