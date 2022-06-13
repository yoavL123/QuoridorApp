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
    public partial class BoardBotOptions : ContentPage
    {
        BoardBotOptionsViewModel vm;
        public BoardBotOptions()
        {
            
            vm = new BoardBotOptionsViewModel();
            BindingContext = vm;
            InitializeComponent();
        }
    }
}