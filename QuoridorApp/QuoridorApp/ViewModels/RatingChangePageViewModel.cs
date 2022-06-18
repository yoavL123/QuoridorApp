using QuoridorApp.Models;
using QuoridorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            set
            {
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
            set
            {
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


        

        private bool IsPlayer(string playerName)
        {
            if (BoardViewModel.isBot(playerName))
            {
                return false;
            }
            return true;
        }

        private Player GetPlayer(string playerName)
        {
            if (playerName == "Me") return CurrentApp.CurrentPlayer;
            return null;
        }
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
            if (ratingChange == null) // Player hasn't played yet
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

        public ICommand ToMainMenuCommand => new Command(OnToMainMenuCommand);

        public async void OnToMainMenuCommand()
        {
            Page p = new Views.MainMenu();
            await App.Current.MainPage.Navigation.PushAsync(p);


        }
        /*
        public async Task InitializeRatings()
        {
            int winnerInit, loserInit, winnerUpdated, loserUpdated;
            winnerInit = await GetRating(Winner);
            loserInit = await GetRating(Loser);

            Device.BeginInvokeOnMainThread(() => {
                WinnerInitRating = winnerInit;
                LoserInitRating = loserInit;

            });
            //WinnerInitRating = await GetRating(Winner);
            //LoserInitRating = await GetRating(Loser);


            int newWinnerRating = RatingChange.EloRating(winnerInit, loserInit, true);
            int newLoserRating = RatingChange.EloRating(loserInit, winnerInit, false);
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            if (IsPlayer(Winner))
            {
                RatingChange winnerRatingChange = new RatingChange(GetPlayer(Winner), newWinnerRating);
                proxy.UpdateRatingChange(winnerRatingChange);
            }
            if (IsPlayer(Loser))
            {
                RatingChange loserRatingChange = new RatingChange(GetPlayer(Loser), newLoserRating);
                proxy.UpdateRatingChange(loserRatingChange);
            }


            winnerUpdated = await GetRating(Winner);
            loserUpdated = await GetRating(Loser);


            Device.BeginInvokeOnMainThread(() => {
                WinnerUpdatedRating = winnerUpdated;
                LoserUpdatedRating = loserUpdated;

            });


            //WinnerUpdatedRating = await GetRating(Winner);
            //LoserUpdatedRating = await GetRating(Loser);
        }
        */
        
        private async Task InitializeRatings()
        {
            int winnerInit, loserInit, winnerUpdated, loserUpdated;
            winnerInit = await GetRating(Winner);
            loserInit = await GetRating(Loser);
            LoserInitRating = loserInit;
            WinnerInitRating = winnerInit;
            //WinnerInitRating = await GetRating(Winner);
            //LoserInitRating = await GetRating(Loser);

            
            int newWinnerRating = RatingChange.EloRating(WinnerInitRating, LoserInitRating, true);
            int newLoserRating = RatingChange.EloRating(LoserInitRating, WinnerInitRating, false);
            QuoridorAPIProxy proxy = QuoridorAPIProxy.CreateProxy();
            if(IsPlayer(Winner))
            {
                RatingChange winnerRatingChange = new RatingChange(GetPlayer(Winner), newWinnerRating);
                await proxy.UpdateRatingChange(winnerRatingChange);
            }
            if (IsPlayer(Loser))
            {
                RatingChange loserRatingChange = new RatingChange(GetPlayer(Loser), newLoserRating);
                await proxy.UpdateRatingChange(loserRatingChange);
            }


            winnerUpdated = await GetRating(Winner);
            loserUpdated = await GetRating(Loser);


            WinnerUpdatedRating = winnerUpdated;
            LoserUpdatedRating = loserUpdated;
            

            //WinnerUpdatedRating = await GetRating(Winner);
            //LoserUpdatedRating = await GetRating(Loser);
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

        private string FixName(string userName)
        {
            if (userName == "EasyBot") return "Easy Bot";
            if (userName == "MediumBot") return "Medium Bot";
            if (userName == "HardBot") return "Hard Bot";
            return userName;
        }
        
        public RatingChangePageViewModel(string winnerPlayer, string loserPlayer, int[][] ratingChangeArr)
        {
            Winner = winnerPlayer;
            Loser = loserPlayer;

            WinnerInitRating = ratingChangeArr[0][0];
            WinnerUpdatedRating = ratingChangeArr[0][1];
            LoserInitRating = ratingChangeArr[1][0];
            LoserUpdatedRating = ratingChangeArr[1][1];

            Winner = FixName(Winner);
            Loser = FixName(Loser);
            //Task.Run(async () => await InitializeRatings());
            //InitializeRatings();
        }

    }

}

