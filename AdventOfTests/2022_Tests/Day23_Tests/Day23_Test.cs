using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day23_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day23(23);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("110"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("3987")); //first : 4068 => issue was with empty X_Entity entry, gotta do better about being sloppy.
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("20"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("938"));
        }
    }
}
