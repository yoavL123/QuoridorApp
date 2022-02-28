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

        public CenterTile(CenterTile other)
        {
            this.r = other.r;
            this.c = other.c;
            this.CenterTileStatus = other.CenterTileStatus;
        }
        public void fill()
        {
            CenterTileStatus = true;
        }
        public void clear()
        {
            CenterTileStatus = false;
        }
    }

    
}
