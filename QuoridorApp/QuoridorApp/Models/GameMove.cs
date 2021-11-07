using System;
using System.Collections.Generic;
using System.Text;

namespace QuoridorApp.Models
{
    public partial class GameMove
    {
        public int GameMoveId { get; set; }
        public int GameMoveGameId { get; set; }
        public int GameMovePlayerId { get; set; }
        public int SecondsFromStart { get; set; }
        public int MoveTime { get; set; }
        public bool IsPawnMove { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string BlockDirection { get; set; }

        public virtual Game GameMoveGame { get; set; }
        public virtual Player GameMovePlayer { get; set; }
    }
}
