using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests {
    public class Day21_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day21(21);
        }
        [Test]
        public void RunPartOneSample() {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("152"));
        }
        [Test]
        public void RunPartOneFull() {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("223971851179174"));
        }
        [Test]
        public void RunPartTwoSample() {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("301"));
        }
        [Test]
        public void RunPartTwoFull() {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("3379022190351"));
        }
    }
}
