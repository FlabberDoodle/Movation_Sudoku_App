using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Movation___SudokuApp.LogicLayer
{
    internal class GenerateGrid
    {
        int cellSize = 40;       // Size of each cell
        int cellPadding = 5;     // Padding between cells
        int blockPadding = 15;   // Extra padding between blocks
        int startX = 150;        // Starting X position for the grid
        int startY = 10;         // Starting Y position for the grid

        // This method generates a grid of TextBoxes to display the Sudoku puzzle
        public TextBox[,] CreateGrid(int gridSize, Control parent)
        {
            TextBox[,] cells = new TextBox[gridSize, gridSize];  // 2D array to store all the TextBox cells
            int blockSize = (gridSize == 4) ? 2 : 3;  // Set the block size (2x2 for 4x4 grids, 3x3 for 9x9 grids)

            // Loop through each row and column to create TextBox cells
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    // Calculate the X and Y position for each cell
                    int xPos = startX + col * (cellSize + cellPadding) + (col / blockSize) * blockPadding;
                    int yPos = startY + row * (cellSize + cellPadding) + (row / blockSize) * blockPadding;

                    // Create a new TextBox for each cell with specific settings
                    cells[row, col] = new TextBox
                    {
                        Width = cellSize,  // Set width of the cell
                        Height = cellSize,  // Set height of the cell
                        MaxLength = 1,  // Allow only 1 character in each cell
                        Location = new Point(xPos, yPos),  // Position of the cell on the form
                        TextAlign = HorizontalAlignment.Center,  // Center align the text
                        Font = new Font("Arial", 14),  // Set font style and size
                        BorderStyle = BorderStyle.FixedSingle  // Set a border around each cell
                    };
                    // Add the TextBox to the form
                    parent.Controls.Add(cells[row, col]);
                }
            }
            return cells;  // Return the grid of TextBoxes
        }
    }
}