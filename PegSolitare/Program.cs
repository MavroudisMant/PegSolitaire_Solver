using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PegSolitare
{
    public enum SearchAlgorithm
    {
        depth,
        best
    }
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";

            

            HelperClass helper = new HelperClass();
            int[,] puzzle = new int[,] { { 0, 0, 1, 1, 1, 0, 0 }, { 0, 1, 1, 1, 1, 1, 0 }, { 1, 1, 1, 2, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1 }, { 0, 1, 1, 1, 1, 1, 0 }, { 0, 0, 1, 1, 1, 0, 0 } };
            //int[,] puzzle = new int[,] { { 0, 0, 2, 0, 0 }, { 0, 2, 1, 2, 0 }, { 2, 1, 1, 1, 2 }, { 0, 2, 1, 2, 0 }, { 0, 2, 1, 2, 0 }, { 0, 0, 2, 0, 0 } };
            //helper.ReadFile(filename, out puzzle, out rows, out cols);

            PuzzleSolver solver = new PuzzleSolver();
            solver.InitializeSearch(puzzle, SearchAlgorithm.depth);
            TreeNode node = solver.Search(SearchAlgorithm.depth);
            if(node != null)
            {
                int moves;
                List<((int,int), (int,int))> solution = solver.ExtractSolution(node, out moves);
                helper.WriteSolutionToFile(solution, moves, "solution.txt");
            }
            else
            {
                Console.WriteLine("No solution");
            }



            
        }
    }
}
