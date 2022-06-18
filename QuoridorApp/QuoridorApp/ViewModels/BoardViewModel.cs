using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public int[][] playerStartLoc = new int[][]{ new int[]{ SIZE / 2, 0 }, new int[]{ SIZE / 2, SIZE - 1 } };

        public static string[] stateString = new string[] { "Empty", "Player1", "Player2" };


        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public string[,] horBlockBoard; // {"Empty", "Player1", "Player2"}
        public string[,] verBlockBoard; // {"Empty", "Player1", "Player2"}
        public string[,] centerBlocked; // {"Empty", "Player1", "Player2"}
        public int[][] playerLoc; // [player index][{0 - (col - left / right), 1 - (row - up / down)}]
        public int[] blocksLeft;

        public string blockStatus; // current state of blocks

        public int curPlayer;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        public BoardViewModel(BoardViewModel b)
        {
            horBlockBoard = new string[SIZE, SIZE - 1];
            verBlockBoard = new string[SIZE - 1, SIZE];
            centerBlocked = new string[SIZE - 1, SIZE - 1];
            playerLoc = new int[][] { new int[] { b.playerLoc[0][0], b.playerLoc[0][1] }, new int[] { b.playerLoc[1][0], b.playerLoc[1][1] } };
            //playerLoc = new int[][] { new int[] { b.playerLoc[0][0], b.playerLoc[1][0] }, new int[] { b.playerLoc[0][1], b.playerLoc[1][1] } };
            Copy1D<int>(ref blocksLeft, b.blocksLeft);
            blockStatus = b.blockStatus;
            curPlayer = b.curPlayer;

            Copy2D<string>(ref horBlockBoard, b.horBlockBoard);
            Copy2D<string>(ref verBlockBoard, b.verBlockBoard);
            Copy2D<string>(ref centerBlocked, b.centerBlocked);
        }

        /*
        #region Go To Rating Change
        public static ICommand ToRatingChangeCommand => new Command(OnToRatingChangeCommand);

        public static async void OnToRatingChangeCommand()
        {
            Page p = new Views.RatingChangePage();
            await App.Current.MainPage.Navigation.PushAsync(p);

        }
        #endregion
        */
        public BoardViewModel()
        {
            horBlockBoard = new string[SIZE, SIZE - 1];
            verBlockBoard = new string[SIZE - 1, SIZE];
            centerBlocked = new string[SIZE - 1, SIZE - 1];
            //playerLoc = playerStartLoc;
            playerLoc = new int[][] { new int[] { playerStartLoc[0][0], playerStartLoc[0][1] }, new int[] { playerStartLoc[1][0], playerStartLoc[1][1] } };
            blocksLeft = new int[] { BLOCKS_NUM, BLOCKS_NUM };
            blockStatus = "Empty";
            curPlayer = 0;

            
            var rows = horBlockBoard.GetLength(0);
            var cols = horBlockBoard.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    horBlockBoard[i, j] = "Empty";
            rows = verBlockBoard.GetLength(0);
            cols = verBlockBoard.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    verBlockBoard[i, j] = "Empty";
            rows = centerBlocked.GetLength(0);
            cols = centerBlocked.GetLength(1);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    centerBlocked[i, j] = "Empty";

        }

        public static bool isBot(string s)
        {
            string[] botNames = new string[] { "EasyBot", "MediumBot", "HardBot" };
            foreach(var str in botNames)
            {
                if (str == s) return true;
            }
            return false;
        }

       
        private bool IsEmpty(int x, int y)
        {
            if (playerLoc[0][0] == x && playerLoc[0][1] == y) return false;
            if (playerLoc[1][0] == x && playerLoc[1][1] == y) return false;
            return true;
        }

        public bool CheckWonPos(int player, int col)
        {
            //return playerLoc[player][1] == SIZE - 1 - playerStartLoc[player][1];
            return col == SIZE - 1 - playerStartLoc[player][1];
        }

        public bool CheckWon()
        {
            return (playerLoc[0][1] == SIZE - 1) || (playerLoc[1][1] == 0);
        }


        public bool CanMoveDFS(int curX, int curY, int newX, int newY)
        {
            if (newX < 0 || newX >= SIZE || newY < 0 || newY >= SIZE) return false;
            if (Math.Abs(curX - newX) + Math.Abs(curY - newY) != 1) return false;
            if (curX == newX)
            {
                return horBlockBoard[curX, Math.Min(curY, newY)] == "Empty";
            }
            else
            {
                //if (curY != newY) return false;
                return verBlockBoard[Math.Min(curX, newX), curY] == "Empty";
            }
        }

        private void DFS(int curX, int curY, bool[,] vis)
        {
            int[] xdir = new int[] { -1, 1, 0, 0 };
            int[] ydir = new int[] { 0, 0, -1, 1 };

            for (int t = 0; t < 4; t++)
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
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    vis[i, j] = false;
                }
            }
            DFS(playerLoc[player][0], playerLoc[player][1], vis);
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (vis[i, j])
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
            if (!IsEmpty(newX, newY)) return false;
            if (blockStatus != "Empty") return false;
            int player = curPlayer;
            if (Math.Abs(playerLoc[player][0] - newX) + Math.Abs(playerLoc[player][1] - newY) == 0) return false;

            int curX = playerLoc[player][0];
            int curY = playerLoc[player][1];
            if (Math.Abs(playerLoc[player][0] - newX) + Math.Abs(playerLoc[player][1] - newY) > 1)
            {
                int nextPlayer = 1 - player;
                if (!CanMoveDFS(curX, curY, playerLoc[nextPlayer][0], playerLoc[nextPlayer][1]))
                {
                    return false;
                }
                if (!CanMoveDFS(playerLoc[nextPlayer][0], playerLoc[nextPlayer][1], newX, newY))
                {
                    return false;
                }
                return true;
            }

            if (curX == newX)
            {
                if (horBlockBoard[curX, Math.Min(curY, newY)] != "Empty")
                {
                    return false;
                }
            }
            else
            {
                if (verBlockBoard[Math.Min(curX, newX), curY] != "Empty")
                {
                    return false;
                }
            }
            
            return true;
        }


        public void MoveBack(int oldX, int oldY)
        {
            curPlayer = 1 - curPlayer;
            playerLoc[curPlayer][0] = oldX;
            playerLoc[curPlayer][1] = oldY;
        }

        
        public void Move(int newX, int newY)
        {
            
            if (!CanMove(newX, newY)) return;
            int player = curPlayer;

            int curX = playerLoc[player][0];
            int curY = playerLoc[player][1];
            /*
            if (Math.Abs(playerLoc[player][0] - newX) + Math.Abs(playerLoc[player][1] - newY) > 1)
            {
                int nextPlayer = 1 - player;
                curX = playerLoc[nextPlayer][0];
                curY = playerLoc[nextPlayer][1];
            }
            */
            playerLoc[player][0] = newX;
            playerLoc[player][1] = newY;
            if (CheckWon())
            {
                //return;
            }
            curPlayer = 1 - curPlayer;
        }

        /*
        Returns true iff a new block is placed.
        */
        public bool CanPlaceBlockHor(int X, int Y)
        {
            int player = curPlayer;
            if (blocksLeft[player] == 0) return false;

            if (horBlockBoard[X, Y] == "Pending") return true;
            if (blockStatus == "Ver" || horBlockBoard[X, Y] != "Empty") return false;

            if (blockStatus == "Empty") return true; // first of the 2 blocks


            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X + 1; int nextY = Y;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE && nextY >= 0 && nextY < SIZE - 1)
                {
                    if (horBlockBoard[nextX, nextY] == "Pending")
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            if (!hasNextTo) return false;
            if (centerBlocked[Math.Min(X, nextX), Y] != "Empty") return false;
            

            // try to put the block and run a dfs to make sure it doesn't completely block one of the players
            blockStatus = "Empty";

            string stateXY = horBlockBoard[X, Y];
            string stateNextXY = horBlockBoard[nextX, nextY];
            horBlockBoard[X, Y] = stateString[curPlayer + 1];
            horBlockBoard[nextX, nextY] = stateString[curPlayer + 1];
            centerBlocked[Math.Min(X, nextX), Y] = stateString[curPlayer + 1];


            bool ans;
            if (!CanWin(0) || !CanWin(1)) ans = false;
            else ans = true;
            blockStatus = "Hor";
            horBlockBoard[X, Y] = stateXY;
            horBlockBoard[nextX, nextY] = stateNextXY;
            centerBlocked[Math.Min(X, nextX), Y] = "Empty";
            return ans;
        }

        public void RemovePendingBlockHor(int X, int Y)
        {
            horBlockBoard[X, Y] = "Empty";
            blockStatus = "Empty";
        }
        public void RemoveBlockHor(int X, int Y)
        {
            curPlayer = 1 - curPlayer;
            centerBlocked[X, Y] = "Empty";
            horBlockBoard[X, Y] = "Empty";
            horBlockBoard[X + 1, Y] = "Empty";
            blocksLeft[curPlayer]++;
        }


        public void PlaceBlockHor(int X, int Y)
        {
            if (!CanPlaceBlockHor(X, Y)) return;
            int player = curPlayer;

            // Empty pending block:
            if (horBlockBoard[X, Y] == "Pending")
            {
                horBlockBoard[X, Y] = "Empty";
                blockStatus = "Empty";
                return;
            }


            if (blockStatus == "Empty") // first of the 2 blocks
            {
                horBlockBoard[X, Y] = "Pending";
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
                    if (horBlockBoard[nextX, nextY] == "Pending")
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X - 1;
                nextY = Y;
            }

            centerBlocked[Math.Min(X, nextX), Y] = stateString[curPlayer + 1];

            blockStatus = "Empty";
            horBlockBoard[X, Y] = stateString[curPlayer + 1];
            horBlockBoard[nextX, nextY] = stateString[curPlayer + 1];
            blocksLeft[player]--;
            curPlayer = 1 - curPlayer;

        }

        public void RemovePendingBlockVer(int X, int Y)
        {
            verBlockBoard[X, Y] = "Empty";
            blockStatus = "Empty";
        }

        public void RemoveBlockVer(int X, int Y)
        {
            curPlayer = 1 - curPlayer;
            centerBlocked[X, Y] = "Empty";
            verBlockBoard[X, Y] = "Empty";
            verBlockBoard[X, Y + 1] = "Empty";
            blocksLeft[curPlayer]++;
        }
        public bool CanPlaceBlockVer(int X, int Y)
        {
            //BoardViewModel board2 = new BoardViewModel(this, false);
            int player = curPlayer;
            if (blocksLeft[player] == 0) return false;

            if (verBlockBoard[X, Y] == "Pending") return true;
            if (blockStatus == "Hor" || verBlockBoard[X, Y] != "Empty") return false;

            if (blockStatus == "Empty") return true; // first of the 2 blocks

            // Now check if the last pending block is next to this
            bool hasNextTo = false;
            int nextX = X; int nextY = Y + 1;

            for (int i = 0; i < 2 && !hasNextTo; i++)
            {
                if (nextX >= 0 && nextX < SIZE - 1 && nextY >= 0 && nextY < SIZE)
                {
                    if (verBlockBoard[nextX, nextY] == "Pending")
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X;
                nextY = Y - 1;
            }

            if (!hasNextTo) return false;
            if (centerBlocked[X, Math.Min(Y, nextY)] != "Empty") return false;




            //// try to put the block and run a dfs to make sure it doesn't completely block one of the players
            //blockStatus = "Empty";

            //string stateXY = horBlockBoard[X, Y];
            //string stateNextXY = horBlockBoard[nextX, nextY];
            //horBlockBoard[X, Y] = stateString[curPlayer + 1];
            //horBlockBoard[nextX, nextY] = stateString[curPlayer + 1];
            //centerBlocked[Math.Min(X, nextX), Y] = stateString[curPlayer + 1];


            //bool ans;
            //if (!CanWin(0) || !CanWin(1)) ans = false;
            //else ans = true;
            //blockStatus = "Hor";
            //horBlockBoard[X, Y] = stateXY;
            //horBlockBoard[nextX, nextY] = stateNextXY;
            //centerBlocked[Math.Min(X, nextX), Y] = "Empty";
            //return ans;



            //centerBlocked[X, Math.Min(Y, nextY)].fill();
            blockStatus = "Empty";

            string stateXY = verBlockBoard[X, Y];
            string stateNextXY = verBlockBoard[nextX, nextY];
            verBlockBoard[X, Y] = stateString[curPlayer + 1];
            verBlockBoard[nextX, nextY] = stateString[curPlayer + 1];
            centerBlocked[X, Math.Min(Y, nextY)] = stateString[curPlayer + 1];
            
            
            bool ans;
            if (!CanWin(0) || !CanWin(1)) ans = false;
            else ans = true;

            blockStatus = "Ver";
            verBlockBoard[X, Y] = stateXY;
            verBlockBoard[nextX, nextY] = stateNextXY;
            centerBlocked[X, Math.Min(Y, nextY)] = "Empty";
            return ans;
        }

        public void PlaceBlockVer(int X, int Y)
        {
            if (!CanPlaceBlockVer(X, Y)) return;
            if (verBlockBoard[X, Y] == "Pending")
            {
                verBlockBoard[X, Y] = "Empty";
                blockStatus = "Empty";
                return;
            }
            if (blockStatus == "Empty") // first of the 2 blocks
            {
                verBlockBoard[X, Y] = "Pending";
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
                    if (verBlockBoard[nextX, nextY] == "Pending")
                    {
                        hasNextTo = true;
                        break;
                    }
                }
                nextX = X;
                nextY = Y - 1;
            }
            //centerTile[X, Math.Min(Y, nextY)].fill();
            //centerTile[X, Math.Min(Y, nextY)].fill();
            centerBlocked[X, Math.Min(Y, nextY)] = stateString[curPlayer + 1];
            blockStatus = "Empty";
            verBlockBoard[X, Y] = stateString[curPlayer + 1];
            verBlockBoard[nextX, nextY] = stateString[curPlayer + 1];
            blocksLeft[curPlayer]--;
            curPlayer = 1 - curPlayer;
            return;
        }















        private bool IsPlayer(string playerName)
        {
            if (BoardViewModel.isBot(playerName))
            {
                return false;
            }
            return true;
        }

        private Player GetPlayer(string playerName)
        {
            if (playerName == "Me") return CurrentApp.CurrentPlayer;
            return null;
        }


        private async Task<int> GetRating(string playerName)
        {
            //return 2222;
            if (BoardViewModel.isBot(playerName))
            {
                return BotViewModel.GetBotRating(playerName);
            }
            if (playerName == "Guest") return -1;
            //return RatingChange.INITIAL_RATING;
            Player player;
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            if (playerName == "Me")
            {
                player = CurrentApp.CurrentPlayer;
            }
            else
            {
                player = CurrentApp.CurrentPlayer;
                //player = await proxy.GetPlayer(playerName);
            }
            RatingChange ratingChange = await proxy.GetLastRatingChange(player);
            if (ratingChange == null) // Player hasn't played yet
            {
                return RatingChange.INITIAL_RATING;
            }
            return ratingChange.AlteredRating;
        }
        // Returns: { {WinnerInitRating, WinnerUpdatedRating}, {LoserInitRating, LoserUpdatedRating}}
        public async Task<int[][]> FinishGameAsync(string Winner, string Loser)
        {
            int WinnerInitRating, WinnerUpdatedRating, LoserInitRating, LoserUpdatedRating;
            int winnerInit, loserInit, winnerUpdated, loserUpdated;
            winnerInit = await GetRating(Winner);
            loserInit = await GetRating(Loser);
            LoserInitRating = loserInit;
            WinnerInitRating = winnerInit;
            //WinnerInitRating = await GetRating(Winner);
            //LoserInitRating = await GetRating(Loser);


            int newWinnerRating = RatingChange.EloRating(WinnerInitRating, LoserInitRating, true);
            int newLoserRating = RatingChange.EloRating(LoserInitRating, WinnerInitRating, false);
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            if (IsPlayer(Winner))
            {
                RatingChange winnerRatingChange = new RatingChange(GetPlayer(Winner), newWinnerRating);
                await proxy.UpdateRatingChange(winnerRatingChange);
            }
            if (IsPlayer(Loser))
            {
                RatingChange loserRatingChange = new RatingChange(GetPlayer(Loser), newLoserRating);
                await proxy.UpdateRatingChange(loserRatingChange);
            }


            winnerUpdated = await GetRating(Winner);
            loserUpdated = await GetRating(Loser);


            WinnerUpdatedRating = winnerUpdated;
            LoserUpdatedRating = loserUpdated;
            return new int[][] { new int[] { WinnerInitRating, WinnerUpdatedRating }, new int[] { LoserInitRating, LoserUpdatedRating } }; ;
        }

        /// /////////////////////////////////////////////////////////////////////////////

    }
}
