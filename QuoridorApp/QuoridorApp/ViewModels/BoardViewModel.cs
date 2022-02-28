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
        public const int SIZE = 9;
        public const int BLOCKS_NUM = 10;
        public const double PAWN_TILE_SIZE = 60;
        public const double BLOCK_TILE_SMALL = 15;
        public const double BLOCK_TILE_BIG = 60;


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
        //public PawnTile[,] pawnBoard = new PawnTile[SIZE, SIZE];
        public PawnTile[,] pawnBoard;

        public BlockTile[,] horBlockBoard; // horizontal block board
        public BlockTile[,] verBlockBoard; // horizontal block board
        public bool[,] centerBlocked; // checks weather the center is blocked. Used to check if we can put a block
        public CenterTile[,] centerTile;

        public static Color[] blockTileColStatus = new Color[] { Color.DarkRed, Color.BurlyWood, Color.BurlyWood };

        public bool[] isBot;
        public int[,] playerLoc; // a 2D array that specifies the locations of the players
        public int[] blocksLeft; // Denotes for each player how many blocks he has left
        //public int curPlayer;
        private int curPlayerP;
        public int curPlayer
        {
            get
            {
                return curPlayerP;
            }
            set
            {
                curPlayerP = value;
                /*
                if(isBot[curPlayerP])
                {
                    BotViewModel bot = new BotViewModel();
                    bot.MakeMove(this, curPlayer, 1);
                    
                }
                */
                
            }
        }
        public string blockStatus; // has 3 values: {"Empty", "Hor", "Ver"}
        public BoardViewModel() { }

        private static void Copy1D<T>(ref T[] a, T[] b)
        {
            a = new T[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                a[i] = b[i];
            }
        }
        private static void Copy2D<T>(ref T[,] a, T[,] b)
        {
            var rows = b.GetLength(0);
            var cols = b.GetLength(1);
            a = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    a[i, j] = b[i, j];
        }

        /*
        private static void Copy2DComplex<T>(ref T[,] a, T[,] b)
        {
            var rows = b.GetLength(0);
            var cols = b.GetLength(1);
            a = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    a[i, j] = new T(b[i, j]);
        }
        */
        /*
        Checks if some player has to move
         */
        /*
        public static void HandleGame(BoardViewModel board)
        {
            while (!board.CheckWon())
            {
                if (board.isBot[board.curPlayer])
                {
                    BotViewModel bot = new BotViewModel();
                    bot.MakeMove(board, board.curPlayer, 1);
                }
            }
        }
        */

        public BoardViewModel(BoardViewModel b)
        {

            pawnBoard = new PawnTile[SIZE, SIZE];
            horBlockBoard = new BlockTile[SIZE, SIZE - 1];
            verBlockBoard = new BlockTile[SIZE - 1, SIZE];
            centerTile = new CenterTile[SIZE - 1, SIZE - 1];
            Copy2D(ref centerBlocked, b.centerBlocked);
            /*
            Copy2D(ref pawnBoard, b.pawnBoard);
            Copy2D(ref horBlockBoard, b.horBlockBoard);
            Copy2D(ref verBlockBoard, b.verBlockBoard);
            
            Copy2D(ref centerTile, b.centerTile);
            */
            var rows = b.pawnBoard.GetLength(0);
            var cols = b.pawnBoard.GetLength(1);
            
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    pawnBoard[i, j] = new PawnTile(b.pawnBoard[i, j]);
            rows = b.horBlockBoard.GetLength(0);
            cols = b.horBlockBoard.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    horBlockBoard[i, j] = new BlockTile(b.horBlockBoard[i, j]);
            rows = b.verBlockBoard.GetLength(0);
            cols = b.verBlockBoard.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    verBlockBoard[i, j] = new BlockTile(b.verBlockBoard[i, j]);
            rows = b.centerTile.GetLength(0);
            cols = b.centerTile.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    centerTile[i, j] = new CenterTile(b.centerTile[i, j]);

            Copy1D(ref blocksLeft, b.blocksLeft);
            Copy2D(ref playerLoc, b.playerLoc);
            Copy1D(ref isBot, b.isBot);
            curPlayer = b.curPlayer;
            blockStatus = b.blockStatus;
            
        }
        public BoardViewModel(AbsoluteLayout theBoard, bool isBot0 = false, bool isBot1 = false)
        {
            isBot = new bool[2]{ isBot0, isBot1};
            pawnBoard = new PawnTile[SIZE, SIZE];
            horBlockBoard = new BlockTile[SIZE, SIZE - 1];
            verBlockBoard = new BlockTile[SIZE - 1, SIZE];
            centerBlocked = new bool[SIZE, SIZE];
            centerTile = new CenterTile[SIZE - 1, SIZE - 1];
            blocksLeft = new int[2] { BLOCKS_NUM, BLOCKS_NUM };
            for (int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    centerBlocked[i, j] = false;
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
                            Command = new Command(() => PlaceBlockVer(ri, rj, this))
                        };
                        startX += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_BIG);
                        theBoard.Children.Add(verBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(verBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                        startX -= PAWN_TILE_SIZE;
                    }
                    if(i < SIZE - 1 && j < SIZE - 1)
                    {
                        centerTile[i, j] = new CenterTile(i, j)
                        {
                            
                        };
                        //startX += PAWN_TILE_SIZE;
                        startX = (i+1) * PAWN_TILE_SIZE + i * BLOCK_TILE_SMALL;
                        startY = (j+1) * PAWN_TILE_SIZE + j * BLOCK_TILE_SMALL;
                        Rectangle centerBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_SMALL);
                        theBoard.Children.Add(centerTile[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(centerTile[i, j], centerBounds); // Add the button to the absolute layout in the view                     
                    }
                    

                }
            }
            blockStatus = "Empty";
            

            pawnBoard[SIZE / 2, 0].PawnTileStatus = PawnTile.DicPawnStatus["Player1"];
            pawnBoard[SIZE / 2, SIZE-1].PawnTileStatus = PawnTile.DicPawnStatus["Player2"];
            playerLoc = new int[,] { { SIZE / 2, 0 }, { SIZE / 2, SIZE - 1 } };
            curPlayer = 0; // this is the current player's turn
            //HandleGame(this);
        }
        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);

        }

        bool CheckWon()
        {
            return (playerLoc[0, 1] == SIZE - 1) || (playerLoc[1, 1] == 0);
        }

        public bool CanMove(int curX, int curY, int newX, int newY)
        {
            if (newX < 0 || newX >= SIZE || newY < 0 || newY >= SIZE) return false;
            if (curX == newX)
            {
                if (horBlockBoard[curX, Math.Min(curY, newY)].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (verBlockBoard[Math.Min(curX, newX), curY].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
                return true;
            }
        }
        
        private void DFS(int curX, int curY, bool[,] vis)
        {
            int[] xdir = new int[] { -1, 1, 0, 0 };
            int[] ydir = new int[] { 0, 0, -1, 1 };

            for(int t = 0; t < 4; t++)
            {
                int x = curX + xdir[t];
                int y = curY + ydir[t];
                if (x < 0 || x >= SIZE || y < 0 || y >= SIZE) continue;
                
                if (vis[x, y]) continue;
                if (!CanMove(curX, curY, x, y)) continue;
                vis[x, y] = true;
                DFS(x, y, vis);
            }
        }


        private bool CanWin(int player)
        {
            bool[,] vis = new bool[SIZE, SIZE];
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    vis[i, j] = false;
                }
            }
            DFS(playerLoc[player, 0], playerLoc[player, 1], vis);
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if(vis[i, j])
                    {
                        if (player == 0 && j == SIZE - 1) return true;
                        if (player == 1 && j == 0) return true;
                    }
                }
            }
            return false;
        }


        public bool CanMove(int newX, int newY, BoardViewModel myBoard)
        {
            if (blockStatus != "Empty") return false;
            int player = myBoard.curPlayer;
            if (Math.Abs(myBoard.playerLoc[player, 0] - newX) + Math.Abs(myBoard.playerLoc[player, 1] - newY) == 0) return false;

            playerLoc = myBoard.playerLoc;
            int curX = myBoard.playerLoc[player, 0];
            int curY = myBoard.playerLoc[player, 1];
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1)
            {
                int nextPlayer = (player + 1) % 2;
                if (Math.Abs(playerLoc[player, 0] - playerLoc[nextPlayer, 0]) + Math.Abs(playerLoc[player, 1] - playerLoc[nextPlayer, 1]) != 1)
                {
                    return false;
                }
                curX = playerLoc[nextPlayer, 0];
                curY = playerLoc[nextPlayer, 1];
            }

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
            return true;
        }

        public void Move(int newX, int newY, BoardViewModel myBoard)
        {
            if (!CanMove(newX, newY, myBoard)) return;
            int player = myBoard.curPlayer;
            
            
            
            
            
            int curX = playerLoc[player, 0];
            int curY = playerLoc[player, 1];
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1)
            {
                int nextPlayer = (player + 1) % 2;
                curX = playerLoc[nextPlayer, 0];
                curY = playerLoc[nextPlayer, 1];
            }
            
            
            
            //Console.WriteLine(playerLoc[player, 0]);
            //Console.WriteLine(pawnBoard[playerLoc[player, 0], playerLoc[player, 1]].PawnTileStatus);
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = PawnTile.DicPawnStatus["Empty"];
            myBoard.playerLoc[player, 0] = newX;
            myBoard.playerLoc[player, 1] = newY;
            myBoard.pawnBoard[myBoard.playerLoc[player, 0], myBoard.playerLoc[player, 1]].PawnTileStatus = player+1;

            if (myBoard.CheckWon())
            {
                Application.Current.MainPage.DisplayAlert("Game ended", $"Player {player + 1} won!", "Back to home");
                OnToMainMenuCommand();
                return;
            }
            
            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;
            if (myBoard.isBot[myBoard.curPlayer])
            {
                BotViewModel bot = new BotViewModel();
                bot.MakeMove(ref myBoard, myBoard.curPlayer, 1);
                //myBoard.curPlayer++;
                //myBoard.curPlayer %= 2;
            }

        }
        /*
        Returns true iff a new block is placed.
        */
        
        public bool CanPlaceBlockHor(int X, int Y, BoardViewModel myBoard2)
        {
            BoardViewModel myBoard = new BoardViewModel(myBoard2);
            int player = myBoard.curPlayer;
            if (myBoard.blocksLeft[player] == 0) return false;

            if (myBoard.horBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return true;
            }
            if (blockStatus == "Ver" || myBoard.horBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }

            if (blockStatus == "Empty") // first of the 2 blocks
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                blockStatus = "Hor";
                return true;
            }
            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X + 1; int nextY = Y;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE && nextY >= 0 && nextY < SIZE - 1)
                {
                    if (myBoard.horBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            if (!hasNextTo) return false;
            if (myBoard.centerBlocked[Math.Min(X, nextX), Y])
            {
                return false;
            }
            myBoard.centerBlocked[Math.Min(X, nextX), Y] = true;
            myBoard.centerTile[Math.Min(X, nextX), Y].fill();

            blockStatus = "Empty";
            myBoard.horBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            myBoard.horBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;

            if (!CanWin(0) || !CanWin(1))
            {
                blockStatus = "Hor";
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                myBoard.horBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                myBoard.centerTile[Math.Min(X, nextX), Y].clear();
                return false;
            }
            blockStatus = "Hor";
            myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            myBoard.horBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
            myBoard.centerTile[Math.Min(X, nextX), Y].clear();
            return true;
        }
        
        public void PlaceBlockHor(int X, int Y, BoardViewModel myBoard)
        {
            if (!CanPlaceBlockHor(X, Y, myBoard)) return;
            int player = myBoard.curPlayer;
            

            if(myBoard.horBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return;
            }
            

            if(blockStatus == "Empty") // first of the 2 blocks
            {
                myBoard.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                blockStatus = "Hor";
                return;
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

            
            
            myBoard.centerBlocked[Math.Min(X, nextX), Y] = true;
            myBoard.centerTile[Math.Min(X, nextX), Y].fill();
            
            blockStatus = "Empty";
            myBoard.horBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            myBoard.horBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;

            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;
            myBoard.blocksLeft[player]--;
            if (myBoard.isBot[myBoard.curPlayer])
            {
                BotViewModel bot = new BotViewModel();
                bot.MakeMove(ref myBoard, myBoard.curPlayer, 1);
            }
        }


        public bool PlaceBlockVer(int X, int Y, BoardViewModel myBoard)
        {
            int player = myBoard.curPlayer;
            if (myBoard.blocksLeft[player] == 0) return false;
            
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
            if (myBoard.centerBlocked[X, Math.Min(Y, nextY)])
            {
                return false;
            }
            myBoard.centerBlocked[X, Math.Min(Y, nextY)] = true;
            myBoard.centerTile[X, Math.Min(Y, nextY)].fill();
            blockStatus = "Empty";
            myBoard.verBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            myBoard.verBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            if (!CanWin(0) || !CanWin(1))
            {
                blockStatus = "Ver";
                myBoard.verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                myBoard.verBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                myBoard.centerTile[X, Math.Min(Y, nextY)].clear();
                return false;
            }

            myBoard.curPlayer++;
            myBoard.curPlayer %= 2;
            myBoard.blocksLeft[player]--;
            return true;
        }
    }
}
