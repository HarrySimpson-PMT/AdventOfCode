using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests
{
    public class Day19_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day19(19, 2023);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("19114"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("420739"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("167409079868000"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("130251901420382")); //129323296664401 <-> 149323296664401
        }

    }
}
