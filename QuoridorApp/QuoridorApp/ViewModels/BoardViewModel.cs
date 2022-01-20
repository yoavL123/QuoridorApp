using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{

    /*
     * Responsible for all of the utility of board. 
     *
     */
    public class BoardViewModel : ViewModelBase
    {
        const int SIZE = 9;
        /*
        Board:
        0 - empty place
        1 - first player
        2 - second player

        Block Board:
        0 - empty
        1 - first player
        2 - second player
        */
        public int[,] pawnBoard;
        public int[,] horBlockBoard; // horizontal block board
        public int[,] verBlockBoard; // vertical block board

        public static Color[] pawnTileCol = new Color[]     { Color.Black, Color.DarkBlue, Color.Gray };
        public static Color[] blockTileCol = new Color[]    { Color.DarkRed, Color.BurlyWood, Color.BurlyWood };

        // a 2D array that specifies the locations of the players
        
        private int[,] playerLoc;
        

        public BoardViewModel()
        {
            pawnBoard = new int[SIZE, SIZE];
            horBlockBoard = new int[SIZE, SIZE - 1];
            verBlockBoard = new int[SIZE - 1, SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    pawnBoard[i, j] = 0;
                }
            }
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE-1; j++)
                {
                    horBlockBoard[i, j] = 0;
                }
            }
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    verBlockBoard[i, j] = 0;
                }
            }

            pawnBoard[SIZE / 2, 0] = 1;
            pawnBoard[SIZE / 2, SIZE-1] = 2;
            playerLoc = new int[,] { { SIZE / 2, 0 }, { SIZE / 2, SIZE - 1 } };
            
        }


        public bool Move(int player, int newX, int newY)
        {
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1) return false;
            pawnBoard[playerLoc[player, 0], playerLoc[player, 1]] = 0;
            playerLoc[player, 0] = newX;
            playerLoc[player, 1] = newY;
            pawnBoard[playerLoc[player, 0], playerLoc[player, 1]] = 1;
            return true;
        }
    }
}
