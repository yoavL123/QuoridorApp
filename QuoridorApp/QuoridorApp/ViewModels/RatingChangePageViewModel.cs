using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuoridorApp.ViewModels
{
    public class RatingChangePageViewModel : ViewModelBase
    {
        #region Winner Player
        private string winner;
        public string Winner
        {
            get => winner;
            set {
                winner = value;
                OnPropertyChanged("Winner");
            }
        }
        #endregion

        #region Loser Player
        private string loser;
        public string Loser
        {
            get => loser;
            set {
                loser = value;
                OnPropertyChanged("Loser");
            }
        }
        #endregion


        #region Winner Player Initial Rating
        private int winnerInitRating;
        public int WinnerInitRating
        {
            get => winnerInitRating;
            set
            {
                winnerInitRating = value;
                OnPropertyChanged("WinnerInitRating");
            }
        }
        #endregion

        #region Loser Player Initial Rating
        private int loserInitRating;
        public int LoserInitRating
        {
            get => loserInitRating;
            set
            {
                loserInitRating = value;
                OnPropertyChanged("LoserInitRating");
            }
        }
        #endregion


        #region Winner Player Updated Rating
        private int winnerUpdatedRating;
        public int WinnerUpdatedRating
        {
            get => winnerUpdatedRating;
            set
            {
                winnerUpdatedRating = value;
                OnPropertyChanged("WinnerUpdatedRating");
            }
        }
        #endregion


        #region Loser Player Updated Rating
        private int loserUpdatedRating;
        public int LoserUpdatedRating
        {
            get => loserUpdatedRating;
            set
            {
                loserUpdatedRating = value;
                OnPropertyChanged("LoserUpdatedRating");
            }
        }
        #endregion


        public RatingChangePageViewModel()
        { }
        
        private async Task<int> GetRating(string playerName)
        {
            //return 2222;
            if (BoardViewModel.isBot(playerName))
            {
                return BotViewModel.GetBotRating(playerName);
            }
            if (playerName == "Guest") return -1;
            //return RatingChange.INITIAL_RATING;
            Player player;
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            if (playerName == "Me")
            {
                player = CurrentApp.CurrentPlayer;
            }
            else
            {
                player = CurrentApp.CurrentPlayer;
                //player = await proxy.GetPlayer(playerName);
            }
            RatingChange ratingChange = await proxy.GetLastRatingChange(player);
            if(ratingChange == null) // Player hasn't played yet
            {
                return RatingChange.INITIAL_RATING;
            }
            return ratingChange.AlteredRating;
        }
        
        /*
        private int GetRating(string playerName)
        {
            if (BoardViewModel.isBot(playerName))
            {
                return BotViewModel.GetBotRating(playerName);
            }
            if (playerName == "Guest") return -1;
            return RatingChange.INITIAL_RATING;
        }
        */
        
        private async Task InitializeRatings()
        {
            WinnerInitRating = await GetRating(Winner);
            LoserInitRating = await GetRating(Loser);


            WinnerUpdatedRating = await GetRating(Winner);
            LoserUpdatedRating = await GetRating(Loser);

        }


        /*
        private void InitializeRatings()
        {
            WinnerInitRating = GetRating(Winner);
            LoserInitRating = GetRating(Loser);


            WinnerUpdatedRating = GetRating(Winner);
            LoserUpdatedRating = GetRating(Loser);
        }
        */
        public RatingChangePageViewModel(string winnerPlayer, string loserPlayer)
        {
            Winner = winnerPlayer;
            Loser = loserPlayer;


            //Device.BeginInvokeOnMainThread(() => {
            //    InitializeRatings();
            //});

            //Task.Run(async () => InitializeRatings()); 

            //Task task = InitializeRatings();
            //task.Wait();

            InitializeRatings();
            //task.RunSynchronously();
        }

    }
}
