using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
//using Xamarin.contro

namespace QuoridorApp.ViewModels
{
    public class RatingHistoryViewModel : ViewModelBase
    {
        
        public RatingHistoryViewModel()
        {
            RatingData = new ObservableCollection<RatingChange>();
            Task.Run(UpdateRatings);
            //List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
        }


        public ICommand ToProfileCommand => new Command(OnToProfileCommand);

        public async void OnToProfileCommand()
        {
            Page p = new Views.Profile();
            await App.Current.MainPage.Navigation.PushAsync(p);
        }

        private ObservableCollection<RatingChange> ratingData;
        public ObservableCollection<RatingChange> RatingData
        {
            get => ratingData;
            private set
            {
                ratingData = value;
                OnPropertyChanged("RatingData");
            }
        }

        private async Task UpdateRatings()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            var data = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
            if (data != null)
            {
                RatingData.Clear();
                data.ForEach(item => RatingData.Add(item));
            }
        }

        /*
        async void CreateGrid()
        {
            TableView table = new TableView();
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
            foreach(RatingChange ratingChange in ratingChanges)
            {
                table.in
            }

        }
        */
    }
}
