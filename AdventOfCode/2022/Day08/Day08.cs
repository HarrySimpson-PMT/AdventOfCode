using System.Diagnostics;

namespace AdventOfCode.Year2022 {
    public class Day08 : Day {

        public Day08(int today) : base(today) {

        }
        public override void RunPart1(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            forest forest = new();
            forest.CountVisibleTrees(data);
            result = forest.Visible.ToString();
            Console.WriteLine(result);
        }
        public override void RunPart2(ArgumentType argumentType) {
            string[] data = argumentType == ArgumentType.Sample ? Sample : Full;
            forest forest = new();
            forest.CalculateScores(data);
            result = forest.MaxScore.ToString();
            Console.WriteLine(result);
        }
        public class forest {
            public int Hidden { get; set; } = 0;
            public int Visible { get; set; } = 0;
            bool[,] _HiddenTreeGrid = new bool[0, 0];
            int[,] _TreeScores = new int[0, 0];
            public int MaxScore { get; set; } = 0;
            public void CalculateScores(string[] data) {
                int n = data.Length, m = data[0].Length;
                _TreeScores = new int[n, m];
                //mark all as 1
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        _TreeScores[i, j] = 1;

                int max = 0;
                Stack<(int, int)> visible = new();
                for (int i = 0; i < n; i++) {
                    visible = new();
                    visible.Push(new(0, 0));
                    for (int j = 0; j < m; j++) {
                        int height = data[i][j];
                        int curvis = 0;
                        while (visible.Count > 0 && data[i][j] > visible.Peek().Item1) {
                            curvis += visible.Pop().Item2;
                        }
                        if (visible.Count > 0)
                            _TreeScores[i, j] *= (curvis + 1);
                        else
                            _TreeScores[i, j] *= curvis;
                        visible.Push((height, curvis + 1));
                    }
                    //right to left
                    visible = new();
                    visible.Push(new(0, 0));
                    for (int j = m - 1; j >= 0; j--) {
                        int height = data[i][j];
                        int curvis = 0;
                        while (visible.Count > 0 && data[i][j] > visible.Peek().Item1) {
                            curvis += visible.Pop().Item2;
                        }
                        if (visible.Count > 0)
                            _TreeScores[i, j] *= (curvis + 1);
                        else
                            _TreeScores[i, j] *= curvis;
                        visible.Push((height, curvis + 1));
                    }
                }
                for (int j = 0; j < m; j++) {
                    visible = new();
                    visible.Push(new(0, 0));
                    for (int i = 0; i < n; i++) {
                        int height = data[i][j];
                        int curvis = 0;
                        while (visible.Count > 0 && data[i][j] > visible.Peek().Item1) {
                            curvis += visible.Pop().Item2;
                        }
                        if (visible.Count > 0)
                            _TreeScores[i, j] *= (curvis + 1);
                        else
                            _TreeScores[i, j] *= curvis;
                        visible.Push((height, curvis + 1));
                    }
                    //right to left
                    visible = new();
                    visible.Push(new(0, 0));
                    for (int i = n - 1; i >= 0; i--) {
                        int height = data[i][j];
                        int curvis = 0;
                        while (visible.Count > 0 && data[i][j] > visible.Peek().Item1) {
                            curvis += visible.Pop().Item2;
                        }
                        if (visible.Count > 0)
                            _TreeScores[i, j] *= (curvis + 1);
                        else
                            _TreeScores[i, j] *= curvis;
                        visible.Push((height, curvis + 1));
                    }
                }
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < m; j++) {
                        MaxScore = Math.Max(_TreeScores[i, j], MaxScore);
                    }
                }
            }
            public void CountVisibleTrees(string[] data) {
                int n = data.Length, m = data[0].Length;
                _HiddenTreeGrid = new bool[n, m];
                //mark all as hidden
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        _HiddenTreeGrid[i, j] = true;
                //we need to scan the forest from each direction and update the hidden tree grid
                //left to right
                int max = -1;
                for (int i = 0; i < n; i++) {
                    max = -1;
                    for (int j = 0; j < m; j++) {
                        if (data[i][j] > max) {
                            max = data[i][j];
                            if (_HiddenTreeGrid[i, j]) {
                                Visible++;
                                _HiddenTreeGrid[i, j] = false;
                            }
                        }
                    }
                    //right to left
                    max = -1;
                    for (int j = m - 1; j >= 0; j--) {
                        if (data[i][j] > max) {
                            max = data[i][j];
                            if (_HiddenTreeGrid[i, j]) {
                                Visible++;
                                _HiddenTreeGrid[i, j] = false;
                            }
                        }
                    }
                }
                for (int j = 0; j < m; j++) {
                    max = -1;
                    for (int i = 0; i < n; i++) {
                        if (data[i][j] > max) {
                            max = data[i][j];
                            if (_HiddenTreeGrid[i, j]) {
                                Visible++;
                                _HiddenTreeGrid[i, j] = false;
                            }
                        }
                    }
                    //right to left
                    max = -1;
                    for (int i = n - 1; i >= 0; i--) {
                        if (data[i][j] > max) {
                            max = data[i][j];
                            if (_HiddenTreeGrid[i, j]) {
                                Visible++;
                                _HiddenTreeGrid[i, j] = false;
                            }
                        }
                    }
                }
            }
        }
    }

}