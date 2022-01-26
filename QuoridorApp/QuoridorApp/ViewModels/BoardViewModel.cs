using QuoridorApp.Models;
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
        //public int[,] pawnBoard;
        public PawnTile[,] pawnBoard;
        public int[,] horBlockBoard; // horizontal block board
        public int[,] verBlockBoard; // vertical block board
        //const int SIZE = 9;
        const double PAWN_TILE_SIZE = 60;
        const double BLOCK_TILE_SMALL = 15;
        const double BLOCK_TILE_BIG = 60;

        public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood };

        // a 2D array that specifies the locations of the players

        private int[,] playerLoc;

        public BoardViewModel() { }
        public BoardViewModel(AbsoluteLayout theBoard)
        {
            pawnBoard = new PawnTile[SIZE, SIZE];

            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    
                    pawnBoard[i, j] = new PawnTile(i, j) // Create the button add its properties:
                    {
                        //BackgroundColor = Color.Black,
                        //BackgroundColor = BoardViewModel.pawnTileColStatus[vm.pawnBoard[i, j]],
                        Command = new Command(() => Move(0, i, j))
                    };
                    double startX = i * PAWN_TILE_SIZE + i * BLOCK_TILE_SMALL;
                    double startY = j * PAWN_TILE_SIZE + j * BLOCK_TILE_SMALL;
                    Rectangle pawnBounds = new Rectangle(startX, startY, PAWN_TILE_SIZE, PAWN_TILE_SIZE);
                    theBoard.Children.Add(pawnBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                    AbsoluteLayout.SetLayoutBounds(pawnBoard[i, j], pawnBounds); // Add the button to the absolute layout in the view
                    

                }
            }
            horBlockBoard = new int[SIZE, SIZE - 1];
            verBlockBoard = new int[SIZE - 1, SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    pawnBoard[i, j] = new PawnTile(i, j);
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

            pawnBoard[SIZE / 2, 0].PawnTileStatus = PawnTile.DicPawnStatus["Player1"];
            pawnBoard[SIZE / 2, SIZE-1].PawnTileStatus = PawnTile.DicPawnStatus["Player2"];
            playerLoc = new int[,] { { SIZE / 2, 0 }, { SIZE / 2, SIZE - 1 } };

            for (int i = 0; i < SIZE-1; i++)
            {
                for (int j = 0; j < SIZE-1; j++)
                {
                    Device.BeginInvokeOnMainThread(() => pawnBoard[i, j].PawnTileStatus = PawnTile.DicPawnStatus["Player1"]);

                }
            }

        }


        public bool Move(int player, int newX, int newY)
        {
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1) return false;
            //pawnBoard[playerLoc[player, 0], playerLoc[player, 1]] = 0;
            playerLoc[player, 0] = newX;
            playerLoc[player, 1] = newY;
            //pawnBoard[playerLoc[player, 0], playerLoc[player, 1]] = 1;
            return true;
        }
    }
}
