using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    class BotViewModel
    {
        const int SIZE = 9;
        const int INF = (int)1e9;
        //const int MAX_DEPTH = 3;
        int MAX_DEPTH;

        static int counter = 1;

        public BotViewModel(string botType)
        {
            MAX_DEPTH = 1;
            if (botType == "EasyBot") MAX_DEPTH = 1;
            if (botType == "MediumBot") MAX_DEPTH = 2;
            if (botType == "HardBot") MAX_DEPTH = 3;
        }




        private bool won(BoardViewModel board, int player)
        {
            if (player == 0 && board.playerLoc[player][1] == BoardViewModel.SIZE - 1) return true;
            if (player == 1 && board.playerLoc[player][1] == 0) return true;
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
            dist[board.playerLoc[player][0], board.playerLoc[player][1]] = 0;
            q.Enqueue((board.playerLoc[player][0], board.playerLoc[player][1]));
            while (q.Count > 0)
            {
                (int curX, int curY) = q.Dequeue();

                for (int t = 0; t < 4; t++)
                {
                    int x = curX + xdir[t];
                    int y = curY + ydir[t];
                    if (x < 0 || x >= SIZE || y < 0 || y >= SIZE) continue;

                    if (dist[x, y] != INF) continue;
                    if (!board.CanMoveDFS(curX, curY, x, y)) continue;
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
            if (myDist <= 1) return INF;
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
        public async Task<double[]> MakePawnMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;
            if (won(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, INF };
            }
            if (lost(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, -INF };
            }
            int bestX = -1;
            int bestY = -1;
            int cntBest = 0;
            double bestEval = -INF - 1;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {

                    if (board.CanMove(i, j))
                    {

                        int oldX = board.playerLoc[board.curPlayer][0];
                        int oldY = board.playerLoc[board.curPlayer][1];
                        board.Move(i, j);
                        double curEval;
                        if (depth < MAX_DEPTH)
                        {
                            curEval = -(await MakeMove(board, depth + 1));
                            
                        }
                        else
                        {
                            //Application.Current.MainPage.DisplayAlert("MakePawnMove:", "evaluating" + depth, "OK");
                            //board.curPlayer = 1 - board.curPlayer;
                            curEval = -evaluate(board, 1 - player);
                            
                        }
                        board.MoveBack(oldX, oldY);


                        if (curEval > bestEval)
                        {
                            bestX = i;
                            bestY = j;
                            bestEval = curEval;
                            cntBest = 1;
                        }
                        else if (curEval == bestEval)
                        {
                            cntBest++;
                            var rand = new Random();

                            // Generate and display 5 random byte (integer) values.
                            //var bytes = new byte[2];
                            if (rand.Next() % cntBest == 0)
                            {
                                bestX = i;
                                bestY = j;
                                bestEval = curEval;
                            }
                        }
                    }
                }
            }
            double[] res = new double[] { bestX, bestY, bestEval };
            return res;
        }


        public async Task<double[]> MakeHorBlockMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;
            if (won(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, INF };
            }
            if (lost(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, -INF };
            }
            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF - 1;
            int cntBest = 0;
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {
                    if (!board.CanPlaceBlockHor(i, j)) continue;
                    board.PlaceBlockHor(i, j);
                    if (!board.CanPlaceBlockHor(i + 1, j))
                    {
                        board.RemovePendingBlockHor(i, j);
                        continue;
                    }

                    board.PlaceBlockHor(i+1, j);

                    double curEval;
                    if (depth < MAX_DEPTH)
                    {
                        curEval = -(await MakeMove(board, depth + 1));
                        
                    }
                    else
                    {
                        curEval = -evaluate(board, 1 - player);
                        
                    }
                    board.RemoveBlockHor(i, j);

                    if (curEval > bestEval)
                    {
                        bestX = i;
                        bestY = j;
                        bestEval = curEval;
                        cntBest = 1;
                    }
                    else if (curEval == bestEval)
                    {
                        cntBest++;
                        var rand = new Random();

                        // Generate and display 5 random byte (integer) values.
                        //var bytes = new byte[2];
                        if (rand.Next() % cntBest == 0)
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
        public async Task<double[]> MakeVerBlockMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;

            if (won(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, INF };
            }
            if (lost(board, player))
            {
                //board.curPlayer = 1 - board.curPlayer;
                return new double[] { -1, -1, -INF };
            }

            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF - 1;
            int cntBest = 0;
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {
                    if (!board.CanPlaceBlockVer(i, j)) continue;
                    board.PlaceBlockVer(i, j);
                    if (!board.CanPlaceBlockVer(i, j + 1))
                    {
                        board.RemovePendingBlockVer(i, j);
                        continue;
                    }
                    board.PlaceBlockVer(i, j + 1);

                    double curEval;
                    if (depth < MAX_DEPTH)
                    {
                        curEval = -(await MakeMove(board, depth + 1));
                    }
                    else
                    {
                        curEval = -evaluate(board, 1 - player);
                        
                    }
                    board.RemoveBlockVer(i, j);


                    if (curEval > bestEval)
                    {
                        bestX = i;
                        bestY = j;
                        bestEval = curEval;
                        cntBest = 1;
                    }
                    else if (curEval == bestEval)
                    {
                        cntBest++;
                        var rand = new Random();
                        if (rand.Next() % cntBest == 0)
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
        
        double max3(double a, double b, double c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        public async Task<double> MakeMove(BoardViewModel board, int depth = 1)
        {
            int player = board.curPlayer;
            /*
            if (depth >= MAX_DEPTH)
            {
                double val = evaluate(board, player);
                return val;
            }
            */
            double[] pawnMoveRes = await MakePawnMove(board, depth);
            double[] horBlockMoveRes = await MakeHorBlockMove(board, depth);
            double[] verBlockMoveRes = await MakeVerBlockMove(board, depth);
            
            double maxV = max3(pawnMoveRes[2], horBlockMoveRes[2], verBlockMoveRes[2]);
            if (pawnMoveRes[2] == maxV)
            {
                int oldX = board.playerLoc[board.curPlayer][0];
                int oldY = board.playerLoc[board.curPlayer][1];
                board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1]);
                if (depth > 1)
                {
                    board.MoveBack(oldX, oldY);
                }
            }
            else if (horBlockMoveRes[2] == maxV)
            {
                board.PlaceBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1]);
                board.PlaceBlockHor((int)horBlockMoveRes[0] + 1, (int)horBlockMoveRes[1]);
                if (depth > 1)
                {
                    board.RemoveBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1]);
                }
            }
            else
            {
                board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1]);
                board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1] + 1);
                if (depth > 1)
                {
                    board.RemoveBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1]);
                }
            }
            return maxV;

        }
    }
}
