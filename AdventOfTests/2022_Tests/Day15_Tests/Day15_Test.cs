using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day15_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day15(15);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("26"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("5525990"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("56000011"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("11756174628223"));
        }

    }
}
