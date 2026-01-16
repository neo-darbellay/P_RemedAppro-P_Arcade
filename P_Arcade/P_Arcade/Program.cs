using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace P_Arcade
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game test = new Game("Bob123");

            test.Start();

            test.HighScores = GetHighScoresFromFile(test);

            SetHighScoresToFile(test);

            DisplayHighestScores(test, 5);
        }

        /// <summary>
        /// Displays the best scores
        /// </summary>
        /// <param name="game"></param>
        /// <param name="rows"></param>
        static void DisplayHighestScores(Game game, int Rows)
        {
            foreach (HighScore score in game.HighScores)
            {
                Console.WriteLine(score.Score + "\t" + score.Initials);
            }
        }


        /// <summary>
        /// Gets the high score for a game
        /// Note: This was made using "Icemanind"'s answer over at https://stackoverflow.com/questions/19456408/add-a-highscore-system-thats-saves-the-data
        /// </summary>
        /// <param name="game">The game to </param>
        /// <returns>A new list of highscores if the file hasn't been found, or the high scores list</returns>
        static List<HighScore> GetHighScoresFromFile(Game game)
        {
            string path = game.Name + "_highscores.xml";

            if (!File.Exists(path))
                return new List<HighScore>();

            XmlSerializer serializer =
                new XmlSerializer(typeof(List<HighScore>));

            using (StreamReader reader = new StreamReader(path))
            {
                return (List<HighScore>)serializer.Deserialize(reader);
            }
        }


        static void SetHighScoresToFile(Game game)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<HighScore>));

            using (StreamWriter writer =
                   new StreamWriter(game.Name + "_highscores.xml", false))
            {
                serializer.Serialize(writer, game.HighScores);
            }
        }
    }
}
