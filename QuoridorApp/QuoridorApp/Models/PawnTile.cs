using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.Models
{
    /*
    public enum PawnTileStatus
    {
        Empty,
        FirstPlayer,
        SecondPlayer
    }
    */

    public class PawnTile : Button
    {
        
        
        int r, c;
        string pawnTileStatus; // "Empty", "Player1", "Player2"

        //public static Color[] centerTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.Beige, Color.DarkOliveGreen };

        public static Dictionary<string, Color> DicColPawnStatus = new Dictionary<string, Color>()
        {
            {"Empty", Color.Black },
            {"Player1", Color.DarkBlue},
            {"Player2", Color.Gray },
        };
        public string PawnTileStatus
        {
            get { return pawnTileStatus; }
            set
            {
                pawnTileStatus = value;
                this.BackgroundColor = DicColPawnStatus[pawnTileStatus];
            }
        }
        public PawnTile(int r, int c)
        {
            this.r = r; this.c = c;
            PawnTileStatus = "Empty";
        }

        public PawnTile(PawnTile other)
        {
            this.r = other.r;
            this.c = other.c;
            this.PawnTileStatus = other.PawnTileStatus;
        }
    }
}
