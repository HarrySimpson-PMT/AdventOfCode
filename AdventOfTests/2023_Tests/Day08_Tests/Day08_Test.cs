using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests {
    public class Day08_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day08(8, 2023);
        }
        [Test]
        public void RunPartOneSample() {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("3"));
        }
        [Test]
        public void RunPartOneFull() {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("19667"));
        }
        [Test]
        public void RunPartTwoSample() {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("6"));
        }
        [Test]
        public void RunPartTwoFull() {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("19185263738117"));
        }

    }
}
