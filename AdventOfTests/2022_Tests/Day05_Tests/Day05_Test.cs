using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day05_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day05(5);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("CMZ"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("VCTFTJQCG"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("MCD"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("GCFGLDNJZ"));
        }

    }
}
