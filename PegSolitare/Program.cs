using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PegSolitare
{
    public enum Action
    {
        depth,
        best,
        wrong,
        verify
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
            if (args.Length == 3)
            {
                HelperClass helper = new HelperClass();

                Action method = helper.GetMethod(args[0]);

                string puzzleFile = args[1];
                string solutionFile = args[2];

                Console.WriteLine(method);

                int[,] puzzle;
                puzzle = helper.ReadFile(puzzleFile);
                if (puzzle == null)
                {
                    Environment.Exit(Environment.ExitCode);
                }
                helper.PrintPuzzle(puzzle);

                if (method == Action.best || method == Action.depth)
                {


                    PuzzleSolver solver = new PuzzleSolver();
                    solver.InitializeSearch(puzzle, method);
                    TreeNode node = solver.Search(method, out SearchOutCome outcome);
                    if (outcome == SearchOutCome.success)
                    {
                        List<((int, int), (int, int))> solution = solver.ExtractSolution(node, out int moves);
                        helper.WriteSolutionToFile(solution, moves, solutionFile);
                    }
                    else if (outcome == SearchOutCome.outOfTime)
                    {
                        Console.WriteLine("The search exceeded the given time (5 minutes).");
                    }
                    else
                    {
                        Console.WriteLine("There is no solution for the given puzzle.");
                    }
                }
                else if (method == Action.verify)
                {
                    VerifySolution verify = new VerifySolution(puzzle, solutionFile);

                    if (verify.CheckSolution())
                    {
                        Console.WriteLine("The given solution is correct");
                    }
                    else
                    {
                        Console.WriteLine("The given solution is wrong");
                    }
                }
                else if (method == Action.wrong)
                {
                    Console.WriteLine("The given action is wrong");
                } 
            }
            else
            {
                Console.WriteLine("Wrong number of arguments given.");
            }
        }

        
    }
}
