using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.Models
{
    public class BlockTile : Button
    {
        //public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood, Color.DarkOliveGreen};
        //public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.Beige, Color.DarkOliveGreen };
        
        



        int r, c;
        string blockTileStatus; // "Empty", "Player1", "Player2"

        //public static Color[] centerTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.Beige, Color.DarkOliveGreen };

        public static Dictionary<string, Color> DicColBlockStatus = new Dictionary<string, Color>()
        {
            {"Empty", Color.DarkRed },
            {"Player1",  Color.BurlyWood},
            {"Player2", Color.Beige },
            {"Pending", Color.DarkOliveGreen }
        };
        public string BlockTileStatus
        {
            get { return blockTileStatus; }
            set
            {
                blockTileStatus = value;
                this.BackgroundColor = DicColBlockStatus[blockTileStatus];
            }
        }
        public BlockTile(int r, int c)
        {
            this.r = r; this.c = c;
            BlockTileStatus = "Empty";
        }

        public BlockTile(BlockTile other)
        {
            this.r = other.r;
            this.c = other.c;
            this.BlockTileStatus = other.BlockTileStatus;
        }

    }
}
