using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day22_Test
    {
        Day day;
        Day22.MonkeyMap monkeyMap;
        int face;
        (int x, int y) Coords;
        [SetUp]
        public void Setup()
        {
            day = new Day22(22);
            monkeyMap = new(day.Sample, true);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("6032"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("76332"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("5031"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("144012"));
        }
        [Test]
        public void Face_0_Section0()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (1,11), ref face);
            Assert.That(Coords, Is.EqualTo((10, 15)));
            Assert.That(face, Is.EqualTo(2));
        }
        [Test]
        public void Face_0_Section0B()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (2, 11), ref face);
            Assert.That(Coords, Is.EqualTo((9, 15)));
            Assert.That(face, Is.EqualTo(2));
        }
        [Test]
        public void Face_0_Section1()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (5, 11), ref face);
            Assert.That(Coords, Is.EqualTo((8, 14)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_0_Section1B()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 11), ref face);
            Assert.That(Coords, Is.EqualTo((8, 15)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_0_Section2()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 15), ref face);
            Assert.That(Coords, Is.EqualTo((0, 11)));
            Assert.That(face, Is.EqualTo(2));
        }
        [Test]
        public void Face_0_Section2B()
        {
            face = 0;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (8, 15), ref face);
            Assert.That(Coords, Is.EqualTo((3, 11)));
            Assert.That(face, Is.EqualTo(2));
        }
        [Test]
        public void Face_1_Section0()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (7, 1), ref face);
            Assert.That(Coords, Is.EqualTo((11, 10)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_1_Section0B()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (7, 3), ref face);
            Assert.That(Coords, Is.EqualTo((11, 8)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_1_Section1()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (7, 4), ref face);
            Assert.That(Coords, Is.EqualTo((11, 8)));
            Assert.That(face, Is.EqualTo(0));
        }
        [Test]
        public void Face_1_Section1B()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (7, 7), ref face);
            Assert.That(Coords, Is.EqualTo((8, 8)));
            Assert.That(face, Is.EqualTo(0));
        }
        [Test]
        public void Face_1_Section2()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 9), ref face);
            Assert.That(Coords, Is.EqualTo((7, 2)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_1_Section2B()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 11), ref face);
            Assert.That(Coords, Is.EqualTo((7, 0)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_1_Section3()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 15), ref face);
            Assert.That(Coords, Is.EqualTo((4, 0)));
            Assert.That(face, Is.EqualTo(0));
        }
        [Test]
        public void Face_1_Section3B()
        {
            face = 1;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 12), ref face);
            Assert.That(Coords, Is.EqualTo((7, 0)));
            Assert.That(face, Is.EqualTo(0));
        }

        [Test]
        public void Face_2_Section0()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (0, 8), ref face);
            Assert.That(Coords, Is.EqualTo((4, 4)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_2_Section0B()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (3, 8), ref face);
            Assert.That(Coords, Is.EqualTo((4, 7)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_2_Section1()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 0), ref face);
            Assert.That(Coords, Is.EqualTo((11, 15)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_2_Section1B()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (7, 0), ref face);
            Assert.That(Coords, Is.EqualTo((11, 12)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_2_Section2()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (8, 8), ref face);
            Assert.That(Coords, Is.EqualTo((7, 7)));
            Assert.That(face, Is.EqualTo(3));
        }
        [Test]
        public void Face_2_Section2B()
        {
            face = 2;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (11, 8), ref face);
            Assert.That(Coords, Is.EqualTo((7, 4)));
            Assert.That(face, Is.EqualTo(3));
        }

        [Test]
        public void Face_3_Section0()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 0), ref face);
            Assert.That(Coords, Is.EqualTo((0, 11)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_3_Section0B()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 3), ref face);
            Assert.That(Coords, Is.EqualTo((0, 8)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_3_Section1()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 4), ref face);
            Assert.That(Coords, Is.EqualTo((0, 8)));
            Assert.That(face, Is.EqualTo(0));
        }
        [Test]
        public void Face_3_Section1B()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (4, 7), ref face);
            Assert.That(Coords, Is.EqualTo((3, 8)));
            Assert.That(face, Is.EqualTo(0));
        }
        [Test]
        public void Face_3_Section2()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (0, 8), ref face);
            Assert.That(Coords, Is.EqualTo((4, 3)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_3_Section2B()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (0, 11), ref face);
            Assert.That(Coords, Is.EqualTo((4, 0)));
            Assert.That(face, Is.EqualTo(1));
        }
        [Test]
        public void Face_3_Section3()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (8, 12), ref face);
            Assert.That(Coords, Is.EqualTo((7, 11)));
            Assert.That(face, Is.EqualTo(2));
        }
        [Test]
        public void Face_3_Section3B()
        {
            face = 3;
            Coords = Day22.MonkeyMap.GlobeWrap(monkeyMap.Map, (8, 15), ref face);
            Assert.That(Coords, Is.EqualTo((4, 11)));
            Assert.That(face, Is.EqualTo(2));
        }
    }
}
