namespace AdventOfCode.Year2022
{
    public class Day05 : Day
    {

        public Day05(int today) : base(today)
        {
            
        }
        public override void RunPart1(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            DockManager dockManager = new(data);
            dockManager.RunInstructions();
            result = dockManager.result;
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType)
        {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            DockManager dockManager = new(data);
            dockManager.RunCorrectInstructions();
            result = dockManager.result;
            Console.WriteLine(result);
        }
        public class DockManager
        {
            public List<Stack<char>> DockStacks;
            public List<string> instructions = new();
            public string result = "";
            public DockManager(string[] data)
            {
                //find empty line and split into instructions
                int emptyLine = Array.IndexOf(data, "");
                instructions = data.Skip(emptyLine + 1).ToList();
                //now we need to create the stacks, we will use the line above the empty line to determine the number of stacks
                //the last int in the line will be the number of stacks
                string line = data[emptyLine - 1].Replace(" ", "");
                int numberOfStacks = int.Parse(line[^1].ToString());
                DockStacks = new List<Stack<char>>();
                for (int i = 0; i < numberOfStacks; i++)
                {
                    DockStacks.Add(new Stack<char>());
                }
                for (int i = emptyLine - 2; i >= 0; i--)
                {
                    for (int j = 1; j <= (numberOfStacks * 4) - 1; j += 4)
                    {
                        if (data[i][j] != ' ' && data[i][j] != '[' && data[i][j] != ']')
                            DockStacks[j / 4].Push(data[i][j]);
                    }
                }
            }
            public void RunInstructions()
            {
                foreach (string instruction in instructions)
                {
                    //instruction format = "move 1 from 2 to 1"
                    string[] InstructionSplit = instruction.Split(' ');
                    int move = int.Parse(InstructionSplit[1]);
                    int from = int.Parse(InstructionSplit[3]) - 1;
                    int to = int.Parse(InstructionSplit[5]) - 1;
                    for (int i = move; i > 0; i--)
                    {
                        DockStacks[to].Push(DockStacks[from].Pop());
                    }
                }
                foreach (Stack<char> DockStack in DockStacks)
                {
                    if (DockStack.Count > 0)
                        result += DockStack.Pop();
                }
            }
            public void RunCorrectInstructions()
            {
                foreach (string instruction in instructions)
                {
                    //instruction format = "move 1 from 2 to 1"
                    Stack<char> crane = new();
                    string[] InstructionSplit = instruction.Split(' ');
                    int move = int.Parse(InstructionSplit[1]);
                    int from = int.Parse(InstructionSplit[3]) - 1;
                    int to = int.Parse(InstructionSplit[5]) - 1;
                    for (int i = move; i > 0; i--)
                    {
                        crane.Push(DockStacks[from].Pop());
                    }
                    for (int i = move; i > 0; i--)
                    {
                        DockStacks[to].Push(crane.Pop());
                    }
                }
                foreach (Stack<char> DockStack in DockStacks)
                {
                    if (DockStack.Count > 0)
                        result += DockStack.Pop();
                }

            }
        }
    }
}