using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests
{
    public class Day10Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day10(10, 2023);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("80"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("7012"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("10"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("395"));
        }

    }
}
