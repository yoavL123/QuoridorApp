using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    class BotViewModel
    {
        const int SIZE = 9;
        const int INF = (int)1e9;
        const int MAX_DEPTH = 2;

        public BotViewModel()
        {

        }
        private bool won(BoardViewModel board, int player)
        {
            if (player == 0 && board.playerLoc[player, 1] == BoardViewModel.SIZE - 1) return true;
            if (player == 1 && board.playerLoc[player, 1] == 0) return true;
            return false;
        }
        private bool lost(BoardViewModel board, int player)
        {
            return won(board, 1 - player);
        }


        private int DistToWin(BoardViewModel board, int player)
        {
            int[] xdir = new int[] { -1, 1, 0, 0 };
            int[] ydir = new int[] { 0, 0, -1, 1 };

            Queue<(int x, int y)> q = new Queue<(int x, int y)>();
            int[,] dist = new int[SIZE, SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    dist[i, j] = INF;
                }
            }
            dist[board.playerLoc[player, 0], board.playerLoc[player, 1]] = 0;
            q.Enqueue((board.playerLoc[player, 0], board.playerLoc[player, 1]));
            while (q.Count > 0)
            {
                (int curX, int curY) = q.Dequeue();

                for (int t = 0; t < 4; t++)
                {
                    int x = curX + xdir[t];
                    int y = curY + ydir[t];
                    if (x < 0 || x >= SIZE || y < 0 || y >= SIZE) continue;

                    if (dist[x, y] != INF) continue;
                    if (!board.CanMove(curX, curY, x, y)) continue;
                    dist[x, y] = dist[curX, curY] + 1;
                    q.Enqueue((x, y));
                }
            }
            int shortestDist = INF;
            for (int i = 0; i < SIZE; i++)
            {
                if (player == 0)
                {
                    shortestDist = Math.Min(shortestDist, dist[i, SIZE - 1]);
                }
                else
                {
                    shortestDist = Math.Min(shortestDist, dist[i, 0]);
                }
            }
            return shortestDist;
        }
        /*
        Evaluates the situation of player in the board
        Higher number - better situation
        For now, we will do it somewhat naively.
        Evaluation will consider:
        1. Shortest distance to win
        2. Shortest distance for other player to win
        3. Number of my blocks
        4. Number of his blocks
        The formula could be:
        (2 - 1) * CONST1 + (3 - 4) * CONST2
         */
        public double evaluate(BoardViewModel board, int player)
        {
            if (won(board, player)) return INF;
            if (lost(board, player)) return -INF;
            int myDist = DistToWin(board, player);
            int hisDist = DistToWin(board, 1 - player);

            int myBlocks = board.blocksLeft[player];
            int hisBlocks = board.blocksLeft[1 - player];

            const int COEF1 = 1;
            const int COEF2 = 1;
            return (hisDist - myDist) * COEF1 + (myBlocks - hisBlocks) * COEF2;
        }

        /*
        
        [bestX, bestY, bestEval]

        */
        public double[] MakePawnMove(ref BoardViewModel board, int player, int depth)
        {
            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF;
            for(int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    BoardViewModel nBoard = new BoardViewModel(board);
                    if(nBoard.CanMove(i, j, nBoard))
                    {
                        double curEval;
                        if(depth < MAX_DEPTH || true)
                        {
                            curEval = MakeMove(ref nBoard, 1 - player, depth + 1);
                        }
                        else
                        {
                            curEval = -evaluate(nBoard, 1 - player);
                        }
                        if(curEval > bestEval)
                        {
                            bestX = i;
                            bestY = j;
                            bestEval = curEval;
                        }
                    }
                }
            }
            double[] res = new double[] { bestX, bestY, bestEval };
            return res;
        }


        public double[] MakeHorBlockMove(ref BoardViewModel board, int player, int depth)
        {
            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE - 2; j++)
                {
                    BoardViewModel nBoard = new BoardViewModel(board);
                    bool b1 = nBoard.CanPlaceBlockHor(i, j, nBoard);
                    if (!b1) continue;
                    bool b2 = nBoard.CanPlaceBlockHor(i, j + 1, nBoard);
                    if (!b2) continue;
                    nBoard.PlaceBlockHor(i, j, nBoard);
                    nBoard.PlaceBlockHor(i, j+1, nBoard);

                    double curEval;
                    if (depth < MAX_DEPTH || true)
                    {
                        curEval = MakeMove(ref nBoard, 1 - player, depth + 1);
                    }
                    else
                    {
                        curEval = -evaluate(nBoard, 1 - player);
                    }
                    if (curEval > bestEval)
                    {
                        bestX = i;
                        bestY = j;
                        bestEval = curEval;
                    }
                    
                }
            }
            double[] res = new double[] { bestX, bestY, bestEval };
            return res;
        }
        public double[] MakeVerBlockMove(ref BoardViewModel board, int player, int depth)
        {
            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF;
            for (int i = 0; i < SIZE - 2; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    BoardViewModel nBoard = new BoardViewModel(board);
                    bool b1 = nBoard.PlaceBlockVer(i, j, nBoard);
                    bool b2 = nBoard.PlaceBlockVer(i + 1, j, nBoard);
                    if (b1 && b2)
                    {
                        double curEval;
                        if (depth < MAX_DEPTH || true)
                        {
                            curEval = MakeMove(ref nBoard, 1 - player, depth + 1);
                        }
                        else
                        {
                            curEval = -evaluate(nBoard, 1 - player);
                        }
                        if (curEval > bestEval)
                        {
                            bestX = i;
                            bestY = j;
                            bestEval = curEval;
                        }
                    }
                }
            }
            double[] res = new double[] { bestX, bestY, bestEval };
            return res;
        }
        /*
        public void makeHorBlockMove()
        {

        }
        */
        double max3(double a, double b, double c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        public double MakeMove(ref BoardViewModel board, int player, int depth)
        {
            if(depth >= MAX_DEPTH) return -evaluate(board, player);
            double[] pawnMoveRes = MakePawnMove(ref board, player, depth);
            double[] horBlockMoveRes = MakeHorBlockMove(ref board, player, depth);
            double[] verBlockMoveRes = MakeVerBlockMove(ref board, player, depth);
            //double[] pawnMoveRes = MakePawnMove(ref board, player, depth);
            double maxV = max3(pawnMoveRes[2], horBlockMoveRes[2], verBlockMoveRes[2]);
            if (pawnMoveRes[2] == maxV)
            {
                board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1], board);
            }
            else if (horBlockMoveRes[2] == maxV)
            {
                board.PlaceBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1], board);
                board.PlaceBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1] + 1, board);
            }
            else
            {
                board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1], board);
                board.PlaceBlockVer((int)verBlockMoveRes[0] + 1, (int)verBlockMoveRes[1], board);
            }
            Application.Current.MainPage.DisplayAlert("maxV:", $"maxV: {maxV}, vPawn: {pawnMoveRes[2]}, vHor: {horBlockMoveRes[2]}, vVer: {verBlockMoveRes[2]}", "Back to home");


            return maxV;
        }
    }
}
