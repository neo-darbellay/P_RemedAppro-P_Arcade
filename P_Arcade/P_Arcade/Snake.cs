using System;

namespace P_Arcade
{
    /// <summary>
    /// The Snake game, ported to C# console
    /// </summary>
    internal class Snake : Game
    {
        public Snake() : base("Snake", false) { }

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
        public override void Start()
        {
            // Clear the screen and add the title back
            Arcade.ShowTitle(Name);

            Console.ReadKey(true);
        }
    }
}