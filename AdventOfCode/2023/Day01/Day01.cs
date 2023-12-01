
using System.Diagnostics;

namespace AdventOfCode.Year2023
{
    public class Day01 : Day
    {
        public Day01(int today, int year) : base(today, year)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            TrebCalibrator calibrator = new TrebCalibrator();
            result = calibrator.Calibrate(data).ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            TrebCalibrator calibrator = new TrebCalibrator();
            result = calibrator.Calibrate2_0(data).ToString();
            Console.WriteLine(result);
        }
        public class TrebCalibrator
        {
            public int Calibrate(string[] data)
            {
                int result = 0;
                foreach (string line in data)
                {
                    int left = 0;
                    int right = line.Length - 1;
                    while (!"1234567890".Contains(line[left]))
                        left++;
                    while (!"1234567890".Contains(line[right]))
                        right--;
                    string number = line[left].ToString()+ line[right].ToString();
                    result += int.Parse(number);
                }
                return result;
            }
            public int Calibrate2_0(string[] data)
            {
                int result = 0;
                foreach (string line in data)
                {
                    int left = 0;
                    string leftvalue = "";
                    int right = line.Length - 1;
                    string rightvalue = "";
                    if (line == "cfmrfgjqgrzcmsix5")
                        Debugger.Break();
                    while (true)
                    {
                        if ("1234567890".Contains(line[left]))
                        {
                            leftvalue = line[left].ToString();
                            break;
                        }
                        if ("soften".Contains(line[left]))
                        {
                            if (left + 3 <= right)
                            {
                                if (line.Substring(left, 3) == "one")
                                {
                                    leftvalue = "1";
                                    break;
                                }
                                if (line.Substring(left, 3) == "two")
                                {
                                    leftvalue = "2";
                                    break;
                                }
                                if (line.Substring(left, 3) == "six")
                                {
                                    leftvalue = "6";
                                    break;
                                }
                            }
                            if(left+4<=right)
                            {
                                if (line.Substring(left, 4) == "four")
                                {
                                    leftvalue = "4";
                                    break;
                                }
                                if (line.Substring(left, 4) == "five")
                                {
                                    leftvalue = "5";
                                    break;
                                }
                                if (line.Substring(left, 4) == "nine")
                                {
                                    leftvalue = "9";
                                    break;
                                }
                            }
                            if (left + 5 <= right)
                            {
                                if (line.Substring(left, 5) == "three")
                                {
                                    leftvalue = "3";
                                    break;
                                }
                                if (line.Substring(left, 5) == "seven")
                                {
                                    leftvalue = "7";
                                    break;
                                }
                                if (line.Substring(left, 5) == "eight")
                                {
                                    leftvalue = "8";
                                    break;
                                }
                            }
                        }
                        left++;
                        if (left >= line.Length)
                            throw new Exception("left went too far");
                    }
                    while (true)
                    {
                        if ("1234567890".Contains(line[right]))
                        {
                            rightvalue = line[right].ToString();
                            break;
                        }
                        if ("eoxrnt".Contains(line[right]))
                        {
                            if (right - 3 >= left)
                            {
                                if (line.Substring(right - 2, 3) == "one")
                                {
                                    rightvalue = "1";
                                    break;
                                }
                                if (line.Substring(right - 2, 3) == "two")
                                {
                                    rightvalue = "2";
                                    break;
                                }
                                if (line.Substring(right - 2, 3) == "six")
                                {
                                    rightvalue = "6";
                                    break;
                                }
                            }
                            if (right - 4 >= left)
                            {
                                if (line.Substring(right - 3, 4) == "four")
                                {
                                    rightvalue = "4";
                                    break;
                                }
                                if (line.Substring(right - 3, 4) == "five")
                                {
                                    rightvalue = "5";
                                    break;
                                }
                                if (line.Substring(right - 3, 4) == "nine")
                                {
                                    rightvalue = "9";
                                    break;
                                }
                            }
                            if (right - 5 >= left)
                            {
                                if (line.Substring(right - 4, 5) == "three")
                                {
                                    rightvalue = "3";
                                    break;
                                }
                                if (line.Substring(right - 4, 5) == "seven")
                                {
                                    rightvalue = "7";
                                    break;
                                }
                                if (line.Substring(right - 4, 5) == "eight")
                                {
                                    rightvalue = "8";
                                    break;
                                }
                            }
                        }
                        right--;
                    }
                    string number = leftvalue + rightvalue;
                    if (number.Length < 2 || number[0] == number[1])
                        Console.WriteLine($"{number} {line}");
                    result += int.Parse(number);
                }
                return result;
            }
        }
    }
    

}