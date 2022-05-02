using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using QuoridorApp.Views;


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
        public ICommand ToMainMenuCommand => new Command(OnToMainMenuCommand);

        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);
            
            
        }
    }
}
