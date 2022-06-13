using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.Models
{
    public class CenterTile : Button
    {
        int r, c;
        string centerTileStatus; // "Empty", "Player1", "Player2"

        //public static Color[] centerTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.Beige, Color.DarkOliveGreen };

        public static Dictionary<string, Color> DicColCenterStatus = new Dictionary<string, Color>()
        {
            {"Empty", Color.DarkRed },
            {"Player1", Color.BurlyWood},
            {"Player2", Color.Beige },
        };
        public string CenterTileStatus
        {
            get { return centerTileStatus; }
            set
            {
                centerTileStatus = value;
                this.BackgroundColor = DicColCenterStatus[centerTileStatus];
            }
        }
        public CenterTile(int r, int c)
        {
            this.r = r; this.c = c;
            CenterTileStatus = "Empty";
        }

        public CenterTile(CenterTile other)
        {
            this.r = other.r;
            this.c = other.c;
            this.CenterTileStatus = other.CenterTileStatus;
        }
    }

    
}
