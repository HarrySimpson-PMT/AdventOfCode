using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests {
    public class Day24_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day24(24, 2023);
        }
        [Test]
        public void RunPartOneSample() {
            (day as Day24).start = 7;
            (day as Day24).end = 27;
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("2"));
        }
        [Test]
        public void RunPartOneFull() {
            (day as Day24).start = 200000000000000;
            (day as Day24).end = 400000000000000;
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("16502"));
        }
        [Test]
        public void RunPartTwoSample() {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("47"));
        }
        [Test]
        public void RunPartTwoFull() {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("673641951253289"));
        }

    }
}
