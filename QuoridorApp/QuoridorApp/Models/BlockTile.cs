using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.Models
{
    public class BlockTile : Button
    {
        public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood, Color.DarkOliveGreen};
        public static Dictionary<string, int> DicBlockStatus = new Dictionary<string, int>()
        {
            {"Empty", 0 },
            {"Player1", 1},
            {"Player2", 2 },
            {"Pending", 3 }
        };
        int r, c;
        int blockTileStatus;
        public int BlockTileStatus
        {
            get { return blockTileStatus; }
            set
            {
                blockTileStatus = value;
                this.BackgroundColor = blockTileColStatus[blockTileStatus];
            }
        }
        //public event EventHandler<int> PawnTileStatusChanged;


        public BlockTile(int row, int col)
        {
            this.r = row; this.c = col;
            BlockTileStatus = DicBlockStatus["Empty"];
            //this.BackgroundColor = pawnTileColStatus[pawnTileStatus];
        }
    }
}
