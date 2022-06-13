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
        const int MAX_DEPTH = 3;

        static int counter = 1;

        public BotViewModel()
        {

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
        public double[] MakePawnMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;
            if (won(board, player)) return new double[] { -1, -1, INF };
            if (lost(board, player)) return new double[] { -1, -1, -INF };
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
                        //BoardViewModel nBoard = new BoardViewModel(board);
                        //BoardViewModel nBoard = new BoardViewModel(board);
                        //nBoard.Move(i, j, false);
                        int oldX = board.playerLoc[board.curPlayer][0];
                        int oldY = board.playerLoc[board.curPlayer][1];
                        board.Move(i, j, depth);
                        //nBoard.Move(i, j, false, depth+1);
                        double curEval;
                        if (depth < MAX_DEPTH)
                        {
                            //board.curPlayer = 1 - board.curPlayer;
                            curEval = -MakeMove(board, depth + 1);
                            //board.curPlayer = 1 - board.curPlayer;
                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert("MakePawnMove:", "evaluating" + depth, "OK");
                            board.curPlayer = 1 - board.curPlayer;
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


        public double[] MakeHorBlockMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;
            const int DIF = 1;
            if (won(board, player)) return new double[] { -1, -1, INF };
            if (lost(board, player)) return new double[] { -1, -1, -INF };
            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF - 1;
            int cntBest = 0;
            int add = 0;
            if (depth <= 1) add += 1;
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {
                    //bool prune = true;
                    //if(abs(board.playerLoc[0, 0])
                    if (!board.CanPlaceBlockHor(i, j)) continue;
                    //BoardViewModel nBoard = new BoardViewModel(board);
                    //bool b1 = nBoard.CanPlaceBlockHor(i, j);
                    //if (!b1) continue;
                    board.PlaceBlockHor(i, j, depth + 1);
                    bool b2 = board.CanPlaceBlockHor(i + 1, j);
                    if (!b2)
                    {
                        board.RemovePendingBlockHor(i, j);
                        continue;
                    }

                    board.PlaceBlockHor(i + 1, j, depth + 1);


                    if ((-evaluate(board, 1 - player) + add < bestEval) && false)
                    {
                        board.RemoveBlockHor(i, j);
                        continue;
                    }


                    double curEval;
                    if (depth < MAX_DEPTH)
                    {
                        //board.curPlayer = 1 - board.curPlayer;
                        curEval = -MakeMove(board, depth + 1);
                        //board.curPlayer = 1 - board.curPlayer;
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("MakeHorBlockMove:", "evaluating" + depth, "OK");
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
        public double[] MakeVerBlockMove(BoardViewModel board, int depth)
        {
            int player = board.curPlayer;
            
            if (won(board, player)) return new double[] { -1, -1, INF };
            if (lost(board, player)) return new double[] { -1, -1, -INF };

            int bestX = -1;
            int bestY = -1;
            double bestEval = -INF - 1;
            int cntBest = 0;
            for (int i = 0; i < SIZE - 1; i++)
            {
                for (int j = 0; j < SIZE - 1; j++)
                {
                    if (!board.CanPlaceBlockVer(i, j)) continue;
                    board.PlaceBlockVer(i, j, depth + 1);
                    bool b2 = board.CanPlaceBlockVer(i, j + 1);
                    if (!b2)
                    {
                        board.RemovePendingBlockVer(i, j);
                        continue;
                    }
                    board.PlaceBlockVer(i, j + 1, depth + 1);


                    if ((-evaluate(board, 1 - player) < bestEval) && false)
                    {
                        board.RemoveBlockVer(i, j);
                        continue;
                    }



                    double curEval;
                    //Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "depth = " + depth, "OK");
                    if (depth < MAX_DEPTH)
                    {
                        //Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "making move" + depth, "OK");
                        //board.curPlayer = 1 - board.curPlayer;
                        curEval = -MakeMove(board, depth + 1);
                        //board.curPlayer = 1 - board.curPlayer;
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "evaluating" + depth, "OK");
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
        /*
        public void makeHorBlockMove()
        {

        }
        */
        double max3(double a, double b, double c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        public double MakeMove(BoardViewModel board, int depth = 1)
        {
            counter++;
            /*
            if(counter % 100 == 0)
            {
                Application.Current.MainPage.DisplayAlert("counter: " + counter, "OK", "OK");
            }
            */
            Console.WriteLine("depth = something");
            int player = board.curPlayer;
            if (depth >= MAX_DEPTH)
            {
                double val = evaluate(board, player);
                //board.curPlayer = 1 - board.curPlayer;
                return val;
            }
            //BoardViewModel nBoard = new BoardViewModel(board);
            double[] pawnMoveRes = MakePawnMove(board, depth);
            //board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1], realGame);
            //return pawnMoveRes[2];

            double[] horBlockMoveRes = MakeHorBlockMove(board, depth);
            double[] verBlockMoveRes = MakeVerBlockMove(board, depth);
            //board.curPlayer = (board.curPlayer + 1) % 2;
            //double[] pawnMoveRes = MakePawnMove(ref board, player, depth);
            double maxV = max3(pawnMoveRes[2], horBlockMoveRes[2], verBlockMoveRes[2]);
            if (depth == 1)
            {
                //Application.Current.MainPage.DisplayAlert("counter: " + counter, "OK", "OK");
            }
            if (pawnMoveRes[2] == maxV)
            {
                int oldX = board.playerLoc[board.curPlayer][0];
                int oldY = board.playerLoc[board.curPlayer][1];
                if (depth == 1)
                {
                    Console.WriteLine("hi");
                }
                board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1], depth);

                if (depth > 1)
                {
                    board.MoveBack(oldX, oldY);
                }
            }
            else if (horBlockMoveRes[2] == maxV)
            {
                board.PlaceBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1], depth);
                board.PlaceBlockHor((int)horBlockMoveRes[0] + 1, (int)horBlockMoveRes[1], depth);
                if (depth > 1)
                {
                    board.RemoveBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1]);
                }
            }
            else
            {
                board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1], depth);
                board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1] + 1, depth);
                if (depth > 1)
                {
                    board.RemoveBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1]);
                }
            }



            return maxV;

        }

        //private bool won(BoardViewModel board, int player)
        //{
        //    if (player == 0 && board.playerLoc[player, 1] == BoardViewModel.SIZE - 1) return true;
        //    if (player == 1 && board.playerLoc[player, 1] == 0) return true;
        //    return false;
        //}
        //private bool lost(BoardViewModel board, int player)
        //{
        //    return won(board, 1 - player);
        //}

        //private int DistToWin(BoardViewModel board, int player)
        //{
        //    int[] xdir = new int[] { -1, 1, 0, 0 };
        //    int[] ydir = new int[] { 0, 0, -1, 1 };

        //    Queue<(int x, int y)> q = new Queue<(int x, int y)>();
        //    int[,] dist = new int[SIZE, SIZE];
        //    for (int i = 0; i < SIZE; i++)
        //    {
        //        for (int j = 0; j < SIZE; j++)
        //        {
        //            dist[i, j] = INF;
        //        }
        //    }
        //    dist[board.playerLoc[player, 0], board.playerLoc[player, 1]] = 0;
        //    q.Enqueue((board.playerLoc[player, 0], board.playerLoc[player, 1]));
        //    while (q.Count > 0)
        //    {
        //        (int curX, int curY) = q.Dequeue();

        //        for (int t = 0; t < 4; t++)
        //        {
        //            int x = curX + xdir[t];
        //            int y = curY + ydir[t];
        //            if (x < 0 || x >= SIZE || y < 0 || y >= SIZE) continue;

        //            if (dist[x, y] != INF) continue;
        //            if (!board.CanMoveDFS(curX, curY, x, y)) continue;
        //            dist[x, y] = dist[curX, curY] + 1;
        //            q.Enqueue((x, y));
        //        }
        //    }
        //    int shortestDist = INF;
        //    for (int i = 0; i < SIZE; i++)
        //    {
        //        if (player == 0)
        //        {
        //            shortestDist = Math.Min(shortestDist, dist[i, SIZE - 1]);
        //        }
        //        else
        //        {
        //            shortestDist = Math.Min(shortestDist, dist[i, 0]);
        //        }
        //    }
        //    return shortestDist;
        //}
        ///*
        //Evaluates the situation of player in the board
        //Higher number - better situation
        //For now, we will do it somewhat naively.
        //Evaluation will consider:
        //1. Shortest distance to win
        //2. Shortest distance for other player to win
        //3. Number of my blocks
        //4. Number of his blocks
        //The formula could be:
        //(2 - 1) * CONST1 + (3 - 4) * CONST2
        // */
        //public double evaluate(BoardViewModel board, int player)
        //{
        //    if (won(board, player)) return INF;
        //    if (lost(board, player)) return -INF;
        //    int myDist = DistToWin(board, player);
        //    if (myDist <= 1) return INF;
        //    int hisDist = DistToWin(board, 1 - player);

        //    int myBlocks = board.blocksLeft[player];
        //    int hisBlocks = board.blocksLeft[1 - player];

        //    const int COEF1 = 1;
        //    const int COEF2 = 1;
        //    return (hisDist - myDist) * COEF1 + (myBlocks - hisBlocks) * COEF2;
        //}

        ///*

        //[bestX, bestY, bestEval]

        //*/
        //public double[] MakePawnMove(BoardViewModel board, int depth)
        //{
        //    int player = board.curPlayer;
        //    if(won(board, player)) return new double[] { -1, -1, INF};
        //    if (lost(board, player)) return new double[] { -1, -1, -INF };
        //    int bestX = -1;
        //    int bestY = -1;
        //    int cntBest = 0;
        //    double bestEval = -INF - 1;
        //    for(int i = 0; i < SIZE; i++)
        //    {
        //        for (int j = 0; j < SIZE; j++)
        //        {

        //            if(board.CanMove(i, j))
        //            {
        //                //BoardViewModel nBoard = new BoardViewModel(board);
        //                //BoardViewModel nBoard = new BoardViewModel(board);
        //                //nBoard.Move(i, j, false);
        //                int oldX = board.playerLoc[board.curPlayer, 0];
        //                int oldY = board.playerLoc[board.curPlayer, 1];
        //                board.Move(i, j, depth);
        //                //nBoard.Move(i, j, false, depth+1);
        //                double curEval;
        //                if(depth < MAX_DEPTH)
        //                {
        //                    //board.curPlayer = 1 - board.curPlayer;
        //                    curEval = -MakeMove(board, depth + 1);
        //                    //board.curPlayer = 1 - board.curPlayer;
        //                }
        //                else
        //                {
        //                    Application.Current.MainPage.DisplayAlert("MakePawnMove:", "evaluating" + depth, "OK");
        //                    board.curPlayer = (board.curPlayer + 1) % 2;
        //                    curEval = -evaluate(board, 1 - player);
        //                }
        //                board.MoveBack(oldX, oldY);
        //                if(curEval > bestEval)
        //                {
        //                    bestX = i;
        //                    bestY = j;
        //                    bestEval = curEval;
        //                    cntBest = 1;
        //                }
        //                else if (curEval == bestEval)
        //                {
        //                    cntBest++;
        //                    var rand = new Random();

        //                    // Generate and display 5 random byte (integer) values.
        //                    //var bytes = new byte[2];
        //                    if (rand.Next()%cntBest == 0)
        //                    {
        //                        bestX = i;
        //                        bestY = j;
        //                        bestEval = curEval;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    double[] res = new double[] { bestX, bestY, bestEval };
        //    return res;
        //}


        //public double[] MakeHorBlockMove(BoardViewModel board, int depth)
        //{
        //    int player = board.curPlayer;
        //    const int DIF = 1;
        //    if (won(board, player)) return new double[] { -1, -1, INF };
        //    if (lost(board, player)) return new double[] { -1, -1, -INF };
        //    int bestX = -1;
        //    int bestY = -1;
        //    double bestEval = -INF - 1;
        //    int cntBest = 0;
        //    int add = 0;
        //    if (depth <= 1) add += 1;
        //    for (int i = 0; i < SIZE - 1; i++)
        //    {
        //        for (int j = 0; j < SIZE - 1; j++)
        //        {
        //            //bool prune = true;
        //            //if(abs(board.playerLoc[0, 0])
        //            if (!board.CanPlaceBlockHor(i, j)) continue;
        //            //BoardViewModel nBoard = new BoardViewModel(board);
        //            //bool b1 = nBoard.CanPlaceBlockHor(i, j);
        //            //if (!b1) continue;
        //            board.PlaceBlockHor(i, j, depth+1);
        //            bool b2 = board.CanPlaceBlockHor(i + 1, j);
        //            if (!b2)
        //            {
        //                board.RemovePendingBlockHor(i, j);
        //                continue;
        //            }

        //            board.PlaceBlockHor(i + 1, j, depth+1);


        //            if ((-evaluate(board, 1 - player) + add < bestEval)&& false)
        //            {
        //                board.RemoveBlockHor(i, j);
        //                continue;
        //            }


        //            double curEval;
        //            if (depth < MAX_DEPTH)
        //            {
        //                //board.curPlayer = 1 - board.curPlayer;
        //                curEval = -MakeMove(board, depth + 1);
        //                //board.curPlayer = 1 - board.curPlayer;
        //            }
        //            else
        //            {
        //                Application.Current.MainPage.DisplayAlert("MakeHorBlockMove:", "evaluating" + depth, "OK");
        //                curEval = -evaluate(board, 1 - player);
        //            }
        //            board.RemoveBlockHor(i, j);
        //            if (curEval > bestEval)
        //            {
        //                bestX = i;
        //                bestY = j;
        //                bestEval = curEval;
        //                cntBest = 1;
        //            }
        //            else if (curEval == bestEval)
        //            {
        //                cntBest++;
        //                var rand = new Random();

        //                // Generate and display 5 random byte (integer) values.
        //                //var bytes = new byte[2];
        //                if (rand.Next()%cntBest == 0)
        //                {
        //                    bestX = i;
        //                    bestY = j;
        //                    bestEval = curEval;
        //                }
        //            }
        //        }
        //    }
        //    double[] res = new double[] { bestX, bestY, bestEval };
        //    return res;
        //}
        //public double[] MakeVerBlockMove(BoardViewModel board, int depth)
        //{
        //    int player = board.curPlayer;
        //    const int DIF = 1;
        //    if (won(board, player)) return new double[] { -1, -1, INF };
        //    if (lost(board, player)) return new double[] { -1, -1, -INF };
        //    int bestX = -1;
        //    int bestY = -1;
        //    double bestEval = -INF - 1;
        //    int cntBest = 0;
        //    int add = 0;
        //    if (depth <= 1) add += 1;
        //    //Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "starting", "OK");
        //    for (int i = 0; i < SIZE - 1; i++)
        //    {
        //        for (int j = 0; j < SIZE - 1; j++)
        //        {
        //            if (!board.CanPlaceBlockVer(i, j)) continue;
        //            //BoardViewModel nBoard = new BoardViewModel(board, false);
        //            //bool b1 = nBoard.CanPlaceBlockVer(i, j);
        //            //if (!b1) continue;
        //            board.PlaceBlockVer(i, j, depth+1);
        //            bool b2 = board.CanPlaceBlockVer(i, j+1);
        //            if (!b2)
        //            {
        //                board.RemovePendingBlockVer(i, j);
        //                continue;
        //            }
        //            board.PlaceBlockVer(i, j+1, depth+1);


        //            if ((-evaluate(board, 1 - player) + add < bestEval)&&false)
        //            {
        //                board.RemoveBlockVer(i, j);
        //                continue;
        //            }



        //            double curEval;
        //            //Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "depth = " + depth, "OK");
        //            if (depth < MAX_DEPTH)
        //            {
        //                //Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "making move" + depth, "OK");
        //                //board.curPlayer = 1 - board.curPlayer;
        //                curEval = -MakeMove(board, depth + 1);
        //                //board.curPlayer = 1 - board.curPlayer;
        //            }
        //            else
        //            {
        //                Application.Current.MainPage.DisplayAlert("MakeVerBlockMove:", "evaluating" + depth, "OK");
        //                curEval = -evaluate(board, 1 - player);
        //            }

        //            board.RemoveBlockVer(i, j);
        //            if (curEval > bestEval)
        //            {
        //                bestX = i;
        //                bestY = j;
        //                bestEval = curEval;
        //                cntBest = 1;
        //            }
        //            else if(curEval == bestEval)
        //            {
        //                cntBest++;
        //                var rand = new Random();

        //                // Generate and display 5 random byte (integer) values.
        //                //var bytes = new byte[2];
        //                if(rand.Next()%cntBest == 0)
        //                {
        //                    bestX = i;
        //                    bestY = j;
        //                    bestEval = curEval;
        //                }
        //            }

        //        }
        //    }
        //    double[] res = new double[] { bestX, bestY, bestEval };
        //    return res;
        //}
        ///*
        //public void makeHorBlockMove()
        //{

        //}
        //*/
        //double max3(double a, double b, double c)
        //{
        //    return Math.Max(a, Math.Max(b, c));
        //}

        //public double MakeMove(BoardViewModel board, int depth = 1)
        //{
        //    counter++;
        //    /*
        //    if(counter % 100 == 0)
        //    {
        //        Application.Current.MainPage.DisplayAlert("counter: " + counter, "OK", "OK");
        //    }
        //    */
        //    Console.WriteLine("depth = something");
        //    int player = board.curPlayer;
        //    if (depth >= MAX_DEPTH)
        //    {
        //        double val = evaluate(board, player);
        //        //board.curPlayer = 1 - board.curPlayer;
        //        return val;
        //    }
        //    //BoardViewModel nBoard = new BoardViewModel(board);
        //    double[] pawnMoveRes = MakePawnMove(board, depth);
        //    //board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1], realGame);
        //    //return pawnMoveRes[2];

        //    double[] horBlockMoveRes = MakeHorBlockMove(board, depth);
        //    double[] verBlockMoveRes = MakeVerBlockMove(board, depth);
        //    //board.curPlayer = (board.curPlayer + 1) % 2;
        //    //double[] pawnMoveRes = MakePawnMove(ref board, player, depth);
        //    double maxV = max3(pawnMoveRes[2], horBlockMoveRes[2], verBlockMoveRes[2]);
        //    if(depth == 1)
        //    {
        //        //Application.Current.MainPage.DisplayAlert("counter: " + counter, "OK", "OK");
        //    }
        //    if (pawnMoveRes[2] == maxV)
        //    {
        //        int oldX = board.playerLoc[board.curPlayer, 0];
        //        int oldY = board.playerLoc[board.curPlayer, 1];
        //        if (depth == 1)
        //        {
        //            Console.WriteLine("hi");
        //        }
        //        board.Move((int)pawnMoveRes[0], (int)pawnMoveRes[1], depth);

        //        if(depth > 1)
        //        {
        //            board.MoveBack(oldX, oldY);
        //        }
        //    }
        //    else if (horBlockMoveRes[2] == maxV)
        //    {
        //        board.PlaceBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1], depth);
        //        board.PlaceBlockHor((int)horBlockMoveRes[0] + 1, (int)horBlockMoveRes[1], depth);
        //        if(depth > 1)
        //        {
        //            board.RemoveBlockHor((int)horBlockMoveRes[0], (int)horBlockMoveRes[1]);
        //        }
        //    }
        //    else
        //    {
        //        board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1], depth);
        //        board.PlaceBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1] + 1, depth);
        //        if (depth > 1)
        //        {
        //            board.RemoveBlockVer((int)verBlockMoveRes[0], (int)verBlockMoveRes[1]);
        //        }
        //    }



        //    return maxV;

        //}
    }
}
