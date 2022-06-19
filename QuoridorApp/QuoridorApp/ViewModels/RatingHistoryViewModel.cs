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
using System.Linq;
//using Xamarin.contro

namespace QuoridorApp.ViewModels
{
    public class RatingHistoryViewModel : ViewModelBase
    {
        
        public RatingHistoryViewModel()
        {
            RatingData = new ObservableCollection<RatingChange>();
            Device.BeginInvokeOnMainThread(async () => await UpdateRatings());
            //Task.Run(UpdateRatings);
            //List<RatingChange> ratingChanges = await proxy.GetRatingChanges(CurrentApp.CurrentPlayer);
        }


        public ICommand ToProfileCommand => new Command(OnToProfileCommand);
        private async Task<int> InitCurrentRating()
        {
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            RatingChange lastRatingChange = await proxy.GetLastRatingChange(CurrentApp.CurrentPlayer);
            if (lastRatingChange != null) return lastRatingChange.AlteredRating;
            else return 0;
        }
        public async void OnToProfileCommand()
        {
            int lastRating = await InitCurrentRating();
            Page p = new Views.Profile(lastRating);
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
            data = data.Distinct().ToList();
            if (data != null  && RatingData.Count() == 0)
            {
                RatingData.Clear();
                //data.ForEach(item => RatingData.Add(item));
                for(int i = data.Count() - 1; i >= 0; i--)
                {
                    RatingData.Add(data[i]);
                    
                }
            }
            //RatingData = (ObservableCollection<RatingChange>)RatingData.Reverse();
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
