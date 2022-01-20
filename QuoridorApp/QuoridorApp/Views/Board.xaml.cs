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
        /*
        Height and width are like in real life
        FIRST VAR IS LEFT/RIGHT
        SECOND VAR IS HIGH/LOW
        */
        const int SIZE = 9;
        const double PAWN_TILE_SIZE = 40;
        const double BLOCK_TILE_SMALL = 10;
        const double BLOCK_TILE_BIG = 40;
        Button[,] pawnBoard;
        Button[,] horBlockBoard; // horizontal block board
        Button[,] verBlockBoard; // vertical block board

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
                        BackgroundColor = Color.Black,
                        //ScaleX = 10,
                        //ScaleY = 10
                        //HeightRequest = 20,
                        //WidthRequest = 20,
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
                            //BackgroundColor = Color.DarkViolet,
                            BackgroundColor = Color.DarkRed,
                            //ScaleX = 10,
                            //ScaleY = 10
                            //HeightRequest = 10,
                            //WidthRequest = 10,
                        };


                        //startR += PAWN_TILE_R;
                        startY += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_BIG, BLOCK_TILE_SMALL);
                        theBoard.Children.Add(horBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(horBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                                                                                          //Console.WriteLine($"StartC: {startC}");
                        startY -= PAWN_TILE_SIZE;
                    }

                    if (i < SIZE - 1) // Horizontal block cell is only between pawn cells
                    {
                        // Create a block tile:
                        verBlockBoard[i, j] = new Button
                        {
                            //BackgroundColor = Color.DarkViolet,
                            BackgroundColor = Color.DarkRed,
                            //ScaleX = 10,
                            //ScaleY = 10
                            //HeightRequest = 10,
                            //WidthRequest = 10,
                        };


                        //startR += PAWN_TILE_R;
                        startX += PAWN_TILE_SIZE;
                        Rectangle blockBounds = new Rectangle(startX, startY, BLOCK_TILE_SMALL, BLOCK_TILE_BIG);
                        theBoard.Children.Add(verBlockBoard[i, j]); // "theBoard" is defined in the view. Handled via binding
                        AbsoluteLayout.SetLayoutBounds(verBlockBoard[i, j], blockBounds); // Add the button to the absolute layout in the view
                                                                                          //Console.WriteLine($"StartC: {startC}");
                        startX += PAWN_TILE_SIZE;
                    }






                }
            }
        }
        public Board()
        {
            this.BindingContext = new BoardViewModel();
            InitializeComponent();
            InitBoard();
            //AbsoluteLayout.SetLayoutBounds(theBoard, new Rectangle(100, 100, 100, 100));
        }

        /*
        public void OnBoardPawnChanged(int i, int j)
        {
            theBoard.
        }
        */
    }
}