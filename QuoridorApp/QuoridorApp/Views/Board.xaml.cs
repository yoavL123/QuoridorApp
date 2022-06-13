//using QuoridorApp.ControlView;
using QuoridorApp.Models;
using QuoridorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
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
        public const double PAWN_TILE_SIZE = 60;
        public const double BLOCK_TILE_SMALL = 15;
        public const double BLOCK_TILE_BIG = 60;

        public const int SIZE = 9;

        PawnTile[,] pawnBoard;
        BlockTile[,] horBlockBoard;
        BlockTile[,] verBlockBoard;
        CenterTile[,] centerBlocked;
        public Board()
        {
            InitializeComponent();
            //vm = new BoardViewModel(theBoard);
            //BindingContext = vm;

            vm = new BoardViewModel();
            BindingContext = vm;



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
            DisplayBoard();
            /*
            #region Move Pawn
            public ICommand MovePawnCommand => new Command(OnMovePawnCommand);


            public async void OnMovePawnCommand(int player, int newX, int newY)
            {

            }
            #endregion
            */
        }


        void DisplayBoard()
        {
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
            
        }


        void Move(int newX, int newY)
        {
            vm.Move(newX, newY);
            DisplayBoard();
        }


        void PlaceBlockHor(int X, int Y)
        {
            vm.PlaceBlockHor(X, Y);
            DisplayBoard();
        }

        void PlaceBlockVer(int X, int Y)
        {
            vm.PlaceBlockVer(X, Y);
            DisplayBoard();
        }


    }
}