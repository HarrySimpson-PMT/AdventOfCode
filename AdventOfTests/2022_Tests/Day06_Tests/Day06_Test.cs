using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day06_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day06(6);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("7"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("1896"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("19"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("3452"));
        }

    }
}
