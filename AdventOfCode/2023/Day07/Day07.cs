
namespace AdventOfCode.Year2023 {
    public class Day07 : Day {
        public Day07(int today, int year) : base(today, year) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfPoker elfPoker = new ElfPoker();
            result = elfPoker.PlayNormal(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            ElfPoker elfPoker = new ElfPoker();
            result = elfPoker.PlayWildCards(data).ToString();
            Console.WriteLine(result);
        }
        public class ElfPoker {
            public List<Hand> hands = new List<Hand>();
            public int PlayWildCards(string[] data) {
                foreach (string s in data) {
                    string hand = s.Split(' ')[0];
                    int bet = int.Parse(s.Split(" ")[1]);
                    hands.Add(new Hand(hand, bet, true));
                }
                int result = 0;
                hands.Sort(new UpdatedHandComparer());
                for (int i = 0; i < hands.Count; i++) {
                    result += (i + 1) * hands[i].bet;
                }
                return result;
            }
            public int PlayNormal(string[] data) {
                foreach (string s in data) {
                    string hand = s.Split(' ')[0];
                    int bet = int.Parse(s.Split(" ")[1]);
                    hands.Add(new Hand(hand, bet));
                }
                int result = 0;
                hands.Sort(new HandComparer());
                for (int i = 0; i < hands.Count; i++) {
                    result += (i + 1) * hands[i].bet;
                }
                return result;
            }
            public class UpdatedHandComparer : IComparer<Hand> {
                public int Compare(Hand x, Hand y) {
                    // First compare by Strength
                    int strengthComparison = x.Strength.CompareTo(y.Strength);
                    if (strengthComparison != 0) {
                        return strengthComparison;
                    }

                    // If strength is equal, compare by cards
                    string xCards = x.Cards;
                    string yCards = y.Cards;

                    // Define a custom dictionary to map card values
                    Dictionary<char, int> cardValues = new Dictionary<char, int>()
    {
      { 'J', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 },
      { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 },
      { '9', 9 }, { 'T', 10 }, { 'Q', 11 }, { 'K', 12 },
      { 'A', 13 }
    };

                    // Compare each character in the cards
                    for (int i = 0; i < Math.Min(xCards.Length, yCards.Length); i++) {
                        char xCard = xCards[i];
                        char yCard = yCards[i];

                        int xValue = cardValues.ContainsKey(xCard) ? cardValues[xCard] : 0;
                        int yValue = cardValues.ContainsKey(yCard) ? cardValues[yCard] : 0;

                        int cardComparison = xValue.CompareTo(yValue);
                        if (cardComparison != 0) {
                            return cardComparison;
                        }
                    }

                    // If all cards match, compare by card length (longer hand wins)
                    return xCards.Length.CompareTo(yCards.Length);
                }
            }
            public class HandComparer : IComparer<Hand> {
                public int Compare(Hand x, Hand y) {
                    // First compare by Strength
                    int strengthComparison = x.Strength.CompareTo(y.Strength);
                    if (strengthComparison != 0) {
                        return strengthComparison;
                    }

                    // If strength is equal, compare by cards
                    string xCards = x.Cards;
                    string yCards = y.Cards;

                    // Define a custom dictionary to map card values
                    Dictionary<char, int> cardValues = new Dictionary<char, int>()
    {
      { '2', 1 }, { '3', 2 }, { '4', 3 }, { '5', 4 },
      { '6', 5 }, { '7', 6 }, { '8', 7 }, { '9', 8 },
      { 'T', 9 }, { 'J', 10 }, { 'Q', 11 }, { 'K', 12 },
      { 'A', 13 }
    };

                    // Compare each character in the cards
                    for (int i = 0; i < Math.Min(xCards.Length, yCards.Length); i++) {
                        char xCard = xCards[i];
                        char yCard = yCards[i];

                        int xValue = cardValues.ContainsKey(xCard) ? cardValues[xCard] : 0;
                        int yValue = cardValues.ContainsKey(yCard) ? cardValues[yCard] : 0;

                        int cardComparison = xValue.CompareTo(yValue);
                        if (cardComparison != 0) {
                            return cardComparison;
                        }
                    }

                    // If all cards match, compare by card length (longer hand wins)
                    return xCards.Length.CompareTo(yCards.Length);
                }
            }
            public class Hand {
                public int bet { get; set; }
                public int Strength { get; set; } = 0;
                public string Cards { get; set; }
                int a = 0;
                int b = 0;
                public override string ToString() {
                    return $"Hand: {Cards}; Strength {Strength}; Bet {bet};";
                }
                public Hand(string hand, int bet, bool wildcards = false) {
                    Cards = hand;
                    this.bet = bet;
                    int[] cards = new int[13];
                    int WildCardCount = 0;
                    foreach (char c in hand) {
                        switch (c) {
                            case '2':
                                cards[0]++;
                                break;
                            case '3':
                                cards[1]++; break;
                            case '4':
                                cards[2]++; break;
                            case '5':
                                cards[3]++; break;
                            case '6':
                                cards[4]++; break;
                            case '7':
                                cards[5]++; break;
                            case '8':
                                cards[6]++; break;
                            case '9':
                                cards[7]++; break;
                            case 'T':
                                cards[8]++; break;
                            case 'J':
                                if (wildcards) {
                                    WildCardCount++;
                                }
                                else {
                                    cards[9]++;
                                }
                                break;
                            case 'Q':
                                cards[10]++; break;
                            case 'K':
                                cards[11]++; break;
                            case 'A':
                                cards[12]++; break;
                        }
                    }
                    foreach (int scoringset in cards) {
                        if (scoringset > a) {
                            b = a;
                            a = scoringset;
                        }
                        else if (scoringset > b) {
                            b = scoringset;
                        }
                    }
                    a += WildCardCount;
                    if (a == 5)
                        Strength = 7;
                    else if (a == 4)
                        Strength = 6;
                    else if (a == 3 && b == 2)
                        Strength = 5;
                    else if (a == 3 && b < 2)
                        Strength = 4;
                    else if (a == 2 && b == 2)
                        Strength = 3;
                    else if (a == 2 && b < 2)
                        Strength = 2;
                    else
                        Strength = 1;
                }
            }
        }
    }
}


