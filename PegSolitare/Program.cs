using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PegSolitare
{
    public enum SearchAlgorithm
    {
        depth,
        best,
        wrong
    }

    public enum SearchOutCome
    {
        outOfTime,
        noSolution,
        success
    }
    class Program
    {
        static void Main(string[] args)
        {
            HelperClass helper = new HelperClass();

            SearchAlgorithm method = helper.GetMethod(args[0]);

            //Console.WriteLine(args[1]);
            string filename = args[1];
            string outFile = args[2];

            Console.WriteLine(method);

            int[,] puzzle;
            puzzle = helper.ReadFile(filename);
            if(puzzle == null)
            {
                Environment.Exit(Environment.ExitCode);
            }
            helper.PrintPuzzle(puzzle);

            PuzzleSolver solver = new PuzzleSolver();
            solver.InitializeSearch(puzzle, method);
            TreeNode node = solver.Search(method, out SearchOutCome outcome);
            if (outcome == SearchOutCome.success)
            {
                List<((int, int), (int, int))> solution = solver.ExtractSolution(node, out int moves);
                helper.WriteSolutionToFile(solution, moves, outFile);
            }
            else if(outcome == SearchOutCome.outOfTime)
            {
                Console.WriteLine("The search exceeded the given time (5 minutes).");
            }
            else
            {
                Console.WriteLine("There is no solution for the given puzzle.");
            }
        }

        
    }
}
