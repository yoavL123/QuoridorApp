using QuoridorApp.ControlView;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Board : ContentPage
    {
        /*
        Height and width are like in real life
        FIRST VAR IS LEFT/RIGHT
        SECOND VAR IS HIGH/LOW
        */
        const int SIZE = 9;
        const double PAWN_TILE_SIZE = 60;
        const double BLOCK_TILE_SMALL = 15;
        const double BLOCK_TILE_BIG = 60;
        Button[,] pawnBoard;
        Button[,] horBlockBoard; // horizontal block board
        Button[,] verBlockBoard; // vertical block board
        BoardViewModel vm;
        void InitBoard()
        {
            pawnBoard = new Button[SIZE, SIZE];
            horBlockBoard = new Button[SIZE, SIZE-1];
            verBlockBoard = new Button[SIZE-1, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    // Create a pawn tile:
                    pawnBoard[i, j] = new Button // Create the button add its properties:
                    {
                        //BackgroundColor = Color.Black,
                        BackgroundColor = BoardViewModel.pawnTileCol[vm.pawnBoard[i, j]]
                    };
                    double startX = i * PAWN_TILE_SIZE + i * BLOCK_TILE_SMALL;
                    double startY = j * PAWN_TILE_SIZE + j * BLOCK_TILE_SMALL;
                    Rectangle pawnBounds = new Rectangle(startX, startY, PAWN_TILE_SIZE, PAWN_TILE_SIZE);
                    theBoard.Children.Add(pawnBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                    AbsoluteLayout.SetLayoutBounds(pawnBoard[i,j], pawnBounds); // Add the button to the absolute layout in the view


                    if(j < SIZE - 1) // Horizontal block cell is only between pawn cells
                    {
                        // Create a block tile:
                        horBlockBoard[i, j] = new Button
                        {
                            //BackgroundColor = Color.DarkRed,
                            BackgroundColor = BoardViewModel.blockTileCol[vm.horBlockBoard[i, j]]
                        };
                        startY += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_BIG, BLOCK_TILE_SMALL);
                        theBoard.Children.Add(horBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(horBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                        startY -= PAWN_TILE_SIZE;
                    }

                    if (i < SIZE - 1) // Horizontal block cell is only between pawn cells
                    {
                        verBlockBoard[i, j] = new Button
                        {
                            //BackgroundColor = Color.DarkRed,
                            //BackgroundColor = 
                            BackgroundColor = BoardViewModel.blockTileCol[vm.verBlockBoard[i, j]]
                        };
                        startX += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_BIG);
                        theBoard.Children.Add(verBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(verBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                        startX += PAWN_TILE_SIZE;
                    }
                }
            }
        }
        public Board()
        {
            vm = new BoardViewModel();
            BindingContext = vm;
            InitializeComponent();

            InitBoard();
            
        }



        /*
        #region Move Pawn
        public ICommand MovePawnCommand => new Command(OnMovePawnCommand);


        public async void OnMovePawnCommand(int player, int newX, int newY)
        {
            
        }
        #endregion
        */
    }
}