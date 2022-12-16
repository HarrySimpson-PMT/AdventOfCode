using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests.Day15_Tests
{
    public class Day13_Test
    {
        Day day;
        [SetUp]
        public void Setup()
        {
            day = new Day13(13);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("13"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("6428"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("140"));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("22464"));
        }

    }
}
