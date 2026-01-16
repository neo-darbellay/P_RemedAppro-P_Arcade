using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P_Arcade
{
    /// <summary>
    /// The base class for any game
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// The current game's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The current game's high scores (if there are any)
        /// </summary>
        public List<HighScore> HighScores { get; set; }

        public Game(string name)
        {
            HighScores = new List<HighScore>();

            Name = name;
        }

        /// <summary>
        /// Starts the game logic
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello, World!");
            Console.ReadLine();
        }
    }
}