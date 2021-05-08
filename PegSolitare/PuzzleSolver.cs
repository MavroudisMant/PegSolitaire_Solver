using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PegSolitare
{
    class PuzzleSolver
    {
        public List<TreeNode> Frontier { get; set; } = new List<TreeNode>();

        /// <summary>
        /// This method finds all the children for a given node
        /// and places the in the Frontier at the appropriete 
        /// position accoriding to the Algorithm that is selected
        /// from the user
        /// </summary>
        /// <param name="currentNode">Current Node</param>
        /// <param name="method">Search Algorithm</param>
        public void FindChildern(TreeNode currentNode, SearchAlgorithm method)
        {
            List<(int, int)> blanks = FindBlanks(currentNode.Puzzle);
            foreach (var blank in blanks)
            {
                //Check if the top pegs can move
                if (blank.Item1 > 1)
                {
                    if (currentNode.Puzzle[blank.Item1 - 1, blank.Item2] == 1
                                && currentNode.Puzzle[blank.Item1 - 2, blank.Item2] == 1)
                    {
                        TreeNode child = new TreeNode(currentNode.Puzzle, currentNode);
                        //Calculate the new puzzle
                        child.Puzzle[blank.Item1 - 2, blank.Item2] = 2;
                        child.Puzzle[blank.Item1 - 1, blank.Item2] = 2;
                        child.Puzzle[blank.Item1, blank.Item2] = 1;

                        //Set the moving peg positions
                        child.OldPosition = (blank.Item1 - 2, blank.Item2);
                        child.NewPosition = (blank.Item1, blank.Item2);

                        if (method == SearchAlgorithm.depth)
                        {
                            Frontier.Insert(0, child);
                        }
                        else if(method == SearchAlgorithm.best)
                        {
                            child.H = CalculateHeuristic(child.Puzzle);
                            AddNodeInOrder(child);
                        }
                    } 
                }

                //Check if the left pegs can move
                if (blank.Item2 > 1)
                {
                    if (currentNode.Puzzle[blank.Item1, blank.Item2 - 1] == 1
                                && currentNode.Puzzle[blank.Item1, blank.Item2 - 2] == 1)
                    {
                        TreeNode child = new TreeNode(currentNode.Puzzle, currentNode);
                        //Calculate the new puzzle
                        child.Puzzle[blank.Item1, blank.Item2 - 2] = 2;
                        child.Puzzle[blank.Item1, blank.Item2 - 1] = 2;
                        child.Puzzle[blank.Item1, blank.Item2] = 1;

                        //Set the moving peg positions
                        child.OldPosition = (blank.Item1, blank.Item2 - 2);
                        child.NewPosition = (blank.Item1, blank.Item2);

                        if (method == SearchAlgorithm.depth)
                        {
                            Frontier.Insert(0, child);
                        }
                        else if (method == SearchAlgorithm.best)
                        {
                            child.H = CalculateHeuristic(child.Puzzle);
                            AddNodeInOrder(child);
                        }
                    } 
                }

                //Check if the right pegs can move
                if (blank.Item2 < currentNode.Puzzle.GetLength(1) - 2)
                {
                    if (currentNode.Puzzle[blank.Item1, blank.Item2 + 1] == 1
                                && currentNode.Puzzle[blank.Item1, blank.Item2 + 2] == 1)
                    {
                        TreeNode child = new TreeNode(currentNode.Puzzle, currentNode);
                        //Calculate the new puzzle
                        child.Puzzle[blank.Item1, blank.Item2 + 2] = 2;
                        child.Puzzle[blank.Item1, blank.Item2 + 1] = 2;
                        child.Puzzle[blank.Item1, blank.Item2] = 1;

                        //Set the moving peg positions
                        child.OldPosition = (blank.Item1, blank.Item2 + 2);
                        child.NewPosition = (blank.Item1, blank.Item2);

                        if (method == SearchAlgorithm.depth)
                        {
                            Frontier.Insert(0, child);
                        }
                        else if (method == SearchAlgorithm.best)
                        {
                            child.H = CalculateHeuristic(child.Puzzle);
                            AddNodeInOrder(child);
                        }
                    } 
                }

                //Check if the bottom pegs can move
                if (blank.Item1 < currentNode.Puzzle.GetLength(0) - 2)
                {
                    if (currentNode.Puzzle[blank.Item1 + 1, blank.Item2] == 1
                                && currentNode.Puzzle[blank.Item1 + 2, blank.Item2] == 1)
                    {
                        TreeNode child = new TreeNode(currentNode.Puzzle, currentNode);
                        //Calculate the new puzzle
                        child.Puzzle[blank.Item1 + 2, blank.Item2] = 2;
                        child.Puzzle[blank.Item1 + 1, blank.Item2] = 2;
                        child.Puzzle[blank.Item1, blank.Item2] = 1;

                        //Set the moving peg positions
                        child.OldPosition = (blank.Item1 + 2, blank.Item2);
                        child.NewPosition = (blank.Item1, blank.Item2);

                        if (method == SearchAlgorithm.depth)
                        {
                            Frontier.Insert(0, child);
                        }
                        else if (method == SearchAlgorithm.best)
                        {
                            child.H = CalculateHeuristic(child.Puzzle);
                            AddNodeInOrder(child);
                        }
                    } 
                }
            }

        }

        /// <summary>
        /// This method initializes the first node of the Frontier
        /// with the puzzle given by the user.
        /// </summary>
        /// <param name="puzzle">The puzzle given by the user</param>
        /// <param name="method">The search algorithm</param>
        public void InitializeSearch(int[,] puzzle, SearchAlgorithm method)
        {
            TreeNode node = new TreeNode(puzzle, null);

            if(method == SearchAlgorithm.best)
            {
                node.H = CalculateHeuristic(node.Puzzle);
            }

            Frontier.Insert(0, node);
        }

        /// <summary>
        /// This method implements the search on the Fronitier.
        /// It is the same for both search algorithms sinces their 
        /// only difference is the way they insert new nodes into the 
        /// Frontier
        /// </summary>
        /// <param name="method">The search algorithm</param>
        /// <returns>
        ///     TreeNode if Search finds a solution.
        ///     Null if there is no solution.
        /// </returns>
        public TreeNode Search(SearchAlgorithm method, out SearchOutCome searchOutCome)
        {
            TreeNode currentNode;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan ts;

            while (Frontier.Count > 0)
            {
                //Stopwatch st = new Stopwatch();
                //st.Start();
                ts = stopwatch.Elapsed;
                if (ts.Minutes > 4)
                {
                    Console.WriteLine("Out of time");
                    searchOutCome = SearchOutCome.outOfTime;
                    return null;
                }

                currentNode = Frontier.First();

                if (IsSolution(currentNode.Puzzle))
                {
                    stopwatch.Stop();
                    ts = stopwatch.Elapsed;
                    Console.WriteLine($"Minutes: {ts.Minutes}, Seconds: {ts.Seconds}, Milliseconds: {ts.Milliseconds}");
                    searchOutCome = SearchOutCome.success;
                    return currentNode;
                    
                }

                Frontier.RemoveAt(0);
                
                FindChildern(currentNode, method);
            }


            searchOutCome = SearchOutCome.noSolution;
            return null;
        }

        /// <summary>
        /// This method will start from the node with
        /// the solved puzzle and trace back the steps 
        /// it took untill it reaches the first state
        /// of the puzzle
        /// </summary>
        /// <param name="solutionNode">The solution node given by Search</param>
        /// <param name="moves">The moves needed to complete the puzzle</param>
        /// <returns>A List of tuples with the nodes old and new position</returns>
        public List<((int, int), (int, int))> ExtractSolution(TreeNode solutionNode, out int moves) 
        {
            List<((int, int), (int, int))> solution = new List<((int, int), (int, int))>();

            
            int count = 0;
            TreeNode tempNode = solutionNode;
            while(tempNode.Parent != null)
            {
                count++;
                solution.Insert(0, (tempNode.OldPosition, tempNode.NewPosition));
                tempNode = tempNode.Parent;
            }

            moves = count;
            return solution;
        }

        /// <summary>
        /// Adds the new node in order accoring to
        /// it's H value
        /// </summary>
        /// <param name="childNode">The node to insert</param>
        private void AddNodeInOrder(TreeNode childNode)
        {
            if (Frontier.Count > 0)
            {
                foreach (TreeNode node in Frontier)
                {
                    if (node.H >= childNode.H)
                    {
                        Frontier.Insert(Frontier.IndexOf(node), childNode);
                        break;
                    }
                } 
            }
            else
            {
                Frontier.Add(childNode);
            }

            //PrintFrontier();
        }

        /// <summary>
        /// This function finds all the blank spaces on the 
        /// puzzle
        /// </summary>
        /// <param name="puzzle">The current stat of the puzzle</param>
        /// <returns>Return a list with tuples of the x,y positions</returns>
        private List<(int, int)> FindBlanks(int[,] puzzle)
        {
            List<(int, int)> blanks = new List<(int, int)>();

            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    if(puzzle[i,j] == 2)
                    {
                        blanks.Add((i, j));
                    }
                }
            }

            return blanks;
        }

        /// <summary>
        /// This method finds all the pegs remaining 
        /// on the given puzzle.
        /// </summary>
        /// <param name="puzzle">The current puzzle</param>
        /// <returns>
        /// It returns a List of tuples with the position
        /// of the pegs remaining.
        /// </returns>
        private List<(int,int)> FindPegs(int[,] puzzle, out int numPegs)
        {
            List<(int, int)> pegs = new List<(int, int)>();

            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    if (puzzle[i, j] == 1)
                    {
                        pegs.Add((i, j));
                    }
                }
            }

            numPegs = pegs.Count;
            return pegs;
        }

        /// <summary>
        /// This method calculates the Heuristic value for
        /// the given puzzle
        /// </summary>
        /// <param name="puzzle">The puzzle of the current node</param>
        /// <returns>The Hueristic value</returns>
        private int CalculateHeuristic(int[,] puzzle)
        {
            int numPegs;
            (int, int)[] pegs = FindPegs(puzzle, out numPegs).ToArray();


            int sum = 0;
            for (int i = 0; i < numPegs; i++)
            {
                for (int j = i+1; j < numPegs; j++)
                {
                    sum += Math.Abs(pegs[i].Item1 - pegs[j].Item1) + Math.Abs(pegs[i].Item2 - pegs[j].Item2);
                }
            }

            int h = sum;

            return h;
        }

        /// <summary>
        /// Counts the number of pegs left to 
        /// check if current puzzle is solution
        /// </summary>
        /// <param name="puzzle">The current puzzle</param>
        /// <returns>
        ///     True if current puzzle is solution
        ///     False if current puzzle is not solution
        /// </returns>
        private bool IsSolution(int[,] puzzle)
        {
            int count = 0;
            for (int i = 0; i < puzzle.GetLength(0); i++)
            {
                for (int j = 0; j < puzzle.GetLength(1); j++)
                {
                    if (puzzle[i, j] == 1)
                    {
                        count++;
                        if (count > 1) return false;
                    }
                }
            }

            return true;
        }


        ///////////////////////////////////////////////
        //The following methods are used for debugging
        ///////////////////////////////////////////////
        private void PrintBlanks(List<(int, int)> blanks)
        {
            foreach (var item in blanks)
            {
                Console.WriteLine(item);
            }
        }

        private void PrintFrontier()
        {
            foreach (var item in Frontier)
            {
                Console.WriteLine($"{item.OldPosition} , {item.NewPosition}, {item.H}");
            }
            Console.WriteLine("------------");
        }
    }
}
