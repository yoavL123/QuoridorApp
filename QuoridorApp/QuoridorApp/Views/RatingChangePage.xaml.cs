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
        public RatingChangePage(string winner, string loser, int[][] ratingChangeArr)
        {
            //InitializeComponent(); // FOR SOME REASON THIS HAS TO COME FIRST

            InitializeComponent();

            //vm = new RatingChangePageViewModel(winner, loser);
            //Task.Run(async () => BindingContext = await RatingChangePageViewModel.createViewModel(winner, loser));
            //Task.Run(async () => BindingContext = await RatingChangePageViewModel.createViewModel(winner, loser));
            //BindingContext = new RatingChangePageViewModel(winner, loser);
            vm = new RatingChangePageViewModel(winner, loser, ratingChangeArr);
            BindingContext = vm;
            //vm.InitializeRatings();




            // FOR SOME REASON THIS HAS TO COME FIRST

        }
    }
}