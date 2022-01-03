using QuoridorApp.ControlView;
using QuoridorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Board : ContentPage
    {

        const int SIZE = 9;
        const double PAWN_TILE_R = 10;
        const double PAWN_TILE_C = 10;
        const double BLOCK_TILE_R = 10;
        const double BLOCK_TILE_C = 10;
        //PawnTile[,] pawnTiles;
        Button[,] pawnBoard;
        Button[,] blockBoard;
        //BlockTile[,] blockTiles;

        
        void InitBoard()
        {
            pawnBoard = new Button[SIZE, SIZE];
            blockBoard = new Button[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    // Create a pawn tile:
                    pawnBoard[i, j] = new Button // Create the button ad its properties:
                    {
                        BackgroundColor = Color.Magenta,
                        ScaleX = 10,
                        ScaleY = 10
                    };
                    double startR = i * PAWN_TILE_R + i * BLOCK_TILE_R;
                    double startC = j * PAWN_TILE_C + j * BLOCK_TILE_C;
                    Rectangle pawnBounds = new Rectangle(startR, startC, PAWN_TILE_R, PAWN_TILE_C);
                    theBoard.Children.Add(pawnBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                    AbsoluteLayout.SetLayoutBounds(pawnBoard[i,j], pawnBounds); // Add the button to the absolute layout in the view


                    

                    // Create a blcok tile:
                    blockBoard[i, j] = new Button
                    {
                        BackgroundColor = Color.DarkViolet,
                        ScaleX = 10,
                        ScaleY = 10
                    };
                    

                    startR += PAWN_TILE_R;
                    startC = PAWN_TILE_C;
                    Rectangle blockBounds = new Rectangle(startR, startC, BLOCK_TILE_R, BLOCK_TILE_C);
                    theBoard.Children.Add(blockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                    AbsoluteLayout.SetLayoutBounds(pawnBoard[i, j], blockBounds); // Add the button to the absolute layout in the view


                }
            }
        }
        public Board()
        {
            this.BindingContext = new BoardViewModel();
            InitializeComponent();
            InitBoard();
        }

        /*
        public void OnBoardPawnChanged(int i, int j)
        {
            theBoard.
        }
        */
    }
}