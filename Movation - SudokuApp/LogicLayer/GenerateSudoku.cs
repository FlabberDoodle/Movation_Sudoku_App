using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movation___SudokuApp.LogicLayer
{
    internal class GenerateSudoku
    {
        private int[,] puzzle;  // This is the grid where the Sudoku puzzle will be stored
        private Random random = new Random();  // This is to get random numbers

        // This method creates the puzzle grid
        public int[,] CreatePuzzle(int gridSize)
        {
            puzzle = new int[gridSize, gridSize];  // Create an empty grid for the puzzle

            FillGrid(0, 0, gridSize);  // Fill the grid with a complete Sudoku solution
            RemoveNumbers(gridSize);  // Remove some numbers to make the puzzle

            return puzzle;  // Return the puzzle grid
        }

        // This method fills the grid with numbers from 1 to gridSize, following Sudoku rules
        private bool FillGrid(int row, int col, int gridSize)
        {
            if (row == gridSize)  // If it reaches the end of rows, the grid is complete
                return true;

            if (col == gridSize)  // If it reaches the end of a row, go to the next row
                return FillGrid(row + 1, 0, gridSize);

            // Shuffle numbers from 1 to gridSize to try them in random order
            var numbers = Enumerable.Range(1, gridSize).OrderBy(x => random.Next()).ToArray();
            foreach (int num in numbers)
            {
                if (IsValid(row, col, num, gridSize))  // Check if the number is valid in this position
                {
                    puzzle[row, col] = num;  // Place the number in the cell

                    // Recursively try to fill the next cell
                    if (FillGrid(row, col + 1, gridSize))
                        return true;

                    puzzle[row, col] = 0;  // If it doesn't work, remove the number and try again
                }
            }
            return false;  // If no number works, return false
        }

        // This method checks if placing a number is allowed by Sudoku rules
        private bool IsValid(int row, int col, int num, int gridSize)
        {
            int blockSize = (int)Math.Sqrt(gridSize);  // Find the size of each block

            // Check if the number is in the same row or column
            for (int i = 0; i < gridSize; i++)
            {
                if (puzzle[row, i] == num || puzzle[i, col] == num)
                    return false;
            }

            // Check if the number is in the same block
            int startRow = row / blockSize * blockSize;
            int startCol = col / blockSize * blockSize;
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    if (puzzle[startRow + i, startCol + j] == num)
                        return false;
                }
            }
            return true;  // If it passes all checks, the number is valid
        }

        // This method removes some numbers from the filled grid to make it a puzzle
        private void RemoveNumbers(int gridSize)
        {
            int cellsToRemove = gridSize * gridSize / 2;  // Decide how many cells to remove

            for (int i = 0; i < cellsToRemove; i++)
            {
                int row = random.Next(gridSize);
                int col = random.Next(gridSize);

                //This makes sure we don't remove a number from the same cell twice
                while (puzzle[row, col] == 0)
                {
                    row = random.Next(gridSize);
                    col = random.Next(gridSize);
                }

                puzzle[row, col] = 0;  // Set the cell to 0 to make it part of the puzzle
            }
        }
    }
}
