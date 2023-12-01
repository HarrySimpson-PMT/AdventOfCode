namespace AdventOfCode.Year2022
{
    public class Day02 : Day
    {

        public Day02(int today) : base(today)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RPSTournament tournament = new RPSTournament();
            tournament.PlayUsingStrategy(data);
            result = tournament.Score.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            RPSTournament tournament = new RPSTournament();
            tournament.PlayUsingUpdatedStrategy(data);
            result = tournament.Score.ToString();
            Console.WriteLine(result);
        }
        public class RPSTournament
        {
            public int Score { get; set; } = 0;
            public void PlayUsingStrategy(string[] data)
            {
                foreach (string line in data)
                {
                    //0 for loss, 3 for tie, 6 for win
                    char player1 = line[0]; //A rock, B Paper, C Scissors
                    char player2 = line[2]; //X rock 1, Y Paper 2, Z Scissors 3
                    switch (player1)
                    {
                        case 'A':
                            switch (player2)
                            {
                                case 'X': //tie
                                    Score += 3+1;
                                    break;
                                case 'Y': //win
                                    Score += 6+2;
                                    break;
                                case 'Z': //loss
                                    Score += 0+3;
                                    break;
                            }
                            break;
                        case 'B':
                            switch (player2)
                            {
                                case 'X': //loss
                                    Score += 0+1;
                                    break;
                                case 'Y':
                                    Score += 3+2;
                                    break;
                                case 'Z':
                                    Score += 6+3;
                                    break;
                            }
                            break;
                        case 'C':
                            switch (player2)
                            {
                                case 'X':
                                    Score += 6+1;
                                    break;
                                case 'Y':
                                    Score += 0+2;
                                    break;
                                case 'Z':
                                    Score += 3+3;
                                    break;
                            }
                            break;
                    }

                }
            }
            public void PlayUsingUpdatedStrategy(string[] data)
            {
                foreach (string line in data)
                {
                    //0 for loss, 3 for tie, 6 for win
                    char player1 = line[0]; //A rock 1, B Paper 2, C Scissors 3
                    char player2 = line[2]; //required result X lose, Y draw, Z win
                    switch (player1)
                    {
                        case 'A': //rock
                            switch (player2)
                            {
                                case 'X': //lose with scissors
                                    Score += 0 + 3;
                                    break;
                                case 'Y': //draw with rock
                                    Score += 3 + 1;
                                    break;
                                case 'Z': //win with paper
                                    Score += 6 + 2;
                                    break;
                            }
                            break;
                        case 'B': //paper
                            switch (player2)
                            {
                                case 'X': //lose with rock
                                    Score += 0 + 1;
                                    break;
                                case 'Y': //draw with paper
                                    Score += 3 + 2;
                                    break;
                                case 'Z': //win with scissors
                                    Score += 6 + 3;
                                    break;
                            }
                            break;
                        case 'C': //scissors
                            switch (player2)
                            {
                                case 'X': //lose with paper
                                    Score += 0 + 2;
                                    break;
                                case 'Y': //draw with scissors
                                    Score += 3 + 3;
                                    break;
                                case 'Z': //win with rock
                                    Score += 6 + 1;
                                    break;
                            }
                            break;
                    }

                }
            }

        }
    }

}