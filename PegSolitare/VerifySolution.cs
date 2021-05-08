using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PegSolitare
{
    class VerifySolution
    {
        public int[,] Puzzle { get; set; }
        public string FileName { get; set; }

        public VerifySolution(int[,] puzzle, string fileName)
        {
            Puzzle = puzzle;
            FileName = fileName;
        }

        public bool CheckSolution()
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(FileName);
            }
            catch (Exception)
            {

                Console.WriteLine("The given file does not exit");
                return false;
            }

            int.TryParse(lines[0], out int moves);

            PuzzleSolver solver = new PuzzleSolver();
            solver.FindPegs(Puzzle, out int numPegs);

            //If the moves are different than the number of pegs -1 then the solution is wrong
            if(moves != (numPegs - 1))
            {
                return false;
            }

            for (int i = 1; i <= moves; i++)
            {
                string[] line = lines[i].Split(" ");
                int.TryParse(line[0], out int currentRow);
                int.TryParse(line[1], out int currentCol);
                int.TryParse(line[2], out int newRow);
                int.TryParse(line[3], out int newCol);

                //Minus 1 so they can manage the Array
                currentRow--;
                currentCol--;
                newRow--;
                newCol--;

                if(newRow >= Puzzle.GetLength(0) || newCol >= Puzzle.GetLength(0))
                {
                    Console.WriteLine($"Error in step {i}. The new row or column are out of bounds");
                    return false;
                }

                if(Puzzle[newRow,newCol] != 2)
                {
                    Console.WriteLine($"Error in step {i}. The new position is not empty");
                    return false;
                }

                Puzzle[currentRow, currentCol] = 2;
                //Peg moves down
                if (newRow > currentRow)
                {
                    Puzzle[currentRow + 1, currentCol] = 2;
                }
                //Peg moves up
                else if(newRow < currentRow)
                {
                    Puzzle[currentRow - 1, currentCol] = 2;
                }
                //Peg moves right
                else if(newCol > currentCol)
                {
                    Puzzle[currentRow, currentCol + 1] = 2;
                }
                //Peg moves left
                else if(newCol < currentCol)
                {
                    Puzzle[currentRow, currentCol - 1] = 2;
                }

                Puzzle[newRow, newCol] = 1;
            }

            

            return solver.IsSolution(Puzzle);
        }
    }
}
