using System;
using System.Collections.Generic;
using System.Text;

namespace QuoridorApp.Models
{
    public partial class Game
    {
        public Game()
        {
            GameMoves = new HashSet<GameMove>();
        }

        public int GameId { get; set; }
        public DateTime GameDate { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }

        public virtual Player Player1 { get; set; }
        public virtual Player Player2 { get; set; }
        public virtual ICollection<GameMove> GameMoves { get; set; }
    }
}
