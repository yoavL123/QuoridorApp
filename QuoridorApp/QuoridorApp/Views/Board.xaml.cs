//using QuoridorApp.ControlView;
using QuoridorApp.Models;
using QuoridorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;


namespace QuoridorApp.Views
{
    struct Pos
    {
        public int Row { get; set; }

        public int Col { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Board : ContentPage
    {

        BoardViewModel vm;
        /*
        public const double PAWN_TILE_SIZE = 60;
        public const double BLOCK_TILE_SMALL = 15;
        public const double BLOCK_TILE_BIG = 60;
        
        */

        public const double PAWN_TILE_SIZE = 60;
        public const double BLOCK_TILE_SMALL = 15;
        public const double BLOCK_TILE_BIG = 60;
        private const int SLEEP_TIME_MILISECONDS = 350;

        public const int SIZE = 9;

        PawnTile[,] pawnBoard;
        BlockTile[,] horBlockBoard;
        BlockTile[,] verBlockBoard;
        CenterTile[,] centerBlocked;

        



        string[] playersType;
        bool hasFinished;

        public object CoreDispatcherPriority { get; }

        public Board(string type1, string type2)
        {
            hasFinished = false;
            playersType = new string[] { type1, type2 };
            InitializeComponent();
            
            
            


            pawnBoard = new PawnTile[SIZE, SIZE];
            horBlockBoard = new BlockTile[SIZE, SIZE - 1];
            verBlockBoard = new BlockTile[SIZE - 1, SIZE];
            //centerBlocked = new bool[SIZE, SIZE];
            centerBlocked = new CenterTile[SIZE - 1, SIZE - 1];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    //centerBlocked[i, j] = false;
                    int ri = i, rj = j;
                    pawnBoard[i, j] = new PawnTile(i, j) // Create the button add its properties:
                    {
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
                    if (i < SIZE - 1 && j < SIZE - 1)
                    {
                        centerBlocked[i, j] = new CenterTile(i, j)
                        {

                        };
                        //startX += PAWN_TILE_SIZE;
                        startX = (i + 1) * PAWN_TILE_SIZE + i * BLOCK_TILE_SMALL;
                        startY = (j + 1) * PAWN_TILE_SIZE + j * BLOCK_TILE_SMALL;
                        Rectangle centerBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_SMALL);
                        theBoard.Children.Add(centerBlocked[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(centerBlocked[i, j], centerBounds); // Add the button to the absolute layout in the view                     
                    }


                }
                
            }
            vm = new BoardViewModel();
            BindingContext = vm;
            //DisplayBoard();

            //HandleGame();
            //Task.Run(() => DisplayBoard());
            Task.Run(() => HandleGame());
            //HandleGame();
            //Task.Run(() => LaunchHandleGame());
            //while (true)
            //{
            //    DisplayBoard();
            //}

            /*
            #region Move Pawn
            public ICommand MovePawnCommand => new Command(OnMovePawnCommand);


            public async void OnMovePawnCommand(int player, int newX, int newY)
            {

            }
            #endregion
            */
        }

        public async Task FinishGame()
        {
            if (hasFinished) return;
            hasFinished = true;
            /*
            Button toRatingChangesButton = new Button();
            toRatingChangesButton.BindingContext = "To rating Change";
            */
            string winnerPlayer = playersType[0];
            string loserPlayer = playersType[1];
            int winnerIndex = 1;
            if (!vm.CheckWonPos(0, vm.playerLoc[0][1]))
            {
                winnerPlayer = playersType[1];
                loserPlayer = playersType[0];
                winnerIndex = 2;
            }
            Button toRatingChangesBtn = new Button
            {
                Text = "Player " + winnerIndex + " won!\n" + "To Rating Changes",
                
                BackgroundColor = Color.BurlyWood,
                
            };
            toRatingChangesBtn.IsVisible = true;
            //toRatingChangesBtn.Clicked += async (sender, args) => await label.RelRotateTo(360, 1000);


            int[][] ratingChangeArr = await vm.FinishGameAsync(winnerPlayer, loserPlayer);
            
            //toRatingChangesBtn.Clicked += (sender, args) => BoardViewModel.OnToRatingChangeCommand();
            toRatingChangesBtn.Clicked += async (sender, args) =>
            {
                Page p = new Views.RatingChangePage(winnerPlayer, loserPlayer, ratingChangeArr);
                await App.Current.MainPage.Navigation.PushAsync(p);
            };
            //toRatingChangesBtn.Clicked += BoardViewModel.OnToRatingChangeCommand();
            Device.BeginInvokeOnMainThread(() => {
                ratingChangesButton.Children.Add(toRatingChangesBtn);
                Rectangle buttonBounds = new Rectangle(0, 0, 200, 50);

                AbsoluteLayout.SetLayoutBounds(toRatingChangesBtn, buttonBounds); // Add the button to the absolute layout in the view
            });


        }
        async void HandleGame()
        {
            DisplayBoard();
            SleepIfNotHardBot();
            //Task.Run(() => DisplayBoard());
            DisplayBoard();
            if (vm.CheckWon())
            {
                await FinishGame();
                return;
            }

            if (BoardViewModel.isBot(playersType[vm.curPlayer]))
            {
                BotViewModel bot = new BotViewModel(playersType[vm.curPlayer]);
                //await bot.MakeMove(vm);
                BoardViewModel nboard = new BoardViewModel(vm);
                
                await bot.MakeMove(nboard);
                
                vm = nboard;
                
                Device.BeginInvokeOnMainThread(() => {
                    BindingContext = vm;

                });
                
                //HandleGame();
                DisplayBoard();
                //if(BoardViewModel.isBot(playersType[vm.curPlayer])) Task.Run(() => HandleGame());
                Task.Run(() => HandleGame());
            }
            
        }

        void DisplayBoard()
        {
            Device.BeginInvokeOnMainThread(() => {
                var rows = horBlockBoard.GetLength(0);
                var cols = horBlockBoard.GetLength(1);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        horBlockBoard[i, j].BlockTileStatus = vm.horBlockBoard[i, j];
                rows = verBlockBoard.GetLength(0);
                cols = verBlockBoard.GetLength(1);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        verBlockBoard[i, j].BlockTileStatus = vm.verBlockBoard[i, j];
                rows = centerBlocked.GetLength(0);
                cols = centerBlocked.GetLength(1);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        centerBlocked[i, j].CenterTileStatus = vm.centerBlocked[i, j];


                rows = pawnBoard.GetLength(0);
                cols = pawnBoard.GetLength(1);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        pawnBoard[i, j].PawnTileStatus = "Empty";

                pawnBoard[vm.playerLoc[0][0], vm.playerLoc[0][1]].PawnTileStatus = "Player1";
                pawnBoard[vm.playerLoc[1][0], vm.playerLoc[1][1]].PawnTileStatus = "Player2";
            });
            
            
        }

        private void SleepIfNotHardBot()
        {
            if(BoardViewModel.isBot(playersType[vm.curPlayer]))
            {
                if(playersType[vm.curPlayer] != "HardBot")
                {
                    Thread.Sleep(SLEEP_TIME_MILISECONDS);
                }
            }
        }
        void Move(int newX, int newY)
        {
            DisplayBoard();
            if (BoardViewModel.isBot(playersType[vm.curPlayer])) return;
            if (vm.CheckWon())
            {
                FinishGame();
                return;
            }
            vm.Move(newX, newY);
            DisplayBoard();
            //HandleGame();
            Task.Run(() => HandleGame());
        }


        void PlaceBlockHor(int X, int Y)
        {
            DisplayBoard();
            if (BoardViewModel.isBot(playersType[vm.curPlayer])) return;
            if (vm.CheckWon())
            {
                FinishGame();
                return;
            }
            vm.PlaceBlockHor(X, Y);
            DisplayBoard();
            //HandleGame();
            Task.Run(() => HandleGame());
        }

        void PlaceBlockVer(int X, int Y)
        {
            DisplayBoard();
            if (BoardViewModel.isBot(playersType[vm.curPlayer])) return;
            if (vm.CheckWon())
            {
                FinishGame();
                return;
            }
            vm.PlaceBlockVer(X, Y);
            DisplayBoard();
            //HandleGame();
            Task.Run(() => HandleGame());
        }


    }
}