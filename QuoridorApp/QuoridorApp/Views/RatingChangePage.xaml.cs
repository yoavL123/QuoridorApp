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
    public partial class RatingChangePage : ContentPage
    {
        RatingChangePageViewModel vm;
        public RatingChangePage(string winner, string loser)
        {
            //InitializeComponent(); // FOR SOME REASON THIS HAS TO COME FIRST

            InitializeComponent();

            vm = new RatingChangePageViewModel(winner, loser);
            BindingContext = vm;
            
            
            
            
            // FOR SOME REASON THIS HAS TO COME FIRST

        }
    }
}