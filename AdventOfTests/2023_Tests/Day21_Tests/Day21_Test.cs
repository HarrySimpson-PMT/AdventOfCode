using AdventOfCode.Year2023;

namespace AdventOfTests._2023_Tests {
    public class Day21_Test {
        Day day;
        [SetUp]
        public void Setup() {
            day = new Day21(21, 2023);
        }
        [Test]
        public void RunPartOneSample() {
            (day as Day21).steps = 6;
            day.RunPart1(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("16"));
        }
        [Test]
        public void RunPartOneFull() {
            (day as Day21).steps = 64;
            day.RunPart1(ArgumentType.Full);
            
            Assert.That(day.result, Is.EqualTo("3746"));
        }
        [Test]
        public void RunPartTwoSample() {
            //(day as Day21).steps = 6;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("16"));

            //(day as Day21).steps = 10;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("50"));

            //(day as Day21).steps = 50;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("1594"));

            //(day as Day21).steps = 100;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("6536"));

            (day as Day21).steps = 500;
            day.RunPart2(ArgumentType.Sample);
            Assert.That(day.result, Is.EqualTo("-755498"));

            //Assert.That(day.result, Is.EqualTo("167004"));

            //(day as Day21).steps = 1000;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("668697"));

            //(day as Day21).steps = 5000;
            //day.RunPart2(ArgumentType.Sample);
            //Assert.That(day.result, Is.EqualTo("16733044"));
        }
        [Test]
        public void RunPartTwoFull() {

            //(day as Day21).steps = 65; //131              => 3889 X##

            //(day as Day21).steps = 196; //393             => 34504
            //(day as Day21).steps = 327; //655             => 95591


            //(day as Day21).steps = 589; //1179            => 309181

            //(day as Day21).steps = 1768; //3537           => 2778700


            // 3889x4 = 15596
            //34504- 15596 = 18908


            //53002731
            //(day as Day21).steps = 338089423; //626568939

            //131

            //530002731
            //131

            //32 => 3220743893 low
            //      3320744015 low
            //      4220744015 No?
            //      351161173431612
            // 26501365

            (day as Day21).steps = 26501365; // => 53002731   404601 202300
            day.RunPart2(ArgumentType.Full);
            Assert.That(day.result, Is.EqualTo("623540829615589"));
        }

    }
}
