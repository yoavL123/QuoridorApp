using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Views;
using QuoridorApp.Services;
using QuoridorApp.Models;

namespace QuoridorApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {

        public ProfileViewModel()
        { }
        public ProfileViewModel(AbsoluteLayout theGraph)
        {
            RatingGraph ratingGraph = new RatingGraph(theGraph);
        }

        public ICommand ToRatingHistoryCommand => new Command(OnToRatingHistoryCommand);

        public async void OnToRatingHistoryCommand()
        {
            Page p = new Views.RatingHistory();
            await App.Current.MainPage.Navigation.PushAsync(p);


        }


        /*
        public ICommand showRatingCommand => new Command(OnShowRatingCommand(Label l));

        public async void OnShowRatingCommand(Label l)
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            
            RatingChange r = await proxy.GetLastRatingChange(CurrentApp.CurrentPlayer);
            l.Text = r.ToString();
        }
        */


        public ICommand ToMainMenuCommand => new Command(OnToMainMenuCommand);

        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);
            
            
        }
    }
}
