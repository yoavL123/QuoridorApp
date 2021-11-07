using System;
using System.Collections.Generic;
using System.Text;

namespace QuoridorApp.Models
{
    public partial class Player
    {
        public Player()
        {
            GameMoves = new HashSet<GameMove>();
            GamePlayer1s = new HashSet<Game>();
            GamePlayer2s = new HashSet<Game>();
            RatingChanges = new HashSet<RatingChange>();
        }

        public int PlayerId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlayerPass { get; set; }

        public virtual ICollection<GameMove> GameMoves { get; set; }
        public virtual ICollection<Game> GamePlayer1s { get; set; }
        public virtual ICollection<Game> GamePlayer2s { get; set; }
        public virtual ICollection<RatingChange> RatingChanges { get; set; }
    }
}
