using QuoridorApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        //public ICommand ToMainMenuCommand => new Command(OnToMainMenuCommand);

        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);

        }
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
        public PawnTile[,] pawnBoard = new PawnTile[SIZE, SIZE];
        public int[,] horBlockBoard; // horizontal block board
        public int[,] verBlockBoard; // vertical block board
        //const int SIZE = 9;
        const double PAWN_TILE_SIZE = 60;
        const double BLOCK_TILE_SMALL = 15;
        const double BLOCK_TILE_BIG = 60;

        public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood };

        // a 2D array that specifies the locations of the players

        private int[,] playerLoc;
        private int curPlayer;
        public BoardViewModel() { }
        public BoardViewModel(AbsoluteLayout theBoard)
        {
            //pawnBoard = new PawnTile[SIZE, SIZE];

            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    int ri = i, rj = j;
                    pawnBoard[i, j] = new PawnTile(i, j) // Create the button add its properties:
                    {
                        //BackgroundColor = Color.Black,
                        //BackgroundColor = BoardViewModel.pawnTileColStatus[vm.pawnBoard[i, j]],
                        //Command = new Command(() => Move(0, i, j, this))
                        Command = new Command(() => Move(ri, rj, this))
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
            /*
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    pawnBoard[i, j] = new PawnTile(i, j);
                }
            }
            */
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
            curPlayer = 0; // this is the current player's turn
            /*
            for (int i = 0; i < SIZE-1; i++)
            {
                for (int j = 0; j < SIZE-1; j++)
                {
                    Device.BeginInvokeOnMainThread(() => pawnBoard[i, j].PawnTileStatus = PawnTile.DicPawnStatus["Player1"]);

                }
            }
            */

        }


        bool check_won()
        {
            return (playerLoc[0, 1] == SIZE - 1) || (playerLoc[1, 1] == 0);
        }
        

        public bool Move(int newX, int newY, BoardViewModel myBoard)
        {
            int player = myBoard.curPlayer;
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1) return false;
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) == 0) return false;
            if (myBoard.pawnBoard[newX, newY].PawnTileStatus != PawnTile.DicPawnStatus["Empty"]) return false;
            Console.WriteLine(playerLoc[player, 0]);
            Console.WriteLine(pawnBoard[playerLoc[player, 0], playerLoc[player, 1]].PawnTileStatus);
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = PawnTile.DicPawnStatus["Empty"];
            myBoard.playerLoc[player, 0] = newX;
            myBoard.playerLoc[player, 1] = newY;
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = player+1;

            if (check_won())
            {
                Application.Current.MainPage.DisplayAlert("Game ended", $"Player {player + 1} won!", "Back to home");
                OnToMainMenuCommand();
            }
            
            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;
            
            return true;
        }
    }
}
