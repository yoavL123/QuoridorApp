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
        
        public Board()
        {
            InitializeComponent();
            vm = new BoardViewModel(theBoard);
            BindingContext = vm;
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