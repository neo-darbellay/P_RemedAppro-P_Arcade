using System;
using System.Runtime.InteropServices;

namespace P_Arcade
{
    internal class SlidingPuzzle : Game
    {
        public SlidingPuzzle() : base("Sliding Puzzle", false) { }

        // Constants used for min/max of the board's size
        const byte VAL_MIN_LENGTH = 3;
        const byte VAL_MIN_WIDTH = 3;
        const byte VAL_MAX_LENGTH = 15;
        const byte VAL_MAX_WIDTH = 15;

        // User input for length and width
        static byte bytLength = 0;
        static byte bytWidth = 0;

        private static int intMoves = 0;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public override void Start()
        {
            // Full screen the app
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 3);

            intMoves = 0;

            // Get user-related values
            GetUserInput();

            // Clear the screen and add the title back
            Arcade.ShowTitle(Name);

            // Generate the grid and draw it
            byte[,] bytGrid = CreateAndFillGrid();
            DrawGrid(bytGrid);

            Console.ReadKey(true);
        }

        /// <summary>
        /// Create the randomized grid
        /// </summary>
        /// <returns></returns>
        private static byte[,] CreateAndFillGrid()
        {
            // Initialize the grid with sequential numbers
            byte[,] bytGrid = new byte[bytWidth, bytLength];
            byte bytCounter = 1;

            for (byte row = 0; row < bytWidth; row++)
            {
                for (byte col = 0; col < bytLength; col++)
                {
                    bytGrid[row, col] = bytCounter;
                    bytCounter++;
                }
            }

            // Set the last cell as empty
            bytGrid[bytWidth - 1, bytLength - 1] = 0;

            // Initialize random number generator
            Random random = new Random();

            // Track the position of the empty tile
            (byte row, byte col) emptyTile = ((byte)(bytWidth - 1), (byte)(bytLength - 1));

            // Shuffle the grid by making random valid moves
            for (int intMove = 0; intMove < bytWidth * bytLength * 50; intMove++)
            {
                // Determine the movement direction
                (sbyte bytRow, sbyte byteCol) = (0, 0);

                switch (random.Next(4))
                {
                    case 0: bytRow = -1; break;
                    case 1: bytRow = 1; break;
                    case 2: byteCol = -1; break;
                    case 3: byteCol = 1; break;
                }

                // Calculate the new position of the empty tile
                byte bytNewRow = (byte)(emptyTile.row + bytRow);
                byte bytNewCol = (byte)(emptyTile.col + byteCol);

                // Check if the new position is within the grid boundaries
                if (bytNewRow >= 0 && bytNewRow < bytWidth &&
                    bytNewCol >= 0 && bytNewCol < bytLength)
                {
                    // Swap the empty tile with the adjacent number
                    bytGrid[emptyTile.row, emptyTile.col] = bytGrid[bytNewRow, bytNewCol];
                    bytGrid[bytNewRow, bytNewCol] = 0;

                    // Update the empty tile position
                    emptyTile = (bytNewRow, bytNewCol);
                }
            }

            return bytGrid;
        }

        /// <summary>
        /// Draw the grid
        /// </summary>
        /// <param name="bytGrid"></param>
        private static void DrawGrid(byte[,] bytGrid)
        {
            byte bytHeight = (byte)bytGrid.GetLength(0);
            byte bytWidth = (byte)bytGrid.GetLength(1);

            for (byte x = 0; x < bytHeight; x++)
            {
                // Draw top border for first row
                if (x == 0)
                    for (int y = 0; y < bytWidth; y++)
                        Console.Write((y == 0 ? "   ╔" : "") + "═══" + (y == bytWidth - 1 ? "╗\n" : "╦"));

                // Draw row content
                Console.Write("   ║");
                for (byte y = 0; y < bytWidth; y++)
                {
                    byte bytValue = bytGrid[x, y];

                    if (bytValue == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("  ");
                        Console.ResetColor();
                        Console.Write(" ║");
                    }
                    else if (bytValue < 10)
                    {
                        Console.Write(" " + bytValue + " ║");
                    }
                    else if (bytValue <= 99)
                    {
                        Console.Write(bytValue + " ║");
                    }
                    else
                    {
                        Console.Write(bytValue + "║");
                    }
                }

                // Draw scoreboard beside the row using switch
                switch (x)
                {
                    case 0:
                        Console.WriteLine("\tInstructions:");
                        break;
                    case 1:
                        Console.WriteLine("\tUse arrow keys to move the empty (red) tile");
                        break;
                    case 2:
                        Console.WriteLine("\tPress ESC to quit the game");
                        break;
                    case 3:
                        Console.WriteLine("\tPress R to restart a new game");
                        break;
                    case 4:
                        Console.WriteLine("\t-----------------------------");
                        break;
                    case 5:
                        Console.WriteLine("\tMoves made: " + intMoves);
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }

                // Draw separation line or bottom border
                if (x < bytHeight - 1)
                    for (int y = 0; y < bytWidth; y++)
                        Console.Write((y == 0 ? "   ╠" : "") + "═══" + (y == bytWidth - 1 ? "╣\n" : "╬"));
                else
                    for (int y = 0; y < bytWidth; y++)
                        Console.Write((y == 0 ? "   ╚" : "") + "═══" + (y == bytWidth - 1 ? "╝\n" : "╩"));
            }
        }


        /// <summary>
        /// Getting the user input
        /// </summary>
        private void GetUserInput()
        {
            Arcade.ShowTitle(Name);

            // Ask the user for the number of rows they want
            Console.Write("   Please enter the length of the board that you want.\n   The value needs to be greater than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_LENGTH);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_LENGTH);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytLength, VAL_MIN_LENGTH, VAL_MAX_LENGTH);


            // Ask the user for the number of columns they want
            Console.Write("\n   Please enter the width of the board that you want.\n   The value needs to be greater than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_WIDTH);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_WIDTH);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytWidth, VAL_MIN_WIDTH, VAL_MAX_WIDTH);
        }

        /// <summary>
        /// Get the user's input, and verify that it is in bound
        /// </summary>
        /// <param name="bytAnswer">The variable that will get changed</param>
        /// <param name="MIN_VALUE">Minimal value</param>
        /// <param name="MAX_VALUE">Maximum value</param>
        private void GetInputInBounds(out byte bytAnswer, byte MIN_VALUE, byte MAX_VALUE)
        {
            bool blnVerification = false;

            do
            {
                Console.Write("   Your input: ");

                bool blnResult = byte.TryParse(Console.ReadLine(), out bytAnswer);
                bool blnResultInBound = blnResult && (bytAnswer >= MIN_VALUE) && (bytAnswer <= MAX_VALUE);

                // Check if the value is correct
                if (blnResultInBound)
                {
                    blnVerification = true;
                }
                else if (blnResult)
                {
                    Console.Write("\n   Your value isn't between {0} and {1}, please retry.\n\n", MIN_VALUE, MAX_VALUE);
                }
                else
                {
                    Console.Write("\n   Your value isn't a number, please retry.\n\n");
                }

                Arcade.Windows11TerminalFix();
            }
            while (!blnVerification);
        }
    }
}
