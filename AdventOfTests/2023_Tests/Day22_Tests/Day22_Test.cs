using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests {
    public class Day22_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day22(22, 2023);
        }
        [Test]
        public void RunPartOneSample() {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("5"));
        }
        [Test]
        public void RunPartOneFull() {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("503"));
        }
        [Test]
        public void RunPartTwoSample() {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("7"));
        }
        [Test]
        public void RunPartTwoFull() {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("98431"));
        }

    }
}
