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
        //public int[,] horBlockBoard; // horizontal block board
        public BlockTile[,] horBlockBoard; // horizontal block board
        public BlockTile[,] verBlockBoard; // horizontal block board
        //public bool[,] centerBlocked; // checks weather the center is blocked. Used to check if we can put a block
        //public int[,] verBlockBoard; // vertical block board
        //const int SIZE = 9;
        const double PAWN_TILE_SIZE = 60;
        const double BLOCK_TILE_SMALL = 15;
        const double BLOCK_TILE_BIG = 60;

        public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood };

        // a 2D array that specifies the locations of the players

        private int[,] playerLoc;
        private int curPlayer;
        private string blockStatus; // has 3 values: {"Empty", "Hor", "Ver"}
        public BoardViewModel() { }
        public BoardViewModel(AbsoluteLayout theBoard)
        {
            //pawnBoard = new PawnTile[SIZE, SIZE];
            horBlockBoard = new BlockTile[SIZE, SIZE - 1];
            verBlockBoard = new BlockTile[SIZE - 1, SIZE];
            for (int i = 0; i < SIZE; i++)
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

                    if (j < SIZE - 1) // Horizontal block cell is only between pawn cells
                    {
                        // Create a block tile:
                        horBlockBoard[i, j] = new BlockTile(i, j)
                        {
                            //BackgroundColor = Color.DarkRed,
                            //BackgroundColor = BoardViewModel.blockTileColStatus[vm.horBlockBoard[i, j]]
                            Command = new Command(() => PlaceBlockHor(ri, rj, this))
                        };
                        startY += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_BIG, BLOCK_TILE_SMALL);
                        theBoard.Children.Add(horBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(horBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                        startY -= PAWN_TILE_SIZE;
                    }


                    if (i < SIZE - 1) // Horizontal block cell is only between pawn cells
                    {
                        verBlockBoard[i, j] = new BlockTile(i, j)
                        {
                            //BackgroundColor = Color.DarkRed,
                            //BackgroundColor = 
                            //BackgroundColor = BoardViewModel.blockTileColStatus[vm.verBlockBoard[i, j]]
                            Command = new Command(() => PlaceBlockVer(ri, rj, this))
                        };
                        startX += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_BIG);
                        theBoard.Children.Add(verBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(verBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                        startX += PAWN_TILE_SIZE;
                    }

                }
            }
            blockStatus = "Empty";
            //horBlockBoard = new int[SIZE, SIZE - 1];
            //verBlockBoard = new int[SIZE - 1, SIZE];
            /*
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    pawnBoard[i, j] = new PawnTile(i, j);
                }
            }
            */
            /*
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE-1; j++)
                {
                    horBlockBoard[i, j] = 0;
                }
            }
            */
            /*
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    verBlockBoard[i, j] = 0;
                }
            }
            */

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


        bool CheckWon()
        {
            return (playerLoc[0, 1] == SIZE - 1) || (playerLoc[1, 1] == 0);
        }
        
        /*
         * Check 
         */
        /*
        bool CheckAdj()
        {

        }
        */
        public bool Move(int newX, int newY, BoardViewModel myBoard)
        {
            if (blockStatus != "Empty") return false;
            int player = myBoard.curPlayer;
            int curX = playerLoc[player, 0];
            int curY = playerLoc[player, 1];
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1) return false;
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) == 0) return false;
            if (curX == newX)
            {
                if (myBoard.horBlockBoard[curX, Math.Min(curY, newY)].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
            }
            else
            {
                if (myBoard.verBlockBoard[Math.Min(curX, newX), curY].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
            }
            if (myBoard.pawnBoard[newX, newY].PawnTileStatus != PawnTile.DicPawnStatus["Empty"]) return false;
            Console.WriteLine(playerLoc[player, 0]);
            Console.WriteLine(pawnBoard[playerLoc[player, 0], playerLoc[player, 1]].PawnTileStatus);
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = PawnTile.DicPawnStatus["Empty"];
            myBoard.playerLoc[player, 0] = newX;
            myBoard.playerLoc[player, 1] = newY;
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = player+1;

            if (myBoard.CheckWon())
            {
                Application.Current.MainPage.DisplayAlert("Game ended", $"Player {player + 1} won!", "Back to home");
                OnToMainMenuCommand();
            }
            
            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;
            
            return true;
        }
        /*
        Returns true iff a new block is placed.
        */
        public bool PlaceBlockHor(int X, int Y, BoardViewModel myBoard)
        {
            int player = myBoard.curPlayer;
            if(myBoard.horBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return false;
            }
            if (blockStatus == "Ver" || myBoard.horBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }

            if(blockStatus == "Empty") // first of the 2 blocks
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                blockStatus = "Hor";
                return false;
            }
            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X + 1; int nextY = Y;
            
            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE && nextY >= 0 && nextY < SIZE - 1)
                {
                    if(myBoard.horBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            if (!hasNextTo) return false;
            
            blockStatus = "Empty";
            myBoard.horBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            myBoard.horBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;

            return true;
        }


        public bool PlaceBlockVer(int X, int Y, BoardViewModel myBoard)
        {
            int player = myBoard.curPlayer;
            if (myBoard.verBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                myBoard.verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return false;
            }
            if (blockStatus == "Hor" || myBoard.verBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }

            if (blockStatus == "Empty") // first of the 2 blocks
            {
                myBoard.verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                blockStatus = "Ver";
                return false;
            }
            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X; int nextY = Y + 1;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE - 1 && nextY >= 0 && nextY < SIZE)
                {
                    if (myBoard.verBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X;
                nextY = Y - 1;
            }

            if (!hasNextTo) return false;

            blockStatus = "Empty";
            myBoard.verBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            myBoard.verBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;

            return true;
        }
    }
}
