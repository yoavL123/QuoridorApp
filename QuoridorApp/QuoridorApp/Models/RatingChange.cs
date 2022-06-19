using System;
using System.Collections.Generic;
using System.Text;

namespace QuoridorApp.Models
{
    public partial class RatingChange
    {
        public int RatingChangeId { get; set; }
        public int RatingChangePlayerId { get; set; }
        public DateTime RatingChangeDate { get; set; }
        public int AlteredRating { get; set; } // new rating after change

        public virtual Player RatingChangePlayer { get; set; }


        public static int INITIAL_RATING = 1500;
        private static int K = 70;
        public RatingChange()
        {

        }
        public RatingChange(Player p, int alteredRating)
        {

            RatingChangePlayerId = p.PlayerId;
            RatingChangeDate = DateTime.Now;
            AlteredRating = alteredRating;
            //RatingChangePlayer = p;
            
            
        }
        public static double ProbabilityToWin(double rating1, double rating2)
        {
            return 1.0 * 1.0 / (1 + 1.0f *
                   (double)(Math.Pow(10, 1.0f *
                     (rating1 - rating2) / 400)));
        }

        // Function to calculate Elo rating
        // K is a constant.
        // d determines whether Player A wins or
        // Player B.
        public static int EloRating(double Ra, double Rb, bool d)
        {

            // To calculate the Winning
            // Probability of Player B
            double Pb = ProbabilityToWin(Ra, Rb);

            // To calculate the Winning
            // Probability of Player A
            double Pa = ProbabilityToWin(Rb, Ra);

            // Case -1 When Player A wins
            // Updating the Elo Ratings
            if (d == true)
            {
                Ra = Ra + K * (1 - Pa);
                Rb = Rb + K * (0 - Pb);
            }

            // Case -2 When Player B wins
            // Updating the Elo Ratings
            else
            {
                Ra = Ra + K * (0 - Pa);
                Rb = Rb + K * (1 - Pb);
            }
            if(Math.Abs(Ra) < 0.5)
            {
                if (Ra < 0) Ra = -1;
                else Ra = 1;
            }
            return (int)Math.Round(Ra);
        }


        public int CalcRatingChange()
        {
            return 10;
        }
    }
}
