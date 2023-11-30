using AdventOfCode.Year2022;

namespace AdventOfTests._2022_Tests
{
    public class Day25_Test
    {
        Day day;
        long IntValue;
        string SnafuValue;
        [SetUp]
        public void Setup()
        {
            day = new Day25(25);
        }
        [Test]
        public void RunPartOneSample()
        {
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("2=-1=0"));
        }
        [Test]
        public void RunPartOneFull()
        {
            day.RunPart1(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("121=2=1==0=10=2-20=2"));
        }
        [Test]
        public void RunPartTwoSample()
        {
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo(""));
        }
        [Test]
        public void RunPartTwoFull()
        {
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo(""));
        }


        [Test]
        public void SnafuChecks()
        {
            SnafuValue = "2=-01";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(976));

            SnafuValue = "1=-0-2";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(1747));

            SnafuValue = "12111";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(906));

            SnafuValue = "2=0=";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(198));

            SnafuValue = "21";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(11));

            SnafuValue = "2=01";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(201));

            SnafuValue = "111";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(31));

            SnafuValue = "20012";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(1257));

            SnafuValue = "112";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(32));

            SnafuValue = "1-12";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(107));

            SnafuValue = "12";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(7));

            SnafuValue = "1=";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(3));

            SnafuValue = "122";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(37));

            SnafuValue = "1121-1110-1=0";
            IntValue = Day25.SNAFU.ConvertFromSnafu(SnafuValue);
            Assert.That(IntValue, Is.EqualTo(314159265));
        }
        //need to test the revers of SnafuChecks
        [Test]
        public void IntChecks()
        {
            IntValue = 976;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("2=-01"));

            IntValue = 1747;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("1=-0-2"));

            IntValue = 906;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("12111"));

            IntValue = 198;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("2=0="));

            IntValue = 11;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("21"));

            IntValue = 201;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("2=01"));

            IntValue = 31;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("111"));

            IntValue = 1257;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("20012"));

            IntValue = 32;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("112"));

            IntValue = 107;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("1-12"));

            IntValue = 7;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("12"));

            IntValue = 3;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("1="));

            IntValue = 37;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("122"));

            IntValue = 314159265;
            SnafuValue = Day25.SNAFU.ConvertFromInt(IntValue);
            Assert.That(SnafuValue, Is.EqualTo("1121-1110-1=0"));

        }
    }
}
