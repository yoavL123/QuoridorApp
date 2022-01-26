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
        
        public static Color[] pawnTileColStatus = new Color[] { Color.Black, Color.DarkBlue, Color.Gray };
        public static Dictionary<string, int> DicPawnStatus = new Dictionary<string, int>()
        {
            {"Empty", 0 },
            {"Player1", 1},
            {"Player2", 2 }
        };
        int r, c;
        int pawnTileStatus;
        public int PawnTileStatus
        {
            get { return pawnTileStatus; }
            set
            {
                pawnTileStatus = value;
                this.BackgroundColor = pawnTileColStatus[pawnTileStatus];
            }
        }
        //public event EventHandler<int> PawnTileStatusChanged;


        public PawnTile(int row, int col)
        {
            this.r = row; this.c = col;
            PawnTileStatus = DicPawnStatus["Empty"];
            //this.BackgroundColor = pawnTileColStatus[pawnTileStatus];
        }
    }
}
