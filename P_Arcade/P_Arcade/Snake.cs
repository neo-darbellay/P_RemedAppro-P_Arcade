using System;

namespace P_Arcade
{
    /// <summary>
    /// The Snake game, ported to C# console
    /// </summary>
    internal class Snake : Game
    {
        /// <summary>
        /// The Snake game's constructor
        /// </summary>
        public Snake() : base("Snake", false) { }

        // Constants used for min/max of length/width
        const byte VAL_MIN_SIZE = 0;
        const byte VAL_MAX_SIZE = 25;

        // User input for length and width
        static byte bytLength = 0;
        static byte bytWidth = 0;

        // The game piece's X and Y position
        static byte bytCursorPosX = 0;
        static byte bytCursorPosY = 0;

        // The first tile's X and Y position
        const byte FIRST_TILE_X = 4;
        const byte FIRST_TILE_Y = 6;

        public override void Start()
        {
            // Get user-related values
            GetUserInput();

            // Clear the screen and add the title back
            Arcade.ShowTitle(Name);

            // Draw the game board
            for (int x = 0; x <= bytLength + 1; x++)
            {
                for (int y = 0; y <= bytWidth + 1; y++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    // Top row
                    if (x == 0)
                    {
                        if (y == 0)
                            Console.Write("   ╔");

                        else if (y == bytWidth + 1)
                            Console.Write("╗");
                        else
                            Console.Write("═");
                    }
                    // Bottom row
                    else if (x == bytLength + 1)
                    {
                        if (y == 0)
                            Console.Write("   ╚");
                        else if (y == bytWidth + 1)
                            Console.Write("╝");
                        else
                            Console.Write("═");
                    }
                    // Middle rows
                    else
                    {
                        if (y == 0)
                            Console.Write("   ");

                        if (y == 0 || y == bytWidth + 1)
                            Console.Write("║");
                        else
                        {
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write(" ");
                        }
                    }

                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            // Create the game grid
            byte[,] GameGrid = new byte[bytLength, bytWidth];

            Console.ReadKey(true);
        }

        /// <summary>
        /// Getting the user input
        /// </summary>
        private void GetUserInput()
        {
            Arcade.ShowTitle(Name);

            // Ask the user for the number of rows they want
            Console.Write("   Please enter the length of the area that you want.\n   The value needs to be greater than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_SIZE);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_SIZE);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytLength, VAL_MIN_SIZE, VAL_MAX_SIZE);


            // Ask the user for the number of columns they want
            Console.Write("\n   Please enter the width of the area that you want.\n   The value needs to be greater than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_SIZE);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_SIZE);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytWidth, VAL_MIN_SIZE, VAL_MAX_SIZE);
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
                bool blnResultInBound = blnResult && (bytAnswer > MIN_VALUE) && (bytAnswer < MAX_VALUE);

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

                Windows11TerminalFix();

            }
            while (!blnVerification);
        }

        /// <summary>
        /// Draws a sprite at the given x and y coordinate
        /// </summary>
        /// <param name="x">X pos</param>
        /// <param name="y">Y pos</param>
        /// <param name="chrSprite">The char to draw</param>
        /// <param name="ccrSpriteColor">The ConsoleColor of the sprite</param>
        /// <param name="blnErase">Whether or not it should draw or erase at the given position</param>
        void DrawTile(int x, int y, char chrSprite, ConsoleColor ccrSpriteColor, bool blnErase)
        {
            int startX = FIRST_TILE_X + x;

            int startY = FIRST_TILE_Y + y;

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ccrSpriteColor;

            Console.SetCursorPosition(startX, startY);
            Console.Write(blnErase ? ' ' : chrSprite);

            Console.ResetColor();
        }

        /// <summary>
        /// Small fix for windows 11's terminal
        /// </summary>
        private void Windows11TerminalFix()
        {
            if (Console.CursorTop == Console.BufferHeight)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 3);
            }
            else if (Console.CursorTop == Console.BufferHeight - 1)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 2);
            }
            else if (Console.CursorTop == Console.BufferHeight - 2)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 1);
            }
        }
    }
}