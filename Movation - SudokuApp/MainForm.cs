using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Movation___SudokuApp.LogicLayer;

namespace Movation___SudokuApp
{
    public partial class MainForm : Form
    {
        private GenerateSudoku sudokuGenerator;  // Calling the class for creating Sudoku puzzles
        private GenerateGrid gridGenerator;      // Calling the class for creating the grid of cells
        private TextBox[,] cells;                // this is a 2D array to hold the cells in the grid
        private int gridSize;                    // Size of the grid (4x4 or 9x9)

        public MainForm()
        {
            InitializeComponent();
            sudokuGenerator = new GenerateSudoku();
            gridGenerator = new GenerateGrid();

            gridSize = 4;  // Default grid size
            boardSizeSelector.SelectedIndex = 0;  // Default selection in ComboBox
        }

        // Creates and displays a new Sudoku puzzle
        private void MakeSudoku(int gridSize)
        {
            // Clears any existing grid on the form
            if (cells != null)
            {
                foreach (var cell in cells)
                    this.Controls.Remove(cell);
            }

            // Creates a new grid layout of TextBox cells, based on gridSize
            cells = gridGenerator.CreateGrid(gridSize, this);

            // Generates a new Sudoku puzzle
            int[,] puzzle = sudokuGenerator.CreatePuzzle(gridSize);

            // Adds numbers to the grid cells based on the puzzle
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (puzzle[row, col] != 0)
                    {
                        cells[row, col].Text = puzzle[row, col].ToString();
                        cells[row, col].ReadOnly = true;         // Pre-filled cells are read-only
                        cells[row, col].ForeColor = Color.Black; // Pre-filled text is black
                    }
                    else
                    {
                        cells[row, col].Text = string.Empty;
                        cells[row, col].ReadOnly = false;       // User input cells are editable
                        cells[row, col].ForeColor = Color.Red;  // User input text is red
                        cells[row, col].TextChanged += UserInputTextChanged; // Event to keep text red
                    }
                }
            }
        }

        // Ensures user input text stays red
        private void UserInputTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.ForeColor = Color.Red;
            }
        }

        // Changes the grid size based on ComboBox selection and creates a new puzzle
        private void boardSizeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridSize = boardSizeSelector.SelectedItem.ToString() == "4 x 4" ? 4 : 9;
            MakeSudoku(gridSize);
        }

        // create a new puzzle
        private void generateButton_Click(object sender, EventArgs e)
        {
            gridSize = boardSizeSelector.SelectedItem.ToString() == "4 x 4" ? 4 : 9;
            MakeSudoku(gridSize);
        }

        // Checks if the solution is correct
        private bool VerifySolution()
        {
            int size = gridSize;
            HashSet<string> seen = new HashSet<string>();  // Set to track seen numbers

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (string.IsNullOrEmpty(cells[row, col].Text))
                        return false; // If any cell is empty, solution is not complete

                    string value = cells[row, col].Text;

                    // Check if the number has already been seen in the row, column, or block
                    if (!seen.Add(value + " in row " + row) ||
                        !seen.Add(value + " in col " + col) ||
                        !seen.Add(value + " in block " + (row / (int)Math.Sqrt(size)) + "-" + (col / (int)Math.Sqrt(size))))
                    {
                        return false; // Duplicate found
                    }
                }
            }
            return true; // Solution is correct
        }

        // Handles the "Check Solution" button click to verify the solution
        private void CheckSolution_Click(object sender, EventArgs e)
        {
            if (VerifySolution())
            {
                MessageBox.Show("Congratulations! The puzzle is solved correctly.", "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The solution is incorrect. Please try again.", "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Clears user input cells, leaving template cells untouched
        private void ClearGrid()
        {
            if (cells != null)
            {
                foreach (var cell in cells)
                {
                    if (!cell.ReadOnly)  // Only clear cells that are not read-only
                    {
                        cell.Clear();
                        cell.ForeColor = Color.Black;
                        cell.TextChanged -= UserInputTextChanged;  // Remove red text event
                    }
                }
            }
        }

        // Handles the "Clear" button click to reset the grid
        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearGrid();
        }
    }
}
