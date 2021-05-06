using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PegSolitare
{
    class HelperClass
    {
        public int ReadFile(string fileName, out int[,] puzzle, out int rows, out int cols)
        {
            string text = File.ReadAllText(fileName);

            if(text == "")
            {
                Console.WriteLine($"Cannot open file {fileName}. Program terminates.\n");
                puzzle = null;
                rows = -1;
                cols = -1;
                return -1;
            }
            else
            {
                string[] nums = text.Split(" ");
                int testInput;
                bool correct = int.TryParse(nums[0],out testInput);
                if (!correct)
                {
                    Console.WriteLine("The rows are wrong");
                    puzzle = null;
                    rows = -1;
                    cols = -1;
                    return -1;
                }
                else
                {
                    rows = testInput;
                }

                correct = int.TryParse(nums[1], out testInput);
                Console.WriteLine(nums[1]);
                if (!correct)
                {
                    Console.WriteLine("The columns are wrong");
                    puzzle = null;
                    rows = -1;
                    cols = -1;
                    return -1;
                }
                else
                {
                    cols = testInput;
                }

                int row = 0;
                int col = 0;
                puzzle = new int[rows, cols];
                foreach (string num in nums)
                {
                    correct = int.TryParse(num, out testInput);
                    if (!correct)
                    {
                        Console.WriteLine($"The item {row},{col} is wrong.");
                        puzzle = null;
                        rows = -1;
                        cols = -1;
                        return -1;
                    }
                    else
                    {
                        puzzle[row,col] = testInput;
                        col++;
                        if(cols%col == 1)
                        {
                            col = 0;
                            row++;
                        }
                    }
                }
            }

            return 0;
        }

        public void WriteSolutionToFile(List<((int, int), (int, int))> solution, int moves, string outFile)
        {
            string lines = $"{moves}\n";
            foreach (var sol in solution)
            {
                lines += $"{sol.Item1.Item1+1} {sol.Item1.Item2+1} {sol.Item2.Item1 + 1} {sol.Item2.Item2 + 1}\n";
            }

            File.WriteAllText(outFile, lines);
        }

        public void PrintPuzzle(int[,] puzzle)
        {
            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    Console.Write($"{puzzle[i, j]} ");
                }
                Console.WriteLine("");
            }

            Console.WriteLine();
        }
    }
}
