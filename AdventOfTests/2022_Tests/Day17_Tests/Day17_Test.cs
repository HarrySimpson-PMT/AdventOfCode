using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests {
    public class Day17_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day17(17);
        }
        [Test]
        public void RunPartOneSample() {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("3068"));
        }
        [Test]
        public void RunPartOneFull() {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("3211"));
        }
        [Test]
        public void RunPartTwoSample() {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("1514285714288"));
        }
        [Test]
        public void RunPartTwoFull() {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("1589142857183"));
        }

    }
}
