using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    public class RatingHistoryViewModel : ViewModelBase
    {
        
        public RatingHistoryViewModel()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            //List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
        }


        public ICommand ToProfileCommand => new Command(OnToProfileCommand);

        public async void OnToProfileCommand()
        {
            Page p = new Views.Profile();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }
    }
}
