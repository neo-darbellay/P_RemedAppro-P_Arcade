using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P_Arcade
{
    [Serializable()]
    public class HighScore
    {
        public int Score { get; set; }
        public string Initials { get; set; }

        public HighScore()
        {
            Score = 0;
            Initials = "John Doe";
        }

        public HighScore(int score, string initials)
        {
            Score = score;
            Initials = initials;
        }
    }
}
