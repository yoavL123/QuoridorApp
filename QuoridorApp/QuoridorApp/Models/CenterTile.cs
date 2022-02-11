using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.Models
{
    public class CenterTile : Label
    {
        int r, c;
        bool centerTileStatus; // false - empty, true - occupied
        public bool CenterTileStatus
        {
            get { return centerTileStatus; }
            set
            {
                centerTileStatus = value;
                if (value == true)
                {
                    this.BackgroundColor = Color.BurlyWood;
                }
                else
                {
                    this.BackgroundColor = Color.DarkRed;
                }
            }
        }
        public CenterTile(int r, int c)
        {
            this.r = r; this.c = c;
            CenterTileStatus = false;
        }

        public void fill()
        {
            CenterTileStatus = true;
        }
    }

    
}
