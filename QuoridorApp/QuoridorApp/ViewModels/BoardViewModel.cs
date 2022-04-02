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
        //public bool[,] centerBlocked; // checks weather the center is blocked. Used to check if we can put a block
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
        public BoardViewModel(BoardViewModel b, int depth = 1)
        {
            pawnBoard = new PawnTile[SIZE, SIZE];
            horBlockBoard = new BlockTile[SIZE, SIZE - 1];
            verBlockBoard = new BlockTile[SIZE - 1, SIZE];
            centerTile = new CenterTile[SIZE - 1, SIZE - 1];
            //Copy2D(ref centerBlocked, b.centerBlocked);
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
            //centerBlocked = new bool[SIZE, SIZE];
            centerTile = new CenterTile[SIZE - 1, SIZE - 1];
            blocksLeft = new int[2] { BLOCKS_NUM, BLOCKS_NUM };
            for (int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    //centerBlocked[i, j] = false;
                    int ri = i, rj = j;
                    pawnBoard[i, j] = new PawnTile(i, j) // Create the button add its properties:
                    {
                        //BackgroundColor = Color.Black,
                        //BackgroundColor = BoardViewModel.pawnTileColStatus[vm.pawnBoard[i, j]],
                        //Command = new Command(() => Move(0, i, j, this))
                        Command = new Command(() => this.Move(ri, rj))
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
                            Command = new Command(() => this.PlaceBlockHor(ri, rj))
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
                            Command = new Command(() => this.PlaceBlockVer(ri, rj))
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

        public bool CanMoveDFS(int curX, int curY, int newX, int newY)
        {
            if (newX < 0 || newX >= SIZE || newY < 0 || newY >= SIZE) return false;
            if (Math.Abs(curX - newX) + Math.Abs(curY - newY) != 1) return false;
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
                if (curY != newY) return false;
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
                if (!CanMoveDFS(curX, curY, x, y)) continue;
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


        public bool CanMove(int newX, int newY)
        {
            if (newX < 0 || newX >= SIZE || newY < 0 || newY >= SIZE) return false;
            if (blockStatus != "Empty") return false;
            int player = curPlayer;
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) == 0) return false;

            int curX = playerLoc[player, 0];
            int curY = playerLoc[player, 1];
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1)
            {
                int nextPlayer = 1 - player;
                if(!CanMoveDFS(curX, curY, playerLoc[nextPlayer, 0], playerLoc[nextPlayer, 1]))
                {
                    return false;
                }
                if (!CanMoveDFS(playerLoc[nextPlayer, 0], playerLoc[nextPlayer, 1], newX, newY))
                {
                    return false;
                }
                return true;
            }

            if (curX == newX)
            {
                if (horBlockBoard[curX, Math.Min(curY, newY)].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
            }
            else
            {
                if (verBlockBoard[Math.Min(curX, newX), curY].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
                {
                    return false;
                }
            }
            if (pawnBoard[newX, newY].PawnTileStatus != PawnTile.DicPawnStatus["Empty"]) return false;
            return true;
        }
        public void MoveBack(int oldX, int oldY)
        {
            
            curPlayer = 1 - curPlayer;
            pawnBoard[playerLoc[curPlayer, 0], playerLoc[curPlayer, 1]].PawnTileStatus = PawnTile.DicPawnStatus["Empty"];
            playerLoc[curPlayer, 0] = oldX;
            playerLoc[curPlayer, 1] = oldY;
            pawnBoard[playerLoc[curPlayer, 0], playerLoc[curPlayer, 1]].PawnTileStatus = curPlayer + 1;
            
        }
        /*
         * realGame - is it a real game or a simulated one
         */
        public void Move(int newX, int newY, int depth = 1)
        {
            if(depth == 1)
            {
                Console.WriteLine("hi");
            }
            if (!CanMove(newX, newY)) return;
            int player = curPlayer;
            
            int curX = playerLoc[player, 0];
            int curY = playerLoc[player, 1];
            if (Math.Abs(playerLoc[player, 0] - newX) + Math.Abs(playerLoc[player, 1] - newY) > 1)
            {
                int nextPlayer = (player + 1) % 2;
                curX = playerLoc[nextPlayer, 0];
                curY = playerLoc[nextPlayer, 1];
            }

            pawnBoard[playerLoc[player, 0], playerLoc[player, 1]].PawnTileStatus = PawnTile.DicPawnStatus["Empty"];
            playerLoc[player, 0] = newX;
            playerLoc[player, 1] = newY;
            pawnBoard[playerLoc[player, 0], playerLoc[player, 1]].PawnTileStatus = player+1;

            
            if (CheckWon())
            {
                if(depth == 1 && !isBot[curPlayer])
                {
                    if (!isBot[1 - curPlayer]) curPlayer = 1 - curPlayer;
                    Application.Current.MainPage.DisplayAlert("Game ended", $"Player {1-curPlayer + 1} won!", "Back to home");
                    OnToMainMenuCommand();
                }
                return;
            }

            curPlayer++;
            curPlayer %= 2;
            //if (isBot[curPlayer] && depth == 1)
            if (isBot[curPlayer] && depth == 1)
            {
                BotViewModel bot = new BotViewModel();
                //bot.MakeMove(ref this, curPlayer, 1);
                bot.MakeMove(this, depth);
                //BoardViewModel cloneBoard = new BoardViewModel(this);
                //bot.MakeMove(ref cloneBoard, depth);
                //.curPlayer++;
                //.curPlayer %= 2;
            }
            
            //Application.Current.MainPage.DisplayAlert("PlayerLoc", $"playerLoc[0,0] = {playerLoc[0, 0]}, playerLoc[0,0] = {playerLoc[0, 1]}", "Back to home");
        }
        /*
        Returns true iff a new block is placed.
        */
        
        public bool CanPlaceBlockHor(int X, int Y)
        {
            //BoardViewModel board2 = new BoardViewModel(this, false);
            int player = curPlayer;
            if (blocksLeft[player] == 0) return false;

            if (horBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                return true;
            }
            if (blockStatus == "Ver" || horBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }

            if (blockStatus == "Empty") // first of the 2 blocks
            {
                return true;
            }
            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X + 1; int nextY = Y;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE && nextY >= 0 && nextY < SIZE - 1)
                {
                    if (horBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            if (!hasNextTo) return false;
            if (centerTile[Math.Min(X, nextX), Y].CenterTileStatus)
            {
                return false;
            }
            //centerBlocked[Math.Min(X, nextX), Y] = true;
            centerTile[Math.Min(X, nextX), Y].fill();

            blockStatus = "Empty";
            int stateXY = horBlockBoard[X, Y].BlockTileStatus;
            int statenextXY = horBlockBoard[nextX, nextY].BlockTileStatus;
            horBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            horBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            bool ans;
            if (!CanWin(0) || !CanWin(1))
            {
                //board2.blockStatus = "Hor";
                //board2.horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                //board2.horBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                //board2.centerTile[Math.Min(X, nextX), Y].clear();
                //return false;
                ans = false;
            }
            else
            {
                ans = true;
            }
            blockStatus = "Hor";
            horBlockBoard[X, Y].BlockTileStatus = stateXY;
            horBlockBoard[nextX, nextY].BlockTileStatus = statenextXY;
            centerTile[Math.Min(X, nextX), Y].clear();
            //centerBlocked[Math.Min(X, nextX), Y] = false;
            return ans;
        }

        public void RemovePendingBlockHor(int X, int Y)
        {
            //curPlayer = 1 - curPlayer;
            //centerBlocked[X, Y] = false;
            //centerTile[X, Y].clear();

            horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            //curPlayer = 1 - curPlayer;

            blockStatus = "Empty";
            
        }
        public void RemoveBlockHor(int X, int Y)
        {
            curPlayer = 1 - curPlayer;
            //centerBlocked[X, Y] = false;
            centerTile[X, Y].clear();

            horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            horBlockBoard[X + 1, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            blocksLeft[curPlayer]++;

            
        }

        
        public void PlaceBlockHor(int X, int Y, int depth = 1)
        {
            if (!CanPlaceBlockHor(X, Y)) return;
            int player = curPlayer;
            

            if(horBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return;
            }
            

            if(blockStatus == "Empty") // first of the 2 blocks
            {
                horBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
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
                    if(horBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            
            
            //centerBlocked[Math.Min(X, nextX), Y] = true;
            centerTile[Math.Min(X, nextX), Y].fill();
            
            blockStatus = "Empty";
            horBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            horBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            blocksLeft[player]--;

            curPlayer = 1 - curPlayer;
            
            
            //if (isBot[curPlayer] && depth == 1)
            if(isBot[curPlayer] && depth == 1)
            {
                BotViewModel bot = new BotViewModel();
                //bot.MakeMove(ref , curPlayer, 1);
                bot.MakeMove(this, depth);
                //BoardViewModel cloneBoard = new BoardViewModel(this);
                //bot.MakeMove(ref cloneBoard, depth);
            }
        }

        public void RemovePendingBlockVer(int X, int Y)
        {
            //curPlayer = 1 - curPlayer;
            //centerBlocked[X, Y] = false;
            //centerTile[X, Y].clear();

            verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            //curPlayer = 1 - curPlayer;

            blockStatus = "Empty";

        }
        public void RemoveBlockVer(int X, int Y)
        {
            curPlayer = 1 - curPlayer;
            //centerBlocked[X, Y] = false;
            centerTile[X, Y].clear();
            /*
            if(verBlockBoard[X, Y].BlockTileStatus != curPlayer)
            {
                Application.Current.MainPage.DisplayAlert("Oops", $"Error while deleting", "Back to home");
            }
            */
            verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            verBlockBoard[X, Y + 1].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            blocksLeft[curPlayer]++;


        }
        public bool CanPlaceBlockVer(int X, int Y)
        {
            //BoardViewModel board2 = new BoardViewModel(this, false);
            int player = curPlayer;
            if (blocksLeft[player] == 0) return false;

            if (verBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                return true;
            }
            if (blockStatus == "Hor" || verBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }

            if (blockStatus == "Empty") // first of the 2 blocks
            {
                return true;
            }
            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X; int nextY = Y + 1;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE - 1 && nextY >= 0 && nextY < SIZE)
                {
                    if (verBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X;
                nextY = Y - 1;
            }

            if (!hasNextTo) return false;
            if (centerTile[X, Math.Min(Y, nextY)].CenterTileStatus)
            {
                return false;
            }
            //centerBlocked[X, Math.Min(Y, nextY)] = true;
            centerTile[X, Math.Min(Y, nextY)].fill();
            blockStatus = "Empty";
            verBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            verBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;
            bool ans;
            if (!CanWin(0) || !CanWin(1))
            {
                //board2.blockStatus = "Ver";
                //board2.verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                //board2.verBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                //board2.centerTile[X, Math.Min(Y, nextY)].clear();
                ans = false;
            }
            else
            {
                ans = true;
            }

            blockStatus = "Ver";
            verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
            verBlockBoard[nextX, nextY].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
            centerTile[X, Math.Min(Y, nextY)].clear();
            //centerBlocked[X, Math.Min(Y, nextY)] = false;
            return ans;
        }

        public void PlaceBlockVer(int X, int Y, int depth = 1)
        {
            /*
            int player = curPlayer;
            if (blocksLeft[player] == 0) return false;
            
            if (verBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return false;
            }
            if (blockStatus == "Hor" || verBlockBoard[X, Y].BlockTileStatus != BlockTile.DicBlockStatus["Empty"])
            {
                return false;
            }
            */
            if (!CanPlaceBlockVer(X, Y)) return;
            if (verBlockBoard[X, Y].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
            {
                verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Empty"];
                blockStatus = "Empty";
                return;
            }
            if (blockStatus == "Empty") // first of the 2 blocks
            {
                verBlockBoard[X, Y].BlockTileStatus = BlockTile.DicBlockStatus["Pending"];
                blockStatus = "Ver";
                return;
            }
            // Now check if the last pending block is next to this
            
            
            bool hasNextTo = false;
            int nextX = X; int nextY = Y + 1;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE - 1 && nextY >= 0 && nextY < SIZE)
                {
                    if (verBlockBoard[nextX, nextY].BlockTileStatus == BlockTile.DicBlockStatus["Pending"])
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X;
                nextY = Y - 1;
            }

            
            //centerBlocked[X, Math.Min(Y, nextY)] = true;
            centerTile[X, Math.Min(Y, nextY)].fill();
            blockStatus = "Empty";
            verBlockBoard[X, Y].BlockTileStatus = curPlayer + 1;
            verBlockBoard[nextX, nextY].BlockTileStatus = curPlayer + 1;

            blocksLeft[curPlayer]--;
            curPlayer = 1 - curPlayer;

            //if (isBot[curPlayer] && depth == 1)
            if (isBot[curPlayer] && depth == 1)
            {
                BotViewModel bot = new BotViewModel();
                bot.MakeMove(this, 1);
                //BoardViewModel cloneBoard = new BoardViewModel(this);
                //bot.MakeMove(ref cloneBoard, depth);
            }
            return;
        }
    }
}
