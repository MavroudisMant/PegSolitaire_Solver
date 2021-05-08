using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PegSolitare
{
    class HelperClass
    {
        public int[,] ReadFile(string fileName)
        {
            int rows, cols;
            int[,] puzzle;
            string[] lines;
            try
            {
               lines  = File.ReadAllLines(fileName);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The given file could not be found");
                return null;
                
            }
            

            if(lines.Length == 0)
            {
                Console.WriteLine($"File was empty");
                return null;
            }
            else
            {
                string[] nums = lines[0].Split(" ");
                int testInput;
                bool correct = int.TryParse(nums[0],out testInput);
                if (!correct)
                {
                    Console.WriteLine("The rows are wrong");
                    return null;
                }
                else
                {
                    rows = testInput;
                }

                correct = int.TryParse(nums[1], out testInput);
                if (!correct)
                {
                    Console.WriteLine("The columns are wrong");
                    return null;
                }
                else
                {
                    cols = testInput;
                }

                puzzle = new int[rows, cols];
                //Used in case the file is not separated into lines
                int puzzleRow = 0;
                foreach (string line in lines)
                {
                    int col = 0;
                    
                    foreach (string num in line.Split(" "))
                    {
                        if (num != "")
                        {
                            correct = int.TryParse(num, out testInput);
                            if (!correct)
                            {
                                Console.WriteLine($"The item {puzzleRow+1},{col+1} is wrong.");
                                return null;
                            }
                            else
                            {
                                puzzle[puzzleRow, col] = testInput;
                                col++;
                                if (col >= cols)
                                {
                                    puzzleRow++;
                                    col = 0;
                                }
                            }
                        }
                    } 
                }
            }

            return puzzle;
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

        public SearchAlgorithm GetMethod(string method)
        {
            if (method == "depth")
                return SearchAlgorithm.depth;
            else if (method == "best")
                return SearchAlgorithm.best;

            return SearchAlgorithm.wrong;
        }
    }
}
