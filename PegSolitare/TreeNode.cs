using System;
using System.Collections.Generic;
using System.Text;

namespace PegSolitare
{
    class TreeNode
    {
        public int[,] Puzzle { get; set; }
        public TreeNode Parent { get; set; }
        public (int, int) OldPosition { get; set; }
        public (int, int) NewPosition { get; set; }
        public int H { get; set; }

        public TreeNode(int[,] puzzle, TreeNode parent)
        {
            Puzzle = (int[,])puzzle.Clone();
            Parent = parent;
        }
    }
}
