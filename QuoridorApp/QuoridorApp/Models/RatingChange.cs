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


        public static int INITIAL_RATING = 1200;

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
        public int CalcRatingChange()
        {
            return 10;
        }
    }
}
