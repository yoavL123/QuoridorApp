using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuoridorApp.ViewModels;

namespace QuoridorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            MainMenuViewModel vm = new MainMenuViewModel();
            BindingContext = vm;
            InitializeComponent();
        }
    }
}